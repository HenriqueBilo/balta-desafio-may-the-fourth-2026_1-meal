using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace RecipeTimeConsoleApp.Agents;

public class RecipeAgent
{
    private readonly AIAgent _agent;

    public RecipeAgent()
    {
        var chatClient = new OllamaChatClient(
            new Uri("http://localhost:11434"),
            modelId: "llama3.2");

        _agent = chatClient.AsAIAgent(
            instructions: File.ReadAllText("agentes/RecipeAgent.md"),
            tools: [AIFunctionFactory.Create(CalendarTool.GetFreeTimeSlots)]);
    }

    public async Task SugerirReceitasAsync(string ingredientes)
    {
        var mensagem = $"Ingredientes disponíveis: {ingredientes}";
        Console.WriteLine(await _agent.RunAsync(mensagem));
    }
}
