using System;
using static War3Api.Common;
using static War3Api.Blizzard;
using WCSharp.Events;
using Source.Units;

namespace Source
{
	public static class Program
	{

		
		public static void Main()
		{
			// Delay a little since some stuff can break otherwise
			var timer = CreateTimer();
			TimerStart(timer, 0.01f, false, () =>
			{
				DestroyTimer(timer);
				Start();
			});

			TimerStart(CreateTimer(), 0.5f, true, () =>
			{
				UnitAI.UpdateUnits();
			});
		}

        private static void Start()
		{
			PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeStartsBeingConstructed, () =>
			{
				unit unit = GetConstructingStructure();
				string name = GetUnitName(unit);

				
				Console.WriteLine($"Constructing {(unit == null ? "NONE" : name)} of player {GetPlayerId(GetOwningPlayer(unit))}");
			});

			PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeIsCreated, () =>
			{
				unit unit = GetTriggerUnit();
				if (unit.IsStructure()) return;

				if (GetUnitTypeId(unit) == Constants.UNIT_PEASANT_WORKER)
                {
					// TODO: Make uncontrollable?
                }
				else
                {
					SetUnitOwner(unit, GetOwningPlayer(unit).GetAIForHuman(), false);
                }
				UnitAI.RegisterUnit(unit);

				//string name = GetUnitName(unit);
				//Console.WriteLine($"Created {(unit == null ? "NONE" : name)}");
			});
			PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeFinishesTraining, () =>
			{
				//Console.WriteLine($"Trained: {GetTriggerUnit().GetName()}, {GetTrainedUnit().GetName()}");
				unit unit = GetTrainedUnit();
				unit building = GetTriggerUnit();
				UnitAI ai = UnitAI.RegisterUnit(unit);
				if (ai != null) ai.SetHome(building);
			});

			PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeAttacks, () =>
			{
				UnitAI.Trigger(PlayerUnitEvent.UnitTypeAttacks, GetAttacker());
				UnitAI.Trigger(PlayerUnitEvent.UnitTypeIsAttacked, GetAttackedUnitBJ());
				UnitAI.TriggerHomeAttacked(GetAttackedUnitBJ(), GetAttacker());
			});


			SetPlayerState(GetLocalPlayer(), PLAYER_STATE_RESOURCE_GOLD, 1000);
			SetPlayerState(GetLocalPlayer(), PLAYER_STATE_RESOURCE_LUMBER, 1000);

			ForGroup(GetUnitsOfPlayerAll(Player(0)), () =>
			{
				UnitAI.RegisterUnit(GetEnumUnit());
			});

			ForGroup(GetUnitsOfPlayerAll(Player(1)), () =>
			{
				UnitAI.RegisterUnit(GetEnumUnit());
			});

			try
			{
				Console.WriteLine("Hello, Azeroth.");
			}
			catch (Exception ex)
			{
				DisplayTextToPlayer(GetLocalPlayer(), 0, 0, ex.Message);
			}
		}
	}
}
