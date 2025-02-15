﻿using System;
using System.Windows;

namespace UECrashReporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string s_AppName = Properties.Resources.AppName;

        private static string s_GameNameWildcard = "$GameName";

        public MainWindow()
        {
            InitializeComponent();

            Explanation.Text = Explanation.Text.Replace(s_GameNameWildcard, s_AppName != string.Empty ? s_AppName : "The game");
            Title = Title.Replace(s_GameNameWildcard, s_AppName != string.Empty ? s_AppName : "").Trim(' ');
        }

        private async void SendAndClose_Click(object sender, RoutedEventArgs e)
        {
            btnSendAndClose.IsEnabled = false;

            var ci = CrashInfo.GetCrashInfo(IncludeCrashLog.IsEnabled);

            await Discord.SendToDiscord(ci, CrashDescription.Text, s_AppName);

            Close();
        }

        private void CloseWithoutSending_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
