using Serilog;
using Xunit.Abstractions;

namespace MazeLibTests.Base
{
    public class UnittestSerilogBase
    {
        public UnittestSerilogBase(ITestOutputHelper output)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Xunit(output)
                .CreateLogger();
        }
    }
}