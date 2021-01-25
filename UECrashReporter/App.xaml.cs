using System;
using System.Windows;


namespace UECrashReporter
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

            foreach(var arg in e.Args)
            {
                if(arg.Contains("-CrashGUID"))
                {
                    shouldStart = true;
                }
                else if(arg.Contains("-Unattended"))
                {
                    triggeredByCrash = false;
                }
                else if(arg.Contains("-AppName="))
                {
                    if (UECrashReporter.MainWindow.s_AppName == string.Empty)
                    {
                        UECrashReporter.MainWindow.s_AppName = arg.Replace("-AppName=UE4-", "");
                    }

                    if (CrashInfo.s_AppName == string.Empty)
                    {
                        CrashInfo.s_AppName = arg.Replace("-AppName=UE4-", "");
                    }
                }
                else if(arg.Contains("/Saved/Crashes/"))
                {
                    CrashInfo.s_CrashReportLocation = arg;
                }
            }

            if (!shouldStart || !triggeredByCrash)
            {
                Shutdown();
            }
        }
    }
}
