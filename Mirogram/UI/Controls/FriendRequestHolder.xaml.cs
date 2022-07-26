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
using MirogramServer.Network.RequestPackets;
using Mirogram.Networking;
using Mirogram.UI.Controls;

namespace Mirogram.UI.Controls
{
    /// <summary>
    /// Interaction logic for FriendRequestHolder.xaml
    /// </summary>
    public partial class FriendRequestHolder : UserControl
    {


        public string FromUser
        {
            get { return (string)GetValue(FromUserProperty); }
            set { SetValue(FromUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FromUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FromUserProperty =
            DependencyProperty.Register("FromUser", typeof(string), typeof(FriendRequestHolder), null);


        public FriendRequestHolder()
        {
            InitializeComponent();
        }

        private void recline_Click(object sender, RoutedEventArgs e)
        {
            var req = new FriendAcceptDeclineRequest() { UserName = NetworkActions.UserNameProvided, RequestedUserID = FromUser, Accepted = false };
            NetworkActions.SendPacketToServer(req);
            this.HideUsingLinearAnimation();
        }

        private void accept_Click(object sender, RoutedEventArgs e)
        {
            var req = new FriendAcceptDeclineRequest() { UserName = NetworkActions.UserNameProvided, RequestedUserID = FromUser, Accepted = true };
            NetworkActions.SendPacketToServer(req);
            App.FriendRequestsPage.UIFriendsHolder.Children.Add(new FriendHolder() { UserName = FromUser, Height = 45 });
            this.HideUsingLinearAnimation();
        }
    }
}
