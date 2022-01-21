using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Blizzard;
using static War3Api.Common;
using WCSharp.Events;
using Source.Units;

namespace Source.Interface
{
    public static class Status
    {
        public static void Init()
        {
            PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeIsSelected, () =>
            {
                unit selected = GetTriggerUnit();
                // TODO: Maybe allow allies?
                // TODO: Temp disabled for debugging
                //if (selected.GetPlayer().GetHumanForAI() != GetTriggerPlayer()) return;
                if (selected.IsDead() || selected.IsStructure()) return;
                UnitAI ai = UnitAI.GetAI(selected);
                if (ai == null) return;
                ShowStatus(ai);
            });
        }

        public static void ShowStatus(UnitAI ai)
        {
            // TODO: Clear prior status
            TextTag.ShowTextTag(ai.Unit, ai.Status, Color.STATUS);
        }
    }
}
