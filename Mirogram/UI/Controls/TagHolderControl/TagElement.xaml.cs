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

namespace Mirogram.UI.Controls.TagHolderControl
{
    /// <summary>
    /// Interaction logic for TagElement.xaml
    /// </summary>
    public partial class TagElement : UserControl
    {


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }



        public TagHolder ParentHolder
        {
            get { return (TagHolder)GetValue(ParentHolderProperty); }
            set { SetValue(ParentHolderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParentHolder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParentHolderProperty =
            DependencyProperty.Register("ParentHolder", typeof(TagHolder), typeof(TagElement), null);



        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TagElement), null);


        public TagElement()
        {
            InitializeComponent();
        }

        private async void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(ParentHolder != null)
            {
                await closeField.HideUsingLinearAnimationAsync();
                ParentHolder.TagElements.Remove(this);
                ParentHolder.tagView.Children.Remove(this);
            }
        }
    }
}
