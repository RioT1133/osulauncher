using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace osulauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private FolderBrowserDialog songsFolderDialog;
        private System.Windows.Forms.Label selectedPath;
        private string songsFolder;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.songsFolderDialog = new FolderBrowserDialog();
            songsFolderDialog.Description = "Select osu! Songs folder";
            songsFolderDialog.RootFolder = Environment.SpecialFolder.LocalApplicationData;
            DialogResult result = songsFolderDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.songsFolder = songsFolderDialog.SelectedPath;
                folderLabel.Content = "Selected folder: " + this.songsFolder;
                folderLabel.FontSize = 12;
            }
        }
    }
}
