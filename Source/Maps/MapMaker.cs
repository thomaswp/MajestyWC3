using System;
using static War3Api.Common;
using static War3Api.Blizzard;
using MapLib;
using Source.Units.Monsters;

namespace Source.Maps
{
    class MapMaker : IMapMaker
    {
        // Size of a "tile" in WC3 (which I believe has a 4x4 subgrid)
        const int WC3_TILE_SIZE = 128;
        // Min X and Y for the map in tiles
        const int CENTER_X = 0, CENTER_Y = CENTER_X;
        // Map size in pixels
        const int SIZE = WC3_TILE_SIZE * 468;
        // Size of a Majesty Tile (max 512) on the WC3 map
        const int MAJESTY_TILE_SIZE = SIZE / Layout.MAX_SIZE;
        
        public float UnitPatternScale => 1;

        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public int RandInt(int min, int max)
        {
            return GetRandomInt(min, max);
        }

        private player GetPlayerByID(int ownerID)
        {
            if (ownerID == Layout.MONSTER_OWNER_ID)
            {
                return Monster.Player;
            }
            else if (ownerID == Layout.NEUTRAL_OWNER_ID)
            {
                return Player(GetPlayerNeutralPassive());
            }
            else
            {
                // Offset by 1
                return Player(ownerID + 1);
            }
        }

        private Point ToWC3(Point point)
        {
            point.Y *= -1;
            point.X = point.X * MAJESTY_TILE_SIZE + CENTER_X;
            point.Y = point.Y * MAJESTY_TILE_SIZE + CENTER_Y;
            return point;
        }

        public void SetPlayerStartingResources(int playerID, int gold, int crystals)
        {
            Console.WriteLine($"Setting player {playerID}'s gold to {gold} and crystals to {crystals}");
            if (playerID == Layout.MONSTER_OWNER_ID || playerID == Layout.NEUTRAL_OWNER_ID) return;
            player player = GetPlayerByID(playerID);
            SetPlayerState(player, PLAYER_STATE_RESOURCE_GOLD, gold);
            SetPlayerState(player, PLAYER_STATE_RESOURCE_LUMBER, crystals);
        }

        public bool TryPlaceUnit(string name, string id, int ownerID, Point location)
        {
            player player = GetPlayerByID(ownerID);
            Point wc3Location = ToWC3(location);
            Console.WriteLine($"Creating unit {name} for player {ownerID} at {location} --> {wc3Location}");
            int unitID = GetUnitID(name, id);
            CreateUnit(player, unitID, wc3Location.X, wc3Location.Y, 0);
            return true;
        }

        private int GetUnitID(string name, string id)
        {
            return Constants.UNIT_DRAENEI_WARRIOR;
        }
    }
}
