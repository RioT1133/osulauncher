using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osulauncher
{
    internal class BeatmapSet
    {
        private int _numBeatmaps;
        private List<Beatmap> _beatmaps;

        public BeatmapSet(int numBeatmaps, List<Beatmap> beatmapSets)
        {
            this._numBeatmaps = numBeatmaps;
            for (int i = 0; i < numBeatmaps; i++)
            {
                this._beatmaps = beatmapSets;
            }
        }
        public int GetNumBeatmaps()
        {
            return this._numBeatmaps;
        }
        public List<Beatmap> GetBeatmaps()
        {
            return _beatmaps;
        }
    }

    
}
