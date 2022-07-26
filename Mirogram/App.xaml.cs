using Mirogram.Networking;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Mirogram
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static Action EmptyDelegate = delegate () { };

        public static MainWindow ProgramWindows { get; set; }
        public static UI.Pages.Main.Register RegisterPage { get; set; }
        public static UI.Pages.Main.Login LoginPage { get; set; }
        public static UI.Pages.Main.ChatPanel ChatPanelPage { get; set; }
        public static UI.Pages.Inner.FriendRequests FriendRequestsPage { get; set; }
        public static UI.Pages.Inner.Search SearchPage { get; set; }
        public static NetworkManager NetworkManager { get; set; } = new NetworkManager();
        

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("fa-IR");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("fa-IR");

            
        }

        private void Application_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            RegisterPage = new UI.Pages.Main.Register();
            ChatPanelPage = new UI.Pages.Main.ChatPanel();
            ChatPanelPage = new UI.Pages.Main.ChatPanel();
            FriendRequestsPage = new UI.Pages.Inner.FriendRequests();
            SearchPage = new UI.Pages.Inner.Search();

        }
    }


    public class StackPanelToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var stackPanel = value as StackPanel;
            if (stackPanel.Children.Count > 0) return Visibility.Collapsed;
            else return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
