using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    class ImplicitTestClass
    {
        ImplicitTestClass child;

        public static implicit operator ImplicitTestClass(int x) => new ImplicitTestClass();

        static void ConstructorBug()
        {
            new ImplicitTestClass() { child = 0 };
        }

        static IEnumerable<ImplicitTestClass> GetEnumBug()
        {
            yield return 0;
        }

        static ImplicitTestClass ReturnNoBug()
        {
            return 0;
        }

        static void ParamMethod(ImplicitTestClass x) { }
        static void ParamNoBug()
        {
            ParamMethod(0);
        }
    }
}
