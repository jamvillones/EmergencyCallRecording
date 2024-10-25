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

namespace ECR.WPF.Views.UserControls {
    /// <summary>
    /// Interaction logic for Pagination_View.xaml
    /// </summary>
    public partial class Pagination_View : UserControl {
        public Pagination_View() {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                var textbox = sender as TextBox;

                if (int.TryParse(textbox!.Text, out int number) && number > 0) {
                    var binding = textbox!.GetBindingExpression(TextBox.TextProperty);
                    binding?.UpdateSource();
                }
                else
                    MessageBox.Show("Value must be a number and greater than 0!", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
