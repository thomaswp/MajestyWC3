using Source.Units;
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
        private static Dictionary<unit, texttag> bountyTextMap = new Dictionary<unit, texttag>();
        private static Dictionary<unit, timer> updateTimerMap = new Dictionary<unit, timer>();
        private static Dictionary<unit, unit> bountyTargetMap = new Dictionary<unit, unit>();

        public const int BOUNTY_INC = 100;
        public const int ATTACK_BOUNTY_RADIUS = 800;

        public static void Init()
        {
            PlayerUnitEvents.Register(PlayerUnitEvent.SpellEffect, () =>
            {
                try
                {
                    unit unit = GetTriggerUnit();
                    int mana = unit.GetFlagBounty() + BOUNTY_INC;
                    SetUnitManaBJ(unit, mana);
                    if (unit.GetTypeID() != Constants.UNIT_EXPLORE_FLAG) return;
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
                    // TODO: Maybe recoverable for only short duration?
                    player.ChangeGoldBy(BOUNTY_INC / 2);
                    if (unit.GetTypeID() != Constants.UNIT_EXPLORE_FLAG) return;
                    // Mana hasn't been reduced yet, so sub 100
                    SetText(unit, (unit.GetFlagBounty() - BOUNTY_INC).ToString());
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
                    int mana = unit.GetFlagBounty();
                    unit.GetPlayer().ChangeGoldBy(mana / 2);
                    KillUnit(unit);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error destroying flag: " + e.Message);
                }
            }, Constants.ABILITY_DESTROY_FLAG);

            PlayerUnitEvents.Register(PlayerUnitEvent.SpellCast, () =>
            {
                unit target = GetSpellTargetUnit();
                unit caster = GetTriggerUnit();
                unit flag = CreateUnitAtLoc(caster.GetPlayer(), Constants.UNIT_ATTACK_FLAG, target.GetLocation(), 0);
                bountyTargetMap[flag] = target;
                timer timer = CreateTimer();
                TimerStart(timer, 0.3f, true, () =>
                {
                    if (flag.IsDead() || target.IsDead())
                    {
                        if (!flag.IsDead())
                        {
                            float bounty = flag.GetFlagBounty();
                            var units = GetUnitsInRangeOfLocAll(ATTACK_BOUNTY_RADIUS, flag.GetLocation()).ToList();
                            var recipients = units
                                .Where(u => !u.IsDead())
                                .Select(u => UnitAI.GetAI(u))
                                .Where(ai => ai != null && !ai.IsEnemy(flag))
                                .ToList();
                            int amount = (int)Math.Round(bounty / recipients.Count);
                            foreach (UnitAI recipient in recipients)
                            {
                                recipient.ReceiveBounty(amount);
                            }
                            
                            KillUnit(flag);
                        }
                        DestroyTimer(timer);
                        return;
                    }
                    flag.OrderMoveTo(target.GetLocation());
                });
            }, Constants.ABILITY_ATTACK_FLAG);

            PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeDies, () =>
            {
                DestroyText(GetTriggerUnit());
            }, Constants.UNIT_EXPLORE_FLAG);

            PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeDies, () =>
            {
                unit flag = GetTriggerUnit();
                DestroyText(flag);
                bountyTargetMap.Remove(flag);
            }, Constants.UNIT_ATTACK_FLAG);

            PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeIsSelected, () =>
            {
                unit flag = GetTriggerUnit();
                if (updateTimerMap.ContainsKey(flag)) return;
                Console.WriteLine("Select");
                timer timer = CreateTimer();
                updateTimerMap[flag] = timer;
                TimerStart(timer, 0.03f, true, () =>
                {
                    if (!updateTimerMap.ContainsKey(flag)) return;
                    if (flag.IsDead())
                    {
                        updateTimerMap.Remove(flag);
                        DestroyTimer(timer);
                        return;
                    }
                    SetText(flag, "" + flag.GetFlagBounty());
                });
            }, Constants.UNIT_ATTACK_FLAG);

            PlayerUnitEvents.Register(PlayerUnitEvent.UnitTypeIsDeselected, () =>
            {
                //Console.WriteLine("Deselect");
                unit flag = GetTriggerUnit();
                if (updateTimerMap.TryGetValue(flag, out timer timer))
                {
                    DestroyTimer(timer);
                    //Console.WriteLine("Destroy");
                    DestroyText(flag);
                    updateTimerMap.Remove(flag);
                }
            }, Constants.UNIT_ATTACK_FLAG);
        }

        public static unit GetFlagTarget(unit flag)
        {
            if (bountyTargetMap.TryGetValue(flag, out unit target)) return target;
            return null;
        }

        private static texttag GetText(unit flag)
        {
            if (!bountyTextMap.TryGetValue(flag, out texttag text))
            {
                bountyTextMap[flag] = text = TextTag.CreatePermenant(Color.GOLD);
                ShowTextTagForceBJ(true, text, GetForceOfPlayer(flag.GetPlayer()));
                //Console.WriteLine("Creating text for flag " + flag.GetName());
            }
            return text;
        }

        private static void SetText(unit flag, string message)
        {
            texttag text = GetText(flag);
            text.SetText(message);
            text.CenterAboveUnit(flag, message);
        }

        private static void DestroyText(unit flag)
        {
            if (bountyTextMap.TryGetValue(flag, out texttag text))
            {
                //Console.WriteLine("Success");
                DestroyTextTag(text);
            }
            bountyTextMap.Remove(flag);
        }

        public static int GetFlagBounty(this unit flag)
        {
            int mana = flag.GetMana();
            int bounty = MathRound(mana * 1f / BOUNTY_INC) * BOUNTY_INC;
            return bounty;
        }
    }
}
