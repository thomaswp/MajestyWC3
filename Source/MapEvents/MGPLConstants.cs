using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.MapEvents
{
    public class MGPLConstants
    {
        public static int easyMonster;
        public static int QNumber_Barren_Waste;
        public static int Message_Barren_start;
        public static int default_spawn_treasure_dist;
        public static int NoHiddenMap;
        public static int sign_barren;
        public static int VictoryCondition_callback_frequency;
        public static int MyPlayer;
        public static int message_barren_guild_built;
        public static int ATTRIB_currentstagebuilt;
        public static int message_barren_Palace_Upgraded;
        public static int message_barren_Palace_third;
        public static int NotMyPlayer;
        public static int message_barren_fairground;
        public static int QNumber_Bell_Book;
        public static int QItem_Book;
        public static int QItem_Bell;
        public static int QItem_Candle;
        public static int message_bbc_intro;
        public static int message_bbc_blacksmith;
        public static int message_bbc_guardhouse;
        public static int message_bbc_inn;
        public static int message_bbc_trading_post;
        public static int message_bbc_found_book;
        public static int message_bbc_found_bell;
        public static int message_bbc_found_candle;

        public static string GetTextConstant(int value)
        {
            Console.WriteLine("Unknown text constant: " + value);
            return "";
        }
    }
}
