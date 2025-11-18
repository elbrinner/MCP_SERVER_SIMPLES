/// <summary>
/// Este es el punto de entrada principal de la aplicación del servidor MCP.
/// El servidor MCP (Model Context Protocol) permite que herramientas externas, como Copilot Chat,
/// interactúen con este programa a través de un protocolo estándar.
/// </summary>
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// Método Main: Es el punto de inicio de cualquier aplicación de consola en C#.
/// Aquí configuramos y ejecutamos el host de la aplicación.
/// </summary>
var builder = Host.CreateApplicationBuilder(args);

// Configuramos el logging para que todos los logs se envíen a stderr (salida de error estándar).
// Esto es importante porque stdout (salida estándar) se usa para los mensajes del protocolo MCP.
// Los logs incluyen información de depuración, errores, etc.
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);

// Agregamos HttpClient al contenedor de dependencias para que las herramientas puedan hacer llamadas HTTP a APIs externas.
builder.Services.AddHttpClient();

// Configuramos los servicios del servidor MCP:
// - AddMcpServer(): Registra el servidor MCP en el contenedor de dependencias.
// - WithStdioServerTransport(): Configura el transporte usando entrada/salida estándar (stdio),
//   que es cómo Copilot Chat se comunica con el servidor.
// - WithTools<...>(): Registra cada clase de herramientas para que estén disponibles en el servidor.
//   Cada WithTools agrega una clase que contiene métodos marcados con [McpServerTool].
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<RandomNumberTools>()  // Herramientas para números aleatorios
    .WithTools<WeatherTools>()       // Herramientas para clima
    .WithTools<MathTools>()          // Herramientas matemáticas
    .WithTools<ConversionTools>()    // Herramientas de conversión
    .WithTools<SecurityTools>()      // Herramientas de seguridad
    .WithTools<TextTools>()          // Herramientas de texto
    .WithTools<CountryTools>();      // Herramientas para países

// Construimos el host con todas las configuraciones anteriores y lo ejecutamos.
// El host maneja el ciclo de vida de la aplicación, incluyendo el inicio del servidor MCP.
// La aplicación se ejecutará hasta que se detenga (por ejemplo, con Ctrl+C).
await builder.Build().RunAsync();
