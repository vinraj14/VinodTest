
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace ZooAssignment.Test
{
    public class LoggerTest
    {
        public LoggerTest()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("Log\\logs-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        [Fact]
        public void LoggerTest_writeLogInformation()
        {
            // Arrange
            // Act
            // Assert
            Log.Information("Hello, world!");
        }
    }
}



