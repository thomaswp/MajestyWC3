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

        public override string VisitFunc_dec([NotNull] MGPLParser.Func_decContext context)
        {
            Debug.WriteLine(context.NAME());
            return base.VisitFunc_dec(context);
        }

        public override string VisitFunc_var_dec([NotNull] MGPLParser.Func_var_decContext context)
        {
            Debug.WriteLine(context.NAME());
            return base.VisitFunc_var_dec(context);
        }

        public override string VisitStat([NotNull] MGPLParser.StatContext context)
        {
            return base.VisitStat(context);
        }

        public override string VisitChildren(IRuleNode node)
        {
            //Debug.WriteLine("{1} {2} \"{3}\"", node, node.GetType(), node.ChildCount, node.GetText());
            if (node.ChildCount == 1)
            {
                return AggregateResult(node.GetText(), base.VisitChildren(node));
            }
            return base.VisitChildren(node);
        }

        protected override string DefaultResult => "";

        protected override string AggregateResult(string aggregate, string nextResult)
        {
            if (nextResult == null)
            {
                nextResult = "";
            }

            return aggregate + nextResult;
        }

        public override string VisitErrorNode(IErrorNode node)
        {
            Debug.WriteLine(node);
            return base.VisitErrorNode(node);
        }
    }
}
