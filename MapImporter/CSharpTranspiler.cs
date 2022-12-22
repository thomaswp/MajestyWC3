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

        private static string CamelCase(string name)
        {
            return string.Join("", name.Split("_").Select(w => Capitalize(w)));
        }

        private static string Capitalize(string word)
        {
            if (word.Length == 0) return word;
            return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
        }

        public override string Visit(IParseTree tree)
        {
            return base.Visit(tree);
        }

        public override string VisitFunc_dec([NotNull] MGPLParser.Func_decContext context)
        {
            vars.Clear();
            string code = string.Format("public static void {0}({1}) {2}\n\n",
                CamelCase(Visit(context.NAME())), Visit(context.func_variables()), Visit(context.func_body()));
            //Debug.WriteLine(code);
            return code;
        }

        public override string VisitTerminal(ITerminalNode node)
        {
            return node.GetText();
        }

        public override string VisitFunc_variables([NotNull] MGPLParser.Func_variablesContext context)
        {
            string code = string.Join(", ", context.func_var_dec().Select(n => Visit(n)));
            return code;
        }

        public override string VisitFunc_var_dec([NotNull] MGPLParser.Func_var_decContext context)
        {
            string type = Visit(context.type());
            type = Capitalize(type);
            string code = string.Join(", ", context.NAME().Select(n => type + " " + Visit(n)));
            return code;
        }

        public override string VisitFunc_body([NotNull] MGPLParser.Func_bodyContext context)
        {
            return Visit(context.block());
        }

        public override string VisitBlock([NotNull] MGPLParser.BlockContext context)
        {

            string code = "{\n" + string.Join("\n", context.stat().Select(s => Visit(s)));
            if (context.laststat() != null)
            {
                code += "\n" + Visit(context.laststat());
            }
            code += "\n}";
            return code;
        }

        public override string VisitVar([NotNull] MGPLParser.VarContext context)
        {
            string code = base.VisitVar(context);
            if (code.StartsWith("$")) code = code.Substring(1);
            if (code.StartsWith("#"))
            {
                code = "Constants." + code.Substring(1);
            }
            return code;
        }

        //public override string VisitFunctioncall([NotNull] MGPLParser.FunctioncallContext context)
        //{
        //    string code = string.Format("{0}{1}",
        //        Capitalize(Visit(context.varOrExp())),

        //    return base.VisitFunctioncall(context);
        //}

        public override string VisitVarlist([NotNull] MGPLParser.VarlistContext context)
        {
            string code = Visit(context.var()[0]);
            if (!vars.Contains(code))
            {
                vars.Add(code);
                code = "var " + code;
            }
            return code;
        }

        public override string VisitStat([NotNull] MGPLParser.StatContext context)
        {
            return base.VisitStat(context);
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
