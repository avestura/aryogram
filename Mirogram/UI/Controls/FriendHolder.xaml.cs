using MirogramServer.Network.RequestPackets;
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

namespace Mirogram.UI.Controls
{
    /// <summary>
    /// Interaction logic for FriendHolder.xaml
    /// </summary>
    public partial class FriendHolder : UserControl
    {



        public List<string> PhoneNumberList
        {
            get { return (List<string>)GetValue(PhoneNumberListProperty); }
            set { SetValue(PhoneNumberListProperty, value); }
        }
        public static readonly DependencyProperty PhoneNumberListProperty =
            DependencyProperty.Register("PhoneNumberList", typeof(List<string>), typeof(FriendHolder), null);



        public string UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }

        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register("UserName", typeof(string), typeof(FriendHolder), null);




        public FriendHolder()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var req = new StartChatRequest()
            {
                UserName = Networking.NetworkActions.UserNameProvided,
                Number = Networking.NetworkActions.NumberProvided,
                WithUser = UserName,
                WithNumber = UIPhoneNumberSelect.SelectedItem.ToString()
            };
            Networking.NetworkActions.SendPacketToServer(req);
        }
    }
}
