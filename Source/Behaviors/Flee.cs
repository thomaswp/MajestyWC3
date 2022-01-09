using Source.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source;

namespace Source.Behaviors
{
    public class Flee : RestAtHome
    {
        Random rand = new Random();

        public override bool CanStart()
        {
            return IsInDanger((float)rand.NextDouble());
        }

        public override void Start()
        {
            base.Start();
            Console.WriteLine($"{AI.Name} fleeing enemiez");
        }
        public override bool IsInCombatOrDanger()
        {
            return true;
        }

        public bool IsInDanger(float fearFactor)
        {
            // TODO: Customize based on unit
            var units = AI.Unit.GetVisibleUnits()
                .Where(u => !u.IsDead() && !u.IsStructure());
            float confidence = units
                .Where(u => !AI.IsEnemy(u) && IsHeroUnitId(u.GetTypeID()))
                .Select(u => AI.GetIntimidation(u))
                .Sum();
            confidence *= AI.Unit.GetHPFraction();
            //Console.WriteLine(enemies.Aggregate("Enemies: ", (s, u) => s + u.GetName() + " "));
            float intimidation = units
                .Where(u => AI.IsEnemy(u))
                .Select(u => AI.GetIntimidation(u))
                .Sum();
            //Console.WriteLine($"Calculating intimidation of {units.Count()} units = " +
            //    $"{intimidation} * {fearFactor} vs {confidence}");
            return intimidation * fearFactor > confidence;
        }
    }
}
