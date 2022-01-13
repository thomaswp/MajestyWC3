using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static War3Api.Common;
using static War3Api.Blizzard;
using Source;
using Source.Behaviors;
using Source.Items;

namespace Source.Units
{
    public abstract class Hero : UnitAI
    {
        // TODO: Lower?
        public const int STARTING_GOLD = 100;

        public static List<int> itemPriorities = new List<int>()
        {
            Constants.ITEM_HEALING_POTION_LEVEL_2,
            Constants.ITEM_HEALING_POTION_LEVEL_2,
            Constants.ITEM_HEALING_POTION_LEVEL_2,
            Constants.ITEM_HEALING_POTION_LEVEL_2,
            Constants.ITEM_HEALING_POTION_LEVEL_2,
            Constants.ITEM_HEALING_POTION_LEVEL_1,
            Constants.ITEM_HEALING_POTION_LEVEL_1,
            Constants.ITEM_HEALING_POTION_LEVEL_1,
            Constants.ITEM_HEALING_POTION_LEVEL_1,
            Constants.ITEM_HEALING_POTION_LEVEL_1,
        };

        protected override void AddBehaviors()
        {
            Preferences prefs = GetPreferences();
            AddBehavior(new Explore(), prefs.Explore);
            AddBehavior(new BountyExploreFlag(), prefs.Explore);
            AddBehavior(new DefendHome());
            AddBehavior(new Fight(), prefs.Fight);
            AddBehavior(new BountyAttackFlag(), prefs.Fight);
            AddBehavior(new Shop(), 5);
            AddBehavior(new RestAtHome(), prefs.Rest);
            AddBehavior(new Flee());
        }

        public struct Preferences
        {
            public int Explore, Fight, Rest;
        }

        protected abstract Preferences GetPreferences();

        protected override IEnumerable<int> GetWantedItemsList()
        {
            return itemPriorities;
        }

        protected override void Init(unit unit)
        {
            base.Init(unit);
            Gold += STARTING_GOLD;
        }

        protected override void DoPreBehaviorActions()
        {
            base.DoPreBehaviorActions();
            if (CheckHealingPotions()) return;
        }

        protected bool CheckHealingPotions()
        {
            if (behavior == null || !behavior.IsInCombatOrDanger()) return false;
            bool interrupted = false;
            bool needsHeal = Unit.GetHPFraction() < 0.3f;
            if (Unit.HasItem(Constants.ITEM_HEALING_POTION_LEVEL_2) &&
                (needsHeal || Unit.GetDamage() >= Items.Items.HP2_HEALING))
            {
                int order = Unit.GetSlotOrderForItem(Constants.ITEM_HEALING_POTION_LEVEL_2);
                IssueImmediateOrderById(Unit, order);
                //Console.WriteLine("Using HP2");
                interrupted = true;
            } else if (Unit.HasItem(Constants.ITEM_HEALING_POTION_LEVEL_1) &&
                (needsHeal || Unit.GetDamage() >= Items.Items.HP1_HEALING))
            {
                int order = Unit.GetSlotOrderForItem(Constants.ITEM_HEALING_POTION_LEVEL_1);
                IssueImmediateOrderById(Unit, order);
                //Console.WriteLine("Using HP1");
                interrupted = true;
            }
            if (interrupted && behavior != null)
            {
                behavior.NeedsRestart = true;
            }

            return true;
        }


        public override void OnAttack(unit target)
        {
            base.OnAttack(target);

            int exp = GetUnitLevel(target);
            AddHeroXP(Unit, exp, true);
        }

        public override void OnAttacked(unit attacker)
        {
            base.OnAttacked(attacker);

            //Console.WriteLine("Trying Flee");
            if (TryInterruptWith(typeof(Flee), true)) return;
            //Console.WriteLine("Trying fight");
            if (TryInterruptWith(typeof(Fight), true)) return;
            //if (!(behavior is Fight)) Console.WriteLine("Failed...");
        }

        public override void OnHomeAttacked(unit attacker)
        {
            base.OnHomeAttacked(attacker);
            DefendHome d = (DefendHome) GetBehavior(typeof(DefendHome));
            if (d == null) return;
            d.OnHomeAttackedBy(attacker);
            TryInterruptWith(d, false);
        }

    }
}
