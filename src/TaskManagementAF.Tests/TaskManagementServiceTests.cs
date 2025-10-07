using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApiAF.Interfaces.IRepositories;
using TaskManagerApiAF.Models;
using TaskManagerApiAF.Services;

namespace TaskManagementAF.Tests
{
    public class TaskManagementServiceTests
    {
        private readonly Mock<ITaskManagementRepository> _mockRepo; 
        private readonly TaskManagementService _service; 
        public TaskManagementServiceTests() 
        { 
            _mockRepo = new Mock<ITaskManagementRepository>(); 
            _service = new TaskManagementService(_mockRepo.Object); 
        }

        [Fact]
        public async Task GetSpecificTask_ShouldReturnTask()
        { 
            var taskId = Guid.NewGuid(); 
            var task = new Tasks { TaskId = taskId, Title = "Sample Task" };
            _mockRepo.Setup(r => r.GetSpecificTask(taskId)) .ReturnsAsync(task); 
            
            var result = await _service.GetSpecificTask(taskId); 
            Assert.NotNull(result); 
            Assert.Equal(taskId, result.TaskId); _mockRepo.Verify(r => r.GetSpecificTask(taskId), Times.Once); 
        }

        [Fact]
        public async Task GetTasks_ShouldReturnTaskList()
        { 
            var tasks = new List<Tasks> 
            { 
                new Tasks { TaskId = Guid.NewGuid(), Title = "Task 1" }, 
                new Tasks { TaskId = Guid.NewGuid(), Title = "Task 2" } 
            };

            _mockRepo.Setup(r => r.GetTasks()) .ReturnsAsync(tasks);            
            var result = await _service.GetTasks(); 
            
            Assert.NotNull(result); 
            Assert.Equal(2, result.Count); 
            _mockRepo.Verify(r => r.GetTasks(), Times.Once); 
        }

        [Fact]
        public async Task CreateTask_ShouldReturnTask()
        { 
            var newReqTask = new TaskBodyRequest { Title = "Unit Test Task", Description = "Creation of new task" };

            var newTask = new Tasks { TaskId = Guid.NewGuid(), Title = newReqTask.Title, Description = newReqTask.Description };
            _mockRepo.Setup(r => r.CreateTask(It.IsAny<Tasks>())) 
                .ReturnsAsync(newTask); 
            
            var result = await _service.CreateTask(newReqTask); 
            Assert.NotNull(result); 
            Assert.Equal("Unit Test Task", result.Title); 
            _mockRepo.Verify(r => r.CreateTask(It.Is<Tasks>(t => t.Title == "Unit Test Task")), Times.Once); 
        }

        [Fact]
        public async Task UpdateTask_ShouldReturnTrueWhenUpdated() 
        {            
            var updatingTask = new TaskBodyRequest { Title = "Unit Test Task Modification.." };
            var task = new Tasks { TaskId = Guid.NewGuid(), Title = "Unit Test Task Modification.." };

            _mockRepo.Setup(r => r.UpdateTask(It.IsAny<Tasks>()))
                .ReturnsAsync(true);            

            var result = await _service.UpdateTask(updatingTask, task.TaskId);

            Assert.True(result); ;
            _mockRepo.Verify(r => r.UpdateTask(It.Is<Tasks>(t => t.TaskId == task.TaskId && t.Title == "Unit Test Task Modification..")), Times.Once);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnTrue_WhenTaskIsDeleted()
        {
            var taskId = Guid.NewGuid();
            var repositoryMock = new Mock<ITaskManagementRepository>();

            repositoryMock.Setup(r => r.DeleteTask(taskId)).ReturnsAsync(true);
            var service = new TaskManagementService(repositoryMock.Object);
            var result = await service.DeleteTask(taskId);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnFalse_WhenTaskNotFound()
        {
            var taskId = Guid.NewGuid();
            var repositoryMock = new Mock<ITaskManagementRepository>();
            repositoryMock.Setup(r => r.DeleteTask(taskId)).ReturnsAsync(false);

            var service = new TaskManagementService(repositoryMock.Object);
            var result = await service.DeleteTask(taskId);

            Assert.False(result);
        }
    }
}
