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
    public class Ranger : Hero
    {
        protected override Preferences GetPreferences()
        {
            return new Preferences
            {
                Fight = 5,
                Explore = 10,
                Rest = 3,
                Glory = 2,
            };
        }

        protected override ItemInfo GetBaseWeaponID()
        {
            return Constants.ITEM_BOW_LEVEL_1;
        }

        protected override ItemInfo GetBaseArmorID()
        {
            return Constants.ITEM_HIDE_ARMOR_LEVEL_1;
        }
        
        public override int[] GetAbilityIDs()
        {
            return new int[] {
                Constants.ABILITY_ENTANGLING_ROOTS_RANGER,
                Constants.ABILITY_TRUESHOT_AURA_RANGER,
                Constants.ABILITY_WIND_WALK_RANGER,
            };
        }
    }
}
