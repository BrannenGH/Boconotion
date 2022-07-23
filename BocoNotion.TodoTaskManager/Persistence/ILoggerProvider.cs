namespace BocoNotion.TodoTaskManager.Persistence
{
    using System;
    using Serilog.Core;

    public interface ILoggerProvider
    {
        Logger GetLogger();
    }
}
