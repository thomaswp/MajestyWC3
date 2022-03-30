using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using static Source.Interface.UnitInfoPanels;
using Source.Units;

namespace Source.Interface
{
    public class TaxableInfoPanel
    {
        public static void Init()
        {
            LoadTemplatesTOC();
            framehandle goldText = null;
            framehandle parent = AddUnitInfoPanelEx(unit =>
            {
                Building building = Buildings.Get(unit);
                if (building == null) return;
                BlzFrameSetText(goldText, $"{building.Gold}");
            }, unit =>
            {
                return Buildings.CanBeTaxed(unit);
            });

            float statusHeight = 0.035f;

            var goldInfo = UnitInfoCreateCustomInfo(parent, " Gold in coffers:",
                @"UI\Widgets\ToolTips\Human\ToolTipGoldIcon", unit =>
            {
                Building building = Buildings.Get(unit);
                if (building == null) return "";
                return $"This building has {building.Gold} gold, which can be collected by the Tax Collector.";
            });
            goldText = goldInfo.textFrame;
            BlzFrameSetPoint(goldInfo.iconFrame, FRAMEPOINT_TOPLEFT,
                BlzGetFrameByName(FRAME_LevelBar, 0), FRAMEPOINT_BOTTOMLEFT, 0, -statusHeight);
        }
    }
}
