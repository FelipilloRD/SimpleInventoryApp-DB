# SimpleInventoryApp — Persistencia con Base de Datos SQLite (EF Core)

## Descripción

**SimpleInventoryApp v3.0** es la evolución final del sistema de inventario. Incorpora **persistencia real de datos** mediante un motor de base de datos **SQLite** utilizando **Entity Framework Core**, garantizando que los datos no se pierdan al cerrar el programa.

## Estructura del Proyecto

```
SimpleInventoryApp-DB/
├── AppDbContext.cs      → Contexto de Entity Framework Core (conexión SQLite y DbSet)
├── Product.cs           → Entidad que mapea la tabla 'Products'
├── InventoryManager.cs  → Lógica CRUD con persistencia en SQLite
├── Program.cs           → Menú principal interactivo (while + switch)
├── inventory.db         → Archivo de base de datos SQLite (se genera automáticamente)
└── README.md            → Documentación del proyecto
```

## Base de Datos y Modelo de Datos

### Tabla `Products` en SQLite

La tabla se crea automáticamente al iniciar la aplicación mediante `context.Database.EnsureCreated()` en el archivo local `inventory.db`:

| Campo | Tipo SQLite | Tipo C# | Clave / Restricciones |
|-------|-------------|---------|-----------------------|
| `Id` | `INTEGER` | `int` | Primary Key, Autoincremental |
| `Name` | `TEXT` | `string` | Nombre del producto |
| `Price` | `TEXT` / `NUMERIC` | `decimal` | Precio del producto |
| `Stock` | `INTEGER` | `int` | Cantidad en almacén |

### Componentes Clave

1. **`AppDbContext.cs`**:
   - Hereda de `DbContext`.
   - Expone `DbSet<Product> Products`.
   - Configura la cadena de conexión mediante `optionsBuilder.UseSqlite("Data Source=inventory.db")`.

2. **`InventoryManager.cs`**:
   - Ejecuta `context.Database.EnsureCreated()` al inicializarse.
   - Realiza operaciones CRUD directas sobre la base de datos:
     - `context.Products.Add(product)` + `SaveChanges()` para inserciones.
     - `context.Products.ToList()` para lecturas completas.
     - `context.Products.Find(id)` para búsquedas por clave primaria.
     - `context.Products.Update(product)` + `SaveChanges()` para modificaciones.
     - `context.Products.Remove(product)` + `SaveChanges()` para eliminaciones.

## Funcionalidades del Sistema

| Opción | Acción | Detalle |
|--------|--------|---------|
| 1 | **Agregar producto** | Inserta el registro en la base de datos `inventory.db` |
| 2 | **Listar productos** | Consulta y muestra todos los registros de la tabla `Products` |
| 3 | **Buscar producto** | Búsqueda por ID exacto (`Find`) o por coincidencia de nombre |
| 4 | **Modificar producto** | Actualiza nombre, precio y stock directamente en SQLite |
| 5 | **Eliminar producto** | Borra el registro tras confirmación del usuario (1. Sí / 2. No) |
| 6 | **Salir** | Cierra la aplicación (los datos permanecen guardados en disco) |

## Requisitos y Paquetes

- [.NET 8 SDK](https://dotnet.microsoft.com/download) o superior
- Paquete NuGet: `Microsoft.EntityFrameworkCore.Sqlite`

## Cómo Ejecutar

```bash
cd SimpleInventoryApp-DB
dotnet run
```

Al ejecutarse por primera vez, se creará automáticamente el archivo `inventory.db` en la carpeta del proyecto.

## Evolución del Proyecto

1. **Fase 1 (Estructurada)**: Datos en colecciones en memoria (`List`, `Dictionary`), sin clases ni funciones.
2. **Fase 2 (POO)**: Encapsulación en clases (`Product`, `InventoryManager`), código limpio y modular.
3. **Fase 3 (Base de Datos)**: Integración con base de datos SQLite y Entity Framework Core, persistencia permanente.

## Autor

Felipillo LF - Proyecto Programacion
