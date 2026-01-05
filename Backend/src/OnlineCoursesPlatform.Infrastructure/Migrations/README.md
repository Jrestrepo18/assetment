# Migrations

Esta carpeta contiene las migraciones de Entity Framework Core.

## Comandos útiles

### Crear una nueva migración
```bash
dotnet ef migrations add NombreMigracion -p src/OnlineCoursesPlatform.Infrastructure -s src/OnlineCoursesPlatform.API
```

### Aplicar migraciones
```bash
dotnet ef database update -p src/OnlineCoursesPlatform.Infrastructure -s src/OnlineCoursesPlatform.API
```

### Revertir última migración
```bash
dotnet ef migrations remove -p src/OnlineCoursesPlatform.Infrastructure -s src/OnlineCoursesPlatform.API
```

### Generar script SQL
```bash
dotnet ef migrations script -p src/OnlineCoursesPlatform.Infrastructure -s src/OnlineCoursesPlatform.API -o script.sql
```
