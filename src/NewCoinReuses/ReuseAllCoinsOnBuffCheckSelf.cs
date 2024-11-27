using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class ReuseAllCoinsOnBuffCheckSelf : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<ReuseAllCoinsOnBuffCheckSelf>();
            harmony.PatchAll(typeof(ReuseAllCoinsOnBuffCheckSelf));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleActionModel.OnStartTurn_AfterLog))]
        [HarmonyPostfix]
        private static void OnStartTurn_AfterLog(BattleActionModel action, BattleUnitModel __instance, BATTLE_EVENT_TIMING timing)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                // checks if our script contains the right Script Name
                if (scriptName.Contains("ReuseAllCoinsOnBuffCheckSelf"))
                {
                    // if buffdata is empty, skip this.
                    if (ability.buffData == null) continue;

                    // parses keyword data from "buffdata keyword" and makes it useable.
                    var keyword_one = ability.buffData.buffKeyword;
                    var keyword_status_one = Enum.Parse<BUFF_UNIQUE_KEYWORD>(keyword_one);

                    // grabs potency/count
                    var potency_check = ability.buffData.stack;
                    var count_check = ability.buffData.turn;

                    // grabs amount of potency on self.
                    var current_potency_amount = __instance.GetActivatedBuffStack(keyword_status_one);
                    var current_count_amount = __instance.GetActivatedBuffTurn(keyword_status_one);

                    // if our current potency is greater than or equal than our input potency, continue 
                    if (current_potency_amount >= potency_check && current_count_amount >= count_check)
                    {
                        // copy all coins of this skill and reuses them
                        action._skill.CopyAllCoinModelAndAddToList();
                    }
                };
            }
        }
    }
}
