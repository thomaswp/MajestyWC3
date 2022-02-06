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
    /**
     * 
-- Examle for a non SimpleFrame Button
-- Shows how much gold and Lumber bounty one gets when slaying an unit
do
    local realFunction = MarkGameStarted
    local lumberText, goldText, parent
    local createContext, infoFrame, iconFrame, labelFrame, textFrame
    local function Init()
        -- create a new Unit Info Panel, this panel can only be shown when the current selected unit's owner gives bounty
        parent = AddUnitInfoPanelEx(function(unit)
            local min, max
            min = BlzGetUnitIntegerField(unit, UNIT_IF_GOLD_BOUNTY_AWARDED_NUMBER_OF_DICE) + BlzGetUnitIntegerField(unit, UNIT_IF_GOLD_BOUNTY_AWARDED_BASE)
            max = BlzGetUnitIntegerField(unit, UNIT_IF_GOLD_BOUNTY_AWARDED_NUMBER_OF_DICE) *BlzGetUnitIntegerField(unit, UNIT_IF_GOLD_BOUNTY_AWARDED_SIDES_PER_DIE) + BlzGetUnitIntegerField(unit, UNIT_IF_GOLD_BOUNTY_AWARDED_BASE)
            BlzFrameSetText(goldText, min.." - "..max)
            min = BlzGetUnitIntegerField(unit, UNIT_IF_LUMBER_BOUNTY_AWARDED_NUMBER_OF_DICE) + BlzGetUnitIntegerField(unit, UNIT_IF_LUMBER_BOUNTY_AWARDED_BASE)
            max = BlzGetUnitIntegerField(unit, UNIT_IF_LUMBER_BOUNTY_AWARDED_NUMBER_OF_DICE) *BlzGetUnitIntegerField(unit, UNIT_IF_LUMBER_BOUNTY_AWARDED_SIDES_PER_DIE) + BlzGetUnitIntegerField(unit, UNIT_IF_LUMBER_BOUNTY_AWARDED_BASE)
            BlzFrameSetText(lumberText, min.." - "..max)
        end,
        function(unit) return IsPlayerFlagSetBJ(PLAYER_STATE_GIVES_BOUNTY, GetOwningPlayer(unit)) end)
        -- define locals
        
        -- create a new custom Info and load all created frames into the locals
        createContext, infoFrame, iconFrame, labelFrame, textFrame = UnitInfoCreateCustomInfo(parent, " Gold:", "UI\\Widgets\\ToolTips\\Human\\ToolTipGoldIcon", function(unit)
        -- this function returns the text shown inside the tooltip when this UnitInfo is mouse hovered.
            local min, max
            min = BlzGetUnitIntegerField(unit, UNIT_IF_GOLD_BOUNTY_AWARDED_NUMBER_OF_DICE) + BlzGetUnitIntegerField(unit, UNIT_IF_GOLD_BOUNTY_AWARDED_BASE)
            max = BlzGetUnitIntegerField(unit, UNIT_IF_GOLD_BOUNTY_AWARDED_NUMBER_OF_DICE) *BlzGetUnitIntegerField(unit, UNIT_IF_GOLD_BOUNTY_AWARDED_SIDES_PER_DIE) + BlzGetUnitIntegerField(unit, UNIT_IF_GOLD_BOUNTY_AWARDED_BASE)
            return "Bounty Gold: "..min.." - "..max
            .."\nWhen an unit owned by you kills this unit, you gain this amount of Gold.\nOnly unallied unit give bounty."
        end)
        local prevIcon = iconFrame
        goldText = textFrame
        BlzFrameSetPoint(iconFrame, FRAMEPOINT_TOPLEFT, BlzGetFrameByName("SimpleHeroLevelBar", 0), FRAMEPOINT_BOTTOMLEFT, 0, -0.001)

        -- 2. Custom Info
        createContext, infoFrame, iconFrame, labelFrame, textFrame = UnitInfoCreateCustomInfo(parent, " Lumber:", "UI\\Widgets\\ToolTips\\Human\\ToolTipLumberIcon", function(unit)
            local min, max
            min = BlzGetUnitIntegerField(unit, UNIT_IF_LUMBER_BOUNTY_AWARDED_NUMBER_OF_DICE) + BlzGetUnitIntegerField(unit, UNIT_IF_LUMBER_BOUNTY_AWARDED_BASE)
            max = BlzGetUnitIntegerField(unit, UNIT_IF_LUMBER_BOUNTY_AWARDED_NUMBER_OF_DICE) *BlzGetUnitIntegerField(unit, UNIT_IF_LUMBER_BOUNTY_AWARDED_SIDES_PER_DIE) + BlzGetUnitIntegerField(unit, UNIT_IF_LUMBER_BOUNTY_AWARDED_BASE)
            return "Bounty Lumber: "..min.." - "..max
            .."\nWhen an unit owned by you kills this unit, you gain this amount of Lumber.\nOnly unallied unit give bounty."
        end)
        lumberText = textFrame
        BlzFrameSetPoint(iconFrame, FRAMEPOINT_TOPLEFT, prevIcon, FRAMEPOINT_TOPLEFT, 0.095, 0)
        prevIcon = nil
    end
    function MarkGameStarted()
        realFunction()
        realFunction = nil
        Init()
        if FrameLoaderAdd then FrameLoaderAdd(Init) end                
    end
end
     */
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
                return UnitAI.GetAI(unit) is Hero;
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
