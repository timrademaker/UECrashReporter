using System;
using System.Windows;


namespace TimsCrashReporter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // True if there's crash information
            bool shouldStart = false;
            // True if the application was triggered by a crash (rather than an assert)
            bool triggeredByCrash = true;
            // The name of the game that crashed
            string appName = String.Empty;

            foreach(var arg in e.Args)
            {
                if(arg.Contains("-CrashGUID"))
                {
                    shouldStart = true;
                }

                if(arg.Contains("-Unattended"))
                {
                    triggeredByCrash = false;
                }

                if(arg.Contains("-Appname="))
                {
                    appName = arg.Replace("-Appname=UE4-", "");
                    TimsCrashReporter.MainWindow.s_AppName = appName;
                }
            }

            if(!shouldStart || !triggeredByCrash)
            {
                Shutdown();
            }
        }
    }
}
