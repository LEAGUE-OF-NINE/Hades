using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class AtValueSpeedChangeSkill : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<AtValueSpeedChangeSkill>();
            harmony.PatchAll(typeof(AtValueSpeedChangeSkill));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnStartTurn_BeforeLog))]
        [HarmonyPostfix]
        private static void StartTurn_BeforeLog(BattleActionModel action, BattleUnitModel __instance)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                if (scriptName.Contains("AtValueSpeedChangeSkill_"))
                {
                    FrogMainClass.Logg.LogInfo("Successfully Detected: " + scriptName);
                    // warning: horrible painful bullshit
                    // code to take an input of what skill we change into.
                    // horrible implementation but I need to do it, else the game can't find this specific skill.
                    var newskillID = Convert.ToInt32(scriptName.Replace("AtValueSpeedChangeSkill_", ""));
                    var whae = delegate (SkillModel x) { return x.GetID() == newskillID; };
                    var naenae = __instance.GetSkillList().Find(whae);

                    // at the input players need to specify the conditional they wan
                    var speed_conditional_needed = ability.buffData.value;

                    var originSpeed = action.Model.GetIntegerOfOriginSpeed();

                    if (originSpeed >= speed_conditional_needed)
                    {
                        FrogMainClass.Logg.LogInfo("Successfully Activated: " + scriptName);
                        action.ChangeSkill(naenae);
                    }
                };
            }
        }
    }
}
