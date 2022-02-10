using System;
using static War3Api.Common;
using static War3Api.Blizzard;
using WCSharp.Events;
using Source.Units;
using System.Collections.Generic;
using Source.Interface;
using Source.Units.Monsters;

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
			//PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeStartsBeingConstructed, () =>
			//{
			//	unit unit = GetConstructingStructure();
			//	string name = GetUnitName(unit);
			//	//Console.WriteLine($"Constructing {(unit == null ? "NONE" : name)} of player {GetPlayerId(GetOwningPlayer(unit))}");
			//});

			Info.Init();
			UnitAI.Init();
			Bounties.Init();
			Monster.Init();
			Status.Init();
			Guilds.Init();
			Buildings.Init();

            SetPlayerState(GetLocalPlayer(), PLAYER_STATE_RESOURCE_GOLD, 50000);
			SetPlayerState(GetLocalPlayer(), PLAYER_STATE_RESOURCE_LUMBER, 1000);

			ForGroup(GetUnitsOfPlayerAll(Player(0)), () =>
			{
				unit unit = GetEnumUnit();
				UnitAI.RegisterUnit(unit);
				Buildings.TryRegister(unit);
			});

			ForGroup(GetUnitsOfPlayerAll(Player(1)), () =>
			{
				UnitAI.RegisterUnit(GetEnumUnit());
			});

			try
			{
				Spawners.SpawCamps();
			}
			catch (Exception e)
            {
				Console.WriteLine("Error spawing camps: " + e.Message);
            }

			try
			{
				Console.WriteLine("Hello, Azeroth.");
			}
			catch (Exception ex)
			{
				DisplayTextToPlayer(GetLocalPlayer(), 0, 0, ex.Message);
			}

			try
            {
				HeroInfoPanel.Init();
				GuildInfoPanel.Init();

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
