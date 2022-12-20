using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapLib;

namespace MapImporter
{
    class MapParser
    {
        BinaryReader reader;
        Quest quest;

        public MapParser(Stream input)
        {
            reader = new BinaryReader(input);
        }

        int ReadInt()
        {
            return reader.ReadInt32();
        }

        string ReadString()
        {
            string s = "";
            char c;
            while (true)
            {
                c = reader.ReadChar();
                if (c == 0) break;
                s += c;
            }
            return s;
        }

        string ReadString(int length)
        {
            return new string(reader.ReadChars(length));
        }

        void ReadHeaderThingList()
        {
            int n = ReadInt();
            for (int i = 0; i < n; i++) ReadHeaderThing();
        }

        void ReadHeaderThing()
        {
            int id = ReadInt();
            ReadInt();
            ReadInt();
            ReadInt();
            ReadInt();
            ReadInt();
            string none = ReadString(4);
            Debug.Assert(none == "NONE");
            ReadString();
            ReadInt();
            ReadInt();
            ReadInt();
            ReadString(4); // BV??
            ReadInt();
            ReadInt();
            ReadInt();
            ReadInt();
            for (int i = 0; i < 3; i++)
            {
                none = ReadString(4);
                Debug.Assert(none == "NONE");
                ReadInt();
                ReadInt();
                ReadInt();
                ReadInt();
            }
        }

        Player[] ReadPlayerList()
        {
            int n = ReadInt();
            Debug.WriteLine("{0} players", n);
            Player[] list = new Player[n];
            for (int i = 0; i < n; i++)
            {
                list[i] = ReadPlayer();
            }
            return list;
        }

        Player ReadPlayer()
        {
            Player player = new Player();
            player.id = ReadInt();
            player.name = ReadString();
            player.isActive = ReadInt() != 0;
            player.startingGold = ReadInt();
            player.startingCrystals = ReadInt();
            int n = ReadInt();
            player.startingUnitPatternIDs = new int[n];
            for (int i = 0; i < n; i++)
            {
                player.startingUnitPatternIDs[i] = ReadInt();
            }
            Debug.WriteLine(player.Summarize());
            return player;
        }

        void ReadRegionList()
        {
            int n = ReadInt();
            for (int i = 0; i < n; i++) ReadRegion();
        }

        void ReadRegion()
        {
            ReadString(8);
            string name = ReadString();
            ReadInt();
            ReadInt();
            int nInstances = ReadInt();
            for (int i = 0; i < nInstances; i++) ReadRegionInstance();
            int nPatches = ReadInt();
            for (int i = 0; i < nPatches; i++) ReadRegionPatch();
        }

        void ReadRegionInstance()
        {
            string patchShortID = ReadString(4);
            int proportion = ReadInt();
            int seeds = ReadInt();
            ReadInt();
            ReadInt();
            Debug.WriteLine("Region Instance: {0}, {1}, {2}", patchShortID, proportion, seeds);
        }

        void ReadRegionPatch()
        {
            ReadString(8);
            string id = ReadString();
            string landscape = ReadString(4);
            string terrain = ReadString(4);
            string fractal = ReadString(4);
            ReadInt();
            ReadInt();
            Debug.WriteLine("Region Patch {0}: {1}/{2}/{3}", id, landscape, terrain, fractal);
        }

        UnitPattern[] ReadUnitPatternList()
        {
            int n = ReadInt();
            Debug.WriteLine("{0} unit patterns", n);
            UnitPattern[] list = new UnitPattern[n];
            for (int i = 0; i < n; i++)
            {
                list[i] = ReadUnitPattern();
            }
            return list;
        }

        UnitPattern ReadUnitPattern()
        {
            UnitPattern pattern = new UnitPattern();
            ReadString(8);
            pattern.name = ReadString();
            pattern.id1 = ReadInt();
            pattern.id2 = ReadInt();
            pattern.terrainShortID = ReadString(4);
            pattern.resolution = ReadInt();
            int n = ReadInt();
            pattern.instances = new UnitPatternInstance[n];
            for (int i = 0; i < n; i++)
            {
                pattern.instances[i] = ReadUnitInstance();
            }
            int zero = ReadInt();
            Debug.Assert(zero == 0); // Seems to be a 0-terminator?
            n = ReadInt();
            for (int i = 0; i < n; i++)
            {
                int rez = ReadInt();
                Debug.Assert(rez == 50);
            }
            for (int i = 0; i < 4; i++)
            {
                ReadInt(); // unknown
            }
            Debug.WriteLine(pattern.Summarize());
            return pattern;
        }

        public UnitPatternInstance ReadUnitInstance()
        {
            UnitPatternInstance instance = new UnitPatternInstance();
            instance.id = ReadString(4);
            ReadInt();
            instance.name = ReadString();
            int n = ReadInt();
            instance.tiles = new char[n];
            for (int i = 0; i < n; i++)
            {
                instance.tiles[i] = reader.ReadChar();
            }
            //Debug.WriteLine(instance.Summarize());
            return instance;
        }

        public Quest ParseQuest()
        {
            quest = new Quest();
            string version = ReadString(4);
            Debug.WriteLine("Version: " + version);
            ReadInt();
            ReadInt();
            ReadInt();
            string initFn = ReadString();
            Debug.WriteLine("InitFn: " + initFn);
            ReadInt();
            ReadInt();
            ReadInt();
            ReadInt();
            int width = ReadInt();
            int height = ReadInt();
            Debug.WriteLine("Dimensions: {0}x{1}", width, height);
            ReadHeaderThingList();

            quest.players = ReadPlayerList();

            ReadInt(); // unknown...
            ReadInt();
            ReadInt();
            ReadRegionList();

            quest.unitPatterns = ReadUnitPatternList();


            return quest;
        }
    }
}
