using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.MapEvents
{
    class BellBookCandleEvents : MapEventsBase
    {
        public static void BELL_BOOK_CANDLE()
        {
            Agent AIRootAgent;
            Agent palace, booksite, bellsite, candlesite;
            List palaces, lairs, lairs2, buildings, chests;
            disableunittype("Warriors_guild");
            disableunittype("Wizards_guild1");
            AIRootAgent = RetrieveAgent("GplAIRoot");
            AIRootAgent["VictoryCondition"] = bbc_victory;
            NewThread(AIRootAgent["VictoryCondition"], Constants.VictoryCondition_callback_frequency);
            AIRootAgent["Quest_Number"] = Constants.QNumber_Bell_Book;
            palaces = ListPalaces();
            palace = listmember(palaces, 1);
            Setup_Quest_Music(AIRootAgent);
            listobjects(palace, "lair", -1, lairs, Constants.NoHiddenMap);
            lairs2 = listtitles(lairs, "ruined_altar1");
            booksite = listmember(lairs2, 1);
            booksite["title"] = "booksite";
            CreateNewInventoryItem(Constants.QItem_Book, booksite);
            lairs2 = listtitles(lairs, "ruined_shrine1");
            bellsite = listmember(lairs2, 1);
            bellsite["title"] = "bellsite";
            CreateNewInventoryItem(Constants.QItem_Bell, bellsite);
            lairs2 = listtitles(lairs, "ruined_keep1");
            candlesite = listmember(lairs2, 1);
            candlesite["title"] = "candlesite";
            CreateNewInventoryItem(Constants.QItem_Candle, candlesite);
            messageflag(palace, Constants.message_bbc_intro);
            listobjects(palace, "special_item", -1, chests, Constants.NoHiddenMap);
            chests = listtitles(chests, "treasure_chest");
            setup_starting_treasure(chests, 100, 100);
            AIRootAgent["Quest_Flag_1"] = false;
            AIRootAgent["Quest_Flag_2"] = false;
            AIRootAgent["Quest_Flag_3"] = false;
        }


        public static void bbc_victory()
        {
            List palaces, lairs, lairs2, buildings;
            Agent palace, booksite, bellsite, candlesite, bldg, lair;
            Agent AIRootAgent;
            Integer sites_gone;
            AIRootAgent = RetrieveAgent("GplAIRoot");
            palaces = ListPalaces();
            palace = listmember(palaces, 1);
            listobjects(palace, "lair", -1, lairs, Constants.NoHiddenMap);
            listobjects(palace, "building", -1, buildings, Constants.NoHiddenMap);
            if (AIRootAgent["Message_Check_1"] == false)
            {
                List palaces, lairs, lairs2, buildings;
                Agent palace, booksite, bellsite, candlesite, bldg, lair;
                Agent AIRootAgent;
                Integer sites_gone;
                if (IsMessageFlagPresent(Constants.message_bbc_intro) == false)
                {
                    List palaces, lairs, lairs2, buildings;
                    Agent palace, booksite, bellsite, candlesite, bldg, lair;
                    Agent AIRootAgent;
                    Integer sites_gone;
                    buildings = listtitles(buildings, "blacksmith");
                    if (listsize(buildings) > 0)
                    {
                        List palaces, lairs, lairs2, buildings;
                        Agent palace, booksite, bellsite, candlesite, bldg, lair;
                        Agent AIRootAgent;
                        Integer sites_gone;
                        bldg = listmember(buildings, 1);
                        messageflag(bldg, Constants.message_bbc_blacksmith);
                    }
                    AIRootAgent["Message_Check_1"] = true;
                }
            }
            else
            {
                List palaces, lairs, lairs2, buildings;
                Agent palace, booksite, bellsite, candlesite, bldg, lair;
                Agent AIRootAgent;
                Integer sites_gone;
                if (AIRootAgent["Message_Check_2"] == false)
                {
                    List palaces, lairs, lairs2, buildings;
                    Agent palace, booksite, bellsite, candlesite, bldg, lair;
                    Agent AIRootAgent;
                    Integer sites_gone;
                    if (IsMessageFlagPresent(Constants.message_bbc_blacksmith) == false)
                    {
                        List palaces, lairs, lairs2, buildings;
                        Agent palace, booksite, bellsite, candlesite, bldg, lair;
                        Agent AIRootAgent;
                        Integer sites_gone;
                        buildings = listtitles(buildings, "guardhouse");
                        if (listsize(buildings) > 0)
                        {
                            List palaces, lairs, lairs2, buildings;
                            Agent palace, booksite, bellsite, candlesite, bldg, lair;
                            Agent AIRootAgent;
                            Integer sites_gone;
                            bldg = listmember(buildings, 1);
                            messageflag(bldg, Constants.message_bbc_guardhouse);
                        }
                        AIRootAgent["Message_Check_2"] = true;
                    }
                }
                else
                {
                    List palaces, lairs, lairs2, buildings;
                    Agent palace, booksite, bellsite, candlesite, bldg, lair;
                    Agent AIRootAgent;
                    Integer sites_gone;
                    if (AIRootAgent["Message_Check_3"] == false)
                    {
                        List palaces, lairs, lairs2, buildings;
                        Agent palace, booksite, bellsite, candlesite, bldg, lair;
                        Agent AIRootAgent;
                        Integer sites_gone;
                        if (IsMessageFlagPresent(Constants.message_bbc_guardhouse) == false)
                        {
                            List palaces, lairs, lairs2, buildings;
                            Agent palace, booksite, bellsite, candlesite, bldg, lair;
                            Agent AIRootAgent;
                            Integer sites_gone;
                            buildings = listtitles(buildings, "Inn");
                            if (listsize(buildings) > 0)
                            {
                                List palaces, lairs, lairs2, buildings;
                                Agent palace, booksite, bellsite, candlesite, bldg, lair;
                                Agent AIRootAgent;
                                Integer sites_gone;
                                bldg = listmember(buildings, 1);
                                messageflag(bldg, Constants.message_bbc_inn);
                            }
                            AIRootAgent["Message_Check_3"] = true;
                        }
                    }
                    else
                    {
                        List palaces, lairs, lairs2, buildings;
                        Agent palace, booksite, bellsite, candlesite, bldg, lair;
                        Agent AIRootAgent;
                        Integer sites_gone;
                        if (AIRootAgent["Message_Check_4"] == false)
                        {
                            List palaces, lairs, lairs2, buildings;
                            Agent palace, booksite, bellsite, candlesite, bldg, lair;
                            Agent AIRootAgent;
                            Integer sites_gone;
                            if (IsMessageFlagPresent(Constants.message_bbc_inn) == false)
                            {
                                List palaces, lairs, lairs2, buildings;
                                Agent palace, booksite, bellsite, candlesite, bldg, lair;
                                Agent AIRootAgent;
                                Integer sites_gone;
                                buildings = listtitles(buildings, "Trading_post");
                                if (listsize(buildings) > 0)
                                {
                                    List palaces, lairs, lairs2, buildings;
                                    Agent palace, booksite, bellsite, candlesite, bldg, lair;
                                    Agent AIRootAgent;
                                    Integer sites_gone;
                                    bldg = listmember(buildings, 1);
                                    messageflag(bldg, Constants.message_bbc_trading_post);
                                }
                                AIRootAgent["Message_Check_4"] = true;
                            }
                        }
                    }
                }
            }
            lairs2 = listtitles(lairs, "booksite");
            if (listsize(lairs2) > 0)
            {
                booksite = listmember(lairs2, 1);
            }
            else
            {
                sites_gone += 1;
            }
            lairs2 = listtitles(lairs, "bellsite");
            if (listsize(lairs2) > 0)
            {
                bellsite = listmember(lairs2, 1);
            }
            else
            {
                List palaces, lairs, lairs2, buildings;
                Agent palace, booksite, bellsite, candlesite, bldg, lair;
                Agent AIRootAgent;
                Integer sites_gone;
                sites_gone += 1;
                Play_Endgame_Music(AIRootAgent);
            }
            lairs2 = listtitles(lairs, "candlesite");
            if (listsize(lairs2) > 0)
            {
                candlesite = listmember(lairs2, 1);
            }
            else
            {
                sites_gone += 1;
            }
            if (sites_gone == 3)
            {
                List palaces, lairs, lairs2, buildings;
                Agent palace, booksite, bellsite, candlesite, bldg, lair;
                Agent AIRootAgent;
                Integer sites_gone;
                Reset_Quest_Music(AIRootAgent);
                declarevictory(palace);
                KillThread(AIRootAgent["VictoryCondition"]);
                AIRootAgent["VictoryCondition2"] = end_game_script_easy;
                NewThread(AIRootAgent["VictoryCondition2"], random_time(600000));
            }
            else
            {
                List palaces, lairs, lairs2, buildings;
                Agent palace, booksite, bellsite, candlesite, bldg, lair;
                Agent AIRootAgent;
                Integer sites_gone;
                if (AIRootAgent["Quest_Flag_4"] == false)
                {
                    if (sites_gone == 2)
                    {
                        List palaces, lairs, lairs2, buildings;
                        Agent palace, booksite, bellsite, candlesite, bldg, lair;
                        Agent AIRootAgent;
                        Integer sites_gone;
                        lair = listmember(lairs, 1);
                        lair["special_spawn_type"] = "Giant_rat";
                        AIRootAgent["Quest_Flag_4"] = true;
                    }
                }
                listobjects(palace, "lair", -1, lairs);
                if (AIRootAgent["Quest_Flag_1"] == false)
                {
                    List palaces, lairs, lairs2, buildings;
                    Agent palace, booksite, bellsite, candlesite, bldg, lair;
                    Agent AIRootAgent;
                    Integer sites_gone;
                    lairs2 = listtitles(lairs, "booksite");
                    if (listsize(lairs2) > 0)
                    {
                        List palaces, lairs, lairs2, buildings;
                        Agent palace, booksite, bellsite, candlesite, bldg, lair;
                        Agent AIRootAgent;
                        Integer sites_gone;
                        bldg = listmember(lairs2, 1);
                        messageflag(bldg, Constants.message_bbc_found_book);
                        AIRootAgent["Quest_Flag_1"] = true;
                    }
                }
                if (AIRootAgent["Quest_Flag_2"] == false)
                {
                    List palaces, lairs, lairs2, buildings;
                    Agent palace, booksite, bellsite, candlesite, bldg, lair;
                    Agent AIRootAgent;
                    Integer sites_gone;
                    lairs2 = listtitles(lairs, "bellsite");
                    if (listsize(lairs2) > 0)
                    {
                        List palaces, lairs, lairs2, buildings;
                        Agent palace, booksite, bellsite, candlesite, bldg, lair;
                        Agent AIRootAgent;
                        Integer sites_gone;
                        bldg = listmember(lairs2, 1);
                        messageflag(bldg, Constants.message_bbc_found_bell);
                        AIRootAgent["Quest_Flag_2"] = true;
                    }
                }
                if (AIRootAgent["Quest_Flag_3"] == false)
                {
                    List palaces, lairs, lairs2, buildings;
                    Agent palace, booksite, bellsite, candlesite, bldg, lair;
                    Agent AIRootAgent;
                    Integer sites_gone;
                    lairs2 = listtitles(lairs, "candlesite");
                    if (listsize(lairs2) > 0)
                    {
                        List palaces, lairs, lairs2, buildings;
                        Agent palace, booksite, bellsite, candlesite, bldg, lair;
                        Agent AIRootAgent;
                        Integer sites_gone;
                        bldg = listmember(lairs2, 1);
                        messageflag(bldg, Constants.message_bbc_found_candle);
                        AIRootAgent["Quest_Flag_3"] = true;
                    }
                }
            }
        }


    }
}
