using Source.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source;

namespace Source.Behaviors
{
    public class Explore : Behavior
    {
        public const int HERO_EXPLORE_TICK_XP = 3;

        protected location exploringLocation;

        public override bool CanStart()
        {
            SelectTarget();
            return exploringLocation != null;
        }

        public override void Start()
        {
            SelectTarget();
            if (exploringLocation == null) return;
            float ex = GetLocationX(exploringLocation), ey = GetLocationY(exploringLocation);
            IssuePointOrderByIdLoc(AI.Unit, Constants.ORDER_MOVE, exploringLocation);
            //PingMinimap(ex, ey, 1);
            Console.WriteLine($"Knight exploring {ex}, {ey}");
        }

        public override void Stop()
        {
            exploringLocation = null;
        }

        public override bool Update()
        {
            //Console.WriteLine($"Updating exploration...");

            if (exploringLocation == null) return false;

            location loc = GetUnitLoc(AI.Unit);
            float x = GetLocationX(loc), y = GetLocationY(loc);
            rect mapRect = GetPlayableMapRect();


            float ex = GetLocationX(exploringLocation), ey = GetLocationY(exploringLocation);
            float length = (float)Math.Sqrt(Math.Pow(x - ex, 2) + Math.Pow(y - ey, 2));
            float sight = AI.Unit.GetSightRange();
            float perc = sight * 1.2f / length;
            location check = Location(Util.lerp(x, ex, perc), Util.lerp(y, ey, perc));
            if (RectContainsLoc(mapRect, check) && IsLocationMaskedToPlayer(check, AI.HumanPlayer))
            {
                AddHeroXP(AI.Unit, HERO_EXPLORE_TICK_XP, true);
            }
            //PingMinimapLocForPlayer(humanPlayer, check, 0.5f);

            // TODO: Configure continue exploring parameters
            if (DistanceBetweenPoints(GetUnitLoc(AI.Unit), exploringLocation) <= 50) return false;

            return true;
        }

        private bool isMasked(location check, rect mapRect)
        {
            return RectContainsLoc(mapRect, check) && IsLocationMaskedToPlayer(check, AI.HumanPlayer);
        }

        private void SelectTarget()
        {
            rect mapRect = GetPlayableMapRect();
            location loc = GetUnitLoc(AI.Unit);
            float x = GetLocationX(loc), y = GetLocationY(loc);

            if (exploringLocation != null)
            {
                if (isMasked(exploringLocation, mapRect)) return;
            }

            exploringLocation = null;
            int radius = 500;
            for (int i = 0; i < 100; i++)
            {
                location check = GetRandomLocInRect(Rect(x - radius, y - radius, x + radius, y + radius));
                //Console.WriteLine($"Checking {GetLocationX(check)}, {GetLocationY(check)}");
                if (isMasked(check, mapRect))
                {
                    exploringLocation = check;
                    return;
                }
                radius += 50;
            }
        }
    }
}
