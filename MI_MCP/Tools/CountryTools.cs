using System.ComponentModel;
using System.Net.Http.Json;
using ModelContextProtocol.Server;

/// <summary>
/// Esta clase contiene herramientas para consultar información de países usando la API REST Countries.
/// Demuestra cómo hacer llamadas HTTP asíncronas en herramientas MCP.
/// Utiliza HttpClient inyectado por el contenedor de dependencias.
/// </summary>
internal class CountryTools
{
    // Campo privado para el cliente HTTP, inyectado en el constructor.
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Constructor que recibe HttpClient a través de inyección de dependencias.
    /// Esto permite que el framework gestione la configuración del cliente HTTP.
    /// </summary>
    /// <param name="httpClient">Instancia de HttpClient configurada.</param>
    public CountryTools(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Método asíncrono que obtiene la capital de un país usando la API REST Countries.
    /// Hace una llamada GET al endpoint /v3.1/name/{countryName} y extrae la capital.
    /// </summary>
    /// <param name="countryName">Nombre del país en inglés.</param>
    /// <returns>Cadena con la capital del país o mensaje de error.</returns>
    [McpServerTool]
    [Description("Obtiene la capital de un país especificado.")]
    public async Task<string> GetCapital(
        [Description("Nombre del país en inglés")] string countryName)
    {
        try
        {
            // Realizamos una llamada HTTP GET asíncrona a la API.
            // GetFromJsonAsync deserializa automáticamente la respuesta JSON a una lista de CountryInfo.
            var response = await _httpClient.GetFromJsonAsync<List<CountryInfo>>($"https://restcountries.com/v3.1/name/{countryName}");

            // Verificamos si la respuesta es nula o vacía (país no encontrado).
            if (response == null || response.Count == 0)
            {
                return $"No se encontró información para el país '{countryName}'.";
            }

            // Tomamos el primer resultado (puede haber múltiples coincidencias).
            var country = response[0];

            // Verificamos que el nombre del país esté disponible.
            if (country.Name?.Common == null)
            {
                return $"No se pudo obtener el nombre del país '{countryName}'.";
            }

            // Extraemos la capital (es un array, tomamos el primero o un mensaje por defecto).
            var capital = country.Capital?.FirstOrDefault() ?? "Capital no disponible";

            // Devolvemos una cadena formateada con el resultado.
            return $"La capital de {country.Name.Common} es {capital}.";
        }
        catch (Exception ex)
        {
            // En caso de error (red, API caída, etc.), devolvemos un mensaje amigable.
            return $"Error al consultar la API: {ex.Message}";
        }
    }

    /// <summary>
    /// Método asíncrono que obtiene la lista de países de una región específica.
    /// Usa el endpoint /v3.1/region/{region} de la API.
    /// </summary>
    /// <param name="region">Nombre de la región (e.g., Europe, Asia).</param>
    /// <returns>Lista de países en la región o mensaje de error.</returns>
    [McpServerTool]
    [Description("Lista los países de una región específica.")]
    public async Task<string> GetCountriesByRegion(
        [Description("Región (e.g., Europe, Asia, Africa, Americas, Oceania)")] string region)
    {
        try
        {
            // Llamada a la API para obtener países por región.
            var response = await _httpClient.GetFromJsonAsync<List<CountryInfo>>($"https://restcountries.com/v3.1/region/{region}");

            // Verificación de respuesta vacía.
            if (response == null || response.Count == 0)
            {
                return $"No se encontraron países en la región '{region}'.";
            }

            // Filtramos países con nombre válido y extraemos los nombres comunes.
            var countries = response.Where(c => c.Name?.Common != null).Select(c => c.Name!.Common).ToList();

            // Devolvemos la lista formateada.
            return $"Países en {region}: {string.Join(", ", countries)}";
        }
        catch (Exception ex)
        {
            return $"Error al consultar la API: {ex.Message}";
        }
    }

    /// <summary>
    /// Método asíncrono que obtiene las capitales de todos los países en una región específica.
    /// Combina la lista de países con sus capitales respectivas.
    /// </summary>
    /// <param name="region">Nombre de la región (e.g., Europe, Asia).</param>
    /// <returns>Lista de capitales en la región o mensaje de error.</returns>
    [McpServerTool]
    [Description("Obtiene las capitales de los países en una región específica.")]
    public async Task<string> GetCapitalsByRegion(
        [Description("Región (e.g., Europe, Asia, Africa, Americas, Oceania)")] string region)
    {
        try
        {
            // Primero, obtenemos la lista de países en la región.
            var countriesResponse = await _httpClient.GetFromJsonAsync<List<CountryInfo>>($"https://restcountries.com/v3.1/region/{region}");

            if (countriesResponse == null || countriesResponse.Count == 0)
            {
                return $"No se encontraron países en la región '{region}'.";
            }

            // Para cada país, necesitamos obtener su capital.
            // Como la API de región no incluye capitales directamente, hacemos llamadas individuales.
            // Nota: En un escenario real, podríamos optimizar con una sola llamada o cache.
            var capitals = new List<string>();
            foreach (var country in countriesResponse)
            {
                if (country.Name?.Common != null)
                {
                    // Intentamos obtener la capital del país actual.
                    var capital = country.Capital?.FirstOrDefault();
                    if (!string.IsNullOrEmpty(capital))
                    {
                        capitals.Add($"{country.Name.Common}: {capital}");
                    }
                }
            }

            if (capitals.Count == 0)
            {
                return $"No se pudieron obtener capitales para la región '{region}'.";
            }

            // Devolvemos la lista formateada.
            return $"Capitales en {region}:\n" + string.Join("\n", capitals);
        }
        catch (Exception ex)
        {
            return $"Error al consultar la API: {ex.Message}";
        }
    }

    // Clases auxiliares para deserializar la respuesta JSON de la API.
    // Estas clases mapean la estructura del JSON devuelto por REST Countries.

    /// <summary>
    /// Clase que representa la información de un país.
    /// </summary>
    private class CountryInfo
    {
        public NameInfo? Name { get; set; } // Información del nombre
        public List<string>? Capital { get; set; } // Lista de capitales (generalmente una)
        public long Population { get; set; } // Población
        public double Area { get; set; } // Área en km²
        public Dictionary<string, CurrencyInfo>? Currencies { get; set; } // Monedas
        public Dictionary<string, string>? Languages { get; set; } // Idiomas
    }

    /// <summary>
    /// Clase para la información del nombre del país.
    /// </summary>
    private class NameInfo
    {
        public string? Common { get; set; } // Nombre común del país
    }

    /// <summary>
    /// Clase para la información de una moneda.
    /// </summary>
    private class CurrencyInfo
    {
        public string? Name { get; set; } // Nombre de la moneda
    }
}