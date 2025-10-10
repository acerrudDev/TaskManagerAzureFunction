# Documentacion en español / English documentacion at the end
# Task Management API - Azure Functions with C#

Este proyecto es un servicio de gestión de tareas que expone una API RESTful, implementado como una Azure Function 
en el modelo Isolated Worker.

---

## Arquitectura y Patrones

El diseño de este proyecto sigue las mejores prácticas para asegurar la mantenibilidad y la testabilidad:

* **Modelo de Desarrollo:** Azure Functions (Isolated Worker - .NET 8.0).
* **Acceso a Datos:** Entity Framework Core (Code First).
* **Patrones que se Implementaron:**
    * **Dependency Injection (DI):** Manejo de dependencias y servicios.
    * **Repository Pattern:** Abstracción de la lógica de acceso a datos.
    * * **Separation of Concerns:** Separacion de preocupaciones (Modelos ajustados a la capa del usuario que luego se adaptan a la capa de datos).
    * **DTOs (Data Transfer Objects):
      * ** Uso de modelos de entrada/salida separados de las entidades de la base de datos.

---

## Configuración y Ejecución Local

Sigue estos pasos para levantar el proyecto en tu entorno de desarrollo.

### Pre-requisitos

* .NET SDK 8.0
* Azure Functions Core Tools (Instalado globalmente)
* Azure SQL Server o SQL SERVER local
* SQL Management Studio (o LocalDB/SQL Express)

### Pasos

1.  **Clonar el Repositorio:**
    ```bash
    git clone https://github.com/acerrudDev/TaskManagerAzureFunction.git
    ```
2.  **Configurar Cadena de Conexión:**
    Asegúrate de que el archivo `local.settings.json` contenga tu cadena de conexión a SQL Server bajo la sección `Values`.
    ```json
    {
      "IsEncrypted": false,
      "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
        "SqlConnectionString": "Server=localhost;Database=TasksDB;..." 
      }
    }
    ```
3.  **Ejecutar Migraciones (Si usas EF Core):**
    ```bash
    # Ejecuta este comando desde el directorio del proyecto
    # Depende del hecho que quieras implementar la base de datos desde el proyecto
    # O si bien deseas crear tu modelo y conectarlo directamente sin el comando
    dotnet ef database update
    ```
4.  **Iniciar la Azure Function:**
    ```bash
    dotnet run
    ```
    La API estará disponible en `http://localhost:7071/`.

---

## Endpoints de la API (Swagger)

La documentación interactiva de la API está disponible automáticamente gracias a **OpenAPI Extensions**.

* **Documentación Interactiva (Swagger UI):**
    `http://localhost:7071/api/swagger/ui`

### Referencia Rápida

| Método | Ruta | Descripción |
| :--- | :--- | :--- |
| **`POST`** | `/api/createtask` | Crea una nueva tarea. |
| **`GET`** | `/api/tasks` | Obtiene la lista completa de tareas. |
| **`PUT`** | `/api/tasks/{taskId:guid}` | Actualiza una tarea por ID. |
| **`DELETE`** | `/api/removetask/{taskId:guid}` | Elimina una tarea por ID. |

---

## Pruebas Unitarias

Las pruebas unitarias utilizan **xUnit** y **Moq** para validar la lógica de negocio en la capa de **Service**, 
asegurando que no haya dependencia de la base de datos.

### Ejecutar Pruebas

Ejecuta todas las pruebas desde la consola:

### bash

dotnet test TaskManagerApiAf.Tests

    #End of Spanish Documentation ##################################################################################################
    
    # English Documentation Start
# Task Management API - Azure Functions with C#

This project is a task management service that exposes a RESTful API, implemented as an **Azure Function** using the **Isolated Worker model**.

---

## Architecture and Patterns

The design of this project follows best practices to ensure maintainability and testability:

* **Development Model:** Azure Functions (Isolated Worker - .NET 8.0).
* **Data Access:** Entity Framework Core (Code First).
* **Implemented Patterns:**
    * **Dependency Injection (DI):** Handles dependencies and services.
    * **Repository Pattern:** Abstraction of data access logic.
    * **Separation of Concerns:** Models are adjusted for the user/presentation layer and then adapted for the data layer (using DTOs).
    * **DTOs (Data Transfer Objects):** Uses separate input/output models from the database entities.

---

## Local Configuration and Execution

Follow these steps to run the project in your development environment.

### Prerequisites

* .NET SDK 8.0
* Azure Functions Core Tools (Globally installed)
* Azure SQL Server or local SQL Server
* SQL Management Studio (or LocalDB/SQL Express)

### Steps

1.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/acerrudDev/TaskManagerAzureFunction.git](https://github.com/acerrudDev/TaskManagerAzureFunction.git)
    ```
2.  **Configure Connection String:**
    Ensure the `local.settings.json` file contains your SQL Server connection string under the `Values` section.
    ```json
    {
      "IsEncrypted": false,
      "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
        "SqlConnectionString": "Server=localhost;Database=TasksDB;..." 
      }
    }
    ```
3.  **Run Migrations (If using EF Core):**
    ```bash
    # Execute this command from the project directory. 
    # This is required if you wish to implement the database from the project 
    # or if you prefer to create your model and connect it directly without this command.
    dotnet ef database update
    ```
4.  **Start the Azure Function:**
    ```bash
    dotnet run
    ```
    The API will be available at `http://localhost:7071/`.

---

## API Endpoints (Swagger)

The interactive API documentation is automatically available thanks to **OpenAPI Extensions**.

* **Interactive Documentation (Swagger UI):**
    `http://localhost:7071/api/swagger/ui`

### Quick Reference

| Method | Path | Description |
| :--- | :--- | :--- |
| **`POST`** | `/api/createtask` | Creates a new task. |
| **`GET`** | `/api/tasks` | Retrieves the complete list of tasks. |
| **`PUT`** | `/api/tasks/{taskId:guid}` | Updates an existing task by ID. |
| **`DELETE`** | `/api/removetask/{taskId:guid}` | Permanently removes a task by ID. |

---

## Unit Tests

Unit tests use **xUnit** and **Moq** to validate the business logic in the **Service** layer, ensuring no dependence on the database.

### Running Tests

Execute all tests from the console:

```bash
dotnet test TaskManagerApiAf.Tests