using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapLib;

namespace MapImporter
{
    class TestLayout : IMapMaker
    {
        const int BASE_SCALE = 512;
        readonly int scale;
        readonly Random rand;

        public TestLayout(int scale, int seed)
        {
            this.scale = scale;
            rand = new Random(seed);
        }

        public int SizeInTiles => BASE_SCALE * scale;

        public float UnitPatternScale => scale;

        public void Debug(string format)
        {
            System.Diagnostics.Debug.WriteLine(format);
        }

        public int RandInt(int min, int max)
        {
            return rand.Next(min, max);
        }

        public void SetPlayerStartingResources(int playerID, int gold, int crystals)
        {
            System.Diagnostics.Debug.WriteLine("Setting player {0}'s gold to {1} and crystals to {2}", playerID, gold, crystals);
        }

        public bool TryPlaceUnit(string name, string id, int ownerID, Point location)
        {
            System.Diagnostics.Debug.WriteLine("Placing unit {0} ({1}) for player {2} at {3}", name, id, ownerID, location);
            return true;
        }
    }
}
