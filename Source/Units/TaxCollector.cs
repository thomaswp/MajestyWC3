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
    public class TaxCollector : UnitAI
    {
        public override bool HasStatusPanel()
        {
            return true;
        }

        protected override void AddBehaviors()
        {
            AddBehavior(new Behaviors.TaxCollect(), 1);
            AddBehavior(new Behaviors.ReturnTaxes(), 1);
            // TODO: This won't work with upgrades...
            var castles = GetUnitsOfPlayerAndTypeId(HumanPlayer, Constants.UNIT_CASTLE_LEVEL_1).ToList();
            if (castles.Count >= 1)
            {
                SetHome(castles[0]);
            } else
            {
                Console.WriteLine("No castle for tax collector...");
            }
        }
    }
}
