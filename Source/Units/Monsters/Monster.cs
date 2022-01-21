using Source.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using WCSharp.Events;
using Source.Interface;

namespace Source.Units.Monsters
{
    public abstract class Monster : FighterAI
    {
        public static int PlayerID = 8;
        public static player Player = Player(PlayerID);

        public static void Init()
        {
            AnyUnitEvents.Register(EVENT_PLAYER_UNIT_DEATH, () =>
            {
                Console.WriteLine("Unit dies");
                try
                {
                    unit unit = GetTriggerUnit();
                    if (GetKillingUnit() != null)
                    {
                        Console.WriteLine(GetKillingUnit().GetName());
                    }
                    int type = unit.GetTypeID();
                    //Console.WriteLine($"Trying to create bounty for {unit.GetName()}");
                    if (!Spawners.ENEMY_UNITS.Contains(type) &&
                        !Spawners.ENEMY_BUILDINGS.Contains(type))
                    {
                        return;
                    }
                    int award = unit.RollBountyAward();
                    if (award == 0) return;
                    Bounties.AwardBounty(unit.GetLocation(), award, ai => ai.IsEnemy(unit));
                } catch (Exception e)
                {
                    Console.WriteLine("Error with bounty: " + e.Message);
                }
            });
        }

        protected override void AddBehaviors()
        {
            AddBehavior(new Wander(), 5);
            AddBehavior(new Fight(), 10);
            AddBehavior(new DefendHome());
            AddBehavior(new Raid(), 3);
        }

        public override void OnAttacked(unit attacker)
        {
            base.OnAttacked(attacker);
            Console.WriteLine($"{Name} attacked by {attacker} => {behavior.GetName()}");
        }
    }
}
