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
    public class Explore : TargetBehavior<location>
    {
        public const int HERO_EXPLORE_TICK_XP = 3;


        public override void Start()
        {
            base.Start();
            float ex = GetLocationX(Target), ey = GetLocationY(Target);
            IssuePointOrderByIdLoc(AI.Unit, Constants.ORDER_MOVE, Target);
            //PingMinimap(ex, ey, 1);
            Console.WriteLine($"Knight exploring {ex}, {ey}");
        }

        public override bool Update()
        {
            if (!base.Update()) return false;

            location loc = GetUnitLoc(AI.Unit);
            float x = GetLocationX(loc), y = GetLocationY(loc);
            rect mapRect = GetPlayableMapRect();


            float ex = GetLocationX(Target), ey = GetLocationY(Target);
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
            if (DistanceBetweenPoints(GetUnitLoc(AI.Unit), Target) <= 50) return false;

            return true;
        }

        protected bool IsMasked(location check, rect mapRect)
        {
            return RectContainsLoc(mapRect, check) && IsLocationMaskedToPlayer(check, AI.HumanPlayer);
        }

        protected override bool IsTargetStillValid(location target)
        {
            return IsLocationMaskedToPlayer(target, AI.HumanPlayer);
        }

        protected override location SelectTarget()
        {
            rect mapRect = GetPlayableMapRect();
            location loc = GetUnitLoc(AI.Unit);
            float x = GetLocationX(loc), y = GetLocationY(loc);

            int radius = 500;
            for (int i = 0; i < 100; i++)
            {
                location check = GetRandomLocInRect(Rect(x - radius, y - radius, x + radius, y + radius));
                //Console.WriteLine($"Checking {GetLocationX(check)}, {GetLocationY(check)}");
                if (IsMasked(check, mapRect))
                {
                    return check;
                }
                radius += 50;
            }
            return null;
        }
    }
}
