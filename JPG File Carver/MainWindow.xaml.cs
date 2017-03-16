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
            if (_documentManger.OpenDocument())
            {
                status.Text = "Document is loaded";
            }
            else
            {
                status.Text = "Nothing loaded...";
            }
        }
    }
}
