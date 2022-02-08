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

            trigger trigger = CreateTrigger();
            TriggerAddAction(trigger, () =>
            {
                framehandle frame = BlzGetTriggerFrame();
                if (!iconToUnitMap.TryGetValue(frame, out var ai)) return;
                ai.Select();
            });


            framehandle parent = AddUnitInfoPanelEx(unit =>
            {
                int gold = 100; // TODO
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
                    BlzFrameSetText(info.labelFrame, hero.Name);
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
                // TODO: Figure out how to make click events register, or substitute icon w/ button or something
                BlzTriggerRegisterFrameEvent(trigger, heroInfo.textFrame, FRAMEEVENT_MOUSE_DOWN);

            }
        }
    }
}
