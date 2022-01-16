using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source;

namespace Source.Units
{
    public abstract class Monster : FighterAI
    {
        static readonly Dictionary<int, int> BUILDINGS = new Dictionary<int, int>()
        {
            { Constants.UNIT_KOBOLD_WARRIOR, Constants.UNIT_KOBOLD_CAMP },
            { Constants.UNIT_DRAENEI_WARRIOR, Constants.UNIT_DRAENEI_CAMP },
            { Constants.UNIT_FOREST_TROLL_WARRIOR, Constants.UNIT_FOREST_TROLL_CAMP },
        };

        static readonly List<int> ENEMY_UNITS = BUILDINGS.Keys.ToList();
        static readonly List<int> ENEMY_BUILDINGS = BUILDINGS.Values.ToList();
        public static readonly player MonsterPlayer = Player(GetPlayerNeutralAggressive());
        public const int MIN_SPAWN_DISTANCE = 1500;

        public static void SpawCamps(int nCamps)
        {
            var weights = ENEMY_BUILDINGS.Select(u => GetCampSpawnWeight(u));
            int totalWeight = weights.Sum();
            List<float> normalizedWeights = weights.Select(w => (float)w / totalWeight).ToList();


            rect mapArea = GetPlayableMapRect();
            int margin = 500;
            rect spawnArea = Rect(
                GetRectMinX(mapArea) - margin,
                GetRectMinY(mapArea) - margin,
                GetRectMaxX(mapArea) - margin,
                GetRectMaxY(mapArea) - margin
            );

            var spawnLocations = HaltonLocationSequence(spawnArea, 2, 3);

            // TODO: All players
            location startLoc = GetPlayerStartLocationLoc(Player(0));
            int campsMade = 0;

            int tries = 0;
            foreach (location spawnLoc in spawnLocations)
            {
                tries++;
                // Safety to preven infinite loop
                if (tries > 1000) break;

                Console.WriteLine("Trying to spawn at " + spawnLoc.ToXY());

                if (DistanceBetweenPoints(startLoc, spawnLoc) < MIN_SPAWN_DISTANCE)
                {
                    continue;
                }
                
                int buildingIndex;
                float rand = GetRandomInt(0, 999) / 1000f;
                for (buildingIndex = 0; buildingIndex < normalizedWeights.Count; buildingIndex++)
                {
                    float weight = normalizedWeights[buildingIndex];
                    if (rand <= weight) break;
                    rand -= weight;
                }
                int buildingID = ENEMY_BUILDINGS[buildingIndex];
                //Console.WriteLine($"Trying to spawn camp #{buildingIndex}");

                unit building = CreateSpawnCamp(buildingID, spawnLoc);
                if (building == null)
                {
                    //Console.WriteLine($"Failed, retrying");
                    continue;
                }
                campsMade++;
                if (campsMade >= nCamps) break;

                int extras = GetCampSpawnNumber(buildingID) - 1;
                rect extraRect = RectFromCenterSizeBJ(spawnLoc, 1000, 1000);
                for (int j = 0; j < 10; j++)
                {
                    if (extras <= 0) break;
                    //Console.WriteLine($"Trying to spawn extra camp");
                    location extraLoc = GetRandomLocInRect(extraRect);
                    unit extraBuilding = CreateSpawnCamp(buildingID, extraLoc);
                    if (extraBuilding == null) continue;
                    extras--;
                }

            }
        }

        private static unit CreateSpawnCamp(int campID, location location)
        {
            // TODO: register
            return CreateUnitAtLoc(MonsterPlayer, campID, location, 0);
        }

        protected static int GetCampSpawnWeight(int campType)
        {
            return GetUnitPointValueByType(campType);
        }

        protected static int GetCampSpawnNumber(int campType)
        {
            return GetFoodMade(campType);
        }

        private static IEnumerable<location> HaltonLocationSequence(rect bounds, int bx, int by)
        {
            float minX = GetRectMinX(bounds);
            float minY = GetRectMinY(bounds);
            float maxX = GetRectMaxX(bounds);
            float maxY = GetRectMaxY(bounds);
            float sizeX = maxX - minX, sizeY = maxY - minY;

            var xSeq = HaltonSequence(bx).GetEnumerator();
            var ySeq = HaltonSequence(by).GetEnumerator();

            //Console.WriteLine("Attempting zip...");
            //var xySeq = xSeq.Zip(ySeq, (x, y) => new { X = x, Y = y });
            //Console.WriteLine("Succeess");
            while (true)
            {
                xSeq.MoveNext();
                ySeq.MoveNext();
                float x = xSeq.Current, y = ySeq.Current;
                yield return Location(minX + sizeX * x, minY + sizeY * y);
            }
        }

        private static IEnumerable<float> HaltonSequence(int b)
        {
            Console.WriteLine("Starting HS...");
            int n = 0, d = 1;
            while (true)
            {
                int x = d - n;
                if (x == 1)
                {
                    n = 1;
                    d *= b;
                }
                else
                {
                    int y = d;
                    while (x <= y)
                    {

                        y /= b;
                        n = (b + 1) * y - x;
                    }
                }
                float v = (float)n / d;
                Console.WriteLine("Yielding " + v);
                yield return v;
            }
        }

    }
}
