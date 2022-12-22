using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.MapEvents
{
    class MapEventsBase
    {
        protected static void Disableunittype(string type)
        {

        }

        protected static Agent RetrieveAgent(string name)
        {
            return null;
        }

        protected static void SetupQuestMusic(Agent agent)
        {
        }

        protected static void ElvesvoiceSetoperative(int isActive)
        {
        }

        protected static void DwarvesvoiceSetoperative(int isActive)
        {
        }

        protected static void Messageflag(Agent unit, int value)
        {
        }

        protected static void SetupRandomTreasure(int number, int distributionType)
        {
        }

        protected static void ListObjects(Agent agent, string type, int limit, out List holdingList, int searchType = 0, int otherconst = 0)
        {
            holdingList = null;
        }

        protected static void PostMessage(Agent agent, int postType)
        {
        }

        protected static void NewThread(object action, int frequency)
        {

        }

        protected static void Enableunittype(string type)
        {
        }

        protected static void PlayEndgameMusic(Agent agent)
        {
        }

        protected static void ResetQuestMusic(Agent agent)
        {
        }

        protected static void Declarevictory(Agent agent, Agent agent2 = null)
        {
        }

        protected static void Killthread(GPLAction action)
        {
        }

        protected static void Createnewinventoryitem(int itemtype, Agent location)
        {
        }

        protected static void SetupStartingTreasure(List list, int number, int number2)
        {
        }

        protected static List ListTitles(List list, string title)
        {
            return list;
        }

        protected static List Listpalaces()
        {
            return null;
        }

        protected static Agent ListMember(List list, int index)
        {
            return null;
        }

        protected static List ListSubtypes(List list, string type)
        {
            return null;
        }

        protected static List ListCompleted(List list)
        {
            return null;
        }

        protected static int ListSize(List list)
        {
            return 0;
        }

        protected static int Getattribute(Agent agent, int index)
        {
            return 0;
        }

        protected static int RandomTime(int max)
        {
            return 0;
        }

        protected static bool Ismessageflagpresent(int where)
        {
            return false;
        }

        protected static void EndGameScriptEasy()
        {
            // 1 - 2 monsters every 10ish days
            Agent AIRootAgent;

            AIRootAgent = RetrieveAgent("GplAIRoot");
            SpawnMonsters(3, Constants.easyMonster);

	        Setthreadinterval(AIRootAgent["VictoryCondition2"], RandomTime(600000));
        }

        protected static void SpawnMonsters(int n, int type)
        {

        }

        protected static void Setthreadinterval(object action, int time)
        {

        }
    }
}
