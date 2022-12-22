using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.MapEvents
{
    class BarrenWasteEvents : MapEventsBase
    {
        public static void BarrenWaste()
        {
            Agent AIRootAgent, palace, sign;
            List palaces, signs;
            AIRootAgent = RetrieveAgent("GplAIRoot");
            AIRootAgent["Quest_Number"] = Constants.QNumber_Barren_Waste;
            palaces = Listpalaces();
            palace = ListMember(palaces, 1);
            SetupQuestMusic(AIRootAgent);
            Disableunittype("Wizards_guild1");
            Disableunittype("Elven_bungalow");
            Disableunittype("Dwarven_settlement");
            Disableunittype("Gnome_hovel");
            Disableunittype("fairgrounds");
            ElvesvoiceSetoperative(0);
            DwarvesvoiceSetoperative(0);
            Messageflag(palace, Constants.Message_Barren_start);
            SetupRandomTreasure(20, Constants.default_spawn_treasure_dist);
            ListObjects(palace, "color", -1, out signs, Constants.NoHiddenMap);
            signs = ListTitles(signs, "Banner_wood");
            sign = ListMember(signs, 1);
            PostMessage(sign, Constants.sign_barren);
            AIRootAgent = RetrieveAgent("GplAIRoot");
            AIRootAgent["VictoryCondition"] = new Action(BarrenVictory);
            NewThread(AIRootAgent["VictoryCondition"], Constants.VictoryCondition_callback_frequency);
        }


        public static void BarrenVictory()
        {
            Agent AIRootAgent, palace, bldg;
            List palaces, bldgs;
            AIRootAgent = RetrieveAgent("GplAIRoot");
            palaces = Listpalaces();
            palace = ListMember(palaces, 1);
            if (AIRootAgent["Quest_Flag_1"] == false)
            {
                ListObjects(palace, "building", -1, out bldgs, Constants.MyPlayer);
                bldgs = ListSubtypes(bldgs, "guild");
                bldgs = ListCompleted(bldgs);
                if (ListSize(bldgs) > 0)
                {
                    Messageflag(ListMember(bldgs, 1), Constants.message_barren_guild_built);
                    AIRootAgent["Quest_Flag_1"] = true;
                }
            }
            else
            {
                if (AIRootAgent["Quest_Flag_2"] == false)
                {
                    if ((palace["level"] == 2) && (Getattribute(palace, Constants.ATTRIB_currentstagebuilt) == 1))
                    {
                        Messageflag(palace, Constants.message_barren_Palace_Upgraded);
                        AIRootAgent["Quest_Flag_2"] = true;
                    }
                }
                else
                {
                    if (AIRootAgent["Quest_Flag_3"] == false)
                    {
                        if ((palace["level"] == 3) && (Getattribute(palace, Constants.ATTRIB_currentstagebuilt) == 1))
                        {
                            Messageflag(palace, Constants.message_barren_Palace_third);
                            AIRootAgent["Quest_Flag_3"] = true;
                        }
                    }
                    else
                    {
                        if (AIRootAgent["Quest_Flag_4"] == false)
                        {
                            ListObjects(palace, "lair", -1, out bldgs, Constants.NotMyPlayer, Constants.NoHiddenMap);
                            bldgs = ListTitles(bldgs, "Dark_castle");
                            if (ListSize(bldgs) == 0)
                            {
                                Messageflag(palace, Constants.message_barren_fairground);
                                Enableunittype("fairgrounds");
                                AIRootAgent["Quest_Flag_4"] = true;
                                PlayEndgameMusic(AIRootAgent);
                            }
                        }
                        else
                        {
                            if (AIRootAgent["Quest_Flag_5"] == false)
                            {
                                ListObjects(palace, "building", -1, out bldgs, Constants.MyPlayer);
                                bldgs = ListTitles(bldgs, "fairgrounds");
                                bldgs = ListCompleted(bldgs);
                                if (ListSize(bldgs) > 0)
                                {
                                    ResetQuestMusic(AIRootAgent);
                                    Declarevictory(palace, ListMember(bldgs, 1));
                                    Killthread(AIRootAgent["VictoryCondition"]);
                                    AIRootAgent["VictoryCondition2"] = new Action(EndGameScriptEasy);
                                    NewThread(AIRootAgent["VictoryCondition2"], RandomTime(600000));
                                    AIRootAgent["Quest_Flag_5"] = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
