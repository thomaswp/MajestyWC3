using static War3Api.Common;
using static War3Api.Blizzard;
using System;

namespace Source.Interface
{
    public static class UnitInfoPanels
    {
        /**
            AddUnitInfoPanel(frame, update[, condition])
                frame is the containerPanel, update a function(unit) that runs every updateTick, condition is a function(unit) that returns true or false it is used to prevent rotating to this panel.
            AddUnitInfoPanelEx(update, condition)
                wrapper for AddUnitInfoPanel, it creates and returns a new frame

            SetUnitInfoPanelFrame(frame)
                makes a non SimpleFrame share visibility with the last Added InfoPanel, supports only one Frame per InfoPanel
            SetUnitInfoPanelFrameEx()
                wrapper SetUnitInfoPanelFrame creates and returns a new empty Frame

            UnitInfoPanelAddTooltipListener(frame, code)
                detects if this frame is visible if it is call the given function which should return a text which is the now wanted tooltipText.

            UnitInfoCreateCustomInfo(parent, label, texture, tooltipCode)
                return createContext, infoFrame, iconFrame, labelFrame, textFrame
            UnitInfoGetUnit([player])
                returns & recacls the current selected Unit
                without a player it will use the local player.
    
            function UnitInfoPanelSetPage(newPage, updateWanted)
                ignores conditions, newPage can be "+" or "-" or a frame
         */

        public const string TEMPLATES_TOC = @"war3mapImported\Templates.toc";

        public const string TYPE_TEXTAREA = "TEXTAREA";

        public const string TEMPLATE_EscMenu = "EscMenuTextAreaTemplate";

        public const string FRAME_LevelBar = "SimpleHeroLevelBar";

        public static void LoadTemplatesTOC()
        {
            BlzLoadTOCFile(TEMPLATES_TOC);
        }

#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it

        /// @CSharpLua.Template = "AddUnitInfoPanel({0}, {1}, {2})"
        public static extern framehandle AddUnitInfoPanel(framehandle frame, Action<unit> update, Func<unit, bool> condition = null);
        /// @CSharpLua.Template = "AddUnitInfoPanelEx({0}, {1})"
        public static extern framehandle AddUnitInfoPanelEx(Action<unit> update, Func<unit, bool> condition = null);

        /// @CSharpLua.Template = "SetUnitInfoPanelFrame({0})"
        public static extern framehandle SetUnitInfoPanelFrame(framehandle frame);
        /// @CSharpLua.Template = "SetUnitInfoPanelFrameEx()"
        public static extern framehandle SetUnitInfoPanelFrameEx();


        /// @CSharpLua.Template = "UnitInfoAddTooltip({0}, {1})"
        public static extern framehandle UnitInfoAddTooltip(framehandle parent, framehandle frame);
        /// @CSharpLua.Template = "UnitInfoPanelAddTooltipListener({0}, {1})"
        public static extern void UnitInfoPanelAddTooltipListener(framehandle frame, Func<unit, string> code);

        /// @CSharpLua.Template = "UnitInfoCreateCustomInfo({0}, {1}, {2}, {3})"
        //public static extern object UnitInfoCreateCustomInfo(framehandle parent, string label, string texture, Func<unit, string> code);

        /// @CSharpLua.Template = "UnitInfoGetUnit({0})"
        public static extern unit UnitInfoGetUnit(player player = null);

        /// @CSharpLua.Template = "UnitInfoPanelSetPage({0}, {1})"
        public static extern void UnitInfoPanelSetPage(object newPage, bool updateWanted);

#pragma warning restore CS0626 // Method, operator, or accessor is marked external and has no attributes on it

        public struct CustomInfo
        {
            public int createContext;
            public framehandle infoFrame, iconFrame, labelFrame, textFrame;
        }

        static int createContext = 1000;
        public static CustomInfo UnitInfoCreateCustomInfo(framehandle parent, string label, string texture, Func<unit, string> tooltipCode)
        {
            createContext++;
            var infoFrame = BlzCreateSimpleFrame("SimpleInfoPanelIconRank", parent, createContext);
            var iconFrame = BlzGetFrameByName("InfoPanelIconBackdrop", createContext);
            var labelFrame = BlzGetFrameByName("InfoPanelIconLabel", createContext);
            var textFrame = BlzGetFrameByName("InfoPanelIconValue", createContext);
            BlzFrameSetText(labelFrame, label);
            BlzFrameSetText(textFrame, "xxx");
            BlzFrameSetTexture(iconFrame, texture, 0, false);
            BlzFrameClearAllPoints(iconFrame);
            BlzFrameSetSize(iconFrame, 0.028f, 0.028f);
            if (tooltipCode != null)
            {
                UnitInfoPanelAddTooltipListener(UnitInfoAddTooltip(infoFrame, iconFrame), tooltipCode);
            }
            return new CustomInfo() { 
                createContext = createContext, infoFrame = infoFrame, iconFrame = iconFrame, 
                labelFrame = labelFrame, textFrame = textFrame 
            };
        }

        public static int NextCreateContext()
        {
            return ++createContext;
        }

    }
}
