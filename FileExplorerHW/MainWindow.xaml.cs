using System;
using System.Collections.Generic;
using System.IO;
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

namespace FileExplorerHW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            foreach (string s in Directory.GetLogicalDrives())
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = s;
                item.Expanded += new RoutedEventHandler(AddItems);
                explorer.Items.Add(item);
            }
        }

        public void AddItems(object sender, RoutedEventArgs e)
        {
            if (((TreeViewItem)sender).Items.Count != 0)
            {
                return;
            }

            string currentFolder = ((TreeViewItem)sender).Header.ToString();

            try
            {

                Parallel.Invoke(() =>
                {


                    foreach (string s in Directory.GetDirectories(currentFolder))
                    {

                        TreeViewItem itemInChild = new TreeViewItem();
                        itemInChild.Header = s;
                        itemInChild.Expanded += new RoutedEventHandler(AddItems);
                        ((TreeViewItem)sender).Items.Add(itemInChild);
                    }


                });


                Parallel.Invoke(() =>
                {

                    foreach (string fileName in Directory.GetFiles(currentFolder))
                    {
                        TreeViewItem newItemFile = new TreeViewItem();
                        newItemFile.Header = fileName;
                        ((TreeViewItem)sender).Items.Add(newItemFile);
                    }
                });
            }
            catch (Exception ex)
            {
                errorText.Text = ex.Message;
            }

        }

    }
}
