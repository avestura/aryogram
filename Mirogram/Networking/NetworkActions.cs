using Mirogram.UI.Pages.Main;
using MirogramServer.Network;
using MirogramServer.Network.RequestPackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static MirogramServer.Network.NetAndCallBackManager;
using Mirogram.UI.Controls;
using Mirogram.UI.Pages.Inner;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;

namespace Mirogram.Networking
{
    public static class NetworkActions
    {

        public static string UserNameProvided { get; set; }
        public static string PasswordProvided { get; set; }
        public static string NumberProvided { get; set; }

        public static void HandleRecivedPacket(Packet packet)
        {


            if (packet.Type == Packet.PacketKinds.StatusServerResonse)
            {
                var data = packet as ServerStatusPacket;

                if (data.ResponseTo == ServerStatusPacket.ResponseTos.Login)
                {
                    HandleLoginResponse(packet);
                }
                else if (data.ResponseTo == ServerStatusPacket.ResponseTos.Register)
                {
                    HandleRegisterResponse(packet);
                }
                else if (data.ResponseTo == ServerStatusPacket.ResponseTos.AddFriend)
                {
                    HandleAddFriendResponse(packet);
                }
            }
            else if (packet.Type == Packet.PacketKinds.ServerFriendRequestList)
            {
                HandleRecivedFriendRequestsList(packet);
            }
            else if (packet.Type == Packet.PacketKinds.ServerFriendList)
            {
                HandleRecivedFriendList(packet);
            }
            else if (packet.Type == Packet.PacketKinds.ServerChatsList)
            {
                HandleRecivedChatList(packet);
            }
            else if (packet.Type == Packet.PacketKinds.MessageRequest)
            {
                HandleRecivedChatList(packet);
            }
            else if (packet.Type == Packet.PacketKinds.ServerMessagesList)
            {
                HandleRecievedMessage(packet);
            }
            

        }

