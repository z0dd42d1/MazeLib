using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;
using Xunit;

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