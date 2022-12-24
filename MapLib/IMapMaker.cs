using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapLib
{
    public interface IMapMaker
    {
        int SizeInTiles { get; }
        float TileScale { get; }

        void SetPlayerStartingResources(int playerID, int gold, int crystals);
        int RandInt(int min, int max);
        bool TryPlaceUnit(string name, string id, int ownerID, Point location);
        void Debug(string format, params object[] args);
    }
}
