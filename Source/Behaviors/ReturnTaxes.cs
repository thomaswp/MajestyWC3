using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source.Behaviors
{
    public class ReturnTaxes : RestAtHome
    {
        const int EXIT_CHECK_WAIT_FRAMES = 60;
        int exitCheckWait = 0;

        public override string GetStatusGerund()
        {
            return $"returning taxes to {AI.Home.GetName()}";
        }

        public override void Start()
        {
            base.Start();
            exitCheckWait = 0;
        }

        protected override bool ShouldExit()
        {
            if (exitCheckWait > 0)
            {
                exitCheckWait--;
                return false;
            }
            exitCheckWait = EXIT_CHECK_WAIT_FRAMES;
            return TaxCollect.GetTaxableUnits(AI.HumanPlayer).Any();
        }

        protected override void OnReturned()
        {
            int gold = AI.Gold;
            AI.Gold -= gold;
            AI.HumanPlayer.ChangeGoldBy(gold);
        }

        public override float StartWeight()
        {
            return AI.Gold / 1000f + 1;
        }
    }
}
