# BackBookRentals - Backend

## Descripción
Backend para el sistema de alquiler de libros desarrollado en .NET 9.0. Este proyecto implementa una API RESTful que permite gestionar clientes, libros y órdenes de alquiler.

## Características
- Autenticación con JWT
- Swagger para documentación de la API
- Validación de datos
- Paginación de resultados
- Manejo de errores
- Mapeo de entidades con AutoMapper
- Patrón Repository
- Inyección de dependencias

## Estructura del Proyecto
```
BackBookRentals/
├── BackBookRentals.Api/           # Capa de presentación (API)
├── BackBookRentals.Dto/           # Objetos de transferencia de datos
├── BackBookRentals.Entities/      # Entidades del dominio
├── BackBookRentals.Repositories/  # Acceso a datos
└── BackBookRentals.Services/      # Lógica de negocio
```

## Requisitos
- .NET 9.0 SDK
- SQL Server
- Visual Studio 2022 o Visual Studio Code

## Configuración
1. Clonar el repositorio
2. Configurar la cadena de conexión en `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BackBookRentals;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

3. Ejecutar las migraciones:
```bash
dotnet ef database update
```

## Docker Compose
El proyecto incluye un archivo `docker-compose.yml` para facilitar el despliegue. Contiene:
- SQL Server
- Backend API

### Requisitos
- Docker Desktop
- Docker Compose

### Pasos para ejecutar con Docker Compose
1. Construir las imágenes:
```bash
docker-compose build
```

2. Iniciar los contenedores:
```bash
docker-compose up -d
```

3. Verificar que los contenedores estén ejecutándose:
```bash
docker-compose ps
```

4. Para detener los contenedores:
```bash
docker-compose down
```

### Variables de entorno
El archivo `docker-compose.yml` utiliza las siguientes variables de entorno:
```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Development
  - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=BackBookRentals;User=sa;Password=Your_password123;TrustServerCertificate=True;
  - JWT__SecretKey=your-secret-key-here
  - JWT__Issuer=BackBookRentals
  - JWT__Audience=BackBookRentals
```

### Puertos expuestos
- API: 7202
- SQL Server: 1433

### Acceso a la API
Una vez que los contenedores estén ejecutándose, la API estará disponible en:
```
http://localhost:7202
```

La documentación de Swagger estará disponible en:
```
http://localhost:7202/swagger
```

## Endpoints Principales

### Clientes
- `GET /api/clients` - Listar clientes (con paginación)
- `POST /api/clients` - Crear cliente
- `PATCH /api/clients/{id}` - Actualizar cliente
- `DELETE /api/clients/{id}` - Eliminar cliente

### Libros
- `GET /api/books` - Listar libros (con paginación)
- `POST /api/books` - Crear libro
- `PATCH /api/books/{id}` - Actualizar libro
- `DELETE /api/books/{id}` - Eliminar libro

### Órdenes
- `GET /api/orders` - Listar órdenes (con paginación)
- `POST /api/orders` - Crear orden
- `GET /api/orders/client/{clientId}/orders` - Obtener órdenes por cliente
- `GET /api/orders/book/{bookId}/orders` - Obtener órdenes por libro
- `PATCH /api/orders/{orderId}/status` - Actualizar estado de orden
- `DELETE /api/orders/{orderId}` - Eliminar orden

## Autenticación
La API utiliza autenticación JWT. Para acceder a los endpoints protegidos:
1. Obtener token de autenticación
2. Incluir el token en el header de las peticiones:
```
Authorization: Bearer <token>
```

## Documentación
La documentación de la API está disponible en Swagger UI:
```
https://localhost:7202/swagger
```

## Pruebas
Para ejecutar las pruebas unitarias:
```bash
dotnet test
```