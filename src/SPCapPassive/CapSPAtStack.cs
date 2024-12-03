using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class CapSPAtStack : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<CapSPAtStack>();
            harmony.PatchAll(typeof(CapSPAtStack));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnStartTurn_BeforeLog))]
        [HarmonyPostfix]
        private static void StartTurn_BeforeLog(BattleActionModel action, BattleUnitModel __instance)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                // checks if our script contains the right Script Name
                var data = __instance._unitDataModel._classInfo;
                if (data == null) return;
                if (data.PassiveSetInfo.PassiveIdList.Contains(18844))
                {
                    // if buffdata is empty, skip this.
                    if (ability.buffData == null) continue;

                    // grabs how much potency was input
                    var sp_requirement = 0;
                    var current_sp = __instance.Mp;

                    var sp_increase = sp_requirement - current_sp;

                    // if our current potency is higher than our input potency, continue 
                    if (current_sp < sp_requirement)
                    {

                        // lowers our status effect. battle_event_timing doesn't really matter
                        __instance.RecoverMp(sp_increase, sp_increase);
                    }
                }
            }
        }
    }
}
