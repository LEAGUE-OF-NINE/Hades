using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class AtLessThanStackMPCheckTargetChangeSkill : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<AtLessThanStackMPCheckTargetChangeSkill>();
            harmony.PatchAll(typeof(AtLessThanStackMPCheckTargetChangeSkill));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnStartTurn_BeforeLog))]
        [HarmonyPostfix]
        private static void StartTurn_BeforeLog(BattleActionModel action, BattleUnitModel __instance)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                if (scriptName.Contains("AtLessThanStackMPCheckTargetChangeSkill_"))
                {
                    FrogMainClass.Logg.LogInfo("Successfully Detected: " + scriptName);
                    var newskillID = Convert.ToInt32(scriptName.Substring("AtLessThanStackMPCheckTargetChangeSkill_".Length));
                    var whae = delegate (SkillModel x) { return x.GetID() == newskillID; };
                    var naenae = __instance.GetSkillList().Find(whae);

                    var speed_conditional_needed = ability.buffData.stack;
                    
                    BattleUnitModel battleUnitModel = (action != null) ? action.GetMainTarget() : null;
                    var target_sp = battleUnitModel.Mp;

                    if (target_sp <= speed_conditional_needed)
                    {
                        FrogMainClass.Logg.LogInfo("Successfully Activated: " + scriptName);
                        action.ChangeSkill(naenae);
                    }
                };
            }
        }
    }
}
