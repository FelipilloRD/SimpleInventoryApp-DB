// ============================================
// Contexto de Base de Datos para Entity Framework Core
// ============================================

using Microsoft.EntityFrameworkCore;

namespace SimpleInventoryApp;

// AppDbContext: gestiona la conexión a SQLite y mapea la tabla Products
public class AppDbContext : DbContext
{
    // Conjunto de entidades que representa la tabla Products en SQLite
    public DbSet<Product> Products { get; set; }

    // Configuración de la conexión a la base de datos local SQLite
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Se conecta al archivo local inventory.db en la raíz del proyecto
        optionsBuilder.UseSqlite("Data Source=inventory.db");
    }
}
