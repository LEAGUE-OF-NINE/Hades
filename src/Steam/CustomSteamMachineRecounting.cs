using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class CustomSteamMachineRecounting : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<CustomSteamMachineRecounting>();
            harmony.PatchAll(typeof(CustomSteamMachineRecounting));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnBattleStart))]
        [HarmonyPostfix]
        private static void BattleStart(BattleActionModel __instance)
        {
            foreach (var ability in __instance._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                // checks if our script contains the right Script Name
                if (scriptName.Contains("CustomSteamMachineRecounting"))
                {
                    // if buffdata is empty, skip this.
                    if (ability.buffData == null) continue;

                    // parses the buff keyword we're checking for
                    var keyword = ability.buffData.buffKeyword;
                    var keyword_status = Enum.Parse<BUFF_UNIQUE_KEYWORD>(keyword);

                    var potency_check = ability.buffData.stack;
                    var count_check = ability.buffData.turn;
                    var active_round = ability.buffData.activeRound;
                }
            }
        }
    }
}
