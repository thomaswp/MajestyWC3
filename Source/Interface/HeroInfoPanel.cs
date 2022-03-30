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
    public class HeroInfoPanel
    {
        public static void Init()
        {
            LoadTemplatesTOC();
            framehandle statusText = null, untaxedGoldText = null, taxedGoldText = null;
            framehandle parent = AddUnitInfoPanelEx(unit =>
            {
                UnitAI ai = UnitAI.GetAI(unit);
                if (ai == null) return;
                BlzFrameSetText(taxedGoldText, $"{ai.GoldTaxed}");
                BlzFrameSetText(untaxedGoldText, $"{ai.GoldUntaxed}");
                BlzFrameSetText(statusText, ai.Status);
            }, unit =>
            {
                var ai = UnitAI.GetAI(unit);
                return ai != null && ai.HasStatusPanel();
            });

            float statusHeight = 0.035f;

            var frameParent = SetUnitInfoPanelFrameEx();
            statusText = BlzCreateFrameByType("TEXT", "", frameParent, "CustomUnitStatusText", 0);
            BlzFrameSetPoint(statusText, FRAMEPOINT_TOP, BlzGetFrameByName(FRAME_LevelBar, 0), FRAMEPOINT_BOTTOM, 0, -0.001f);
            BlzFrameSetSize(statusText, 0.175f, statusHeight);

            var taxedInfo = UnitInfoCreateCustomInfo(parent, " Taxed Gold:",
                @"UI\Widgets\ToolTips\Human\ToolTipGoldIcon", unit =>
            {
                UnitAI ai = UnitAI.GetAI(unit);
                if (ai == null) return "";
                return $"This hero has {ai.GoldTaxed} where taxed have alrady been paid.";
            });
            taxedGoldText = taxedInfo.textFrame;
            BlzFrameSetPoint(taxedInfo.iconFrame, FRAMEPOINT_TOPLEFT,
                BlzGetFrameByName(FRAME_LevelBar, 0), FRAMEPOINT_BOTTOMLEFT, 0, -statusHeight);

            var untaxedInfo = UnitInfoCreateCustomInfo(parent, " Untaxed Gold:",
                @"UI\Widgets\ToolTips\Human\ToolTipGoldIcon", unit =>
            {
                UnitAI ai = UnitAI.GetAI(unit);
                if (ai == null) return "";
                return $"This hero has {ai.GoldUntaxed} untaxed gold.\n" +
                $"When the hero returns to its home, it will pay taxes on this gold through its guild.";
            });
            untaxedGoldText = untaxedInfo.textFrame;
            BlzFrameSetPoint(untaxedInfo.iconFrame, FRAMEPOINT_TOPLEFT, taxedInfo.iconFrame,
                FRAMEPOINT_TOPLEFT, 0.085f, 0);
        }
    }
}
