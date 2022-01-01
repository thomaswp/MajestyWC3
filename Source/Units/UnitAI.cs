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
    public abstract class UnitAI
    {

        public unit unit { get; private set; }
        public player myPlayer { get; private set; }
        public player humanPlayer { get; private set; }

        public UnitAI()
        {
        }

        private void init(unit unit)
        {
            this.unit = unit;
            myPlayer = GetOwningPlayer(unit);
            humanPlayer = myPlayer.GetHumanForAI();
        }

        public abstract void Update();

        static Dictionary<unit, UnitAI> unitMap = new Dictionary<unit, UnitAI>();

        public static void RegisterUnit(unit unit)
        {
            if (IsUnitIdType(GetUnitTypeId(unit), UNIT_TYPE_STRUCTURE)) return;

            if (!unitMap.ContainsKey(unit))
            {
                int unitID = GetUnitTypeId(unit);
                UnitAI ai = null;
                switch (unitID)
                {
                    case Constants.UNIT_PEASANT_WORKER:
                        ai = new Peasant();
                        break;
                    case Constants.UNIT_KNIGHT:
                        ai = new Knight();
                        break;
                    case Constants.UNIT_BUILDER:
                        return;
                }
                if (ai != null)
                {
                    Console.WriteLine($"Creating new AI for {unit.Name()}");
                    ai.init(unit);
                    unitMap[unit] = ai;
                }
                else
                {
                    Console.WriteLine($"Unknown AI for {unit.Name()}");
                }
            }
        }

        public static void UpdateUnits()
        {
            //Console.WriteLine($"Updating {unitMap.Count} units...");
            foreach (var unit in unitMap.Values)
            {
                //if (unit is Knight) Console.WriteLine("Knight!");
                try
                {
                    unit.Update();
                } catch (Exception e)
                {
                    Console.WriteLine($"Error with {GetUnitName(unit.unit)}: {e.Message}");
                }
            }
        }
    }
}
