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
    internal class OnSaDealAbsHpDmgIfFactionCheck : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<OnSaDealAbsHpDmgIfFactionCheck>();
            harmony.PatchAll(typeof(OnSaDealAbsHpDmgIfFactionCheck));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleActionModel.OnAttackConfirmed))]
        [HarmonyPostfix]
        private static void OnAttackConfirmed(CoinModel coin, BattleUnitModel target, BattleUnitModel __instance, BATTLE_EVENT_TIMING timing)
        {
            foreach (var abilitydata in coin._classInfo.GetAbilityScript())
            {
                var scriptName = abilitydata.scriptName;
                if (scriptName.Contains("OnSaFailedDMGTakeHpDMG_"))
                {
                    if (abilitydata.buffData == null) continue;

                    var factionname = scriptName.Replace("OnSaFailedDMGTakeHpDMG_", "");
                    var parsed_association = Enum.Parse<UNIT_KEYWORD>(factionname);

                    var stack_check = abilitydata.buffData.stack;

                    if (target.AssociationList.Contains(parsed_association))
                    {
                        target.GiveAbsHpDamage(target, stack_check, stack_check, 0, BATTLE_EVENT_TIMING.ON_WIN_DUEL, DAMAGE_SOURCE_TYPE.COMBAT, null);
                    }
                }
            }
        }
    }
}
