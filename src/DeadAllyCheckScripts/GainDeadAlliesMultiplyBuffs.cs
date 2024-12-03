using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;
using EGOGift;

namespace BaseMod
{
    internal class GainDeadAlliesMultiplyBuffs : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<GainDeadAlliesMultiplyBuffs>();
            harmony.PatchAll(typeof(GainDeadAlliesMultiplyBuffs));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnStartTurn_BeforeLog))]
        [HarmonyPostfix]
        private static void OnStartTurn_BeforeLog(BattleActionModel action, BattleUnitModel __instance)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                if (scriptName.Contains("GainDeadAlliesMultiplyBuffs"))
                {
                    // if buffdata is empty, skip this.
                    if (ability.buffData == null) continue;

                    var potency_check = ability.buffData.stack;
                    var count_check = ability.buffData.turn;

                    var keyword = ability.buffData.buffKeyword;
                    var keyword_status = Enum.Parse<BUFF_UNIQUE_KEYWORD>(keyword);

                    var active_round = ability.buffData.activeRound;

                    var deadallycount = action.Model._deadAllyCount;
                    var valuecheck = Convert.ToInt32(ability.buffData.value);
                    var finaldeadallycount = deadallycount - valuecheck;

                    var final_potency_adder = potency_check * finaldeadallycount;
                    var final_count_adder = count_check * finaldeadallycount;

                    if (finaldeadallycount >= 1)
                    {
                        __instance.AddBuff_Giver(keyword_status, final_potency_adder, __instance, BATTLE_EVENT_TIMING.ON_START_TURN, final_count_adder, active_round, ABILITY_SOURCE_TYPE.SKILL, null, final_potency_adder, final_count_adder);
                    }
                };
            }
        }
    }
}
