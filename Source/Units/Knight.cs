using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static War3Api.Common;
using static War3Api.Blizzard;
using Source;

namespace Source.Units
{
    public class Knight : Hero
    {

        public override void Update()
        {
            //Console.WriteLine($"Updating knight");
            UpdateExploration();
        }
    }
}
