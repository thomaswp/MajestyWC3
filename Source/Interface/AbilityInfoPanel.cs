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
    public static class AbilityInfoPanel
    {
        internal struct Frame
        {
            public int Index;
            public int Skill;
            public framehandle Icon, Text, Button, ToolTip;
        }

        private static bool AbiFilter(ability abi, string text)
        {
            if (BlzGetAbilityBooleanField(abi, ABILITY_BF_ITEM_ABILITY)) return false;
            if (text == "Tool tip missing!" || text == "" || text == " ") return false;
            return true;
        }

        public static void Init()
        {
            LoadTemplatesTOC();

            // TODO: Should really make a 1x4 pannel with wider text.
            const int buttonCount = 8;

            // Don't name this trigger! It won't work for some scope/transpiling issue
            trigger trigger2 = CreateTrigger();

            Frame[] abilityFrames = new Frame[buttonCount];

            var parent = BlzCreateSimpleFrame("CustomUnitInfoPanel2x4", BlzGetFrameByName("SimpleInfoPanelUnitDetail", 0), 0);
            for (int i = 0; i < buttonCount; i++)
            {
                // frames are 1-indexed so we need to offset by 1
                int frameIndex = i + 1;
                var frame = BlzGetFrameByName("CustomUnitInfoButton" + frameIndex, 0);
                var tooltip = BlzCreateFrameByType("SIMPLEFRAME", "", frame, "", 0);
                var iconFrame = BlzGetFrameByName("CustomUnitInfoButtonIcon" + frameIndex, 0);
                var textFrame = BlzGetFrameByName("CustomUnitInfoButtonText" + frameIndex, 0);
                BlzTriggerRegisterFrameEvent(trigger2, frame, FRAMEEVENT_CONTROL_CLICK);
                BlzFrameSetTooltip(frame, tooltip);
                BlzFrameSetVisible(tooltip, false);
                BlzFrameSetVisible(frame, false);

                abilityFrames[i] = new Frame() { Index = i, Icon = iconFrame, Text = textFrame, Button = frame, ToolTip = tooltip };

                // Cannot use i inside of a function, since it will change later
                int permIndex = i;

                UnitInfoPanelAddTooltipListener(tooltip, unit =>
                {
                    //the tooltip function is async do not create or destroy here

                    var abi = BlzGetUnitAbilityByIndex(unit, abilityFrames[permIndex].Skill);
                    int level = GetUnitAbilityLevel(unit, abilityFrames[permIndex].Skill);

                    // TODO: For some reason the level here stays level 1...
                    return BlzGetAbilityStringLevelField(abi, ABILITY_SLF_TOOLTIP_NORMAL, level) + "\n" +
                        BlzGetAbilityStringLevelField(abi, ABILITY_SLF_TOOLTIP_NORMAL_EXTENDED, level);

                    //if (BlzGetAbilityBooleanField(abi, ABILITY_BF_HERO_ABILITY))
                    //{

                    //    return BlzGetAbilityStringLevelField(abi, ABILITY_SLF_TOOLTIP_LEARN, 0) + "\n" + 
                    //    BlzGetAbilityStringLevelField(abi, ABILITY_SLF_TOOLTIP_LEARN_EXTENDED, 0);
                    //}
                    //else
                    //{
                    //    return BlzGetAbilityStringLevelField(abi, ABILITY_SLF_TOOLTIP_NORMAL, 0) + "\n" +
                    //    BlzGetAbilityStringLevelField(abi, ABILITY_SLF_TOOLTIP_NORMAL_EXTENDED, 0);
                    //}
                });
            }

            AddUnitInfoPanel(parent, unit =>
            {
                int buttonIndex = 0;
                int abiIndex = 0;
                while (buttonIndex < buttonCount)
                {
                    var abi = BlzGetUnitAbilityByIndex(unit, abiIndex);
                    if (abi != null)
                    {
                        var text = BlzGetAbilityStringLevelField(abi, ABILITY_SLF_TOOLTIP_NORMAL, 0);
                        if (AbiFilter(abi, text))
                        {
                            Frame frame = abilityFrames[buttonIndex];
                            abilityFrames[buttonIndex].Skill = abiIndex;
                            BlzFrameSetVisible(frame.Button, true);
                            BlzFrameSetTexture(frame.Icon, BlzGetAbilityStringLevelField(abi, ABILITY_SLF_ICON_NORMAL, 0), 0, false);
                            BlzFrameSetText(frame.Text, text);

                            buttonIndex++;
                        }
                        abiIndex++;
                    }
                    else
                    {
                        BlzFrameSetVisible(abilityFrames[buttonIndex].Button, false);
                        buttonIndex++;
                    }
                }

            }, unit =>
            {
                var ai = UnitAI.GetAI(unit);
                return ai != null && ai.GetAbilityIDs().Length > 0;
            });

            TriggerAddAction(trigger2, () =>
            {
                // There's not actually anything to do per se... but this was a useful test.
                Console.WriteLine("You clicked an ability...");
            });
        }
    }
}
