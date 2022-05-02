using Point_of_sale_terminal;

var productManager = new InMemoryProductManager();
var ui = new ConsoleUI();
var app = new PointOfSaleApp(ui, productManager);
app.Run();