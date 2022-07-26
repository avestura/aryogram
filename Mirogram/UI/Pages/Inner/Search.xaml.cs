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

namespace Mirogram.UI.Pages.Inner
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Page
    {
        public Search()
        {
            InitializeComponent();
        }

        private void UIReqByNameButton_Click(object sender, RoutedEventArgs e)
        {
            var request = new AddFriendByIDRequest() { UserName = NetworkActions.UserNameProvided, ToUserID = UIReqByNameTextBox.Text };
            NetworkActions.SendPacketToServer(request);
        }

        private void UIReqByPhoneButton_Click(object sender, RoutedEventArgs e)
        {
            var request = new AddFriendByNumberRequest() { UserName = NetworkActions.UserNameProvided, PhoneNumber = UIReqByNameTextBox.Text };
            NetworkActions.SendPacketToServer(request);
        }
    }
}
