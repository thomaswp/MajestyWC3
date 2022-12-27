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

        List<Player> ReadPlayerList()
        {
            int n = ReadInt();
            Debug.WriteLine("{0} players", n);
            var list = new List<Player>();
            for (int i = 0; i < n; i++)
            {
                list.Add(ReadPlayer());
            }
            return list;
        }

        Player ReadPlayer()
        {
            Player player = new Player();
            player.Id = ReadInt();
            player.Name = ReadString();
            player.IsActive = ReadInt() != 0;
            player.StartingGold = ReadInt();
            player.StartingCrystals = ReadInt();
            int n = ReadInt();
            player.StartingUnitPatternIDs = new int[n];
            for (int i = 0; i < n; i++)
            {
                player.StartingUnitPatternIDs[i] = ReadInt();
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

        List<UnitPattern> ReadUnitPatternList()
        {
            int n = ReadInt();
            Debug.WriteLine("{0} unit patterns", n);
            var list = new List<UnitPattern>();
            for (int i = 0; i < n; i++)
            {
                list.Add(ReadPattern(new UnitPattern()));
            }
            return list;
        }

        T ReadPattern<T>(T pattern) where T : Pattern
        {
            ReadString(8);
            pattern.Name = ReadString();
            pattern.Id1 = ReadInt();
            pattern.Id2 = ReadInt();
            pattern.TerrainShortID = ReadString(4);
            pattern.Resolution = ReadInt();
            int n = ReadInt();
            pattern.Instances = new List<PatternInstance>();
            for (int i = 0; i < n; i++)
            {
                pattern.Instances.Add(ReadUnitInstance());
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

        public PatternInstance ReadUnitInstance()
        {
            PatternInstance instance = new PatternInstance();
            instance.Id = ReadString(4);
            ReadInt();
            instance.Name = ReadString();
            int n = ReadInt();
            instance.Tiles = new char[n];
            for (int i = 0; i < n; i++)
            {
                instance.Tiles[i] = reader.ReadChar();
            }
            //Debug.WriteLine(instance.Summarize());
            return instance;
        }

        List<ForcePattern> ReadForcePatternList()
        {
            int n = ReadInt();
            Debug.WriteLine("{0} force patterns", n);
            List<ForcePattern> list = new List<ForcePattern>();
            for (int i = 0; i < n; i++)
            {
                list.Add(ReadPattern(new ForcePattern()));
            }
            return list;
        }

        public Quest ParseQuest()
        {
            quest = new Quest();
            quest.FileVersion = ReadString(4);
            Debug.WriteLine("Version: " + quest.FileVersion);
            ReadInt();
            ReadInt();
            ReadInt();
            quest.InitFunction = ReadString();
            Debug.WriteLine("InitFn: " + quest.InitFunction);
            ReadInt();
            ReadInt();
            ReadInt();
            ReadInt();
            quest.Width = ReadInt();
            quest.Height = ReadInt();
            Debug.WriteLine("Dimensions: {0}x{1}", quest.Width, quest.Height);
            ReadHeaderThingList();

            quest.Players = ReadPlayerList();

            ReadInt(); // unknown...
            ReadInt();
            ReadInt();
            ReadRegionList();

            quest.UnitPatterns = ReadUnitPatternList();

            quest.ForcePatterns = ReadForcePatternList();

            Debug.Assert(reader.BaseStream.Position == reader.BaseStream.Length);

            return quest;
        }
    }
}
