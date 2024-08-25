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
            List palaces, lairs, lairs2, chests;
            Disableunittype("Warriors_guild");
            Disableunittype("Wizards_guild1");
            AIRootAgent = RetrieveAgent("GplAIRoot");
            AIRootAgent["VictoryCondition"] = new Action(BbcVictory);
            NewThread(AIRootAgent["VictoryCondition"], MGPLConstants.VictoryconditionCallbackFrequency);
            AIRootAgent["Quest_Number"] = MGPLConstants.QnumberBellBook;
            palaces = ListPalaces();
            palace = ListMember(palaces, 1);
            SetupQuestMusic(AIRootAgent);
            ListObjects(palace, "lair", -1, out lairs, MGPLConstants.Nohiddenmap);
            lairs2 = ListTitles(lairs, "ruined_altar1");
            booksite = ListMember(lairs2, 1);
            booksite["title"] = "booksite";
            Createnewinventoryitem(MGPLConstants.QitemBook, booksite);
            lairs2 = ListTitles(lairs, "ruined_shrine1");
            bellsite = ListMember(lairs2, 1);
            bellsite["title"] = "bellsite";
            Createnewinventoryitem(MGPLConstants.QitemBell, bellsite);
            lairs2 = ListTitles(lairs, "ruined_keep1");
            candlesite = ListMember(lairs2, 1);
            candlesite["title"] = "candlesite";
            Createnewinventoryitem(MGPLConstants.QitemCandle, candlesite);
            Messageflag(palace, MGPLConstants.MessageBbcIntro);
            ListObjects(palace, "special_item", -1, out chests, MGPLConstants.Nohiddenmap);
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
            palaces = ListPalaces();
            palace = ListMember(palaces, 1);
            ListObjects(palace, "lair", -1, out lairs, MGPLConstants.Nohiddenmap);
            ListObjects(palace, "building", -1, out buildings, MGPLConstants.Nohiddenmap);
            if (AIRootAgent["Message_Check_1"] == false)
            {
                if (Ismessageflagpresent(MGPLConstants.MessageBbcIntro) == false)
                {
                    buildings = ListTitles(buildings, "blacksmith");
                    if (ListSize(buildings) > 0)
                    {
                        bldg = ListMember(buildings, 1);
                        Messageflag(bldg, MGPLConstants.MessageBbcBlacksmith);
                    }
                    AIRootAgent["Message_Check_1"] = true;
                }
            }
            else
            {
                if (AIRootAgent["Message_Check_2"] == false)
                {
                    if (Ismessageflagpresent(MGPLConstants.MessageBbcBlacksmith) == false)
                    {
                        buildings = ListTitles(buildings, "guardhouse");
                        if (ListSize(buildings) > 0)
                        {
                            bldg = ListMember(buildings, 1);
                            Messageflag(bldg, MGPLConstants.MessageBbcGuardhouse);
                        }
                        AIRootAgent["Message_Check_2"] = true;
                    }
                }
                else
                {
                    if (AIRootAgent["Message_Check_3"] == false)
                    {
                        if (Ismessageflagpresent(MGPLConstants.MessageBbcGuardhouse) == false)
                        {
                            buildings = ListTitles(buildings, "Inn");
                            if (ListSize(buildings) > 0)
                            {
                                bldg = ListMember(buildings, 1);
                                Messageflag(bldg, MGPLConstants.MessageBbcInn);
                            }
                            AIRootAgent["Message_Check_3"] = true;
                        }
                    }
                    else
                    {
                        if (AIRootAgent["Message_Check_4"] == false)
                        {
                            if (Ismessageflagpresent(MGPLConstants.MessageBbcInn) == false)
                            {
                                buildings = ListTitles(buildings, "Trading_post");
                                if (ListSize(buildings) > 0)
                                {
                                    bldg = ListMember(buildings, 1);
                                    Messageflag(bldg, MGPLConstants.MessageBbcTradingPost);
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
                        Messageflag(bldg, MGPLConstants.MessageBbcFoundBook);
                        AIRootAgent["Quest_Flag_1"] = true;
                    }
                }
                if (AIRootAgent["Quest_Flag_2"] == false)
                {
                    lairs2 = ListTitles(lairs, "bellsite");
                    if (ListSize(lairs2) > 0)
                    {
                        bldg = ListMember(lairs2, 1);
                        Messageflag(bldg, MGPLConstants.MessageBbcFoundBell);
                        AIRootAgent["Quest_Flag_2"] = true;
                    }
                }
                if (AIRootAgent["Quest_Flag_3"] == false)
                {
                    lairs2 = ListTitles(lairs, "candlesite");
                    if (ListSize(lairs2) > 0)
                    {
                        bldg = ListMember(lairs2, 1);
                        Messageflag(bldg, MGPLConstants.MessageBbcFoundCandle);
                        AIRootAgent["Quest_Flag_3"] = true;
                    }
                }
            }
        }

    }
}
