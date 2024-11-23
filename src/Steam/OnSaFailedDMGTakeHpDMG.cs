using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;
using BepInEx.Logging;

namespace BaseMod
{
    internal class OnSaFailedDMGTakeHpDMG : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<OnSaFailedDMGTakeHpDMG>();
            harmony.PatchAll(typeof(OnSaFailedDMGTakeHpDMG));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleActionModel.OnAttackConfirmed))]
        [HarmonyPostfix]
        private static void OnAttackConfirmed(CoinModel coin, BattleUnitModel target, BattleUnitModel __instance, BATTLE_EVENT_TIMING timing)
        {
            foreach (var abilitydata in coin._classInfo.GetAbilityScript())
            {
                var scriptName = abilitydata.scriptName;
                if (scriptName.Contains("OnSaFailedDMGTakeHpDMG"))
                {
                    if (abilitydata.buffData == null) continue;

                    var stack_check = abilitydata.buffData.stack;

                    foreach (BattleUnitModel model in BattleObjectManager.Instance.GetAliveList(true))
                    {
                        __instance.GiveAbsHpDamage(__instance, stack_check, stack_check, 0, BATTLE_EVENT_TIMING.ON_WIN_DUEL, DAMAGE_SOURCE_TYPE.COMBAT, null);
                    }
                }
            }
        }
    }
}
