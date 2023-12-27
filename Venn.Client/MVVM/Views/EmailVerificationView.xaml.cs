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

namespace Venn.Client.MVVM.Views
{
    /// <summary>
    /// Interaction logic for EmailVerificationView.xaml
    /// </summary>
    public partial class EmailVerificationView : UserControl
    {
        public EmailVerificationView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text[0]);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = (TextBox)sender;

            if (e.Key == Key.Back)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    
                    if (textBox.Name != "Digit1")
                    {
                        var prevDigit = int.Parse(textBox.Name.Substring(5)) - 1;
                        var prevTextBox = (TextBox)FindName("Digit" + prevDigit);
                        prevTextBox?.Focus();
                        prevTextBox.Clear();
                    }
                }
                else
                {
                    textBox.Clear();
                }
            }
            else if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                if (!string.IsNullOrEmpty(textBox.Text) &&
                    textBox.Name.Length == 6 && textBox.Name != "Digit6")
                {
                    var nextDigit = int.Parse(textBox.Name.Substring(5)) + 1;
                    var nextTextBox = (TextBox)FindName("Digit" + nextDigit);
                    nextTextBox?.Focus();
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void DigitTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            bool allFilled = true;
            foreach (var textBox in FindVisualChildren<TextBox>(this))
            {
                if (textBox.Name.StartsWith("Digit") && string.IsNullOrEmpty(textBox.Text))
                {
                    allFilled = false;
                    break;
                }
            }

            VerifyButton.IsEnabled = allFilled;
        }

        private IEnumerable<T> FindVisualChildren<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
                    if (child != null && child is T t)
                    {
                        yield return t;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
