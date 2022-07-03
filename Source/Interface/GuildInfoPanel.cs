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
    public class GuildInfoPanel
    {
        public const int MAX_HEROES = 4;
        private const int MAX_NAME = 12;

        public static void Init()
        {
            LoadTemplatesTOC();
            var heroInfos = new CustomInfo[MAX_HEROES];
            Dictionary<framehandle, UnitAI> iconToUnitMap = new();
            framehandle statusText = null;

            // Don't name this trigger! It won't work for some scope/transpiling issue
            trigger clickTrigger = CreateTrigger();
            TriggerAddAction(clickTrigger, () =>
            {
                Console.WriteLine("CLICK!!!");
                // TODO: This causes an error
                var unit = UnitInfoGetUnit(GetTriggerPlayer());
                Console.WriteLine(unit);
                framehandle frame = BlzGetTriggerFrame();
                if (!iconToUnitMap.TryGetValue(frame, out var ai)) return;
                ai.Select();
            });


            framehandle parent = AddUnitInfoPanelEx(unit =>
            {
                int gold = Buildings.GetGold(unit);
                BlzFrameSetText(statusText, $"{unit.GetName()} has {gold}g in coffers.");
                var heroes = Guilds.GetHeroes(unit);
                iconToUnitMap.Clear();
                for (int i = 0; i < MAX_HEROES; i++)
                {
                    var info = heroInfos[i];
                    bool visible = i < heroes.Count;
                    BlzFrameSetVisible(info.infoFrame, visible);
                    if (!visible) continue;
                    UnitAI hero = heroes[i];
                    int level = GetHeroLevel(hero.Unit);
                    BlzFrameSetText(info.labelFrame, hero.Name + " [" + level + "]");
                    BlzFrameSetText(info.textFrame, hero.ProperName);
                    string name = hero.ProperName;
                    if (name.Length > MAX_NAME) name = name.Substring(0, MAX_NAME - 3) + "...";
                    BlzFrameSetText(info.textFrame, name);
                    string icon = BlzGetAbilityIcon(hero.Unit.GetTypeID());
                    if (icon != null)
                    {
                        BlzFrameSetTexture(info.iconFrame, icon, 0, false);
                    }
                    iconToUnitMap.Add(info.textFrame, hero);
                }
            }, unit => unit.GetPlayer() == GetLocalPlayer() && unit.IsGuild());

            framehandle levelBar = BlzGetFrameByName(FRAME_LevelBar, 0);

            float statusHeight = 0.018f;
            var frameParent = SetUnitInfoPanelFrameEx();
            statusText = BlzCreateFrameByType("TEXT", "", frameParent, "CustomUnitStatusText", 0);
            BlzFrameSetPoint(statusText, FRAMEPOINT_TOPLEFT, levelBar, FRAMEPOINT_TOPLEFT, 0, -0.005f);
            BlzFrameSetSize(statusText, 0.175f, statusHeight);

            for (int i = 0; i < MAX_HEROES; i++)
            {
                int index = i;
                var heroInfo = UnitInfoCreateCustomInfo(parent, $"Hero {i + 1}:",
                   @"UI\Widgets\ToolTips\Human\ToolTipGoldIcon", unit =>
                   {
                       var heroes = Guilds.GetHeroes(unit);
                       if (index >= heroes.Count) return "Hero not recruited yet";
                       UnitAI hero = heroes[index];
                       return $"{hero.ProperName} is {hero.Status}"; //\n\nClick to jump to hero.";
                   });
                heroInfos[i] = heroInfo;
                int row = i / 2, col = i % 2;
                BlzFrameSetPoint(heroInfo.iconFrame, FRAMEPOINT_TOPLEFT,
                    levelBar, FRAMEPOINT_TOPLEFT, 0.005f + col * 0.09f, -statusHeight - row * 0.032f);

                // TODO: Apparently only buttons can be clicked (ok I guess that makes sense),
                // which means you eithern need an invis button on top or to recreate this...
                var button = BlzCreateSimpleFrame("CustomUnitInfoButtonTemplate", parent, 0);
                BlzFrameSetPoint(button, FRAMEPOINT_TOPLEFT,
                    heroInfo.iconFrame, FRAMEPOINT_TOPLEFT, 0, 0);

                Console.WriteLine("making a trigger...");
                BlzTriggerRegisterFrameEvent(clickTrigger, button, FRAMEEVENT_CONTROL_CLICK);
                BlzTriggerRegisterFrameEvent(clickTrigger, heroInfo.iconFrame, FRAMEEVENT_CONTROL_CLICK);
            }
            BlzTriggerRegisterFrameEvent(clickTrigger, parent, FRAMEEVENT_MOUSE_ENTER);
        }
    }
}
