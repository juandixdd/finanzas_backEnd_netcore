# 📦 BaseBackend API

<div align="center">

![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-Auth-000000?style=for-the-badge&logo=json-web-tokens&logoColor=white)

**Una API REST profesional, escalable y modular basada en Clean Architecture.**

[Reportar Bug](https://github.com/) · [Solicitar Feature](https://github.com/)

</div>

---

## 📑 Tabla de Contenido
- [📍 Descripción General](#-descripción-general)
- [🏗 Arquitectura](#-arquitectura)
- [🛠 Tecnologías](#-tecnologías)
- [⚙ Configuración e Instalación](#-configuración-e-instalación)
- [🔐 Autenticación y Seguridad](#-autenticación-y-seguridad)
- [📘 Documentación de API](#-documentación-de-api)
- [✨ Características Clave](#-características-clave)
- [🚀 Flujo de Prueba](#-flujo-de-prueba)
- [🔮 Roadmap](#-roadmap)

---

## 📍 Descripción General

**BaseBackend** es una plantilla de arquitectura robusta diseñada para acelerar el desarrollo de servicios RESTful en .NET. Implementa las mejores prácticas de la industria, incluyendo separación de responsabilidades, inyección de dependencias, manejo global de errores y optimización de consultas a base de datos.

Es ideal para iniciar proyectos que requieran escalabilidad, mantenibilidad y seguridad desde el día uno.

---

## 🏗 Arquitectura

El proyecto sigue estrictamente los principios de **Clean Architecture**, asegurando que el núcleo del negocio sea independiente de frameworks externos, UI o bases de datos.

```text
BaseBackend
│
├── 📂 Api                 → (Presentation) Controladores, Middlewares, Entry Point.
├── 📂 Application         → (Core) Lógica de negocio, DTOs, Interfaces, Mappings.
├── 📂 Domain              → (Core) Entidades, Value Objects, Interfaces de Repositorio.
└── 📂 Infrastructure      → (External) EF Core, SQL Server, Implementación de Repositorios, JWT.

Distribución de Responsabilidades
Capa	Descripción
Domain	Contiene las entidades y las reglas de negocio empresariales. No tiene dependencias externas.
Application	Orquesta los casos de uso. Contiene DTOs, validaciones y lógica de aplicación.
Infrastructure	Implementa la persistencia de datos (EF Core), servicios de identidad y acceso a sistemas externos.
Api	Punto de entrada de la aplicación (Controllers). Gestiona la configuración y exposición HTTP.
🛠 Tecnologías

    Framework: .NET 8 SDK

    Web API: ASP.NET Core

    ORM: Entity Framework Core

    Base de Datos: SQL Server

    Autenticación: JWT Bearer Authentication

    Mapeo: AutoMapper

    Documentación: Swagger / OpenAPI

⚙ Configuración e Instalación
1. Prerrequisitos

    .NET 8 SDK instalado.

    SQL Server (LocalDB o instancia completa).

2. Configuración (appsettings.json)

Configura tu cadena de conexión y las claves secretas para JWT en el proyecto Api.
JSON

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=BaseBackendDb;Trusted_Connection=True;TrustServerCertificate=True;"
},
"Jwt": {
  "Key": "TU_CLAVE_SUPER_SECRETA_DEBE_SER_LARGA",
  "Issuer": "BaseBackend",
  "Audience": "BaseBackendUsers"
}

3. Ejecución

Las migraciones se aplican automáticamente al iniciar la aplicación (db.Database.Migrate()).
Bash

# Clonar el repositorio
git clone [https://github.com/tu-usuario/BaseBackend.git](https://github.com/tu-usuario/BaseBackend.git)

# Restaurar dependencias
dotnet restore

# Ejecutar la API
dotnet run --project BaseBackend.Api

Accede a la documentación interactiva en:

👉 https://localhost:{port}/swagger
🔐 Autenticación y Seguridad

La API utiliza tokens JWT (JSON Web Tokens).
Para acceder a los endpoints protegidos, debes incluir el token en el encabezado de la petición:
HTTP

Authorization: Bearer {token}

📘 Documentación de API
🔑 Auth Module (Público)
Método	Endpoint	Descripción
POST	/api/Auth/register	Registro de nuevo usuario.
POST	/api/Auth/login	Inicio de sesión. Retorna el JWT.

<details>
<summary>👁‍🗨 Ver ejemplo de Login</summary>

Request:
JSON

{
  "email": "user@email.com",
  "password": "password123"
}

Response (200 OK):
JSON

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}

</details>
📦 Product Module (Protegido)
Método	Endpoint	Descripción
GET	/api/Product	Listado paginado de productos.
GET	/api/Product/{id}	Obtener detalle de un producto.
POST	/api/Product	Crear un producto.
PUT	/api/Product/{id}	Actualizar un producto.
DELETE	/api/Product/{id}	Eliminar un producto.

<details>
<summary>👁‍🗨 Ver ejemplo de Paginación</summary>

GET /api/Product?page=1&pageSize=10
JSON

{
  "items": [
    { "id": 1, "name": "Laptop", "price": 1500 }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 25,
  "totalPages": 3
}

</details>
✨ Características Clave
⚠ Manejo Global de Errores

Middleware personalizado que intercepta excepciones y estandariza la respuesta HTTP.

    400 Bad Request: ValidationException

    401 Unauthorized: UnauthorizedException

    404 Not Found: NotFoundException

    500 Internal Error: Exception genérica

Respuesta estándar:
JSON

{
  "success": false,
  "status": 400,
  "error": "El nombre del producto es obligatorio."
}

📄 Paginación Optimizada

La paginación se realiza directamente en la base de datos usando Skip y Take, asegurando eficiencia incluso con millones de registros.
🗺 AutoMapper

Mapeo automático entre Entidades y DTOs (Entity ↔ DTO) para reducir el código repetitivo y desacoplar la capa de persistencia de la capa de presentación.
🚀 Flujo de Prueba

Sigue estos pasos para verificar el funcionamiento:

    Registrar Usuario: Usa el endpoint POST /Auth/register.

    Login: Usa POST /Auth/login y copia el token de la respuesta.

    Autorizar en Swagger: Haz clic en el botón Authorize (candado) y escribe Bearer TU_TOKEN.

    Crear Producto: Usa POST /Product con el usuario autenticado.

    Listar Productos: Usa GET /Product para ver la paginación en acción.

🔮 Roadmap

    [ ] Filtros dinámicos y búsqueda avanzada.

    [ ] Ordenamiento dinámico de columnas.

    [ ] Implementación de Soft Delete (Borrado lógico).

    [ ] Versionado de API (v1, v2).

    [ ] Implementación de Rate Limiting.

    [ ] Patrón CQRS con MediatR.

    [ ] Unit Testing con xUnit.

<div align="center">
<sub>Desarrollado con ❤️ en .NET 8</sub>
</div>