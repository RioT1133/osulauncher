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
        private string songsFolder = "not set";
        private string defaultSongsFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/osu!/Songs";
        private string configFile = Environment.CurrentDirectory + "/config.txt";
        private string[] config;

        public MainWindow()
        {
            InitializeComponent();
            if(!File.Exists(configFile)) //if config file hasn't been created yet
            {
                //if default folder exists, set it as songs folder
                if (Directory.Exists(defaultSongsFolder))
                {
                    songsFolder = defaultSongsFolder;
                }
                folderLabel.Content = "Songs folder: " + songsFolder;
                makeConfigFile();
            } else //if there is a config file
            {
                updateConfigFile();
            }
        }

        //choose folder
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.songsFolderDialog = new FolderBrowserDialog();
            songsFolderDialog.Description = "Select osu! Songs folder";
            DialogResult result = songsFolderDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.songsFolder = songsFolderDialog.SelectedPath;
            }
        }

        private void makeConfigFile()
        {
            using (System.IO.FileStream fs = System.IO.File.Create(configFile))
            {
                string songsPath = "songsPath:" + songsFolder;
                AddText(fs, songsPath);
            }
        }

        private void updateConfigFile()
        {
            this.config = File.ReadAllLines(configFile);
            this.songsFolder = String.Join("", config[0].Split(':').Skip(0));
        }

        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
    }
}
