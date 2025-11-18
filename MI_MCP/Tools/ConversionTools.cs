using System.ComponentModel;
using ModelContextProtocol.Server;

/// <summary>
/// Esta clase contiene herramientas para conversiones de unidades.
/// Actualmente incluye conversión de temperatura, pero puede expandirse a otras unidades.
/// </summary>
internal class ConversionTools
{
    /// <summary>
    /// Método que convierte temperatura de grados Celsius a Fahrenheit.
    /// Utiliza la fórmula estándar: F = (C * 9/5) + 32.
    /// </summary>
    /// <param name="celsius">La temperatura en grados Celsius a convertir.</param>
    /// <returns>Un mensaje con la temperatura equivalente en grados Fahrenheit.</returns>
    [McpServerTool]
    [Description("Convierte temperatura de Celsius a Fahrenheit.")]
    public string ConvertirTemperatura(
        [Description("Temperatura en grados Celsius")] double celsius)
    {
        // Aplicamos la fórmula de conversión de Celsius a Fahrenheit.
        // Primero multiplicamos por 9/5, luego sumamos 32.
        double fahrenheit = (celsius * 9 / 5) + 32;
        return $"{celsius} grados Celsius equivalen a {fahrenheit} grados Fahrenheit.";
    }
}