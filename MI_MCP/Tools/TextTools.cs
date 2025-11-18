using System.ComponentModel;
using ModelContextProtocol.Server;

/// <summary>
/// Esta clase contiene herramientas para manipulación y análisis de texto.
/// Incluye funciones como contar palabras, que son útiles para procesamiento de texto.
/// </summary>
internal class TextTools
{
    /// <summary>
    /// Método que cuenta el número de palabras en un texto proporcionado.
    /// Divide el texto por espacios, tabuladores, saltos de línea y elimina entradas vacías.
    /// </summary>
    /// <param name="texto">El texto del que se van a contar las palabras.</param>
    /// <returns>Un mensaje con el número de palabras en el texto.</returns>
    [McpServerTool]
    [Description("Cuenta el número de palabras en un texto proporcionado.")]
    public string ContarPalabras(
        [Description("Texto del que contar las palabras")] string texto)
    {
        // Verificamos si el texto es nulo o está vacío (después de quitar espacios).
        if (string.IsNullOrWhiteSpace(texto))
            return "El texto está vacío o solo contiene espacios."; // Si es así, no hay palabras que contar.

        // Dividimos el texto por caracteres de espacio: espacio, tabulador, nueva línea, retorno de carro.
        // StringSplitOptions.RemoveEmptyEntries elimina las entradas vacías resultantes de múltiples espacios.
        // Finalmente, contamos los elementos del array resultante.
        int count = texto.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        return $"El texto contiene {count} palabra{(count != 1 ? "s" : "")}.";
    }
}