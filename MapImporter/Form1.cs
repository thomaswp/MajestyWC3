using MapLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapImporter
{
    public partial class Form1 : Form
    {
        const string MajestyDir = @"C:\Program Files (x86)\Steam\steamapps\common\Majesty HD\";
        static readonly string[] QuestDirs = new string[]
        {
            @"Quests", @"QuestsMX"
        }.Select(s => MajestyDir + s).ToArray();
        const string TestMap = MajestyDir + @"Quests\Bell_book_candle.q";
        //const string TestMap = MajestyDir + @"Quests\fertile_plain.q";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var stream = File.OpenRead(TestMap);
            Quest quest = new MapParser(stream).ParseQuest();
            Debug.WriteLine(quest.Summarize());
            DumpOptions options = new DumpOptions();
            //options.MaxLevel = 100;
            //options.SetPropertiesOnly = false;
            options.DumpStyle = DumpStyle.CSharp;
            options.IndentSize = 4;
            Debug.WriteLine(ObjectDumper.Dump(quest, options));
        }
    }
}
