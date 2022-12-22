using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.MapEvents
{
    class MapEventsBase
    {
        protected static void disableunittype(string type)
        {

        }

        protected static Agent RetrieveAgent(string name)
        {
            return null;
        }

        protected static void Setup_Quest_Music(Agent agent)
        {
        }

        protected static void ElvesVoice_setOperative(int isActive)
        {
        }

        protected static void dwarvesVoice_setOperative(int isActive)
        {
        }

        protected static void messageflag(Agent unit, int value)
        {
        }

        protected static void setup_random_treasure(int number, int distributionType)
        {
        }

        protected static void listobjects(Agent agent, string type, int limit, out List holdingList, int searchType, int otherconst = 0)
        {
            holdingList = null;
        }

        protected static void post_message(Agent agent, int postType)
        {
        }

        protected static void NewThread(object action, int frequency)
        {

        }

        protected static void enableunittype(string type)
        {
        }

        protected static void Play_Endgame_Music(Agent agent)
        {
        }

        protected static void Reset_Quest_Music(Agent agent)
        {
        }

        protected static void declarevictory(Agent agent, Agent agent2 = null)
        {
        }

        protected static void KillThread(GPLAction action)
        {
        }

        protected static void CreateNewInventoryItem(int itemtype, Agent location)
        {
        }

        protected static void setup_starting_treasure(List list, int number)
        {
        }

        protected static List listtitles(List list, string title)
        {
            return list;
        }

        protected static List ListPalaces()
        {
            return null;
        }

        protected static Agent listmember(List list, int index)
        {
            return null;
        }

        protected static List listsubtypes(List list, string type)
        {
            return null;
        }

        protected static List listcompleted(List list)
        {
            return null;
        }

        protected static int listsize(List list)
        {
            return 0;
        }

        protected static int getattribute(Agent agent, int index)
        {
            return 0;
        }

        protected static int random_time(int max)
        {
            return 0;
        }
    }
}
