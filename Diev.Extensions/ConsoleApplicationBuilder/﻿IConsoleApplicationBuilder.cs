// https://blog.peterritchie.com/posts/announcing-consoleapplicationbuilder
// https://github.com/peteraritchie/ConsoleApplicationBuilder/

using Microsoft.Extensions.Hosting;

namespace Diev.Extensions.ConsoleApplicationBuilder;

/// <summary>
/// Represents a console application builder which helps manage configuration, logging, lifetime, and more.
/// </summary>
public interface IConsoleApplicationBuilder : IHostApplicationBuilder
{

    /// <summary>
    /// Builds the host. This method can only be called once.
    /// </summary>
    /// <returns>An initialized <see cref="T"/>.</returns>
    T Build<T>() where T : class;
}
