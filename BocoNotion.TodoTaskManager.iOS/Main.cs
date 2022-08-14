using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Serilog;
using UIKit;

namespace BocoNotion.TodoTaskManager.iOS
{
    public class Application
    {
        private static ILogger logger = null;

        public static ILogger Logger
        {
            private get => Application.logger;
            set => Application.logger = value;
        }
        
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            try
            {
                // if you want to use a different Application Delegate class from "AppDelegate"
                // you can specify it here.
                UIApplication.Main(args, null, "AppDelegate");
            }
            catch (Exception e)
            {
                Logger?.Fatal(e, "Application crashed.");
                throw e;
            }
        }
    }
}
