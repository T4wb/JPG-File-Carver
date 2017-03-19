/*
 * Dear friend. Please take it into consideration that this program is not robust. 
 * Reasons:
 * # It's missing some essential (i.e. file reading/saving) exception handlers.
 * # Some elements are hardcoded ((i.e. init. value of pointer). This has been done to reducing the programming time.
 * # Some variables in the object classes (other than DocumentManager) are not used as assigning values may increase the programming time, unnecessarily. 
 * # Next, this is not suited for dumpfiles where the File table is missing or the structure is not as follows: 
 *  Blocks:
 *  1          = File MetaData
 *  2          = File Directory
 *  3 - 11     = FileTable
 *  12 - EOFB* = File Data (EOFB = End Of File Blocks)
 * 
 * 
 * Creater: T4wb
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                status.Text = "I couldn't load the file.";

                MessageBox.Show("Ouch... I couldn't load the file. Is it in use or did you close the previous window?");
            }
        }

        private void CarveDocument(object sender, ExecutedRoutedEventArgs e)
        {
            status.Text = "I'm trying to carve it... one moment";
            OpenFile.IsEnabled = false;

            if (_documentManger.CarveDocument())
            {
                status.Text = "JPG file has been carved";

                MessageBox.Show("JPG file has been carved. Let's open it!");

                Process.Start(@_documentManger.getCurrentFile());
            }
            else
            {
                status.Text = "I couldn't carve the file :(";

                MessageBox.Show("I couldn't carve the file. Please forgive me :(.");
            }

            OpenFile.IsEnabled = true;
            CarveFile.IsEnabled = false;
        }
    }
}
