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
    public class Visitor : MGPLBaseVisitor<object>
    {
        public override object VisitFunc_dec([NotNull] MGPLParser.Func_decContext context)
        {
            Debug.WriteLine(context.NAME());
            return base.VisitFunc_dec(context);
        }
    }
}
