using Antlr4.Runtime;
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
using System.Text.RegularExpressions;
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
        //const string TestMap = MajestyDir + @"Quests\Bell_book_candle.q";
        const string TestMap = MajestyDir + @"Quests\fertile_plain.q";
        //const string QuestLogic = MajestyDir + @"SDK\OriginalQuests\GPL\Rules\epic_quest_scripts.gpl";
        const string QuestLogic = MajestyDir + @"SDK\OriginalQuests\GPL\Rules\epic_quest_scripts_sample.gpl";

        const string ConstantsFilesDir = MajestyDir + @"SDK\OriginalQuests\GPL\";
        static readonly string[] ConstantsFiles = new string[]
        {
            "defines.gpl",
            "globals.gpl",
            "LowLevel.gpl",
            "QItems.gpl",
            @"Rules\epic_quest_scripts.gpl",
            @"Rules\victory_conditions.gpl",
            @"TaskModules\Buildings\tax_spawner.gpl",
            @"TaskModules\Subtasks\Spells.gpl",
        };

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

            new MapLib.Layout(new TestLayout(2, 1234), quest).Start();

            string questLogic = File.ReadAllText(QuestLogic);

            try
            {
                AntlrInputStream inputStream = new AntlrInputStream(questLogic);
                MGPLLexer lexer = new MGPLLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                MGPLParser parser = new MGPLParser(commonTokenStream);

                parser.AddErrorListener(new ErrorListener());

                var code = parser.chunk();

                //Debug.WriteLine(code.GetText());

                CSharpTranspiler visitor = new CSharpTranspiler();
                string transpiled = "" + visitor.Visit(code);
                //Debug.WriteLine(transpiled);
                this.textBox1.Text = transpiled.Replace("\n", "\r\n");

                foreach (string name in visitor.funcCalls)
                {
                    //Debug.WriteLine(string.Format("protected static void {0}()", name) + "\n{\n}\n");
                }

                foreach (string name in visitor.constants)
                {
                    //Debug.WriteLine(string.Format("public static int {0};", name));
                }


            } catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            Regex pattern = new Regex(@"\s*expression\s+#([a-zA-Z0-9_-]+)\s+([0-9]*)");
            foreach (string path in ConstantsFiles)
            {
                string text = File.ReadAllText(ConstantsFilesDir + path);
                string[] lines = text.Split('\n');
                foreach (string line in lines)
                {
                    var match = pattern.Match(line);
                    if (match.Success)
                    {
                        string name = CSharpTranspiler.CamelCase(match.Groups[1].Value.Trim());
                        string expr = match.Groups[2].Value.Trim();
                        Debug.WriteLine("public const int " + name + " = " + expr + ";");
                    } else if (line.Contains("expression") && line.Contains("#"))
                    {
                        Debug.WriteLine("// " + line);
                    }
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
