// https://blog.peterritchie.com/posts/announcing-consoleapplicationbuilder
// https://github.com/peteraritchie/ConsoleApplicationBuilder/

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Diev.Extensions.ConsoleApplicationBuilder;

internal class ApplicationEnvironment : IHostEnvironment
{
    public string ApplicationName { get; set; } = string.Empty;
    public string EnvironmentName { get; set; } = string.Empty;
    public string ContentRootPath { get; set; } = string.Empty;
    public required IFileProvider ContentRootFileProvider { get; set; }
}
