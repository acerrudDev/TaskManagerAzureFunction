# Task Management API - Azure Functions

Este proyecto es un servicio de gestión de tareas que expone una API RESTful, implementado como una Azure Function 
en el modelo Isolated Worker.

---

## Arquitectura y Patrones

El diseño de este proyecto sigue las mejores prácticas para asegurar la mantenibilidad y la testabilidad:

* **Modelo de Desarrollo:** Azure Functions (Isolated Worker - .NET [8.0]).
* **Acceso a Datos:** Entity Framework Core (Code First).
* **Patrones Implementados:**
    * **Dependency Injection (DI):** Manejo de dependencias y servicios.
    * **Repository Pattern:** Abstracción de la lógica de acceso a datos.
    * **DTOs (Data Transfer Objects):
      * ** Uso de modelos de entrada/salida separados de las entidades de la base de datos.

---

## Configuración y Ejecución Local

Sigue estos pasos para levantar el proyecto en tu entorno de desarrollo.

### Pre-requisitos

* .NET SDK [8.0]
* Azure Functions Core Tools (Instalado globalmente)
* Azure SQL Server o SQL SERVER local
* SQL Management Studio (o LocalDB/SQL Express)

### Pasos

1.  **Clonar el Repositorio:**
    ```bash
    git clone [https://aws.amazon.com/es/what-is/repo/](https://aws.amazon.com/es/what-is/repo/)
    cd TaskManagerApiAf
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