using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class AtValueSpeedBecomeUnclashable : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<AtValueSpeedBecomeUnclashable>();
            harmony.PatchAll(typeof(AtValueSpeedBecomeUnclashable));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnStartTurn_BeforeLog))]
        [HarmonyPostfix]
        private static void StartTurn_BeforeLog(BattleActionModel action, BattleUnitModel __instance)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                if (scriptName.Contains("AtValueSpeedBecomeUnclashable_"))
                {
                    if (ability.buffData == null) continue;

                    // warning: horrible hack, very bad.
                    // code to take an input of what skill becomes unclashable.
                    // horrible implementation but I need to do it, else the game can't find this specific skill.
                    var newskillID = Convert.ToInt32(scriptName.Replace("AtValueSpeedBecomeUnclashable_", ""));
                    var whae = delegate (SkillModel x) { return x.GetID() == newskillID; };
                    var naenae = __instance.GetSkillList().Find(whae);

                    // didn't feel like doing a conditional in the script name when I can just check value.
                    var speed_conditional_needed = ability.buffData.value;

                    // I think this checks our currentspeed???? Idk????
                    var speedvalue = __instance.GetHitDamagePrevRound();

                    if (speedvalue >= speed_conditional_needed)
                    {
                        naenae._skillData.canDuel = false;
                    }
                };
            }
        }
    }
}
