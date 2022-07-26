using Mirogram.Networking;
using MirogramServer.Network.RequestPackets;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Mirogram.UI.Pages.Inner
{
    /// <summary>
    /// Interaction logic for FriendRequests.xaml
    /// </summary>
    public partial class FriendRequests : Page
    {
        public FriendRequests()
        {
            InitializeComponent();
        }

        private async void page_Loaded(object sender, RoutedEventArgs e)
        {


            var getMyFriends = new GetFriendsRequest() { UserName = NetworkActions.UserNameProvided };
            NetworkActions.SendPacketToServer(getMyFriends);

            await Task.Delay(100);

            var getMyFriendRequests = new GetFriendRequestsToMeRequest() { UserName = NetworkActions.UserNameProvided };
            NetworkActions.SendPacketToServer(getMyFriendRequests);


        }
    }
}
