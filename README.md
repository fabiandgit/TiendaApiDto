# TiendaApiDto
# Tienda API Backend

Este proyecto es una API REST construida con ASP.NET Core y Entity Framework Core para gestionar productos, empleados y ventas de una tienda.

## 🚀 Tecnologías

- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- Visual Studio

## 📦 Funcionalidades

- CRUD de productos y empleados
- Registro de ventas con relación a producto y empleado
- DTOs para respuestas limpias
- Validaciones automáticas
- Relación uno a muchos entre entidades

## 📄 Endpoints principales

- `GET /api/Venta` → Lista todas las ventas con nombres de producto y empleado
- `GET /api/Venta/{id}` → Consulta una venta por ID
- `POST /api/Venta` → Registra una nueva venta

## 🛠️ Cómo ejecutar

1. Clona el repositorio
2. Abre el proyecto en Visual Studio
3. Configura la cadena de conexión en `appsettings.json`
4. Ejecuta las migraciones con `dotnet ef database update`
5. Ejecuta el proyecto y prueba con Postman

## 📌 Próximos pasos

- Filtros por fecha
- Exportación a Excel/PDF
- Autenticación con JWT
- Integración con frontend

---
