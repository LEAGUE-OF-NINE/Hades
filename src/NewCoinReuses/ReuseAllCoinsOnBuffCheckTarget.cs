using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class ReuseAllCoinsOnBuffCheckTarget : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<ReuseAllCoinsOnBuffCheckTarget>();
            harmony.PatchAll(typeof(ReuseAllCoinsOnBuffCheckTarget));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleActionModel.OnStartTurn_AfterLog))]
        [HarmonyPostfix]
        private static void OnStartTurn_AfterLog(BattleActionModel action, BattleUnitModel __instance, BATTLE_EVENT_TIMING timing)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                // checks if our script contains the right Script Name
                if (scriptName.Contains("ReuseAllCoinsOnBuffCheckTarget"))
                {
                    FrogMainClass.Logg.LogInfo("Successfully Detected: " + scriptName);
                    // if buffdata is empty, skip this.
                    if (ability.buffData == null) continue;

                    // parses keyword data from "buffdata keyword" and makes it useable.
                    var keyword_one = ability.buffData.buffKeyword;
                    var keyword_status_one = Enum.Parse<BUFF_UNIQUE_KEYWORD>(keyword_one);

                    // grabs potency/count
                    var potency_check = ability.buffData.stack;
                    var count_check = ability.buffData.turn;

                    BattleUnitModel battleUnitModel = (action != null) ? action.GetMainTarget() : null;
                    if (battleUnitModel != null && battleUnitModel.GetActivatedBuffStack(keyword_status_one) >= potency_check && battleUnitModel.GetActivatedBuffTurn(keyword_status_one) >= count_check)
                    {
                        FrogMainClass.Logg.LogInfo("Successfully Activated: " + scriptName);
                        // copy all coins of this skill and reuses them
                        action._skill.CopyAllCoinModelAndAddToList(action, timing);
                    }
                };
            }
        }
    }
}
