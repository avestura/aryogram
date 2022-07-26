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

namespace Mirogram.UI.Pages.Inner
{
    /// <summary>
    /// Interaction logic for ChatArea.xaml
    /// </summary>
    public partial class ChatArea : Page
    {



        public string UserChattingWith
        {
            get { return (string)GetValue(UserChattingWithProperty); }
            set { SetValue(UserChattingWithProperty, value); }
        }



        public string PhoneNumberChattingWith
        {
            get { return (string)GetValue(PhoneNumberChattingWithProperty); }
            set { SetValue(PhoneNumberChattingWithProperty, value); }
        }

        public static readonly DependencyProperty PhoneNumberChattingWithProperty =
            DependencyProperty.Register("PhoneNumberChattingWith", typeof(string), typeof(ChatArea), null);



        public static readonly DependencyProperty UserChattingWithProperty =
            DependencyProperty.Register("UserChattingWith", typeof(string), typeof(ChatArea), null);




        public ChatArea()
        {
            InitializeComponent();
        }

        private void UIChatSendButton_Click(object sender, RoutedEventArgs e)
        {
            var textBlock = new TextBlock()
            {
                Text = UIChatTextbox.Text,
                FontSize = 15,
                Foreground = Brushes.DarkGreen,
                VerticalAlignment = VerticalAlignment.Center
            };
            var border = new Border()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e6fff7")),
                Margin = new Thickness(10,2,10,2),
                Padding = new Thickness(8, 5, 8, 5),
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(0, 0, 0, 2),
                CornerRadius = new CornerRadius(3),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            border.Child = textBlock;
            UIChatsPlace.Children.Add(border);


            var req = new MessageRequestPacket()
            {
                FromUserID = Networking.NetworkActions.UserNameProvided,
                FromNumber = Networking.NetworkActions.NumberProvided,
                ToUserId = UserChattingWith,
                ToNumber = PhoneNumberChattingWith,
                TimeSent = DateTime.Now.ToShortDateString(),
                Content = UIChatTextbox.Text,
                UserName = Networking.NetworkActions.UserNameProvided,


            };
            Networking.NetworkActions.SendPacketToServer(req);

            UIChatTextbox.Clear();

        }

        private void ChatAreaPage_Loaded(object sender, RoutedEventArgs e)
        {
            var chats = new GetMessagesRequest()
            {
                FromUserID = Networking.NetworkActions.UserNameProvided,
                FromNumber = Networking.NetworkActions.NumberProvided,
                ToUserId = UserChattingWith,
                ToNumber = PhoneNumberChattingWith,
                UserName = Networking.NetworkActions.UserNameProvided,
            };
            Networking.NetworkActions.SendPacketToServer(chats);
        }
    }
}
