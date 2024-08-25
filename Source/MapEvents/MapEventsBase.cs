using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using static Source.Util;
using WCSharp.Events;
using Source.Units;
using Source.Interface;
using Source.Units.Monsters;

namespace Source.MapEvents
{
    class MapEventsBase
    {
        public static player QuestPlayer
        {
            get { return GetLocalPlayer(); }
        }

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
            SetPlayerUnitAvailableBJ(wc3Type, enabled, QuestPlayer);
        }

        static Agent GPLRootAgent = new Agent(null);

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
            unit wc3Unit = unit.Unit;
            MessageFlags.CreateMessageFlag(wc3Unit.GetLocation(), wc3Unit.GetPlayer(), MGPLConstants.GetTextConstant(value));
        }

        protected static void SetupRandomTreasure(int number, int distributionType)
        {
            // TODO: Neeg to figire out what this does
        }

        protected static void ListObjects(Agent agent, string type, int limit, out List holdingList, int flag1 = -1, int flag2 = -1)
        {
            holdingList = new List();
            List<player> players = new List<player>();
            HashSet<int> flags = new HashSet<int>
            {
                flag1,
                flag2
            };

            // TODO: Handle allies and not allies too
            if (!flags.Contains(MGPLConstants.Myplayer))
            {
                players.Add(Player(0));
                players.Add(Player(1));
            }
            if (!flags.Contains(MGPLConstants.Notmyplayer))
            {
                players.Add(Monster.Player);
            }

            List<unit> units = new List<unit>();
            foreach (player p in players)
            {
                units.AddRange(GetUnitsOfPlayerAll(p).ToList());
            }

            // No clear what this means... don't think it just means visible to the player
            if (flags.Contains(MGPLConstants.Nohiddenmap))
            {

            }

            // TODO add units with type filter type, up to the limit
            // TODO filter also by flags like visibility
            // TODO - may need to convert these to agents? and store a map of them to not recreate them?
        }

        protected static void PostMessage(Agent agent, int postType)
        {
            // TODO: I think this sets a message on a sign unit (different than a message flag)
        }

        protected static Dictionary<Action, trigger> threadMap = new();

        protected static void NewThread(Action action, int frequency)
        {
            trigger t = CreateTrigger();
            TriggerAddAction(t, action);
            // May need to add a conversion (e.g. s to ms?)
            TriggerRegisterTimerEventPeriodic(t, frequency);
            threadMap[action] = t;
        }

        protected static void Setthreadinterval(object action, int time)
        {
            if (!threadMap.TryGetValue(action as Action, out trigger t))
            {
                Console.WriteLine($"NewThread called with non-action: " + action);
                return;
            }
            // TODO: Need to release any existing time register
            TriggerRegisterTimerEventPeriodic(t, time); // TODO: may need to convert
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
            CustomVictoryDialogBJ(agent.Unit.GetPlayer());
        }

        protected static void Createnewinventoryitem(int itemtype, Agent location)
        {
            UnitAddItemById(location.Unit, itemtype);
        }

        protected static void SetupStartingTreasure(List list, int number, int number2)
        {
            // TODO: Need to figure out what this does
        }

        protected static List ListTitles(List list, string title)
        {
            return new List(list.items.Where(agent => agent.Title == title).ToList());
        }

        protected static List ListPalaces()
        {
            BuildingInfo castleInfo = Constants.UNIT_CASTLE_LEVEL_1;
            List list = new List();
            foreach (int id in castleInfo.UpgradeChain)
            {
                list.items.AddRange(GetUnitsOfTypeIdAll(id).ToList().Select(u => new Agent(u)));
            }
            return list;
        }

        protected static Agent ListMember(List list, int index)
        {
            // MGPL lists are 1-indexed
            return list.items[index - 1];
        }

        protected static List ListSubtypes(List list, string type)
        {
            if (type.ToLower() == "guid")
            {
                return new List(list.items.Where(agent => Guilds.IsGuild(agent)).ToList());
            }
            Console.WriteLine("Unknown subtype: " + type);
            return new List();
        }

        protected static List ListCompleted(List list)
        {
            // Hack: don't know how to check construction progress (other than keeping track of all starts and finishes)
            // So I'm just saying it needs to be at full health.
            return new List(list.items.Where(a => a.Unit.IsStructure() && a.Unit.GetHPFraction() > 0.99f).ToList());
        }

        protected static int ListSize(List list)
        {
            return list.items.Count;
        }

        protected static int Getattribute(Agent agent, int index)
        {
            // TODO
            return 0;
        }

        protected static int RandomTime(int max)
        {
            // TODO
            return 0;
        }

        protected static bool Ismessageflagpresent(int where)
        {
            // TODO
            return false;
        }

        protected static void EndGameScriptEasy()
        {
            // 1 - 2 monsters every 10ish days
            Agent AIRootAgent;

            AIRootAgent = RetrieveAgent("GplAIRoot");
            SpawnMonsters(3, MGPLConstants.Easymonster);

	        Setthreadinterval(AIRootAgent["VictoryCondition2"], RandomTime(600000));
        }

        protected static void SpawnMonsters(int n, int type)
        {
            // TODO
        }
    }
}
