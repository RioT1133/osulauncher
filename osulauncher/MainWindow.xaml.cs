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
        private List<BeatmapSet> _allBeatmapSets = new List<BeatmapSet>();

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
                string[] files = System.IO.Directory.GetFiles(d);

                int diffCount = 0;

                List<string> artists = new List<string>();
                List<string> titles = new List<string>();
                List<string> diffNames = new List<string>();
                List<string> bgPaths = new List<string>();
                List<string> songPaths = new List<string>();

                List<Beatmap> beatmaps = new List<Beatmap>();

                foreach (string file in files)
                {
                    string fileExtension = System.IO.Path.GetExtension(file);

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
                                for (int j = 0; j < 7; j++)
                                {
                                    if (lines[i + j].Contains("Title")) //older .osu file formats dont have "TitleUnicode" tag in [Metadata]
                                    {
                                        title = lines[i + j].Split(':')[1];
                                    }
                                    if (lines[i + j].Contains("Artist")) //older .osu file formats dont have "ArtistUnicode" tag in [Metadata]
                                    {
                                        artist = lines[i + j].Split(':')[1];
                                    }
                                    if (lines[i + j].Contains("Version"))
                                    {
                                        diffName = lines[i + j].Split(':')[1];
                                    }
                                }
                            }
                            if (lines[i].Trim() == "[General]")
                            {
                                songPath = file + "/" + lines[i+1].Split(':')[1].Trim(); //song path
                            }
                            if (lines[i].Trim() == "[Events]")
                            {
                                for (int j = 0;j < lines[i].Length; j++)
                                {
                                    if (lines[i + j].Contains(".jpg")) //if bg path is specified
                                    {
                                        bgPath = file + "/" + lines[i + j].Split(',')[2].Replace("\"", "");
                                    }
                                }
                            }
                        }
                        artists.Add(artist);
                        titles.Add(title);
                        diffNames.Add(diffName);
                        bgPaths.Add(bgPath);
                        songPaths.Add(songPath);
                        Beatmap temp = new Beatmap(artist, title, diffName, bgPath, songPath);
                        beatmaps.Add(temp);
                    }
                }
                BeatmapSet beatmapSet = new BeatmapSet(diffNames.Count, beatmaps);
                this._allBeatmapSets.Add(beatmapSet);
            } //handle song importing
            DisplaySongs();
        }

        private void DisplaySongs()
        {
            for (int i = 0; i < this._allBeatmapSets.Count; i++)
            {
                for (int j = 0; j < this._allBeatmapSets[i].GetNumBeatmaps(); j++)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = this._allBeatmapSets[i].GetBeatmaps()[j].GetMetadata()[0] + //artist
                                 " - "                                                     +
                                 this._allBeatmapSets[i].GetBeatmaps()[j].GetMetadata()[1] + //title
                                 "["                                                       + 
                                 this._allBeatmapSets[i].GetBeatmaps()[j].GetMetadata()[2] + //diffname
                                 "]";  
                    songBox.Items.Add(item);
                }
            }
        }

        private void UpdateConfigFile(string key, string value)
        {
            if (!_config.AppSettings.Settings.AllKeys.Contains(key))
            {
                _config.AppSettings.Settings.Add(key, value);
            } else
            {
                _config.AppSettings.Settings[key].Value = value;
            }
            _config.AppSettings.SectionInformation.ForceSave = true;
            _config.Save(ConfigurationSaveMode.Full);
        }
        private string ReadConfigFile(string key)
        {
            return _config.AppSettings.Settings[key].Value;
        }

    }
}
