using RecipeTimeConsoleApp.Agents;

Console.WriteLine("Assistente de receitas");
Console.WriteLine();

Console.Write("Ingredientes (separe por vírgulas): ");
var ingredientes = Console.ReadLine();

Console.WriteLine();
Console.WriteLine("Consultando sua agenda e buscando receitas...");
Console.WriteLine();

var agent = new RecipeAgent();
await agent.SugerirReceitasAsync(ingredientes!);
