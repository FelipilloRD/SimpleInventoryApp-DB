// ============================================
// Clase que maneja la lógica de negocio con persistencia en SQLite
// ============================================

namespace SimpleInventoryApp;

// InventoryManager: gestiona las operaciones CRUD directamente en la base de datos
public class InventoryManager
{
    // Contexto de Entity Framework Core para la base de datos SQLite
    private AppDbContext context = new AppDbContext();

    // Constructor: asegura que el archivo inventory.db y la tabla Products existan al iniciar
    public InventoryManager()
    {
        context.Database.EnsureCreated();
    }

    // =============================================
    // Agregar un nuevo producto a la base de datos
    // =============================================
    public void AddProduct(string name, decimal price, int stock)
    {
        // Crear la entidad Product
        Product newProduct = new Product(name, price, stock);

        // Agregar al contexto y guardar cambios en SQLite
        context.Products.Add(newProduct);
        context.SaveChanges();

        Console.WriteLine($">>> Producto agregado exitosamente con ID: {newProduct.Id} (guardado en SQLite)");
    }

    // =============================================
    // Retornar la lista completa de productos desde SQLite
    // =============================================
    public List<Product> GetAllProducts()
    {
        return context.Products.ToList();
    }

    // =============================================
    // Buscar un producto por ID exacto en la base de datos
    // =============================================
    public Product SearchById(int id)
    {
        // Buscar directamente por llave primaria
        return context.Products.Find(id);
    }

    // =============================================
    // Buscar productos cuyo nombre contenga el texto ingresado
    // =============================================
    public List<Product> SearchByName(string name)
    {
        List<Product> results = new List<Product>();

        // Recorrer los productos de la base de datos para buscar coincidencias
        foreach (Product product in context.Products.ToList())
        {
            if (product.Name.ToLower().Contains(name.ToLower()))
            {
                results.Add(product);
            }
        }

        return results;
    }

    // =============================================
    // Modificar los datos de un producto en SQLite
    // =============================================
    public bool UpdateProduct(int id, string newName, decimal newPrice, int newStock)
    {
        // Buscar el producto en la base de datos
        Product product = context.Products.Find(id);

        if (product != null)
        {
            // Actualizar campos
            product.Name = newName;
            product.Price = newPrice;
            product.Stock = newStock;

            // Guardar cambios en SQLite
            context.Products.Update(product);
            context.SaveChanges();

            Console.WriteLine(">>> Producto modificado exitosamente en la base de datos.");
            return true;
        }

        return false;
    }

    // =============================================
    // Eliminar un producto de SQLite por su ID
    // =============================================
    public bool DeleteProduct(int id)
    {
        // Buscar el producto a eliminar
        Product product = context.Products.Find(id);

        if (product != null)
        {
            // Remover y persistir en la base de datos
            context.Products.Remove(product);
            context.SaveChanges();

            Console.WriteLine(">>> Producto eliminado exitosamente de la base de datos.");
            return true;
        }

        return false;
    }
}
