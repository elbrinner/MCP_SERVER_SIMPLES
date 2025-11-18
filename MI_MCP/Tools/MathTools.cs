using System.ComponentModel;
using ModelContextProtocol.Server;

/// <summary>
/// Esta clase contiene herramientas para realizar operaciones matemáticas básicas.
/// Demuestra el uso de switch expressions y manejo de errores en herramientas.
/// </summary>
internal class MathTools
{
    /// <summary>
    /// Método que realiza una operación matemática entre dos números basada en una cadena de operación.
    /// Utiliza switch expressions (introducidos en C# 8) para seleccionar la operación.
    /// </summary>
    /// <param name="num1">El primer número de la operación.</param>
    /// <param name="num2">El segundo número de la operación.</param>
    /// <param name="operacion">La operación a realizar: "suma", "resta", "multiplicacion", "division".</param>
    /// <returns>El resultado de la operación como cadena formateada, o un mensaje de error.</returns>
    [McpServerTool]
    [Description("Realiza operaciones matemáticas básicas entre dos números.")]
    public string Calcular(
        [Description("Primer número")] double num1,
        [Description("Segundo número")] double num2,
        [Description("Operación a realizar: suma, resta, multiplicacion, division")] string operacion)
    {
        // Verificamos si la operación es nula o vacía
        if (string.IsNullOrWhiteSpace(operacion))
        {
            return "Error: La operación no puede estar vacía. Use: suma, resta, multiplicacion, division.";
        }

        // Usamos switch expression para seleccionar la operación basada en la cadena.
        // Convertimos a minúsculas para hacer la comparación case-insensitive.
        double resultado;
        try
        {
            resultado = operacion.ToLower() switch
            {
                "suma" => num1 + num2,  // Suma simple
                "resta" => num1 - num2, // Resta
                "multiplicacion" => num1 * num2, // Multiplicación
                "division" => num2 != 0 ? num1 / num2 : throw new ArgumentException("No se puede dividir por cero."), // División con verificación
                _ => throw new ArgumentException("Operación no válida. Use: suma, resta, multiplicacion, division.") // Operación desconocida
            };
        }
        catch (ArgumentException ex)
        {
            return $"Error: {ex.Message}";
        }

        // Devolvemos el resultado formateado como cadena
        return $"El resultado de {num1} {operacion.ToLower()} {num2} es {resultado}";
    }
}