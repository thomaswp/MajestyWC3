using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapLib
{
    public struct Point
    {
        public float X, Y;

        public Point(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }

    interface Transformer
    {
        public Point T(Point p);
    }

    class IdentityTransformer : Transformer
    {
        public Point T(Point p) { return p; }
    }

    class RotateTransformer : Transformer
    {
        int nTurns;

        RotateTransformer(int nTurns)
        {
            this.nTurns = nTurns;
        }

        public Point T(Point p) { return p; }
    }

    class FlipTransformer : Transformer
    {
        public Point T(Point p) { return new Point(p.Y, p.X); }
    }

    public class Layout
    {
        public const int MONSTER_OWNER_ID = 7;
        public const int NEUTRAL_OWNER_ID = -1;

        public const int MAX_SIZE = 512;
        public const int MAX_REZ = 10;
        public const int GRID_SIZE = 5;
        //public const int TILE_SIZE = 32;

        private IMapMaker maker;
        private Quest quest;
        private readonly float mapSize;
        private readonly float tileScale;

        private Dictionary<int, int> forceToPlayerMap = new Dictionary<int, int>();

        private static Transformer[] Transformers = new Transformer[]
        {
            new IdentityTransformer(),
        };

        public Layout(IMapMaker mapMaker, Quest quest)
        {
            this.maker = mapMaker;
            this.quest = quest;
            tileScale = maker.TileScale;
            mapSize = (float)mapMaker.SizeInTiles * quest.Width / MAX_SIZE;
        }

        public void Start()
        {
            LoadPlayers();
            var forcePattern = quest.ForcePatterns[0];
            LayoutForcePattern(forcePattern);
        }

        void LoadPlayers()
        {
            foreach (var player in quest.Players)
            {
                if (!player.IsActive) continue;
                maker.SetPlayerStartingResources(player.Id, player.StartingGold, player.StartingCrystals);
                foreach (int id in player.StartingUnitPatternIDs) forceToPlayerMap[id] = player.Id;
            }
        }



        int charToIndex(char c)
        {
            int index = c - 'A';
            if (index < 0)
            {
                maker.Debug("Inappropriate char: " + c);
                index = 0;
            }
            if (index > 24)
            {
                maker.Debug("Inappropriate char: " + c);
                index = 24;
            }
            return index;
        }

        void LayoutPattern(Pattern pattern, Func<PatternInstance, int, Point, bool> callback)
        {
            Transformer t = Transformers[maker.RandInt(0, Transformers.Length - 1)];
            bool[] taken = new bool[GRID_SIZE * GRID_SIZE];
            for (int i = 0; i < pattern.Instances.Count; i++)
            {
                var instance = pattern.Instances[i];
                var availablePoints = instance.Tiles
                    .Select(t => charToIndex(t))
                    .Where(p => !taken[p])
                    .ToList();

                while (true)
                {
                    if (availablePoints.Count == 0)
                    {
                        maker.Debug("Unable to find location for " + instance.Name);
                        break;
                    }

                    int index = availablePoints[maker.RandInt(0, availablePoints.Count - 1)];
                    taken[index] = true;

                    // Scale between -0.5 and 0.5 for transform
                    Point relativeLocation = new Point(
                        (index % GRID_SIZE + 0.5f) / GRID_SIZE - 0.5f,
                        (index / GRID_SIZE + 0.5f) / GRID_SIZE - 0.5f
                    );
                    Point transformedLocation = t.T(relativeLocation);

                    if (callback(instance, i, transformedLocation)) break;
                }
            }
        }

        void LayoutForcePattern(ForcePattern pattern)
        {
            //float cellSize = mapSize / GRID_SIZE;
            LayoutPattern(pattern, (instance, index, relLoc) =>
            {
                Point scaledLocation = new Point(
                    relLoc.X * mapSize,
                    relLoc.Y * mapSize
                );

                int ownerID = -1;
                if (forceToPlayerMap.TryGetValue(index, out int id)) ownerID = id;

                maker.Debug($"Laying out {instance.Name} at {scaledLocation}");
                LayoutUnitPattern(instance.Name, scaledLocation, ownerID);
                return true;
            });
        }

        private void LayoutUnitPattern(string name, Point center, int ownerID)
        {
            var patterns = quest.UnitPatterns.Where(p => p.Name == name).ToList();
            if (patterns.Count == 0)
            {
                maker.Debug("Cannot find unit pattern with name: " + name);
                return;
            }
            var pattern = patterns[0];
            float gridTiles = pattern.Resolution * tileScale * GRID_SIZE;
            LayoutPattern(pattern, (instance, index, relLoc) =>
            {
                Point scaledLocation = new Point(
                    relLoc.X * gridTiles + center.X,
                    relLoc.Y * gridTiles + center.Y
                );

                return maker.TryPlaceUnit(instance.Name, instance.Id, ownerID, scaledLocation);
            });
        }

    }
}
