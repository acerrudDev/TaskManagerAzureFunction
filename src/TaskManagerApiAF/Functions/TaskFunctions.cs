using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using TaskManagerApiAF.Models;
using TaskManagerApiAF.Data.Context;
using TaskManagerApiAF.Interfaces.IServices;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;

namespace TaskManagerApiAF.Functions
{
    public class TaskFunctions
    {
        private readonly ITaskManagementService _taskManagementService;

        public TaskFunctions(ITaskManagementService taskManagementService)
        {
            _taskManagementService = taskManagementService;
        }

        [Function("GetTasks")]
        [OpenApiOperation(operationId: "getTasks", tags: new[] { "tasks" }, Summary = "Get all the created tasks", Description = "Return a List of all created task in the system.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Tasks>), Summary = "List if all Tasks.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent, Summary = "Task List is empty.")]
        public async Task<HttpResponseData> GetTasks(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasks")] HttpRequestData req)
        {
            var tasks = await _taskManagementService.GetTasks();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(tasks);
            return response;
        }

        [Function("CreateTask")]
        [OpenApiOperation(operationId: "createTask", tags: new[] { "tasks" }, Summary = "Create a new task", Description = "Create a new tasks and add it to database.")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(TaskBodyRequest), Description = "The Task Object to be created (not including TaskId which is generated automatically).")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Tasks), Summary = "Tasks Successfully Created with assigned ID.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Summary = "Internal Server Error.")]
        public async Task<HttpResponseData> CreateTask(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "createtask")] HttpRequestData req)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var taskData = JsonSerializer.Deserialize<TaskBodyRequest>(body);

            var createdTask = await _taskManagementService.CreateTask(taskData);

            var response = req.CreateResponse(System.Net.HttpStatusCode.Created);
            await response.WriteStringAsync(JsonSerializer.Serialize(createdTask));
            return response;
        }

        
        [Function("UpdateTask")]
        [OpenApiOperation(operationId: "updateTask", tags: new[] { "tasks" }, Summary = "Update specific task", Description = "Modifies task data fields and send it to database.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The ID of the task to be updated.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Tasks), Summary = "Tasks Successfully Updated with assigned ID.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Internal Server Error.")]
        public async Task<HttpResponseData> UpdateTask(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "tasks/{id:guid}")] HttpRequestData req,
            Guid id)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var taskData = JsonSerializer.Deserialize<TaskBodyRequest>(body);

            var updated = await _taskManagementService.UpdateTask(taskData, id);

            var response = req.CreateResponse(updated ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound);
            return response;
        }

        [Function("DeleteTask")]
        [OpenApiOperation(operationId: "deleteTask", tags: new[] { "tasks" }, Summary = "Remove specific tasks", Description = "Removed permanently specific task from system.")]
        [OpenApiParameter(name: "taskId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "ID of Task to be removed.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Summary = "Task successfully removed.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Task ID not found.")]
        public async Task<HttpResponseData> DeleteTask(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "removetask/{id:guid}")] HttpRequestData req,
            Guid id)
        {
            var deleted = await _taskManagementService.DeleteTask(id);

            var response = req.CreateResponse(deleted ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound);
            response.Headers.Add("Content-Type", "application/json");

            if (deleted)
            {
                await response.WriteStringAsync(JsonSerializer.Serialize(new
                {
                    message = "Task successfully removed",
                    taskId = id
                }));
            }
            else
            {
                await response.WriteStringAsync(JsonSerializer.Serialize(new
                {
                    error = "Task not found",
                    taskId = id
                }));
            }
            return response;
        }
    }
}
