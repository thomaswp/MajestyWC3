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

#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it

        /// @CSharpLua.Template = "AddUnitInfoPanel({0}, {1}, {2})"
        public static extern void AddUnitInfoPanel(framehandle frame, Action<unit> update, Func<unit> condition = null);
        /// @CSharpLua.Template = "AddUnitInfoPanelEx({0}, {1})"
        public static extern void AddUnitInfoPanelEx(Action<unit> update, Func<unit, bool> condition = null);

        /// @CSharpLua.Template = "SetUnitInfoPanelFrame({0})"
        public static extern void SetUnitInfoPanelFrame(framehandle frame);
        /// @CSharpLua.Template = "SetUnitInfoPanelFrameEx()"
        public static extern void SetUnitInfoPanelFrameEx();


        /// @CSharpLua.Template = "UnitInfoPanelAddTooltipListener({0}, {1})"
        public static extern void UnitInfoPanelAddTooltipListener(framehandle frame, Func<string> code);

        /// @CSharpLua.Template = "UnitInfoCreateCustomInfo({0}, {1}, {2}, {3})"
        public static extern void UnitInfoCreateCustomInfo(framehandle parent, string label, string texture, Func<string> code);
        /// @CSharpLua.Template = "UnitInfoGetUnit({0})"
        public static extern void UnitInfoGetUnit(player player = null);

        /// @CSharpLua.Template = "UnitInfoPanelSetPage({0}, {1})"
        public static extern void UnitInfoPanelSetPage(object newPage, bool updateWanted);

#pragma warning restore CS0626 // Method, operator, or accessor is marked external and has no attributes on it


    }
}
