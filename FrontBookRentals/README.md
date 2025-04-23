# FrontBookRentals

FrontBookRentals es una aplicación web desarrollada con Angular que permite gestionar el alquiler de libros. La aplicación proporciona una interfaz intuitiva para usuarios y administradores, facilitando la gestión de libros, préstamos y devoluciones.

## Estructura del Proyecto

```
src/app/
├── api-client/         # Clientes y modelos generados por OpenAPI
├── auth/              # Componentes y servicios de autenticación
├── core/              # Interceptores, guards y servicios core
├── dashboard/         # Componentes del dashboard principal
├── services/          # Servicios de la aplicación
├── shared/            # Componentes y directivas compartidas
├── app.component.*    # Componente principal de la aplicación
├── app.config.ts      # Configuración de la aplicación
└── app.routes.ts      # Rutas de la aplicación
```

## Tecnologías Utilizadas

- Angular 17+
- OpenAPI Generator (para generar clientes y modelos desde swagger.json)
- Docker & Docker Compose
- Nginx (para el servidor de producción)

## Requisitos Previos

- Node.js (versión 18 o superior)
- npm (incluido con Node.js)
- Docker y Docker Compose (para despliegue con contenedores)

## Desarrollo Local

### Instalación de Dependencias

```bash
npm install
```

### Generación de Clientes OpenAPI

Si necesitas regenerar los clientes y modelos desde el swagger.json:

```bash
openapi-generator-cli generate -i swagger.json -g typescript-angular -o src/app/api-client --additional-properties=ngVersion=15.0
```

### Servidor de Desarrollo

Para iniciar el servidor de desarrollo local:

```bash
npm start
```

La aplicación estará disponible en `http://localhost:4200/`. La aplicación se recargará automáticamente cuando modifiques cualquier archivo fuente.

### Construcción del Proyecto

Para crear una versión de producción:

```bash
npm build
```

Los archivos compilados se almacenarán en el directorio `dist/`.

## Despliegue con Docker

### Construir e Iniciar con Docker Compose

Para construir e iniciar la aplicación en un contenedor Docker:

```bash
docker-compose up --build
```

La aplicación estará disponible en `http://localhost:4200/`

### Detener los Contenedores

Para detener la aplicación:

```bash
docker-compose down
```

## Configuración del Backend

La aplicación está configurada para conectarse a un backend en `http://localhost:5000`. Si necesitas cambiar esta URL, modifica la variable `backendUrl` en [`src/app/app.config.ts`](./src/app/app.config.ts).

## Recursos Adicionales

- [Documentación de Angular](https://angular.io/docs)
- [OpenAPI Generator](https://openapi-generator.tech/)
- [Documentación de Docker](https://docs.docker.com/)
- [Documentación de Docker Compose](https://docs.docker.com/compose/)
- [`Documentacion del Backend`](../BackBookRentals/README.md)