# Aprende+ | Modern Course Platform

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![React](https://img.shields.io/badge/React-18-61DAFB)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-336791)
![Architecture](https://img.shields.io/badge/Architecture-Clean-green)

**[English](#english) | [Español](#español)**

---

<a name="english"></a>
## English

### Overview
**Aprende+** is an enterprise-grade web application tailored for educational content management. It bridges a high-performance **.NET 8** backend with a dynamic **React** frontend, delivering a seamless experience for instructors to create, organize, and publish courses.

The project enforces **Clean Architecture**, ensuring separation of concerns, scalability, and testability.

### Key Features
*   **Course Lifecycle**: Create drafts, update content, publish/unpublish, and perform soft deletes.
*   **Drag & Drop Lessons**: Intuitive reordering of course lessons.
*   **Secure Authentication**: JWT-based auth with Role-Based Access Control (Admin/Teacher).
*   **Smart Dashboard**: Real-time aggregation of course statistics using optimized queries.
*   **Premium UI**: Glassmorphism effects, Lucide iconography, and responsive CSS variables.

### Architecture & Tech Stack

#### Backend (.NET 8)
Built on **Clean Architecture** principles:
1.  **Core (Domain)**: Contains entities (`Course`, `Lesson`), interfaces, and pure business logic services. No external dependencies.
2.  **Infrastructure**: Implementation of persistence (Entity Framework Core), repositories, and database configuration.
3.  **API (Presentation)**: RESTful controllers, DTOs, and Dependency Injection setup.

#### Frontend (React + Vite)
*   **State Management**: React Context API for Authentication.
*   **Routing**: React Router DOM v6 with protected routes.
*   **HTTP Client**: Axios with interceptors for automatic token injection and 401 handling.
*   **Debouncing**: Optimized search inputs to reduce API load.

### Complete Project Structure (Scaffolding)

```bash
Aprende+
├── backend/                             # .NET Solution
│   ├── src/
│   │   ├── CoursePlatform.API/          # Entry point & Controllers
│   │   │   ├── Controllers/             # Auth, Courses, Lessons endpoints
│   │   │   ├── Program.cs               # DI Container & Middleware config
│   │   │   └── appsettings.json         # DB Connection strings & JWT Settings
│   │   ├── CoursePlatform.Core/         # Domain Layer (Inner Circle)
│   │   │   ├── Entities/                # Course, Lesson, User models
│   │   │   ├── Interfaces/              # IRepository, IService contracts
│   │   │   └── Services/                # Business logic (validations, reordering)
│   │   └── CoursePlatform.Infrastructure/ # Data Layer (Outer Circle)
│   │       ├── Data/                    # ApplicationDbContext & SeedData
│   │       ├── Migrations/              # EF Core database migrations
│   │       └── Repositories/            # SQL implementations of logic
│   └── tests/                           # Unit & Integration Tests (xUnit)
└── Frontend/                            # React Application
    ├── public/                          # Static assets
    ├── src/
    │   ├── api/                         # Axios instance configuration
    │   ├── components/
    │   │   ├── auth/                    # Login & Register forms
    │   │   ├── courses/                 # CourseList, CourseItem components
    │   │   └── common/                  # Modals, Navbar, shared UI
    │   ├── context/                     # AuthContext (Global state)
    │   ├── pages/                       # Page-level components (Routes)
    │   ├── services/                    # API wrappers (authService, courseService)
    │   ├── index.css                    # Global styles & CSS Variables
    │   └── App.jsx                      # Route definitions
    ├── .env                             # Environment variables
    └── package.json                     # Dependencies
```

### Getting Started

#### 1. Database Setup
Ensure **PostgreSQL** is running. Update the connection string if needed in `backend/src/CoursePlatform.API/appsettings.json`.

#### 2. Backend Installation
```bash
cd backend
# Restore dependencies
dotnet restore

# Apply migrations & Seed Database
dotnet ef database update --project src/CoursePlatform.Infrastructure --startup-project src/CoursePlatform.API

# Run Server (http://localhost:5000)
dotnet run --project src/CoursePlatform.API
```

#### 3. Frontend Installation
```bash
cd Frontend
# Install dependencies
npm install

# Run Dev Server (http://localhost:5173)
npm run dev
```

#### 4. Docker Deployment
Quickly spin up the entire stack (Database, API, Frontend):

```bash
# Build and start services
sudo docker compose up --build
```
*   **Frontend**: http://localhost:3000
*   **API**: http://localhost:5000

### Test Credentials
The system comes pre-seeded with an **Administrator** account:
*   **Email**: `test@test.com`
*   **Password**: `Test12345*`

---

<a name="español"></a>
## Español

### Visión General
**Aprende+** es una plataforma web de nivel empresarial diseñada para la gestión de contenido educativo. Combina un backend de alto rendimiento en **.NET 8** con un frontend dinámico en **React**, ofreciendo una experiencia fluida para crear, organizar y publicar cursos.

El proyecto sigue estrictamente la **Clean Architecture** (Arquitectura Limpia), asegurando separación de responsabilidades, escalabilidad y facilidad de pruebas.

### Características Clave
*   **Ciclo de Vida del Curso**: Creación de borradores, edición, publicación/despublicación y eliminación lógica (soft delete).
*   **Lecciones Drag & Drop**: Lógica de reordenamiento inteligente para las lecciones.
*   **Autenticación Segura**: JWT con control de acceso basado en roles (Admin/Profesor).
*   **Dashboard Inteligente**: Agregación en tiempo real de estadísticas usando consultas optimizadas.
*   **UI Premium**: Efectos Glassmorphism, iconos Lucide y sistema de variables CSS responsivo.

### Arquitectura y Stack

#### Backend (.NET 8)
Construido sobre **Clean Architecture**:
1.  **Core (Dominio)**: Entidades (`Curso`, `Lección`), interfaces y lógica de negocio pura. Sin dependencias externas.
2.  **Infrastructure**: Implementación de persistencia (EF Core), repositorios y configuración de BD.
3.  **API (Presentación)**: Controladores RESTful, DTOs e inyección de dependencias.

#### Frontend (React + Vite)
*   **Estado**: React Context API para autenticación global.
*   **Ruteo**: React Router DOM v6 con rutas protegidas.
*   **Cliente HTTP**: Axios con interceptores para inyección de token y manejo de errores 401.

### Estructura Completa del Proyecto (Encarpetado)

```bash
Aprende+
├── backend/                             # Solución .NET
│   ├── src/
│   │   ├── CoursePlatform.API/          # Punto de entrada y Controladores
│   │   │   ├── Controllers/             # Endpoints de Auth, Cursos, Lecciones
│   │   │   ├── Program.cs               # Configuración de DI y Middleware
│   │   │   └── appsettings.json         # Cadenas de conexión y Config JWT
│   │   ├── CoursePlatform.Core/         # Capa de Dominio (Círculo Interior)
│   │   │   ├── Entities/                # Modelos de Curso, Lección, Usuario
│   │   │   ├── Interfaces/              # Contratos de Repositorios y Servicios
│   │   │   └── Services/                # Lógica de negocio (validaciones, orden)
│   │   └── CoursePlatform.Infrastructure/ # Capa de Datos (Círculo Exterior)
│   │       ├── Data/                    # ApplicationDbContext y SeedData
│   │       ├── Migrations/              # Migraciones de base de datos EF Core
│   │       └── Repositories/            # Implementación SQL de la lógica
│   └── tests/                           # Pruebas Unitarias e Integración (xUnit)
└── Frontend/                            # Aplicación React
    ├── public/                          # Activos estáticos
    ├── src/
    │   ├── api/                         # Configuración de instancia Axios
    │   ├── components/
    │   │   ├── auth/                    # Formularios de Login y Registro
    │   │   ├── courses/                 # Componentes de Lista e Ítem de Cursos
    │   │   └── common/                  # Modales, Navbar, UI compartida
    │   ├── context/                     # AuthContext (Estado global)
    │   ├── pages/                       # Componentes de Página (Rutas)
    │   ├── services/                    # Wrappers de API (authService, courseService)
    │   ├── index.css                    # Estilos globales y Variables CSS
    │   └── App.jsx                      # Definición de rutas
    ├── .env                             # Variables de entorno
    └── package.json                     # Dependencias
```

### Guía de Instalación

#### 1. Configuración de Base de Datos
Asegúrate de tener **PostgreSQL** ejecutándose. Ajusta la cadena de conexión en `backend/src/CoursePlatform.API/appsettings.json` si es necesario.

#### 2. Instalación del Backend
```bash
cd backend
# Restaurar dependencias
dotnet restore

# Aplicar migraciones y Sembrar Datos (Seed)
dotnet ef database update --project src/CoursePlatform.Infrastructure --startup-project src/CoursePlatform.API

# Iniciar Servidor (http://localhost:5000)
dotnet run --project src/CoursePlatform.API
```

#### 3. Instalación del Frontend
```bash
cd Frontend
# Instalar dependencias
npm install

# Iniciar Servidor de Desarrollo (http://localhost:5173)
npm run dev
```

#### 4. Despliegue con Docker
Levanta todo el entorno (BD, API, Frontend) rápidamente:

```bash
# Construir e iniciar servicios
sudo docker compose up --build
```
*   **Frontend**: http://localhost:3000
*   **API**: http://localhost:5000

### Credenciales de Prueba
El sistema incluye una cuenta de **Administrador** pre-configurada:
*   **Correo**: `test@test.com`
*   **Contraseña**: `Test12345*`
