using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source;

namespace Source.Units.Monsters
{
    public abstract class Spawners
    {
        static readonly Dictionary<int, List<int>> BUILDINGS = new Dictionary<int, List<int>>()
        {
            {  Constants.UNIT_KOBOLD_CAMP, new List<int>() { Constants.UNIT_KOBOLD_WARRIOR, } },
            { Constants.UNIT_DRAENEI_CAMP, new List<int>() { Constants.UNIT_DRAENEI_WARRIOR, } },
            { Constants.UNIT_FOREST_TROLL_CAMP, new List<int>() { Constants.UNIT_FOREST_TROLL_WARRIOR, } },
        };

        //static readonly List<int> ENEMY_UNITS = BUILDINGS.Values.ToList();
        static readonly List<int> ENEMY_BUILDINGS = BUILDINGS.Keys.ToList();
        public static readonly player MonsterPlayer = Player(GetPlayerNeutralAggressive());
        public const int MIN_SPAWN_DISTANCE = 1500;
        public const float DELAY_BASE = 0; // TODO: Much higher
        public const float DIS_DELAY_FACTOR = 60 / 3000f;

        public static void SpawCamps(int nCamps = 10)
        {
            var weights = ENEMY_BUILDINGS.Select(u => GetCampSpawnWeight(u));
            int totalWeight = weights.Sum();
            List<float> normalizedWeights = weights.Select(w => (float)w / totalWeight).ToList();

            //Console.WriteLine("Gold: " + GetUnitGoldCost(Constants.UNIT_KOBOLD_CAMP));

            rect mapArea = GetPlayableMapRect();
            int margin = 500;
            rect spawnArea = Rect(
                GetRectMinX(mapArea) - margin,
                GetRectMinY(mapArea) - margin,
                GetRectMaxX(mapArea) - margin,
                GetRectMaxY(mapArea) - margin
            );

            var spawnLocations = HaltonLocationSequence(spawnArea, 2, 3);

            int campsMade = 0;

            int tries = 0;
            foreach (location spawnLoc in spawnLocations)
            {
                tries++;
                // Safety to preven infinite loop
                if (tries > 1000) break;

                //Console.WriteLine("Trying to spawn at " + spawnLoc.ToXY());

                float playerDis = DistanceToClosestPlayer(spawnLoc);
                if (playerDis < MIN_SPAWN_DISTANCE)
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

                unit building = CreateSpawnCamp(buildingID, spawnLoc, playerDis);
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
                    unit extraBuilding = CreateSpawnCamp(buildingID, extraLoc, playerDis);
                    if (extraBuilding == null) continue;
                    extras--;
                }

            }
        }

        private static List<location> playerLocations = new List<int>
        {
            0
        }.Select(p => GetPlayerStartLocationLoc(Player(p))).ToList();

        private static float DistanceToClosestPlayer(location loc)
        {
            return playerLocations.Select(l => DistanceBetweenPoints(l, loc)).Min();
        }

        private static unit CreateSpawnCamp(int campID, location location, float playerDistance)
        {
            unit camp = CreateUnitAtLoc(MonsterPlayer, campID, location, 0);
            float delay = DELAY_BASE + (playerDistance - MIN_SPAWN_DISTANCE) * DIS_DELAY_FACTOR;
            delay *= Util.RandBetween(0.75f, 1.25f);
            SpawnIn(camp, delay);
            return camp;
        }

        private static void SpawnIn(unit camp, float delay)
        {
            if (camp.IsDead()) return;
            //Console.WriteLine($"Delay: {delay}");
            try
            {
                timer timer = CreateTimer();
                TimerStart(timer, delay, false, () =>
                {
                    DestroyTimer(timer);
                    Spawn(camp);
                    float period = GetCampSpawnPeriod(camp.GetTypeID());
                    if (period == 0)
                    {
                        Console.WriteLine("Invalid spawn period: " + camp.GetName());
                        return;
                    }
                    period *= Util.RandBetween(0.75f, 1.25f);
                    SpawnIn(camp, period);
                });
            } catch (Exception e)
            {
                Console.WriteLine($"Failed to spawn for {camp.GetName()}: {e.Message}");
            }
        }

        private static void Spawn(unit camp)
        {
            //Console.WriteLine("Trying to spawn...");
            if (!BUILDINGS.TryGetValue(camp.GetTypeID(), out List<int> unitTypes)) return;
            int unitType = unitTypes[GetRandomInt(0, unitTypes.Count - 1)];
            unit spawn = CreateUnitAtLoc(camp.GetPlayer(), unitType, camp.GetLocation(), 0);
            UnitAI.RegisterUnit(spawn);
        }

        protected static int GetCampSpawnWeight(int campType)
        {
            return GetUnitPointValueByType(campType);
        }

        protected static int GetCampSpawnNumber(int campType)
        {
            return GetFoodMade(campType);
        }

        protected static int GetCampSpawnPeriod(int campType)
        {
            int period = GetUnitGoldCost(campType);
            //Console.WriteLine($"Spawn time for {campType} is {period}");
            return period;
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
            //Console.WriteLine("Starting HS...");
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
                //Console.WriteLine("Yielding " + v);
                yield return v;
            }
        }

    }
}
