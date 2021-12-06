using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osulauncher
{
    internal class Beatmap
    {
        private string _artist;
        private string _title;
        private string _diffName;
        private string _bgPath;
        private string _songPath;

        public Beatmap(string artist, string title, string diffName, string bgPath, string songPath)
        {
            this._artist = artist;
            this._title = title;
            this._diffName = diffName;
            this._bgPath = bgPath;
            this._songPath = songPath;
        }
        public List<string> GetMetadata()
        {
            List<string> ts = new List<string>();
            ts.Add(_artist);
            ts.Add(_title);
            ts.Add(_diffName);
            return ts;
        }
    }
}
