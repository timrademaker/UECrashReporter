using System;
using System.Windows;

namespace TimsCrashReporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string s_AppName = String.Empty;

        private static string s_GameNameWildcard = "$GameName";

        public MainWindow()
        {
            InitializeComponent();
            
            Explanation.Text = Explanation.Text.Replace(s_GameNameWildcard, s_AppName != String.Empty ? s_AppName : "The game");
            this.Title= this.Title.Replace(s_GameNameWildcard, s_AppName != String.Empty ? s_AppName : "Tim's");
        }

        private void SendAndClose_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement
        }

        private void CloseWithoutSending_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
