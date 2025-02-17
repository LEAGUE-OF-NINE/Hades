using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class HealSPOnEvadeEveryAlly : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<HealSPOnEvadeEveryAlly>();
            harmony.PatchAll(typeof(HealSPOnEvadeEveryAlly));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnSucceedEvade))]
        [HarmonyPostfix]
        private static void OnSucceedEvade(BattleUnitModel __instance, BattleActionModel evadeAction, BattleActionModel attackAction, BATTLE_EVENT_TIMING timing)
        {
            foreach (var ability in evadeAction._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                // checks if our script contains the right Script Name
                if (scriptName.Contains("HealSPOnEvadeEveryAlly"))
                {
                    FrogMainClass.Logg.LogInfo("Successfully Detected: " + scriptName);
                    // if buffdata is empty, skip this.
                    if (ability.buffData == null) continue;

                    var value = ability.buffData.stack;

                    UNIT_FACTION thisFaction = __instance.Faction;
                    foreach (BattleUnitModel model in BattleObjectManager.Instance.GetAliveList(false, thisFaction))
                    {
                        model.HealTargetMp(model, value, ABILITY_SOURCE_TYPE.SKILL, timing);
                    }
                };
            }
        }
    }
}
