using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mirogram.Networking;
using MirogramServer.Network;
using MirogramServer.Network.RequestPackets;
using Mirogram.UI.Pages.Inner;
using Mirogram.UI.Controls;

namespace Mirogram.UI.Pages.Main
{
    /// <summary>
    /// Interaction logic for ChatPanel.xaml
    /// </summary>
    public partial class ChatPanel : Page
    {
        public ChatPanel()
        {
            InitializeComponent();
        }

        public StackPanel UIChatWithFriend { get {return UIChatWithFriendHolder; } }

        private void logOutButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                App.NetworkManager.Disconnect();
                App.NetworkManager.Connect();
                App.NetworkManager.StartListeningToServer();
            //}
            //catch { }
            App.ProgramWindows.FrameNavigate(App.LoginPage);
        }

        private async void page_Loaded(object sender, RoutedEventArgs e)
        {
            

            var getChatsWithFriend = new GetListOfChatsRequest() { UserName = NetworkActions.UserNameProvided, Number = NetworkActions.NumberProvided };
            NetworkActions.SendPacketToServer(getChatsWithFriend);
            
            
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            resetRadio.IsChecked = true;
            chatStartLogo.Visibility = Visibility.Collapsed;
            chatPanelFrame.Navigate(App.SearchPage);
        }

        private void friendsButton_Click(object sender, RoutedEventArgs e)
        {
            resetRadio.IsChecked = true;
            chatStartLogo.Visibility = Visibility.Collapsed;
            chatPanelFrame.Navigate(App.FriendRequestsPage);
        }

      

        private void notificationButton_Click(object sender, RoutedEventArgs e)
        {
    

            App.ProgramWindows.FrameNavigate(App.ChatPanelPage);

        }
    }
}
