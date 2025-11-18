using System.ComponentModel;
using ModelContextProtocol.Server;

/// <summary>
/// Esta clase contiene herramientas relacionadas con la simulación de clima.
/// Demuestra cómo crear herramientas que pueden usar variables de entorno para configuración.
/// </summary>
internal class WeatherTools
{
    /// <summary>
    /// Método que simula el clima aleatorio para una ciudad dada.
    /// Utiliza una variable de entorno para personalizar las opciones de clima.
    /// </summary>
    /// <param name="city">El nombre de la ciudad para la que se quiere el clima.</param>
    /// <returns>Una cadena describiendo el clima aleatorio en la ciudad.</returns>
    [McpServerTool]
    [Description("Describe el clima aleatorio en la ciudad proporcionada.")]
    public string GetCityWeather(
        [Description("Nombre de la ciudad para la que devolver el clima")] string city)
    {
        // Intentamos leer la variable de entorno WEATHER_CHOICES.
        // Si no existe, usamos valores por defecto separados por comas.
        // Esto permite personalizar el comportamiento sin cambiar el código.
        var weather = Environment.GetEnvironmentVariable("WEATHER_CHOICES");
        if (string.IsNullOrWhiteSpace(weather))
        {
            weather = "templado,lluvioso,tormentoso";
        }

        // Dividimos la cadena en un array de opciones de clima.
        var weatherChoices = weather.Split(",");

        // Generamos un índice aleatorio para seleccionar una opción.
        var selectedWeatherIndex = Random.Shared.Next(0, weatherChoices.Length);

        // Devolvemos una cadena formateada con la ciudad y el clima seleccionado.
        return $"El clima en {city} es {weatherChoices[selectedWeatherIndex]}.";
    }
}