        public static void HandleRecievedMessage(Packet packet)
        {

            var data = packet as ServerMessagesList;
            try
            {
                var cp = ((ChatPanel)App.ProgramWindows.mainFrame.Content);
                var chatArea = ((ChatArea)cp.chatPanelFrame.Content);

                chatArea.UIChatsPlace.Children.Clear();
                foreach(var item in data.Messages)
                {
                    if (chatArea.UserChattingWith == item.ToUserID && chatArea.PhoneNumberChattingWith == item.ToNumber)
                    {
                        var textBlock = new TextBlock()
                        {
                            Text = item.Content,
                            FontSize = 15,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        var border = new Border()
                        {
                            Background = Brushes.White,
                            Margin = new Thickness(10, 2, 10, 2),
                            Padding = new Thickness(8, 5, 8, 5),
                            BorderBrush = Brushes.LightGray,
                            BorderThickness = new Thickness(0, 0, 0, 2),
                            CornerRadius = new CornerRadius(3),
                            HorizontalAlignment = HorizontalAlignment.Left
                        };
                        border.Child = textBlock;
                        chatArea.UIChatsPlace.Children.Add(border);
                    }
                    else
                    {
                        var textBlock = new TextBlock()
                        {
                            Text = item.Content,
                            FontSize = 15,
                            Foreground = Brushes.DarkGreen,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        var border = new Border()
                        {
                            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e6fff7")),
                            Margin = new Thickness(10, 2, 10, 2),
                            Padding = new Thickness(8, 5, 8, 5),
                            BorderBrush = Brushes.LightGray,
                            BorderThickness = new Thickness(0, 0, 0, 2),
                            CornerRadius = new CornerRadius(3),
                            HorizontalAlignment = HorizontalAlignment.Right
                        };
                        border.Child = textBlock;
                        chatArea.UIChatsPlace.Children.Add(border);
                    }
                }
    
            }
            catch { }
        }

        public static void HandleRecivedChatList(Packet packet)
        {

                var data = packet as ServerChatsList;
                    App.ChatPanelPage.UIChatWithFriendHolder.Children.Clear();
                var style = (Style)App.Current.FindResource("ChatLogoButtonFantasy");
                foreach (var item in data.Items)
                {
                    

                    var chatPage = new ChatArea() { UserChattingWith = item.UserName, PhoneNumberChattingWith = item.PhoneNumber };
                    var tempChatHolder = new ChatLogoButton
                    {
                        Height = 50,
                        ChatWithUser = item.UserName,
                        ChatWithNumber = item.PhoneNumber,
                        ChatPage = chatPage,
                        Style = style,
                        Visibility = Visibility.Visible
                    };

                    App.ChatPanelPage.UIChatWithFriendHolder.Children.Add(tempChatHolder);
                    App.ChatPanelPage.InitializeComponent();


                }


        }

        public static void HandleRecivedFriendList(Packet packet)
        {
            var data = packet as ServerFriendList;
            App.FriendRequestsPage.UIFriendsHolder.Children.Clear();
            foreach (var item in data.Friends)
            {
                var tempFriendHolder = new FriendHolder { UserName = item.UserName, PhoneNumberList = item.PhoneNumbers, Height = 45 };
                App.FriendRequestsPage.UIFriendsHolder.Children.Add(tempFriendHolder);
            }
        }

        public static void HandleRecivedFriendRequestsList(Packet packet)
        {
            var data = packet as ServerFriendRequestList;
            App.FriendRequestsPage.UIRequestItemsHolder.Children.Clear();
            foreach (var item in data.Requests)
            {
                App.FriendRequestsPage.UIRequestItemsHolder.Children.Add(
                    new FriendRequestHolder { FromUser = item, Height = 45 });
            }
        }

        public static void HandleLoginResponse(Packet packet)
        {
            var data = packet as ServerStatusPacket;

            if (data.ServerResonseStatus == ServerStatusPacket.Status.Successful)
            {
                App.ChatPanelPage.usernameAlias.Text = NetworkActions.UserNameProvided;
                App.ProgramWindows.FrameNavigate(App.ChatPanelPage);
            }
            else
            {
                App.ProgramWindows.FrameNavigate(App.LoginPage);
                App.LoginPage.alert.Text = "نام کاربری و یا کلمه عبور وارد شده معتبر نیست";
                App.LoginPage.alert.ShowUsingLinearAnimation();
            }
        }
        public static void HandleRegisterResponse(Packet packet)
        {
            var data = packet as ServerStatusPacket;

            if (data.ServerResonseStatus == ServerStatusPacket.Status.Successful)
            {
                App.ProgramWindows.FrameNavigate(App.LoginPage);
                App.LoginPage.alert.Text = "حساب کاربری ساخته شد";
                App.LoginPage.alert.ShowUsingLinearAnimation();
            }
            else
            {
                App.ProgramWindows.FrameNavigate(App.RegisterPage);
                App.RegisterPage.alert.Text = "کاربری با همین نام در لیست کابران حضور دارد";
                App.RegisterPage.alert.ShowUsingLinearAnimation();
            }
        }

        public static void HandleAddFriendResponse(Packet packet)
        {
            var data = packet as ServerStatusPacket;

            if (data.ServerResonseStatus == ServerStatusPacket.Status.Successful)
            {
                App.SearchPage.alert.Text = "درخواست دوستی ارسال شد";
                App.SearchPage.alert.ShowUsingLinearAnimation();
            }
            else
            {
                App.SearchPage.alert.Text = "درخواست دوستی ارسال نشد. ممکن با فرد مورد نظر دوست باشید و یا او به شما درخواست داده است";
                App.SearchPage.alert.ShowUsingLinearAnimation();
            }
        }

        public static void SendPacketToServer(Packet packet)
        {

            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = packet.SerializedPacket();

            // Begin sending the data to the remote device.
            App.NetworkManager.MySocket.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), packet);

        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                //Packet client = (Packet)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = App.NetworkManager.MySocket.EndSend(ar);
                Console.WriteLine(">> Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.
                App.NetworkManager.SendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


    }
}
