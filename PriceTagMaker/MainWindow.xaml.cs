using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using System.IO;




namespace PriceTagMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Receipt> receipts = new List<Receipt>();
        string[] receiptTypes = new string[] { "Стандартный", "Телефонный", "Огромный", "Большой", "Акционный", "A4", "" };
        System.Text.Encoding win1251 = System.Text.Encoding.GetEncoding(1251);

        public MainWindow()
        {
            InitializeComponent();
            cmbReceiptType.ItemsSource = receiptTypes;

            btnSetReceiptType.IsEnabled = false;
            btnPrintReceipts.IsEnabled = false;
            cmbReceiptType.IsEnabled = false;
            dtgFileData.IsEnabled = false;
            
            //readCSV(@"c:\users\alexey\desktop\печатка\Пример файла.csv", new int[] { 0, 1, 2, 3 });
            //dtgFileData.ItemsSource = receipts;
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            //dlg.FileName = "Document"; 
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV файлы (.csv)|*.csv";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                
                FieldRelationsWindow fieldRelationsWindow = new FieldRelationsWindow(File.ReadLines(filename, win1251).ToList()[0].Split(';'));
                if (fieldRelationsWindow.ShowDialog() == true)
                {
                    readCSV(filename, fieldRelationsWindow.Order);
                    txtFileName.Text = filename;
                    btnSetReceiptType.IsEnabled = true;
                    btnPrintReceipts.IsEnabled = true;
                    cmbReceiptType.IsEnabled = true;
                    dtgFileData.IsEnabled = true;
                }
                dtgFileData.ItemsSource = receipts;
            }
        }

        private void readCSV(string filename, int[] columnsOrder)
        {
            foreach (var line in File.ReadAllLines(filename, win1251).Skip(1))
            {
                var fields = line.Split(';');
                if (fields[0] != string.Empty)
                {
                    receipts.Add(new Receipt
                    {
                        Code = fields[columnsOrder[0]].ToString(),
                        Name = fields[columnsOrder[1]].ToString(),
                        Price = fields[columnsOrder[2]].ToString(),
                        OldPrice = fields[columnsOrder[3]].ToString()
                    });
                }

            }
        }
        private void btnSetReceiptType_Click(object sender, RoutedEventArgs e)
        {
            foreach (Receipt receipt in dtgFileData.SelectedItems)
            {
                receipt.ReceiptType = cmbReceiptType.SelectedItem.ToString();
            }
            dtgFileData.Items.Refresh();
        }

        private void btnPrintReceipts_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string folderName = dialog.SelectedPath;
                    foreach (var receiptType in receiptTypes)
                    {
                        Printer.Print(receipts.Where<Receipt>(r => r.ReceiptType == receiptType).ToList(), folderName);
                    }

                }

            }


        }


    }
}
