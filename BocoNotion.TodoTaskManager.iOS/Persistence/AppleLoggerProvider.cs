using System;
using BocoNotion.TodoTaskManager.Persistence;
using Serilog;
using Serilog.Core;

namespace BocoNotion.TodoTaskManager.iOS.Persistence
{
    public class AppleLoggerProvider: ILoggerProvider
    {
        public AppleLoggerProvider()
        {
        }

        public Logger GetLogger()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.NSLog()
                .CreateLogger();
        }
    }
}
