using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using static Source.Util;
using WCSharp.Events;

namespace Source.MapEvents
{
    class MapEventsBase
    {
        public static int MajestyUnitToWC3Unit(string type)
        {
            Console.WriteLine("Unknown unit type: " + type);
            return 0;
        }

        protected static void Enableunittype(string type)
        {
            SetUnitEnabled(type, true);
        }

        protected static void Disableunittype(string type)
        {
            SetUnitEnabled(type, false);
        }

        private static void SetUnitEnabled(string type, bool enabled)
        {
            int wc3Type = MajestyUnitToWC3Unit(type);
            if (wc3Type == 0) return;
            SetPlayerUnitAvailableBJ(wc3Type, enabled, GetLocalPlayer());
        }

        static Agent GPLRootAgent = new Agent();

        protected static Agent RetrieveAgent(string name)
        {
            if ("GplAIRoot".Equals(name, StringComparison.CurrentCultureIgnoreCase)) return GPLRootAgent;
            Console.WriteLine($"Unknown Agent: {name}");
            return null;
        }

        protected static void SetupQuestMusic(Agent agent)
        {
            // NOOP
        }

        protected static void ElvesvoiceSetoperative(int isActive)
        {
            // NOOP
        }

        protected static void DwarvesvoiceSetoperative(int isActive)
        {
            // NOOP
        }

        protected static void PlayEndgameMusic(Agent agent)
        {
            // NOOP
        }

        protected static void ResetQuestMusic(Agent agent)
        {
            // NOOP
        }

        protected static void Messageflag(Agent unit, int value)
        {
            unit wc3Unit = unit.ToUnit();
            MessageFlags.CreateMessageFlag(wc3Unit.GetLocation(), wc3Unit.GetPlayer(), Constants.GetTextConstant(value));
        }

        protected static void SetupRandomTreasure(int number, int distributionType)
        {
            // TODO: Neeg to figire out what this does
        }

        protected static void ListObjects(Agent agent, string type, int limit, out List holdingList, int searchType = 0, int otherconst = 0)
        {
            holdingList = null;
        }

        protected static void PostMessage(Agent agent, int postType)
        {
            // TODO: I think this sets a message on a sign unit (different than a message flag)
        }

        protected static Dictionary<Action, trigger> threadMap = new();

        protected static void NewThread(object action, int frequency)
        {
            if (!(action is GPLAction))
            {
                Console.WriteLine($"NewThread called with non-action: " + action);
                return;
            }
            Action action1= (Action)action;
            trigger t = CreateTrigger();
            TriggerAddAction(t, action1);
            TriggerRegisterTimerEventPeriodic(t, frequency); // TODO: may need to convert
            threadMap[action1] = t;
        }

        protected static void Killthread(Action action)
        {
            if (!threadMap.ContainsKey(action)) return;
            trigger t = threadMap[action];
            threadMap.Remove(action);
            DestroyTrigger(t);
        }

        protected static void Declarevictory(Agent agent, Agent agent2 = null)
        {
            CustomVictoryDialogBJ(agent.ToUnit().GetPlayer());
        }

        protected static void Createnewinventoryitem(int itemtype, Agent location)
        {
            UnitAddItemById(location.ToUnit(), itemtype);
        }

        protected static void SetupStartingTreasure(List list, int number, int number2)
        {
            // TODO: Need to figure out what this does
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
