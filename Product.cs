// ============================================
// Clase que representa la entidad Product para la base de datos
// ============================================

namespace SimpleInventoryApp;

// Entidad Product correspondiente a la tabla Products en SQLite
public class Product
{
    // Clave primaria autoincremental en SQLite
    public int Id { get; set; }

    // Nombre del producto
    public string Name { get; set; }

    // Precio del producto
    public decimal Price { get; set; }

    // Cantidad disponible en stock
    public int Stock { get; set; }

    // Constructor sin parámetros requerido por Entity Framework Core
    public Product()
    {
    }

    // Constructor básico para crear productos fácilmente
    public Product(string name, decimal price, int stock)
    {
        Name = name;
        Price = price;
        Stock = stock;
    }
}
