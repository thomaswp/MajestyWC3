using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapImporter
{
    public class CSharpTranspiler : MGPLBaseVisitor<string>
    {
        private List<string> vars = new List<string>();
        private List<string> storedStats = new List<string>();
        private Dictionary<string, string> varSubs = new Dictionary<string, string>();
        private int indentLevel = 0;
        public List<string> funcCalls = new List<string>();
        public List<string> constants = new List<string>();

        private static string CamelCase(string name)
        {
            //string firstChar = name.Substring(0, 1);
            //string rest = name.Substring(1);
            //if (firstChar == firstChar.ToUpper() && rest != rest.ToLower())
            //{
            //    // If the name starts with a capital letter and the rest isn't lowercase
            //    // it's already as good as it'll get.
            //    return name;
            //}
            if (name.ToLower().StartsWith("list")) name = "List_" + name.Substring(4);
            return string.Join("", name.Split("_").Select(w => Capitalize(w)));
        }

        private static string Capitalize(string word)
        {
            if (word.Length == 0) return word;
            return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
        }

        private string GetIndent()
        {
            return new string(' ', 4 * indentLevel);
        }

        public override string Visit(IParseTree tree)
        {
            return base.Visit(tree);
        }

        public override string VisitFunc_dec([NotNull] MGPLParser.Func_decContext context)
        {
            vars.Clear();
            string funcName = Visit(context.NAME());
            string funcNameCamel = CamelCase(funcName);
            varSubs.Add(funcName, funcNameCamel);
            Visit(context.func_variables());
            string code = string.Format("public static void {0}() {1}\n\n",
                funcNameCamel, 
                Visit(context.func_body()));
            //Debug.WriteLine(code);
            return code;
        }

        public override string VisitIf_stat([NotNull] MGPLParser.If_statContext context)
        {
            var bodies = context.stat_or_block();
            string code = string.Format("if ({0}) {1}",
                Visit(context.exp()),
                Visit(bodies[0]));
            if (bodies.Length == 2)
            {
                code += string.Format(" else {0} ", Visit(bodies[1]));
            }

            return code;
        }

        public override string VisitStat_or_block([NotNull] MGPLParser.Stat_or_blockContext context)
        {
            string code;
            if (context.block() != null)
            {
                code = Visit(context.block());
            } 
            else
            {
                code = " {\n";
                indentLevel++;
                code += Visit(context.stat());
                indentLevel--;
                code += string.Format("\n{0}", GetIndent()) + "}";
            }
            return code;
        }

        public override string VisitTerminal(ITerminalNode node)
        {
            string code = node.GetText();
            if (code == "<EOF>") return "";
            return code;
        }

        public override string VisitFunc_variables([NotNull] MGPLParser.Func_variablesContext context)
        {
            foreach (var line in context.func_var_dec().Select(n => Visit(n)))
            {
                storedStats.Add(line);
            }
            return "";
        }

        private string addVar(string var)
        {
            vars.Add(var);
            varSubs[var.ToLower()] = var;
            return var;
        }

        public override string VisitFunc_var_dec([NotNull] MGPLParser.Func_var_decContext context)
        {
            string type = Visit(context.type());
            string suffix = "";
            if (type.ToLower() == "integer")
            {
                type = "int";
                suffix = " = 0";
            }
            else
            {
                type = Capitalize(type);
            }
            string code = type + " " + string.Join(", ", context.NAME().Select(n => addVar(Visit(n)) + suffix)) + ";";
            return code;
        }

        public override string VisitFunc_body([NotNull] MGPLParser.Func_bodyContext context)
        {
            return Visit(context.block());
        }

        public override string VisitBlock([NotNull] MGPLParser.BlockContext context)
        {

            indentLevel++;
            string code = "{\n";
            foreach (var line in storedStats)
            {
                code += GetIndent() + line + "\n";
            }
            storedStats.Clear();
            code += string.Join("\n", context.stat().Select(s => Visit(s)));
            if (context.laststat() != null)
            {
                code += "\n" + Visit(context.laststat());
            }
            indentLevel--;
            code += "\n" + GetIndent() + "}";
            return code;
        }

        public override string VisitVar([NotNull] MGPLParser.VarContext context)
        {
            string code = base.VisitVar(context);
            string codeKey = code.ToLower();
            if (varSubs.ContainsKey(codeKey))
            {
                return varSubs[codeKey];
            }
            if (code.StartsWith("$"))
            {
                code = CamelCase(code.Substring(1));
                // Hard to distinguish delegates... may have to do manually
                //if (!code.Contains("[") && !code.Contains("("))
                //{
                //    varSubs[codeKey] = code;
                //    return string.Format("new Action(() => {0}())", code);
                //}
            }
            else if (code.StartsWith("#"))
            {
                string v = code.Substring(1);
                if (!constants.Contains(v)) constants.Add(v);
                code = "Constants." + v;
            }
            varSubs[codeKey] = code;
            return code;
        }

        public override string VisitPropertyAccessor([NotNull] MGPLParser.PropertyAccessorContext context)
        {
            string code = string.Format("[{0}]", Visit(context.@string()));
            return code;
        }

        public override string VisitExp([NotNull] MGPLParser.ExpContext context)
        {
            string code = base.VisitExp(context);
            string lowerCode = code.ToLower();
            if (lowerCode == "true" || lowerCode == "false") return lowerCode;
            return code;
        }

        public override string VisitFunctioncall([NotNull] MGPLParser.FunctioncallContext context)
        {
            string name = Visit(context.varOrExp());
            if (!funcCalls.Contains(name)) funcCalls.Add(name);
            string code = base.VisitFunctioncall(context);
            return code;
        }

        public override string VisitVarlist([NotNull] MGPLParser.VarlistContext context)
        {
            string code = Visit(context.var()[0]);
            if (!code.Contains("[") && !vars.Contains(code))
            {
                vars.Add(code);
                code = "var " + code;
            }
            return code;
        }

        public override string VisitStat([NotNull] MGPLParser.StatContext context)
        {
            string code = string.Format("{0}{1}",
                GetIndent(),
                base.VisitStat(context));
            return code;
        }

        public override string VisitChildren(IRuleNode node)
        {
            //Debug.WriteLine("{1} {2} \"{3}\"", node, node.GetType(), node.ChildCount, node.GetText());
            //if (node.ChildCount == 1)
            //{
            //    return AggregateResult(node.GetText(), base.VisitChildren(node));
            //}
            return base.VisitChildren(node);
        }

        protected override string DefaultResult => "";

        protected override string AggregateResult(string aggregate, string nextResult)
        {
            return aggregate + nextResult;
        }

        public override string VisitErrorNode(IErrorNode node)
        {
            Debug.WriteLine(node);
            return base.VisitErrorNode(node);
        }
    }
}
