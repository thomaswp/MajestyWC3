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
    public class Knight : Hero
    {
        protected override Preferences GetPreferences()
        {
            return new Preferences
            {
                Fight = 10,
                Explore = 5,
                Rest = 3,
                Glory = 10,
            };
        }

        protected override int GetBaseWeaponID()
        {
            return Constants.ITEM_SWORD_LEVEL_1;
        }

        protected override int GetBaseArmorID()
        {
            return Constants.ITEM_PLATE_ARMOR_LEVEL_1;
        }

    }
}
