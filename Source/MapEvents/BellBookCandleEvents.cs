using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.MapEvents
{
    class BellBookCandleEvents : MapEventsBase
    {
        public static void BellBookCandle()
        {
            Agent AIRootAgent;
            Agent palace, booksite, bellsite, candlesite;
            List palaces, lairs, lairs2, buildings, chests;
            Disableunittype("Warriors_guild");
            Disableunittype("Wizards_guild1");
            AIRootAgent = RetrieveAgent("GplAIRoot");
            AIRootAgent["VictoryCondition"] = new Action(BbcVictory);
            NewThread(AIRootAgent["VictoryCondition"], Constants.VictoryCondition_callback_frequency);
            AIRootAgent["Quest_Number"] = Constants.QNumber_Bell_Book;
            palaces = Listpalaces();
            palace = ListMember(palaces, 1);
            SetupQuestMusic(AIRootAgent);
            ListObjects(palace, "lair", -1, out lairs, Constants.NoHiddenMap);
            lairs2 = ListTitles(lairs, "ruined_altar1");
            booksite = ListMember(lairs2, 1);
            booksite["title"] = "booksite";
            Createnewinventoryitem(Constants.QItem_Book, booksite);
            lairs2 = ListTitles(lairs, "ruined_shrine1");
            bellsite = ListMember(lairs2, 1);
            bellsite["title"] = "bellsite";
            Createnewinventoryitem(Constants.QItem_Bell, bellsite);
            lairs2 = ListTitles(lairs, "ruined_keep1");
            candlesite = ListMember(lairs2, 1);
            candlesite["title"] = "candlesite";
            Createnewinventoryitem(Constants.QItem_Candle, candlesite);
            Messageflag(palace, Constants.message_bbc_intro);
            ListObjects(palace, "special_item", -1, out chests, Constants.NoHiddenMap);
            chests = ListTitles(chests, "treasure_chest");
            SetupStartingTreasure(chests, 100, 100);
            AIRootAgent["Quest_Flag_1"] = false;
            AIRootAgent["Quest_Flag_2"] = false;
            AIRootAgent["Quest_Flag_3"] = false;
        }


        public static void BbcVictory()
        {
            List palaces, lairs, lairs2, buildings;
            Agent palace, booksite, bellsite, candlesite, bldg, lair;
            Agent AIRootAgent;
            int sites_gone = 0;
            AIRootAgent = RetrieveAgent("GplAIRoot");
            palaces = Listpalaces();
            palace = ListMember(palaces, 1);
            ListObjects(palace, "lair", -1, out lairs, Constants.NoHiddenMap);
            ListObjects(palace, "building", -1, out buildings, Constants.NoHiddenMap);
            if (AIRootAgent["Message_Check_1"] == false)
            {
                if (Ismessageflagpresent(Constants.message_bbc_intro) == false)
                {
                    buildings = ListTitles(buildings, "blacksmith");
                    if (ListSize(buildings) > 0)
                    {
                        bldg = ListMember(buildings, 1);
                        Messageflag(bldg, Constants.message_bbc_blacksmith);
                    }
                    AIRootAgent["Message_Check_1"] = true;
                }
            }
            else
            {
                if (AIRootAgent["Message_Check_2"] == false)
                {
                    if (Ismessageflagpresent(Constants.message_bbc_blacksmith) == false)
                    {
                        buildings = ListTitles(buildings, "guardhouse");
                        if (ListSize(buildings) > 0)
                        {
                            bldg = ListMember(buildings, 1);
                            Messageflag(bldg, Constants.message_bbc_guardhouse);
                        }
                        AIRootAgent["Message_Check_2"] = true;
                    }
                }
                else
                {
                    if (AIRootAgent["Message_Check_3"] == false)
                    {
                        if (Ismessageflagpresent(Constants.message_bbc_guardhouse) == false)
                        {
                            buildings = ListTitles(buildings, "Inn");
                            if (ListSize(buildings) > 0)
                            {
                                bldg = ListMember(buildings, 1);
                                Messageflag(bldg, Constants.message_bbc_inn);
                            }
                            AIRootAgent["Message_Check_3"] = true;
                        }
                    }
                    else
                    {
                        if (AIRootAgent["Message_Check_4"] == false)
                        {
                            if (Ismessageflagpresent(Constants.message_bbc_inn) == false)
                            {
                                buildings = ListTitles(buildings, "Trading_post");
                                if (ListSize(buildings) > 0)
                                {
                                    bldg = ListMember(buildings, 1);
                                    Messageflag(bldg, Constants.message_bbc_trading_post);
                                }
                                AIRootAgent["Message_Check_4"] = true;
                            }
                        }
                    }
                }
            }
            lairs2 = ListTitles(lairs, "booksite");
            if (ListSize(lairs2) > 0)
            {
                booksite = ListMember(lairs2, 1);
            }
            else
            {
                sites_gone += 1;
            }
            lairs2 = ListTitles(lairs, "bellsite");
            if (ListSize(lairs2) > 0)
            {
                bellsite = ListMember(lairs2, 1);
            }
            else
            {
                sites_gone += 1;
                PlayEndgameMusic(AIRootAgent);
            }
            lairs2 = ListTitles(lairs, "candlesite");
            if (ListSize(lairs2) > 0)
            {
                candlesite = ListMember(lairs2, 1);
            }
            else
            {
                sites_gone += 1;
            }
            if (sites_gone == 3)
            {
                ResetQuestMusic(AIRootAgent);
                Declarevictory(palace);
                Killthread(AIRootAgent["VictoryCondition"]);
                AIRootAgent["VictoryCondition2"] = new Action(EndGameScriptEasy);
                NewThread(AIRootAgent["VictoryCondition2"], RandomTime(600000));
            }
            else
            {
                if (AIRootAgent["Quest_Flag_4"] == false)
                {
                    if (sites_gone == 2)
                    {
                        lair = ListMember(lairs, 1);
                        lair["special_spawn_type"] = "Giant_rat";
                        AIRootAgent["Quest_Flag_4"] = true;
                    }
                }
                ListObjects(palace, "lair", -1, out lairs);
                if (AIRootAgent["Quest_Flag_1"] == false)
                {
                    lairs2 = ListTitles(lairs, "booksite");
                    if (ListSize(lairs2) > 0)
                    {
                        bldg = ListMember(lairs2, 1);
                        Messageflag(bldg, Constants.message_bbc_found_book);
                        AIRootAgent["Quest_Flag_1"] = true;
                    }
                }
                if (AIRootAgent["Quest_Flag_2"] == false)
                {
                    lairs2 = ListTitles(lairs, "bellsite");
                    if (ListSize(lairs2) > 0)
                    {
                        bldg = ListMember(lairs2, 1);
                        Messageflag(bldg, Constants.message_bbc_found_bell);
                        AIRootAgent["Quest_Flag_2"] = true;
                    }
                }
                if (AIRootAgent["Quest_Flag_3"] == false)
                {
                    lairs2 = ListTitles(lairs, "candlesite");
                    if (ListSize(lairs2) > 0)
                    {
                        bldg = ListMember(lairs2, 1);
                        Messageflag(bldg, Constants.message_bbc_found_candle);
                        AIRootAgent["Quest_Flag_3"] = true;
                    }
                }
            }
        }

    }
}
