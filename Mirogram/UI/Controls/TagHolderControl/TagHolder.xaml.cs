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
    /// Interaction logic for TagHolder.xaml
    /// </summary>
    public partial class TagHolder : UserControl
    {
        public enum TagFormats
        {
            String, Number
        }

        public List<TagElement> TagElements { get; private set; } = new List<TagElement>();

        public TagFormats TagFormat
        {
            get { return (TagFormats)GetValue(TagFormatProperty); }
            set { SetValue(TagFormatProperty, value); }
        }

        public static readonly DependencyProperty TagFormatProperty =
            DependencyProperty.Register("TagFormat", typeof(TagFormats), typeof(TagHolder), null);



        public TagHolder()
        {
            InitializeComponent();
        }

       

        private void tagInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(TagFormat == TagFormats.Number)
            {
                ulong x;
                string textToFirstSpace = "";

                if (tagInput.Text.Contains(" "))
                    textToFirstSpace = tagInput.Text.Substring(0, tagInput.Text.IndexOf(' '));
                else
                    textToFirstSpace = "";

                if (ulong.TryParse(textToFirstSpace, out x))
                {
                    var tElement = new TagElement() { Text = textToFirstSpace, ParentHolder = this };
                    tagView.Children.Add(tElement);
                    TagElements.Add(tElement);
                    tagInput.Text = string.Empty;
                }
            }
        }
    }
}
