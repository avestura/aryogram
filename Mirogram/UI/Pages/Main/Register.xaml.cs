using Mirogram.Networking;
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

namespace Mirogram.UI.Pages.Main
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
            mirogramLogo.BlinkEasing(3000, 0.7, 1);

        }

        private void signupButton_Click(object sender, RoutedEventArgs e)
        {
            App.ProgramWindows.AppFrame.Navigate(App.LoginPage);
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if(phoneNumbers.TagElements.Count > 0 &&
                passwordTextbox.Password.Trim() != "" &&
                usernameTextbox.Text.Trim() != "")
            {
                NetworkActions.UserNameProvided = usernameTextbox.Text;
                NetworkActions.PasswordProvided = passwordTextbox.Password;
                string phones = string.Empty;
                foreach (var element in phoneNumbers.TagElements)
                {
                    phones += element.Text + ",";
                }
                phones = phones.Substring(0, phones.Length - 1);
                var registerRequest = new RegisterRequestPacket()
                {
                    UserName = usernameTextbox.Text,
                    Password = passwordTextbox.Password,
                    PhoneNumbers = phones
                };
                App.ProgramWindows.AppFrame.Navigate(new ConnectPage());
                await Task.Delay(1000);
                NetworkActions.SendPacketToServer(registerRequest);
            }
            else
            {
                alert.Text = "ابتدا فیلد های زیر را با ورودی مناسب پر کنید";
                alert.ShowUsingLinearAnimation();
            }
          
        }
    }
}
