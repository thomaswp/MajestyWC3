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
    public abstract class Hero : FighterAI
    {
        // TODO: Lower?
        public const int STARTING_GOLD = 1000;


        private Preferences prefs;

        public Hero()
        {
            prefs = GetPreferences();
        }

        protected override void AddBehaviors()
        {
            AddBehavior(new Explore(), prefs.Explore);
            AddBehavior(new BountyExploreFlag(), prefs.Explore);
            AddBehavior(new DefendBuilding());
            AddBehavior(new DefendRealm());
            AddBehavior(new Fight(), prefs.Fight);
            AddBehavior(new BountyAttackFlag(), prefs.Fight);
            AddBehavior(new Shop(), 5);
            AddBehavior(new RestAtHome(), prefs.Rest);
            AddBehavior(new Flee());
            AddBehavior(new Wander(), 1);
            AddBehavior(new Raid(), (prefs.Fight + prefs.Glory) / 2);
        }

        public override bool HasStatusPanel()
        {
            return true;
        }

        public struct Preferences
        {
            public int Explore, Fight, Rest, Glory;
        }

        protected abstract Preferences GetPreferences();
        protected abstract ItemInfo GetBaseWeaponID();
        protected abstract ItemInfo GetBaseArmorID();

        protected override IEnumerable<int> GetWantedItemsList()
        {
            for (int i = 0; i < 2; i++)
            {
                yield return Constants.ITEM_HEALING_POTION_LEVEL_1;
            }

            ItemInfo weaponID = GetBaseWeaponID();
            ItemInfo armorID = GetBaseArmorID();

            int[] weapons = weaponID.UpgradeChain;
            int[] armors = armorID.UpgradeChain;

            for (int i = 0; i < weapons.Length || i < armors.Length; i++)
            {
                if (i < weapons.Length) yield return weapons[i];
                if (i < armors.Length) yield return armors[i];
                yield return Constants.ITEM_HEALING_POTION_LEVEL_1;
                yield return Constants.ITEM_HEALING_POTION_LEVEL_2;
            }
        }

        protected override void Init(unit unit)
        {
            base.Init(unit);
            GoldTaxed += STARTING_GOLD;

            //GetWantedItemsList().ToList().ForEach(i => Console.WriteLine("" + (ItemInfo)i));
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
            if (Unit.HasExactItem(Constants.ITEM_HEALING_POTION_LEVEL_2) &&
                (needsHeal || Unit.GetDamage() >= Items.Items.HP2_HEALING))
            {
                int order = Unit.GetSlotOrderForItem(Constants.ITEM_HEALING_POTION_LEVEL_2);
                IssueImmediateOrderById(Unit, order);
                //Console.WriteLine("Using HP2");
                interrupted = true;
            } else if (Unit.HasExactItem(Constants.ITEM_HEALING_POTION_LEVEL_1) &&
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

        public override void OnRealmAttacked(unit building, unit attacker)
        {
            base.OnRealmAttacked(building, attacker);

            float diminishRange = building.IsCastle() ? 7000 : 1000;
            float distance = Unit.DistanceTo(building);
            float willingness = Math.Max(1, diminishRange / distance);
            willingness *= prefs.Glory * Util.RandFloat();
            //Console.WriteLine($"Realm attacked for {Name}; willingness={willingness}");

            if (willingness < 1) return;

            DefendRealm d = (DefendRealm)GetBehavior(typeof(DefendRealm));
            if (d == null) return;
            d.OnHomeAttackedBy(attacker, building);
            TryInterruptWith(d, false);
        }

        public static readonly int[] AbilityIDs = new int[]
        {
            Constants.ABILITY_ENTANGLING_ROOTS_RANGER,
            Constants.ABILITY_TRUESHOT_AURA_RANGER,

        };

        protected void TryLearnAbilities()
        {
            // TODO: It seems there's no way to know if a hero has an ability available
            // So we have can do one of the following:
            // - Try leveling up all hero skills, and repeat if the available points goes down (downside: can't distribute points).
            //   Would need to probably have this trigger on level up and research, rather than each frame (not a bad idea regardless)
            // - Add the data redundantly to each Hero class (this is probably simpler). If the redundancy is annoying, could just add the skill
            //   manually when researched, and not put it in the editor. This would also allow each guild to have separate tech.
            int availablePoints = GetHeroSkillPoints(Unit);
            if (availablePoints == 0) return;
            Console.WriteLine($"Points {Unit.GetName()}: {availablePoints}");
            foreach (int ability in AbilityIDs)
            {
                int level = GetUnitAbilityLevel(Unit, ability);
                Console.WriteLine($"Level for {Unit.GetName()} of {ability}: {level}");
                SelectHeroSkill(Unit, ability);
            }
        }

        public override void Update()
        {
            base.Update();
            TryLearnAbilities();
        }

    }
}
