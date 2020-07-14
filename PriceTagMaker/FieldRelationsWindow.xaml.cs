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
using System.Windows.Shapes;

namespace PriceTagMaker
{
    /// <summary>
    /// Interaction logic for FieldRelationsWindow.xaml
    /// </summary>
    public partial class FieldRelationsWindow : Window
    {

        public FieldRelationsWindow(string[] headers)
        {
            InitializeComponent();

            cbCode.ItemsSource = headers;
            cbName.ItemsSource = headers;
            cbPrice.ItemsSource = headers;
            cbOldPrice.ItemsSource = headers;


        }
        private void cbFileField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private int[] _relations;
        public int[] Order { get { return _relations; } set { _relations = value; } }
        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if(cbCode.SelectedItem != null && cbName.SelectedItem != null
                && cbPrice.SelectedItem != null && cbOldPrice.SelectedItem != null)
            {
                Order = new int[] { cbCode.SelectedIndex, cbName.SelectedIndex, 
                    cbPrice.SelectedIndex, cbOldPrice.SelectedIndex };
                this.DialogResult = true;
            }
            
            //this.Close();
        }
    }
}
