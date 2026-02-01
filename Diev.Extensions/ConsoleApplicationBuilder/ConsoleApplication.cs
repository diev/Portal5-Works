// https://blog.peterritchie.com/posts/announcing-consoleapplicationbuilder
// https://github.com/peteraritchie/ConsoleApplicationBuilder/

namespace Diev.Extensions.ConsoleApplicationBuilder;

public class ConsoleApplication
{
    public static IConsoleApplicationBuilder CreateBuilder(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        return CreateBuilder(new ConsoleApplicationBuilderSettings { Args = args });
    }

    public static IConsoleApplicationBuilder CreateBuilder(ConsoleApplicationBuilderSettings settings)
    {
        return new DefaultConsoleApplicationBuilder(settings);
    }
}
