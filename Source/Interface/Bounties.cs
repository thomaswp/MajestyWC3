using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCSharp.Events;
using static War3Api.Blizzard;
using static War3Api.Common;

namespace Source.Interface
{
    public static class Bounties
    {
        private static Dictionary<unit, texttag> bountyMap = new Dictionary<unit, texttag>();

        public const int BOUNTY_INC = 100;

        private const float MEAN_CHAR_WIDTH = 5.5f;
        private const float MAX_TEXT_SHIFT = 32f;
        private const float FONT_SIZE = 0.026f;
        private const float Y_OFFSET = 125f;

        public static void Init()
        {
            PlayerUnitEvents.Register(PlayerUnitEvent.SpellEffect, () =>
            {
                try
                {
                    unit unit = GetTriggerUnit();
                    int mana = (int) Math.Round((unit.GetMana() + BOUNTY_INC) * 1f / BOUNTY_INC) * BOUNTY_INC;
                    SetUnitManaBJ(unit, mana);
                    SetText(unit, mana.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error increasing bounty: " + e.Message);
                }
            }, Constants.ABILITY_INCREASE_BOUNTY);

            PlayerUnitEvents.Register(PlayerUnitEvent.SpellCast, () =>
            {
                Console.WriteLine("Decrease..." + GetSpellAbilityId());
                try
                { 
                    unit unit = GetTriggerUnit();
                    player player = unit.GetPlayer();
                    player.ChangeGoldBy(BOUNTY_INC / 2);
                    // Mana hasn't been reduced yet, so sub 100
                    SetText(unit, (unit.GetMana() - BOUNTY_INC).ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error decreasing bounty: " + e.Message);
                }
            }, Constants.ABILITY_DECREASE_BOUNTY);

            PlayerUnitEvents.Register(PlayerUnitEvent.SpellCast, () =>
            {
                try
                {
                    unit unit = GetTriggerUnit();
                    int mana = unit.GetMana();
                    unit.GetPlayer().ChangeGoldBy(mana / 2);
                    KillUnit(unit);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error destroying flag: " + e.Message);
                }
            }, Constants.ABILITY_DESTROY_FLAG);

            PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeDies, () =>
            {
                DestroyText(GetTriggerUnit());
            }, Constants.UNIT_EXPLORE_FLAG);
        }

        private static texttag GetText(unit flag)
        {
            if (!bountyMap.TryGetValue(flag, out texttag text))
            {
                bountyMap[flag] = text = CreateTextTag();
                SetTextTagPermanent(text, true);
                SetTextTagColor(text, 255, 220, 0, 255);
                SetTextTagVisibility(text, true);
                //Console.WriteLine("Creating text for flag " + flag.GetName());
            }
            return text;
        }

        private static void SetText(unit flag, string message)
        {
            texttag text = GetText(flag);
            SetTextTagText(text, message, FONT_SIZE);
            float shift = Math.Min(StringLength(message) * MEAN_CHAR_WIDTH / 2, MAX_TEXT_SHIFT) + 20;
            SetTextTagPos(text, GetUnitX(flag) - shift, GetUnitY(flag), Y_OFFSET);
        }

        private static void DestroyText(unit flag)
        {
            if (bountyMap.TryGetValue(flag, out texttag text))
            {
                DestroyTextTag(text);
            }
            bountyMap.Remove(flag);
        }
    }
}
