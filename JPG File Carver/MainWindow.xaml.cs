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

namespace JPG_File_Carver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DocumentManager _documentManger;

        public MainWindow()
        {
            InitializeComponent();

            _documentManger = new DocumentManager();
        }

        private void OpenDocument(object sender, ExecutedRoutedEventArgs e)
        {
            status.Text = "I'm trying to load it... one moment";

            if (_documentManger.OpenDocument())
            {
                status.Text = "JPG file has been loaded";
                CarveFile.IsEnabled = true;
            }
            else
            {
                status.Text = "I couldn't load the file. Is it in use?";
            }
        }

        private void CarveDocument(object sender, ExecutedRoutedEventArgs e)
        {
            status.Text = "I'm trying to carve it... one moment";
            OpenFile.IsEnabled = false;

            if (_documentManger.CarveDocument())
            {
                status.Text = "JPG file has been carved";
            }
            else
            {
                status.Text = "I couldn't carve the file :(";
            }

            OpenFile.IsEnabled = true;
            CarveFile.IsEnabled = false;
        }
    }
}
