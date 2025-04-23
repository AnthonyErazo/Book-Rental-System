# BackBookRentals - Backend

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

## Configuración Local
Para ejecutar el proyecto localmente:

1. Navegar al directorio del proyecto API:
```bash
cd src/BackBookRentals.Api
```

2. Ejecutar el proyecto en modo desarrollo:
```bash
dotnet run --environment Development
```

## Docker Compose
El proyecto incluye un archivo `docker-compose.yml` para facilitar el despliegue. Contiene:
- SQL Server
- Backend API
- Adminer (Gestor de base de datos)

### Requisitos
- Docker Desktop
- Docker Compose

### Pasos para ejecutar con Docker Compose
1. Construir las imágenes e iniciar contenedores:
```bash
docker-compose up --build
```

2. Verificar que los contenedores estén ejecutándose:
```bash
docker-compose ps
```

3. Para detener los contenedores:
```bash
docker-compose down
```

### Puertos expuestos con Docker Compose
- API: 5000
- SQL Server: 1433
- Adminer: 8081

### Acceso a Adminer
Adminer es una interfaz web ligera para gestionar bases de datos. Para acceder y configurar la conexión a la base de datos:

1. Abre http://localhost:8081 en tu navegador
2. Configura la conexión con los siguientes datos:
   - Sistema: Microsoft SQL Server (MS SQL)
   - Servidor: sqlserver
   - Usuario: SA
   - Contraseña: P@ssw0rd123
   - Base de datos: BookRentals

Una vez conectado, podrás:
- Ver y gestionar las tablas de la base de datos
- Ejecutar consultas SQL
- Importar/exportar datos
- Gestionar usuarios y permisos
- Ver la estructura de la base de datos

### Acceso a la API
Una vez que los contenedores estén ejecutándose, la API estará disponible en:
```
http://localhost:5000
```

La documentación de Swagger estará disponible en:
```
http://localhost:5000/swagger
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