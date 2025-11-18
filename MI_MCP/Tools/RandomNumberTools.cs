using System.ComponentModel;
using ModelContextProtocol.Server;

/// <summary>
/// Esta clase contiene herramientas relacionadas con la generación de números aleatorios.
/// Es un ejemplo simple de cómo crear herramientas que pueden ser invocadas por clientes MCP.
/// Las herramientas se marcan con el atributo [McpServerTool] para que el servidor las reconozca.
/// </summary>
internal class RandomNumberTools
{
    /// <summary>
    /// Método que genera un número aleatorio dentro de un rango especificado.
    /// Este es el único método en esta clase y demuestra cómo usar parámetros con descripciones.
    /// </summary>
    /// <param name="min">El valor mínimo del rango (inclusivo). Por defecto es 0.</param>
    /// <param name="max">El valor máximo del rango (exclusivo). Por defecto es 100.</param>
    /// <returns>Un mensaje con el número entero aleatorio generado.</returns>
    [McpServerTool]
    [Description("Genera un número aleatorio entre los valores mínimo y máximo especificados.")]
    public string GetRandomNumber(
        [Description("Valor mínimo (inclusivo)")] int min = 0,
        [Description("Valor máximo (exclusivo)")] int max = 100)
    {
        // Random.Shared es una instancia compartida de Random introducida en .NET 6,
        // que es thread-safe y eficiente para usos concurrentes.
        // Next(min, max) genera un número aleatorio >= min y < max.
        int numero = Random.Shared.Next(min, max);
        return $"Número aleatorio generado: {numero}";
    }
}
