using System;
using static War3Api.Common;
using static War3Api.Blizzard;
using MapLib;
using Source.Units.Monsters;

namespace Source.Maps
{
    class MapMaker : IMapMaker
    {
        const int MIN_X = -5376, MIN_Y = MIN_X;
        const int SIZE = 13824;
        const int TILE_SIZE = 64;

        public int SizeInTiles => SIZE / TILE_SIZE;
        public float TileScale => 2;

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
            point.X = point.X * TILE_SIZE + (MIN_X + SIZE) / 2;
            point.Y = point.Y * TILE_SIZE + (MIN_Y + SIZE) / 2;
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
