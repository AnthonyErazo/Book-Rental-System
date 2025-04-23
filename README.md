# Sistema de Alquiler de Libros

Sistema completo para la gestión de alquiler de libros, compuesto por un backend en .NET 9.0 y un frontend en Angular 17+.

## Estructura del Proyecto

```
book_rentals/
├── BackBookRentals/          # Backend en .NET 9.0
│   ├── src/                  # Código fuente del backend
│   │   ├── BackBookRentals.Api/      # Capa de presentación
│   │   ├── BackBookRentals.Dto/      # Objetos de transferencia
│   │   ├── BackBookRentals.Entities/ # Entidades del dominio
│   │   ├── BackBookRentals.Repositories/ # Acceso a datos
│   │   └── BackBookRentals.Services/ # Lógica de negocio
│   └── docker-compose.yml    # Configuración Docker del backend
│
└── FrontBookRentals/         # Frontend en Angular 17+
    ├── src/                  # Código fuente del frontend
    │   ├── app/              # Componentes y servicios
    │   │   ├── api-client/   # Clientes generados por OpenAPI
    │   │   ├── auth/         # Autenticación
    │   │   ├── core/         # Servicios core
    │   │   ├── dashboard/    # Componentes principales
    │   │   └── shared/       # Componentes compartidos
    │   └── assets/           # Recursos estáticos
    └── docker-compose.yml    # Configuración Docker del frontend
```

## Requisitos Previos

- Docker y Docker Compose
- .NET 9.0 SDK (para desarrollo local)
- Node.js 18+ y npm (para desarrollo local)
- SQL Server (para desarrollo local)

## Despliegue con Docker Compose

El sistema puede ser desplegado completamente usando Docker Compose. El archivo `docker-compose.yml` en la raíz del proyecto configura todos los servicios necesarios:

```bash
# Construir e iniciar todos los servicios
docker-compose up --build

# Detener todos los servicios
docker-compose down
```

### Servicios Desplegados

- **Frontend**: http://localhost:4200
- **Backend API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **Adminer (Gestor de BD)**: http://localhost:8081

### Variables de Entorno

Las siguientes variables de entorno pueden ser configuradas:

- `ASPNETCORE_ENVIRONMENT`: Entorno de ejecución del backend (Docker/Development)
- `SA_PASSWORD`: Contraseña del usuario SA de SQL Server
- `DOCKER`: Indica si el frontend está ejecutándose en Docker

Nota: Puede revisar la documentacion del Backend para mas informacion

## Desarrollo Local y Documentacion adicional

- [Documentación del Backend](./BackBookRentals/README.md)
- [Documentación del Frontend](./FrontBookRentals/README.md)

## Tecnologías Utilizadas

### Backend
- .NET 9.0
- Entity Framework Core
- SQL Server
- AutoMapper
- JWT Authentication
- Swagger/OpenAPI

### Frontend
- Angular 17+
- TypeScript
- OpenAPI Generator
- Nginx
- Docker