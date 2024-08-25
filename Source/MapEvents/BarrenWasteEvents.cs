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
            AIRootAgent["Quest_Number"] = MGPLConstants.QnumberBarrenWaste;
            palaces = ListPalaces();
            palace = ListMember(palaces, 1);
            SetupQuestMusic(AIRootAgent);
            Disableunittype("Wizards_guild1");
            Disableunittype("Elven_bungalow");
            Disableunittype("Dwarven_settlement");
            Disableunittype("Gnome_hovel");
            Disableunittype("fairgrounds");
            ElvesvoiceSetoperative(0);
            DwarvesvoiceSetoperative(0);
            Messageflag(palace, MGPLConstants.MessageBarrenStart);
            SetupRandomTreasure(20, MGPLConstants.DefaultSpawnTreasureDist);
            ListObjects(palace, "color", -1, out signs, MGPLConstants.Nohiddenmap);
            signs = ListTitles(signs, "Banner_wood");
            sign = ListMember(signs, 1);
            PostMessage(sign, MGPLConstants.SignBarren);
            AIRootAgent = RetrieveAgent("GplAIRoot");
            AIRootAgent["VictoryCondition"] = new Action(BarrenVictory);
            NewThread(AIRootAgent["VictoryCondition"], MGPLConstants.VictoryconditionCallbackFrequency);
        }


        public static void BarrenVictory()
        {
            Agent AIRootAgent, palace;
            List palaces, bldgs;
            AIRootAgent = RetrieveAgent("GplAIRoot");
            palaces = ListPalaces();
            palace = ListMember(palaces, 1);
            if (AIRootAgent["Quest_Flag_1"] == false)
            {
                ListObjects(palace, "building", -1, out bldgs, MGPLConstants.Myplayer);
                bldgs = ListSubtypes(bldgs, "guild");
                bldgs = ListCompleted(bldgs);
                if (ListSize(bldgs) > 0)
                {
                    Messageflag(ListMember(bldgs, 1), MGPLConstants.MessageBarrenGuildBuilt);
                    AIRootAgent["Quest_Flag_1"] = true;
                }
            }
            else
            {
                if (AIRootAgent["Quest_Flag_2"] == false)
                {
                    if ((palace["level"] == 2) && (Getattribute(palace, MGPLConstants.ATTRIB_currentstagebuilt) == 1))
                    {
                        Messageflag(palace, MGPLConstants.MessageBarrenPalaceUpgraded);
                        AIRootAgent["Quest_Flag_2"] = true;
                    }
                }
                else
                {
                    if (AIRootAgent["Quest_Flag_3"] == false)
                    {
                        if ((palace["level"] == 3) && (Getattribute(palace, MGPLConstants.ATTRIB_currentstagebuilt) == 1))
                        {
                            Messageflag(palace, MGPLConstants.MessageBarrenPalaceThird);
                            AIRootAgent["Quest_Flag_3"] = true;
                        }
                    }
                    else
                    {
                        if (AIRootAgent["Quest_Flag_4"] == false)
                        {
                            ListObjects(palace, "lair", -1, out bldgs, MGPLConstants.Notmyplayer, MGPLConstants.Nohiddenmap);
                            bldgs = ListTitles(bldgs, "Dark_castle");
                            if (ListSize(bldgs) == 0)
                            {
                                Messageflag(palace, MGPLConstants.MessageBarrenFairground);
                                Enableunittype("fairgrounds");
                                AIRootAgent["Quest_Flag_4"] = true;
                                PlayEndgameMusic(AIRootAgent);
                            }
                        }
                        else
                        {
                            if (AIRootAgent["Quest_Flag_5"] == false)
                            {
                                ListObjects(palace, "building", -1, out bldgs, MGPLConstants.Myplayer);
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
