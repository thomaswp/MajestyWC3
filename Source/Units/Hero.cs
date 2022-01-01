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
    public abstract class Hero : UnitAI
    {
        public const int HERO_EXPLORE_TICK_XP = 3;

        protected location exploringLocation;
        protected void UpdateExploration()
        {
            location loc = GetUnitLoc(unit);
            float x = GetLocationX(loc), y = GetLocationY(loc);
            rect mapRect = GetPlayableMapRect();

            if (exploringLocation != null)
            {
                float ex = GetLocationX(exploringLocation), ey = GetLocationY(exploringLocation);
                float length = (float) Math.Sqrt(Math.Pow(x - ex, 2) + Math.Pow(y - ey, 2));
                float sight = BlzGetUnitRealField(unit, UNIT_RF_SIGHT_RADIUS);
                float perc = sight * 1.2f / length;
                location check = Location(Util.lerp(x, ex, perc), Util.lerp(y, ey, perc));
                if (RectContainsLoc(mapRect, check) && IsLocationMaskedToPlayer(check, humanPlayer))
                {
                    AddHeroXP(unit, HERO_EXPLORE_TICK_XP, true);
                }
                PingMinimapLocForPlayer(humanPlayer, check, 0.5f);

                if (DistanceBetweenPoints(GetUnitLoc(unit), exploringLocation) > 50) return;
            }

            //Console.WriteLine($"Updating exploration...");

            exploringLocation = null;
            int radius = 500;
            for (int i = 0; i < 100; i++)
            {
                location check = GetRandomLocInRect(Rect(x - radius, y - radius, x + radius, y + radius));
                //Console.WriteLine($"Checking {GetLocationX(check)}, {GetLocationY(check)}");
                if (RectContainsLoc(mapRect, check) && IsLocationMaskedToPlayer(check, humanPlayer))
                {
                    exploringLocation = check;
                    break;
                }
                radius += 50;
            }
            if (exploringLocation != null)
            {
                float ex = GetLocationX(exploringLocation), ey = GetLocationY(exploringLocation);
                IssuePointOrderByIdLoc(unit, Constants.ORDER_MOVE, exploringLocation);
                //PingMinimap(ex, ey, 1);
                Console.WriteLine($"Knight exploring {ex}, {ey}");
            }
        }
    }
}
