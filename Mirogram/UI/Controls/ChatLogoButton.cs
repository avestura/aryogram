using Mirogram.UI.Pages.Inner;
using Mirogram.UI.Pages.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mirogram.UI.Controls
{
    public class ChatLogoButton : RadioButton
    {

        public ChatLogoButton() : base() { UpdateName(); }


        public ChatArea ChatPage { get; set; }

        public string ChatWithUser
        {
            get { return (string)GetValue(ChatWithUserProperty); }
            set { SetValue(ChatWithUserProperty, value); UpdateName(); }
        }

        public string ChatWithNumber
        {
            get { return (string)GetValue(ChatWithNumberProperty); }
            set { SetValue(ChatWithNumberProperty, value); UpdateName(); }
        }

        public static readonly DependencyProperty ChatWithNumberProperty =
            DependencyProperty.Register("ChatWithNumber", typeof(string), typeof(ChatLogoButton), null);


        public static readonly DependencyProperty ChatWithUserProperty =
            DependencyProperty.Register("ChatWithUser", typeof(string), typeof(ChatLogoButton), null);


        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);

            if(IsEnabled == true)
            {
                var parent = base.Parent;
                while (parent.GetType() != typeof(ChatPanel)) parent = ((FrameworkElement)parent).Parent;

                ((ChatPanel)parent).chatPanelFrame.Navigate(ChatPage);
            }
       
        }

        public void UpdateName()
        {
            Content = ChatWithUser + " - " + ChatWithNumber;
        }



    }
}
