using System.ComponentModel;
using ModelContextProtocol.Server;

/// <summary>
/// Esta clase contiene herramientas relacionadas con la seguridad.
/// Incluye generación de contraseñas aleatorias, que es útil para crear credenciales seguras.
/// </summary>
internal class SecurityTools
{
    /// <summary>
    /// Método que genera una contraseña aleatoria de longitud especificada.
    /// Utiliza caracteres alfanuméricos y símbolos para crear contraseñas fuertes.
    /// </summary>
    /// <param name="longitud">La longitud deseada de la contraseña. Por defecto es 8.</param>
    /// <returns>Una cadena con la contraseña generada aleatoriamente.</returns>
    [McpServerTool]
    [Description("Genera una contraseña aleatoria de longitud especificada.")]
    public string GenerarContrasena(
        [Description("Longitud de la contraseña")] int longitud = 8)
    {
        // Definimos el conjunto de caracteres permitidos: letras mayúsculas, minúsculas, números y símbolos.
        const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";

        // Creamos un array de caracteres para almacenar la contraseña.
        var resultado = new char[longitud];

        // Para cada posición en la contraseña, seleccionamos un carácter aleatorio del conjunto.
        for (int i = 0; i < longitud; i++)
        {
            resultado[i] = caracteres[Random.Shared.Next(caracteres.Length)];
        }

        // Convertimos el array de caracteres a una cadena y la devolvemos.
        return new string(resultado);
    }
}