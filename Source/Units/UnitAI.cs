using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source;
using WCSharp.Events;
using Source.Behaviors;
using Source.Items;
using Source.Interface;
using Source.Units.Monsters;

namespace Source.Units
{
    public abstract class UnitAI
    {

        public unit Unit { get; private set; }
        public string Name { get { return Unit.GetName(); } }
        public string ProperName { get 
            {
                string properName = GetHeroProperName(Unit);
                if (properName != null && properName.Length != 0) return properName;
                return Name;
            } }
        public player MyPlayer { get; private set; }
        public player HumanPlayer { get; private set; }
        public player AIPlayer { get; private set; }

        public bool IsNeutralHostile { 
            get {
                return MyPlayer == Monster.Player;
            } 
        }

        public unit Home { get; private set; }
        public unit InBuilding { get; set; }
        public bool IsInBuilding { get { return InBuilding != null; } }
        public bool IsHome { get { return InBuilding != null && InBuilding == Home; } }

        public int GoldTaxed { get; set; }
        public int GoldUntaxed { get; set; }

        private string _status = "";
        public string Status {
            get { return _status; } 
            set
            {
                if (_status == value) return;
                _status = value;
                if (IsUnitSelected(Unit, HumanPlayer))
                {
                    //Interface.Status.ShowStatus(this);
                }
            }
        }

        const float RECOVER_RATE = 0.01f, RECOVER_RATE_AT_HOME = RECOVER_RATE * 2;

        public int Gold 
        { 
            get { return GoldTaxed + GoldUntaxed; } 
            set
            {
                int oldGold = Gold;
                // Gaining gold, goes to untaxed by default
                if (value > oldGold) GoldUntaxed += value - oldGold;
                else
                {
                    // Losing gold, comes from taxed first, then untaxed
                    int loss = oldGold - value;
                    if (loss > GoldTaxed) GoldUntaxed -= loss - GoldTaxed;
                    GoldTaxed = Math.Max(0, GoldTaxed - loss);
                }
            }
        }

        protected List<Behavior> behaviors = new List<Behavior>();
        protected Behavior behavior = null;

        private static Dictionary<unit, UnitAI> unitMap = new Dictionary<unit, UnitAI>();

        public static void Init()
        {
            PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeIsCreated, () =>
            {
                unit unit = GetTriggerUnit();
                try
                {
                    if (unit.IsStructure()) return;

                    if (GetUnitTypeId(unit) == Constants.UNIT_PEASANT_WORKER)
                    {
                        SetUnitOwner(unit, GetOwningPlayer(unit).GetAIForHuman(), false);
                        RegisterUnit(unit);
                    }
                    else if (IsHeroUnitId(unit.GetTypeID()))
                    {
                        //Console.WriteLine($"{GetConstructingStructure().GetName()} constr {unit.GetName()}");
                        SetUnitOwner(unit, GetOwningPlayer(unit).GetAIForHuman(), false);
                        RegisterUnit(unit);
                    }

                    //string name = GetUnitName(unit);
                    //Console.WriteLine($"Created {(unit == null ? "NONE" : name)}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error initializing {unit.GetName()}: {e.Message}");
                }
            });

            PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeFinishesTraining, () =>
            {
                //Console.WriteLine($"Trained: {GetTriggerUnit().GetName()}, {GetTrainedUnit().GetName()}");
                unit unit = GetTrainedUnit();
                unit building = GetTriggerUnit();
                UnitAI ai = RegisterUnit(unit);
                if (ai != null) ai.SetHome(building);
            });

            AnyUnitEvents.Register(EVENT_PLAYER_UNIT_ATTACKED, () =>
            {
                Trigger(PlayerUnitEvent.UnitTypeAttacks, GetAttacker());
                Trigger(PlayerUnitEvent.UnitTypeIsAttacked, GetAttackedUnitBJ());
                TriggerHomeAttacked(GetAttackedUnitBJ(), GetAttacker());
            });

            AnyUnitEvents.Register(EVENT_PLAYER_UNIT_DEATH, () =>
            {
                unit unit = GetTriggerUnit();
                UnitAI ai = GetAI(unit);
                if (ai != null) ai.OnDeath();
            });

            PlayerUnitEvents.Register(PlayerUnitEvent.HeroTypeLevels, Util.TryAction(() =>
            {
                GetAI(GetTriggerUnit())?.TryLearnAbilities();
            }, "UnitAI.HeroTypeLeves"));

