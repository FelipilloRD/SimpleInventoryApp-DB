// ============================================
// SimpleInventoryApp — Fase 3 (Base de Datos SQLite)
// Menú principal interactivo con persistencia real
// ============================================

using SimpleInventoryApp;

// Crear la instancia del gestor de inventario (inicializa SQLite automáticamente)
InventoryManager inventory = new InventoryManager();

// Variable de control del bucle principal
bool running = true;

Console.WriteLine("============================================");
Console.WriteLine("   Bienvenido a SimpleInventoryApp");
Console.WriteLine("   Sistema de Inventario de Productos");
Console.WriteLine("   Versión 3.0 — Persistencia en SQLite (EF Core)");
Console.WriteLine("============================================");
Console.WriteLine();

// Bucle principal del programa — solo termina cuando el usuario elige Salir
while (running)
{
    Console.WriteLine("------------- Menú Principal -------------");
    Console.WriteLine("1. Agregar producto");
    Console.WriteLine("2. Listar todos los productos");
    Console.WriteLine("3. Buscar producto");
    Console.WriteLine("4. Modificar producto");
    Console.WriteLine("5. Eliminar producto");
    Console.WriteLine("6. Salir");
    Console.WriteLine("------------------------------------------");
    Console.Write("Seleccione una opción: ");

    string input = Console.ReadLine();
    int option;

    // Validar que el usuario ingresó un número válido
    if (!int.TryParse(input, out option))
    {
        Console.WriteLine();
        Console.WriteLine(">>> Error: Debe ingresar un número válido. Intente de nuevo.");
        Console.WriteLine();
        continue;
    }

    Console.WriteLine();

    switch (option)
    {
        // =============================================
        // CASO 1: Agregar un nuevo producto
        // =============================================
        case 1:
            try
            {
                Console.WriteLine("--- Agregar Nuevo Producto ---");

                // Solicitar el nombre
                Console.Write("Ingrese el nombre del producto: ");
                string productName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(productName))
                {
                    Console.WriteLine(">>> Error: El nombre no puede estar vacío.");
                    Console.WriteLine();
                    break;
                }

                // Solicitar el precio
                Console.Write("Ingrese el precio del producto: ");
                string priceInput = Console.ReadLine();
                decimal productPrice;

                if (!decimal.TryParse(priceInput, out productPrice) || productPrice < 0)
                {
                    Console.WriteLine(">>> Error: El precio debe ser un número válido y no negativo.");
                    Console.WriteLine();
                    break;
                }

                // Solicitar el stock
                Console.Write("Ingrese la cantidad en stock: ");
                string stockInput = Console.ReadLine();
                int productStock;

                if (!int.TryParse(stockInput, out productStock) || productStock < 0)
                {
                    Console.WriteLine(">>> Error: La cantidad debe ser un número entero no negativo.");
                    Console.WriteLine();
                    break;
                }

                // Llamar al método de InventoryManager para agregar el producto a la BD
                inventory.AddProduct(productName, productPrice, productStock);
            }
            catch (Exception ex)
            {
                Console.WriteLine($">>> Error inesperado: {ex.Message}");
            }
            Console.WriteLine();
            break;

        // =============================================
        // CASO 2: Listar todos los productos
        // =============================================
        case 2:
            try
            {
                Console.WriteLine("--- Lista de Productos (desde SQLite) ---");

                // Obtener la lista completa de productos
                List<Product> allProducts = inventory.GetAllProducts();

                if (allProducts.Count == 0)
                {
                    Console.WriteLine(">>> No hay productos registrados en la base de datos.");
                    Console.WriteLine();
                    break;
                }

                // Mostrar encabezado de tabla
                Console.WriteLine(String.Format("{0,-6} {1,-25} {2,-15} {3,-10}", "ID", "Nombre", "Precio", "Stock"));
                Console.WriteLine(new string('-', 60));

                // Recorrer la lista y mostrar cada producto
                foreach (Product p in allProducts)
                {
                    Console.WriteLine(String.Format("{0,-6} {1,-25} {2,-15:C2} {3,-10}", p.Id, p.Name, p.Price, p.Stock));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($">>> Error inesperado: {ex.Message}");
            }
            Console.WriteLine();
            break;

        // =============================================
        // CASO 3: Buscar producto por ID o por nombre
        // =============================================
        case 3:
            try
            {
                Console.WriteLine("--- Buscar Producto ---");
                Console.WriteLine("1. Buscar por ID");
                Console.WriteLine("2. Buscar por nombre");
                Console.Write("Seleccione el tipo de búsqueda: ");

                string searchTypeInput = Console.ReadLine();
                int searchType;

                if (!int.TryParse(searchTypeInput, out searchType))
                {
                    Console.WriteLine(">>> Error: Debe ingresar 1 o 2.");
                    Console.WriteLine();
                    break;
                }

                if (searchType == 1)
                {
                    // Búsqueda por ID exacto
                    Console.Write("Ingrese el ID del producto: ");
                    string searchIdInput = Console.ReadLine();
                    int searchId;

                    if (!int.TryParse(searchIdInput, out searchId))
                    {
                        Console.WriteLine(">>> Error: Debe ingresar un número válido.");
                        Console.WriteLine();
                        break;
                    }

                    // Llamar al método SearchById de InventoryManager
                    Product found = inventory.SearchById(searchId);

                    if (found != null)
                    {
                        Console.WriteLine();
                        Console.WriteLine(">>> Producto encontrado en la base de datos:");
                        Console.WriteLine($"    ID:     {found.Id}");
                        Console.WriteLine($"    Nombre: {found.Name}");
                        Console.WriteLine($"    Precio: {found.Price:C2}");
                        Console.WriteLine($"    Stock:  {found.Stock}");
                    }
                    else
                    {
                        Console.WriteLine($">>> No se encontró ningún producto con el ID: {searchId}");
                    }
                }
                else if (searchType == 2)
                {
                    // Búsqueda por coincidencia de nombre
                    Console.Write("Ingrese el nombre (o parte del nombre): ");
                    string searchName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(searchName))
                    {
                        Console.WriteLine(">>> Error: Debe ingresar un texto para buscar.");
                        Console.WriteLine();
                        break;
                    }

                    // Llamar al método SearchByName de InventoryManager
                    List<Product> results = inventory.SearchByName(searchName);

                    Console.WriteLine();
                    Console.WriteLine(String.Format("{0,-6} {1,-25} {2,-15} {3,-10}", "ID", "Nombre", "Precio", "Stock"));
                    Console.WriteLine(new string('-', 60));

                    if (results.Count == 0)
                    {
                        Console.WriteLine($">>> No se encontraron productos que coincidan con: \"{searchName}\"");
                    }
                    else
                    {
                        foreach (Product p in results)
                        {
                            Console.WriteLine(String.Format("{0,-6} {1,-25} {2,-15:C2} {3,-10}", p.Id, p.Name, p.Price, p.Stock));
                        }
                    }
                }
                else
                {
                    Console.WriteLine(">>> Error: Opción de búsqueda no válida.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($">>> Error inesperado: {ex.Message}");
            }
            Console.WriteLine();
            break;

        // =============================================
        // CASO 4: Modificar un producto existente
        // =============================================
        case 4:
            try
            {
                Console.WriteLine("--- Modificar Producto ---");

                // Verificar si hay productos
                List<Product> productsToEdit = inventory.GetAllProducts();
                if (productsToEdit.Count == 0)
                {
                    Console.WriteLine(">>> No hay productos registrados para modificar.");
                    Console.WriteLine();
                    break;
                }

                // Mostrar lista de productos
                Console.WriteLine(String.Format("{0,-6} {1,-25} {2,-15} {3,-10}", "ID", "Nombre", "Precio", "Stock"));
                Console.WriteLine(new string('-', 60));
                foreach (Product p in productsToEdit)
                {
                    Console.WriteLine(String.Format("{0,-6} {1,-25} {2,-15:C2} {3,-10}", p.Id, p.Name, p.Price, p.Stock));
                }
                Console.WriteLine();

                // Solicitar el ID a modificar
                Console.Write("Ingrese el ID del producto a modificar: ");
                string editIdInput = Console.ReadLine();
                int editId;

                if (!int.TryParse(editIdInput, out editId))
                {
                    Console.WriteLine(">>> Error: Debe ingresar un número válido.");
                    Console.WriteLine();
                    break;
                }

                // Buscar el producto para mostrar sus datos actuales
                Product toEdit = inventory.SearchById(editId);

                if (toEdit == null)
                {
                    Console.WriteLine($">>> Error: No se encontró ningún producto con el ID: {editId}");
                    Console.WriteLine();
                    break;
                }

                // Mostrar datos actuales
                Console.WriteLine();
                Console.WriteLine(">>> Datos actuales del producto:");
                Console.WriteLine($"    Nombre: {toEdit.Name}");
                Console.WriteLine($"    Precio: {toEdit.Price:C2}");
                Console.WriteLine($"    Stock:  {toEdit.Stock}");
                Console.WriteLine();

                // Solicitar nuevos datos (Enter para mantener el actual)
                Console.Write($"Nuevo nombre (Enter para mantener \"{toEdit.Name}\"): ");
                string newName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newName))
                {
                    newName = toEdit.Name;
                }

                Console.Write($"Nuevo precio (Enter para mantener {toEdit.Price:C2}): ");
                string newPriceInput = Console.ReadLine();
                decimal newPrice = toEdit.Price;
                if (!string.IsNullOrWhiteSpace(newPriceInput))
                {
                    if (!decimal.TryParse(newPriceInput, out newPrice) || newPrice < 0)
                    {
                        Console.WriteLine(">>> Precio no válido. Se mantiene el precio anterior.");
                        newPrice = toEdit.Price;
                    }
                }

                Console.Write($"Nuevo stock (Enter para mantener {toEdit.Stock}): ");
                string newStockInput = Console.ReadLine();
                int newStock = toEdit.Stock;
                if (!string.IsNullOrWhiteSpace(newStockInput))
                {
                    if (!int.TryParse(newStockInput, out newStock) || newStock < 0)
                    {
                        Console.WriteLine(">>> Stock no válido. Se mantiene el stock anterior.");
                        newStock = toEdit.Stock;
                    }
                }

                // Llamar al método UpdateProduct de InventoryManager
                inventory.UpdateProduct(editId, newName, newPrice, newStock);
            }
            catch (Exception ex)
            {
                Console.WriteLine($">>> Error inesperado: {ex.Message}");
            }
            Console.WriteLine();
            break;

        // =============================================
        // CASO 5: Eliminar un producto
        // =============================================
        case 5:
            try
            {
                Console.WriteLine("--- Eliminar Producto ---");

                // Verificar si hay productos
                List<Product> productsToDelete = inventory.GetAllProducts();
                if (productsToDelete.Count == 0)
                {
                    Console.WriteLine(">>> No hay productos registrados para eliminar.");
                    Console.WriteLine();
                    break;
                }

                // Mostrar lista de productos
                Console.WriteLine(String.Format("{0,-6} {1,-25} {2,-15} {3,-10}", "ID", "Nombre", "Precio", "Stock"));
                Console.WriteLine(new string('-', 60));
                foreach (Product p in productsToDelete)
                {
                    Console.WriteLine(String.Format("{0,-6} {1,-25} {2,-15:C2} {3,-10}", p.Id, p.Name, p.Price, p.Stock));
                }
                Console.WriteLine();

                // Solicitar el ID a eliminar
                Console.Write("Ingrese el ID del producto a eliminar: ");
                string deleteIdInput = Console.ReadLine();
                int deleteId;

                if (!int.TryParse(deleteIdInput, out deleteId))
                {
                    Console.WriteLine(">>> Error: Debe ingresar un número válido.");
                    Console.WriteLine();
                    break;
                }

                // Buscar el producto para mostrar sus datos antes de confirmar
                Product toDelete = inventory.SearchById(deleteId);

                if (toDelete == null)
                {
                    Console.WriteLine($">>> Error: No se encontró ningún producto con el ID: {deleteId}");
                    Console.WriteLine();
                    break;
                }

                // Mostrar el producto seleccionado
                Console.WriteLine();
                Console.WriteLine(">>> Producto seleccionado para eliminar:");
                Console.WriteLine($"    ID:     {toDelete.Id}");
                Console.WriteLine($"    Nombre: {toDelete.Name}");
                Console.WriteLine($"    Precio: {toDelete.Price:C2}");
                Console.WriteLine($"    Stock:  {toDelete.Stock}");
                Console.WriteLine();

                // Pedir confirmación
                Console.WriteLine("¿Está seguro que desea eliminar este producto de la base de datos?");
                Console.WriteLine("1. Sí");
                Console.WriteLine("2. No");
                Console.Write("Seleccione una opción: ");

                string confirmInput = Console.ReadLine();
                int confirm;

                if (!int.TryParse(confirmInput, out confirm))
                {
                    Console.WriteLine(">>> Operación cancelada. Entrada no válida.");
                    Console.WriteLine();
                    break;
                }

                if (confirm == 1)
                {
                    // Llamar al método DeleteProduct de InventoryManager
                    inventory.DeleteProduct(deleteId);
                }
                else
                {
                    Console.WriteLine(">>> Operación cancelada. El producto no fue eliminado.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($">>> Error inesperado: {ex.Message}");
            }
            Console.WriteLine();
            break;

        // =============================================
        // CASO 6: Salir del programa
        // =============================================
        case 6:
            Console.WriteLine(">>> Gracias por usar SimpleInventoryApp. ¡Hasta luego!");
            running = false;
            break;

        // =============================================
        // OPCIÓN NO VÁLIDA
        // =============================================
        default:
            Console.WriteLine(">>> Error: Opción no válida. Por favor seleccione una opción del 1 al 6.");
            Console.WriteLine();
            break;
    }
}
