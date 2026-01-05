# OnlineCoursesPlatform

Plataforma de cursos en línea desarrollada con .NET 8 y Clean Architecture.

## Estructura del Proyecto

```
proyecto-cursos/
├── Backend/           # API REST con Clean Architecture
│   ├── src/
│   │   ├── Domain/        # Entidades y lógica de dominio
│   │   ├── Application/   # Servicios de aplicación y DTOs
│   │   ├── Infrastructure/# Persistencia y servicios externos
│   │   └── API/           # Controladores y configuración
│   └── tests/             # Pruebas unitarias e integración
│
└── Frontend/          # Aplicación web con Razor Pages
    ├── Pages/         # Páginas de la aplicación
    ├── Services/      # Clientes API
    ├── Models/        # ViewModels
    └── wwwroot/       # Archivos estáticos
```

## Requisitos

- .NET 8 SDK
- SQL Server (o Docker)
- Node.js (opcional, para herramientas de frontend)

## Inicio Rápido

### Backend

```bash
cd Backend
dotnet restore
dotnet build
cd src/OnlineCoursesPlatform.API
dotnet run
```

### Frontend

```bash
cd Frontend
dotnet restore
dotnet run
```

## Configuración

1. Actualiza la cadena de conexión en `appsettings.json`
2. Ejecuta las migraciones: `dotnet ef database update`
3. Inicia la aplicación

## Licencia

MIT License