            PlayerUnitEvents.Register(PlayerUnitEvent.ResearchIsFinished, Util.TryAction(() =>
            {
                player player = GetTriggerPlayer();
                foreach (UnitAI ai in unitMap.Values)
                {
                    if (ai.HumanPlayer == player) ai.TryLearnAbilities();
                }
            }, "UnitAI.HeroTypeLeves"));

        }

        public static int GetHeroCount(player player)
        {
            return unitMap.Values.Where(h => h is Hero && h.HumanPlayer == player).Count();
        }

        public UnitAI()
        {
        }

        public virtual bool HasStatusPanel()
        {
            return false;
        }

        protected abstract void AddBehaviors();

        protected void AddBehavior(Behavior behavior, int weight = 0)
        {
            behavior.init(this, weight);
            behaviors.Add(behavior);
            //Console.WriteLine($"Adding {behavior.GetName()} for {Unit.GetName()}");
        }

        public Behavior GetBehavior(Type type)
        {
            return behaviors.Find(b => b.GetType() == type) ?? null;
        }

        public bool TryInterruptWith(Type type, bool checkIfCanStart)
        {
            Behavior b = GetBehavior(type);
            if (b == null) return false;
            return TryInterruptWith(b, checkIfCanStart);
        }

        public bool TryInterruptWith(Behavior newBehavior, bool checkIfCanStart)
        {
            if (behavior == newBehavior)
            {
                // Should we let a behavior know its being interrupted?
                return false;
            }
            if (checkIfCanStart && !newBehavior.CanStart()) return false;
            if (behavior != null && !behavior.TryInterrupt(newBehavior)) return false;
            //Console.WriteLine($"{Name} interrupted with {newBehavior.GetName()}");
            behavior = newBehavior;
            StartBehavior();
            return true;
        }

        protected virtual void Init(unit unit)
        {
            Unit = unit;
            MyPlayer = GetOwningPlayer(unit);
            // TODO: Handle Monsters
            AIPlayer = MyPlayer.GetAIForHuman();
            HumanPlayer = MyPlayer.GetHumanForAI();
            AddBehaviors();
        }

        protected virtual void RegainLife()
        {
            if (InBuilding == null) return;
            float rate = IsHome ? RECOVER_RATE_AT_HOME : RECOVER_RATE;
            SetUnitLifeBJ(Unit, Unit.GetHP() * (1 + rate));
        }

        public void EnterBuilding(unit building)
        {
            InBuilding = building;
            ShowUnitHide(Unit);
            PauseUnit(Unit, true);
            if (IsHome)
            {
                int untaxed = GoldUntaxed;
                int tax = Guilds.PayTax(building, untaxed);
                GoldUntaxed = 0;
                GoldTaxed += untaxed - tax;
            }
        }

        public void ReceiveBounty(int amount)
        {
            GoldUntaxed += amount;
            Unit.ShowTextTag("+" + amount, Color.GOLD);
        }

        public float GetBountyMotivation(unit flag)
        {
            // TODO: Add preferences
            return SquareRoot((float)flag.GetFlagBounty() / Bounties.BOUNTY_INC);
        }


        public void ExitBuilding()
        {
            // TODO: move to bottom of building?
            InBuilding = null;
            ShowUnitShow(Unit);
            PauseUnit(Unit, false);
        }

        public bool IsEnemy(unit unit)
        {
            // TODO: Alliances, Make sure this works for neutrals
            player owner = unit.GetPlayer();
            //Console.WriteLine($"{unit.GetName()} owned by {GetPlayerId(owner)} vs " +
            //    $"H:{GetPlayerId(HumanPlayer)} and AI:{GetPlayerId(AIPlayer)}");
            return owner != HumanPlayer && owner != AIPlayer;
        }

        public virtual float GetIntimidation(unit enemy)
        {
            // TODO: Consistent system
            return GetUnitLevel(enemy) * enemy.GetHP();
        }

        public virtual float GetConfidence(unit ally)
        {
            // TODO: Consistent system
            return  (1 + (GetUnitLevel(ally) - 1) / 0.2f) * ally.GetHP();
        }

        private void StartBehavior()
        {
            if (behavior == null) return;
            //Console.WriteLine($"Starting {behavior.GetName()} for {Unit.GetName()}");
            behavior.Start();
            string name = GetHeroProperName(Unit);
            if (name == null || name.Length == 0) name = Unit.GetName();
            //Status = $"{name} ({Gold}g) is {behavior.GetStatusGerund()}.";
            Status = $"{Capitalize(behavior.GetStatusGerund())}.";
        }

        private static string Capitalize(string s)
        {
            if (s.Length == 0) return s;
            return s.Substring(0, 1).ToUpper() + s.Substring(1);
        }

        public virtual int[] GetAbilityIDs()
        {
            return new int[0];
        }

        protected void TryLearnAbilities()
        {
            int availablePoints = GetHeroSkillPoints(Unit);
            if (availablePoints == 0) return;
            Console.WriteLine($"Points {Unit.GetName()}: {availablePoints}");

            int[] ids = GetAbilityIDs();
            foreach (int ability in ids.OrderBy(id => GetUnitAbilityLevel(Unit, id)))
            {
                SelectHeroSkill(Unit, ability);
                // Check if we succeded in learning the ability; if so stop
                if (availablePoints > GetHeroSkillPoints(Unit))
                {
                    Console.WriteLine($"{Unit.GetName()} learned: {ability}");
                    // Go again if there are more points left
                    if (availablePoints > 2) TryLearnAbilities();
                    break;
                }
            }
        }

        public virtual void Update()
        {
            RegainLife();
            DoPreBehaviorActions();
            if (behaviors == null) return;
            //Console.WriteLine("Starting update...");
            if (behavior == null)
            {
                behavior = ChooseIdleBehavior();
                StartBehavior();
            }

            if (behavior == null) return;

            //Console.WriteLine("Updating behavior...");
            if (behavior.NeedsRestart)
            {
                //Console.WriteLine($"Restarting {behavior}");
                behavior.NeedsRestart = false;
                StartBehavior();
            }
            if (!behavior.Update())
            {
                behavior.Stop();
                behavior = null;
            }
        }

        protected virtual void DoPreBehaviorActions()
        {
            
        }

        protected virtual Behavior ChooseIdleBehavior()
        {
            //Console.WriteLine($"Selecting from {behaviors.Count} behaviors");
            var weights = behaviors.ToDictionary(b => b, 
                // Only calculate StartWeight for non-0
                b => b.Weight == 0 ? 0 : b.StartWeight() * b.Weight);
            var possible = behaviors.Where(b => weights[b] > 0);
            float den = possible.Select(b => weights[b]).Sum();
            if (den == 0) return null;
            float rand = GetRandomInt(0, 999) / 1000f;
            foreach (Behavior b in possible)
            {
                float slice = weights[b] / den;
                if (rand <= slice) return b;
                rand -= slice;
            }
            Console.WriteLine("no behaviors...");
            return null;
        }

        public float GetFightConfidence(location location)
        {
            return GetFightConfidence(null, location);
        }

        public float GetFightConfidence(unit enemy)
        {
            return GetFightConfidence(enemy, enemy.GetLocation());
        }

        public float GetFightConfidence(unit enemy, location location)
        {
            // TODO: Customize based on unit
            float radius = Unit.GetSightRange();
            var allUnits = GetUnitsInRangeOfLocAll(radius, location).ToList();
            //Console.WriteLine($"Searching {allUnits.Count} units in radius {radius} of {location.ToXY()}");
            var units = allUnits
                .Where(u => u.IsVisibleToPlayer(HumanPlayer) && !u.IsDead() && !u.IsStructure());
            float confidence = units
                .Where(u => u != Unit && !IsEnemy(u) && IsHeroUnitId(u.GetTypeID()))
                .Select(u => GetIntimidation(u))
                .Sum();
            // Include this unit regardless of the location
            confidence += GetIntimidation(Unit);
            confidence *= Unit.GetHPFraction();
            //Console.WriteLine(enemies.Aggregate("Enemies: ", (s, u) => s + u.GetName() + " "));
            float intimidation = units
                .Where(u => u != enemy && IsEnemy(u))
                .Select(u => GetIntimidation(u))
                .Sum();
            // Make sure to include the target enemy
            if (enemy != null) intimidation += GetIntimidation(enemy);

            //Console.WriteLine($"Calculating confidence for {Unit.GetName()}" +
            //    $" of {units.Count()} units = " +
            //    $"{confidence} / {intimidation}");

            return confidence / Math.Max(1, intimidation);
        }

        public static UnitAI GetAI(unit unit)
        {
            if (unitMap.TryGetValue(unit, out UnitAI ai)) return ai;
            return null;
        }

        public static UnitAI RegisterUnit(unit unit)
        {
            if (unit.IsStructure()) return null;

            if (!unitMap.ContainsKey(unit))
            {
                int unitID = GetUnitTypeId(unit);
                UnitAI ai = null;
                switch (unitID)
                {
                    case Constants.UNIT_PEASANT_WORKER:
                        ai = new Peasant();
                        break;
                    case Constants.UNIT_TAX_COLLECTOR:
                        ai = new TaxCollector();
                        break;
                    case Constants.UNIT_KNIGHT:
                        ai = new Knight();
                        break;
                    case Constants.UNIT_RANGER:
                        ai = new Ranger();
                        break;
                    case Constants.UNIT_DRAENEI_WARRIOR:
                        ai = new Draenei();
                        break;
                    case Constants.UNIT_FOREST_TROLL_WARRIOR:
                        ai = new Troll();
                        break;
                    case Constants.UNIT_KOBOLD_WARRIOR:
                        ai = new Kobold();
                        break;
                    case Constants.UNIT_BUILDER:
                        return null;
                }
                if (ai != null)
                {
                    Console.WriteLine($"Creating new AI for {unit.GetName()}");
                    ai.Init(unit);
                    unitMap[unit] = ai;
                }
                else
                {
                    Console.WriteLine($"Unknown AI for {unit.GetName()}");
                }
            }
            return unitMap[unit];
        }
        public void SetHome(unit home)
        {
            Guilds.SetHome(this, home);
            Home = home;
            OnHomeSet();
        }

        protected virtual IEnumerable<int> GetWantedItemsList()
        {
            return new List<int>();
        }

        public IEnumerable<ItemInfo> GetWantedItems()
        {
            Dictionary<int, int> itemCounts = new Dictionary<int, int>();
            // TODO: Should probably cache
            foreach (int itemID in GetWantedItemsList())
            {
                if (!itemCounts.TryGetValue(itemID, out int count))
                {
                    count = itemCounts[itemID] = Unit.GetItemOrUpgradeCount(itemID);
                }
                if (count > 0)
                {
                    itemCounts[itemID]--;
                }
                else
                {
                    // Explicit cast for yeild return bug
                    ItemInfo item = itemID;
                    yield return item;
                }
            }
        }

        public bool TryPurchase(ItemInfo info, unit shop)
        {
            int cost = info.Cost;
            if (Gold < cost) return false;
            item item;
            bool bought = false;
            if (Unit.HasExactItem(info))
            {
                item = GetItemOfTypeFromUnitBJ(Unit, info);

                SetItemCharges(item, GetItemCharges(item) + 1);
                bought = true;
            }
            else
            {
                Unit.RemoveReplacedItems(info);
                if (UnitInventoryCount(Unit) < 6)
                {
                    UnitAddItemById(Unit, info);
                    bought = true;
                }
            }
            if (bought)
            {
                Gold -= cost;
                Buildings.AddTax(shop, cost);
            }
            //Console.WriteLine($"{Unit.GetName()} bought {itemID} successful: {bought}");

            return bought;
        }

        public virtual void OnAttack(unit target)
        {

        }

        public virtual void OnAttacked(unit attacker)
        {

        }

        public virtual void OnHomeAttacked(unit attacker)
        {

        }

        public virtual void OnDeath()
        {
            Guilds.RemoveAI(this);
            unitMap.Remove(Unit);
        }

        public virtual void OnBuildingAttacked(unit attacker)
        {
            if (behavior != null)
            {
                behavior.Stop();
                behavior = null;
            }
            TryInterruptWith(typeof(Fight), true);
        }

        public virtual void OnRealmAttacked(unit building, unit attacker)
        {
        }

        public virtual void OnHomeSet()
        {

        }

        public static void Trigger(PlayerUnitEvent ev, unit unit)
        {
            if (!unitMap.ContainsKey(unit)) return;
            UnitAI ai = unitMap[unit];
            try
            {
                switch (ev)
                {
                    case PlayerUnitEvent.UnitTypeAttacks:
                        ai.OnAttack(GetAttackedUnitBJ());
                        break;
                    case PlayerUnitEvent.UnitTypeIsAttacked:
                        ai.OnAttacked(GetAttacker());
                        break;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error with {GetUnitName(ai.Unit)} event {ev}: {e.Message}");
            }
        }

        public static void TriggerHomeAttacked(unit home, unit attacker)
        {
            try
            {
                // TODO: Should cache attack alerts, so they're once per ~3 seconds

                if (!home.IsStructure()) return;

                //Console.WriteLine($"Checking home {home.GetName()} attacked by {attacker.GetName()}...");
                foreach (UnitAI ai in unitMap.Values.Where(v => v.InBuilding == home))
                {
                    ai.OnBuildingAttacked(attacker);
                }

                Guilds.OnBuildingAttacked(home, attacker);

                player owner = home.GetPlayer();
                // Monsters don't defend the realm 
                if (owner == Monster.Player) return;

                foreach (UnitAI ai in unitMap.Values.Where(v => v.HumanPlayer == owner))
                {
                    ai.OnRealmAttacked(home, attacker);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error handing home {home.GetName()} attacked by {attacker.GetName()}: {e.Message}");
            }
        }

        public static void UpdateUnits()
        {
            //Console.WriteLine($"Updating {unitMap.Count} units...");
            foreach (var unit in unitMap.Values)
            {
                try
                {
                    unit.Update();
                } catch (Exception e)
                {
                    Console.WriteLine($"Error updating {GetUnitName(unit.Unit)}: {e.Message}");
                }
            }
        }

        public void Select()
        {
            Select(HumanPlayer);
        }

        public void Select(player player)
        {
            SelectUnitForPlayerSingle(Unit, player);
            SetCameraPositionLocForPlayer(player, Unit.GetLocation());
        }
    }
}
