using MirogramServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Mirogram.UI.Pages.Main
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
            App.LoginPage = this;
            mirogramLogo.BlinkEasing(3000, 0.7, 1);
        }

        private void signupButton_Click(object sender, RoutedEventArgs e)
        {
            App.ProgramWindows.AppFrame.Navigate(App.RegisterPage);
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {

            if(usernameTextbox.Text.Trim() != "" &&
                passwordTextbox.Password.Trim() != "")
            {
                NetworkActions.UserNameProvided = usernameTextbox.Text;
                NetworkActions.PasswordProvided = passwordTextbox.Password;
                NetworkActions.NumberProvided = numberTextbox.Text;
                var loginRequest = new LoginRequestPacket()
                {
                    UserName = usernameTextbox.Text,
                    Password = passwordTextbox.Password,
                    PhoneNumber = numberTextbox.Text
                };

                App.ProgramWindows.AppFrame.Navigate(new ConnectPage());
                await Task.Delay(1000);
                NetworkActions.SendPacketToServer(loginRequest);
            }
            else
            {
                alert.Text = "ابتدا فیلد های زیر را با ورودی مناسب پر کنید";
                alert.ShowUsingLinearAnimation();
            }
         
            

        }

        private void page_Loaded(object sender, RoutedEventArgs e)
        { 
        }
    }
}
