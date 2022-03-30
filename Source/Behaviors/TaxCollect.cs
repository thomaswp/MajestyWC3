using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source.Units;

namespace Source.Behaviors
{
    public class TaxCollect : TargetBehavior<unit>
    {
        const float DISTANCE_MODIFIER = 0.01f;

        OrderChain orders;

        public override string GetStatusGerund()
        {
            return $"collecting taxes from {Target.GetName()}";
        }

        public override void Start()
        {
            base.Start();
            orders = new OrderMove(Target.GetLocation(), OrderMove.BUILDING_RADIUS)
                .Init(AI)
                .Then(new OrderEnter(Target))
                .Then(new OrderWait(1))
                .Then(new OrderDo(() => Collect(Target)))
                .Then(new OrderExit())
                ;
        }

        private void Collect(unit target)
        {
            int gold = Buildings.GetGold(target);
            Buildings.ChangeGold(target, -gold);
            AI.GoldTaxed += gold;
        }

        public override bool Update()
        {
            if (!base.Update()) return false;
            return orders.Update();
        }

        public override void Stop()
        {
            AI.ExitBuilding();
        }

        protected override bool IsTargetStillValid(unit target)
        {
            if (!base.IsTargetStillValid(target)) return false;
            return IsTaxable(target);
        }

        private static bool IsTaxable(unit target)
        {
            if (target.IsDead()) return false;
            Building b = Buildings.Get(target);
            if (b == null) return false;
            if (!b.Info.CanBeTaxed) return false;
            return b.Gold > 0;
        }

        public static IEnumerable<unit> GetTaxableUnits(player player)
        {
            return GetUnitsOfPlayerAll(player).ToList()
                .Where(u => u.IsStructure() && IsTaxable(u));
        }

        protected override unit SelectTarget()
        {
            var taxable = GetTaxableUnits(AI.HumanPlayer);
            if (!taxable.Any()) return null;

            return taxable
                .OrderBy(u => -Buildings.Get(u).Gold + DISTANCE_MODIFIER * u.DistanceTo(AI.Unit))
                .First();
        }
    }
}
