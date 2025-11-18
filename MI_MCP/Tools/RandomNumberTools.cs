using System.ComponentModel;
using ModelContextProtocol.Server;

/// <summary>
/// Herramientas MCP de ejemplo para fines de demostración.
/// Estas herramientas pueden ser invocadas por clientes MCP para realizar varias operaciones.
/// </summary>
internal class RandomNumberTools
{
    [McpServerTool]
    [Description("Genera un número aleatorio entre los valores mínimo y máximo especificados.")]
    public int GetRandomNumber(
        [Description("Valor mínimo (inclusivo)")] int min = 0,
        [Description("Valor máximo (exclusivo)")] int max = 100)
    {
        return Random.Shared.Next(min, max);
    }
}
