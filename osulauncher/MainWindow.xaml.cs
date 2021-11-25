using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
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
        private FolderBrowserDialog _songsFolderDialog;
        private string _songsFolder = "not set";
        private string _defaultSongsFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/osu!/Songs";
        private System.Configuration.Configuration _config =
                ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.None);
        private List<BeatmapSet> _allBeatmapSets;

        public MainWindow()
        {
            InitializeComponent();

            if (Directory.Exists(_defaultSongsFolder))
            {
                _songsFolder = _defaultSongsFolder;
                UpdateConfigFile("songsFolder", _songsFolder);
                Console.WriteLine("\n" + ReadConfigFile("songsFolder") + "\n");
            }
            else if (ReadConfigFile("songsFolder") != null)
            {
                ReadConfigFile("songsFolder");
            }
            folderLabel.Content = "Songs folder: " + _songsFolder;
        }

        //choose folder
        private void SongFolderButton(object sender, RoutedEventArgs e)
        {
            this._songsFolderDialog = new FolderBrowserDialog();
            _songsFolderDialog.Description = "Select osu! Songs folder";
            DialogResult result = _songsFolderDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this._songsFolder = _songsFolderDialog.SelectedPath;
                folderLabel.Content = "Songs folder: " + _songsFolder;
                UpdateConfigFile("songsFolder", _songsFolder);
            }
        }

        private void ImportSongsButton(object sender, RoutedEventArgs e)
        {
            string[] directories = System.IO.Directory.GetDirectories(_songsFolder);
            foreach (string d in directories)
            {
                string[] files = System.IO.Directory.GetFiles(_songsFolder + d);

                int diffCount = 0;

                foreach (string file in files)
                {
                    List<Beatmap> beatmaps = null;
                    string fileExtension = System.IO.Path.GetExtension(file);

                    List<string> artists = null;
                    List<string> titles = null;
                    List<string> diffNames = null;
                    List<string> bgPaths = null;
                    List<string> songPaths = null;

                    if (fileExtension == ".osu") //keep in mind these are in alphabetical order
                    {
                        string artist = "";
                        string title = "";
                        string diffName = "";
                        string bgPath = "";
                        string songPath = "";

                        diffCount++;
                        string[] lines = File.ReadAllLines(file);
                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i].Trim() == "[Metadata]")
                            {
                                title = lines[i+2].Split(':')[1]; //unicode title
                                artist = lines[i+4].Split(':')[1]; //unicode artist
                                diffName = lines[i+6].Split(':')[1]; //diffname
                            }
                            if (lines[i].Trim() == "[General]")
                            {
                                songPath = file + "/" + lines[i+1].Split(':')[1]; //song path
                            }
                            if (lines[i].Trim() == "//Background and Video events")
                            {
                                bgPath = file + "/" + lines[i+1].Split(':')[2].Replace("\"", "");
                            }
                        }
                        artists.Append(artist);
                        titles.Append(title);
                        diffNames.Append(diffName);
                        bgPaths.Append(bgPath);
                        songPaths.Append(songPath);
                        Beatmap temp = new Beatmap(artist, title, diffName, bgPath, songPath);
                        beatmaps.Append(temp);
                    }
                    BeatmapSet beatmapSet = new BeatmapSet(diffNames.Count, beatmaps);
                    this._allBeatmapSets.Append(beatmapSet);
                }

            } //handle song importing
            DisplaySongs();
        }

        private void DisplaySongs()
        {

        }

        private void UpdateConfigFile(string key, string value)
        {
            _config.AppSettings.Settings[key].Value = value;
            _config.AppSettings.SectionInformation.ForceSave = true;
            _config.Save(ConfigurationSaveMode.Full);
        }
        private string ReadConfigFile(string key)
        {
            return _config.AppSettings.Settings[key].Value;
        }

    }
}
