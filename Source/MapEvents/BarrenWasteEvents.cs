using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.MapEvents
{
    class BarrenWasteEvents : MapEventsBase
    {
        public static void BARREN_WASTE()
        {
            Agent AIRootAgent, palace, sign;
            List palaces, signs;
            AIRootAgent = RetrieveAgent("GplAIRoot");
            AIRootAgent["Quest_Number"] = Constants.QNumber_Barren_Waste;
            palaces = ListPalaces();
            palace = listmember(palaces, 1);
            Setup_Quest_Music(AIRootAgent);
            disableunittype("Wizards_guild1");
            disableunittype("Elven_bungalow");
            disableunittype("Dwarven_settlement");
            disableunittype("Gnome_hovel");
            disableunittype("fairgrounds");
            ElvesVoice_setOperative(0);
            dwarvesVoice_setOperative(0);
            messageflag(palace, Constants.Message_Barren_start);
            setup_random_treasure(20, Constants.default_spawn_treasure_dist);
            listobjects(palace, "color", -1, out signs, Constants.NoHiddenMap);
            signs = listtitles(signs, "Banner_wood");
            sign = listmember(signs, 1);
            post_message(sign, Constants.sign_barren);
            AIRootAgent = RetrieveAgent("GplAIRoot");
            AIRootAgent["VictoryCondition"] = new Action(() => barren_victory());
            NewThread(AIRootAgent["VictoryCondition"], Constants.VictoryCondition_callback_frequency);
        }


        public static void barren_victory()
        {
            Agent AIRootAgent, palace, bldg;
            List palaces, bldgs;
            AIRootAgent = RetrieveAgent("GplAIRoot");
            palaces = ListPalaces();
            palace = listmember(palaces, 1);
            if (AIRootAgent["Quest_Flag_1"] == false)
            {
                listobjects(palace, "building", -1, out bldgs, Constants.MyPlayer);
                bldgs = listsubtypes(bldgs, "guild");
                bldgs = listcompleted(bldgs);
                if (listsize(bldgs) > 0)
                {
                    messageflag(listmember(bldgs, 1), Constants.message_barren_guild_built);
                    AIRootAgent["Quest_Flag_1"] = true;
                }
            }
            else
            {
                if (AIRootAgent["Quest_Flag_2"] == false)
                {
                    if ((palace["level"] == 2) && (getattribute(palace, Constants.ATTRIB_currentstagebuilt) == 1))
                    {
                        messageflag(palace, Constants.message_barren_Palace_Upgraded);
                        AIRootAgent["Quest_Flag_2"] = true;
                    }
                }
                else
                {
                    if (AIRootAgent["Quest_Flag_3"] == false)
                    {
                        if ((palace["level"] == 3) && (getattribute(palace, Constants.ATTRIB_currentstagebuilt) == 1))
                        {
                            messageflag(palace, Constants.message_barren_Palace_third);
                            AIRootAgent["Quest_Flag_3"] = true;
                        }
                    }
                    else
                    {
                        if (AIRootAgent["Quest_Flag_4"] == false)
                        {
                            listobjects(palace, "lair", -1, out bldgs, Constants.NotMyPlayer, Constants.NoHiddenMap);
                            bldgs = listtitles(bldgs, "Dark_castle");
                            if (listsize(bldgs) == 0)
                            {
                                messageflag(palace, Constants.message_barren_fairground);
                                enableunittype("fairgrounds");
                                AIRootAgent["Quest_Flag_4"] = true;
                                Play_Endgame_Music(AIRootAgent);
                            }
                        }
                        else
                        {
                            if (AIRootAgent["Quest_Flag_5"] == false)
                            {
                                listobjects(palace, "building", -1, out bldgs, Constants.MyPlayer);
                                bldgs = listtitles(bldgs, "fairgrounds");
                                bldgs = listcompleted(bldgs);
                                if (listsize(bldgs) > 0)
                                {
                                    Reset_Quest_Music(AIRootAgent);
                                    declarevictory(palace, listmember(bldgs, 1));
                                    KillThread(AIRootAgent["VictoryCondition"]);
                                    AIRootAgent["VictoryCondition2"] = end_game_script_easy;
                                    NewThread(AIRootAgent["VictoryCondition2"], random_time(600000));
                                    AIRootAgent["Quest_Flag_5"] = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
