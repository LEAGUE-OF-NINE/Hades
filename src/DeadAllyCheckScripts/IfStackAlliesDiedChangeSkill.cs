using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class IfStackAlliesDiedChangeSkill : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<IfStackAlliesDiedChangeSkill>();
            harmony.PatchAll(typeof(IfStackAlliesDiedChangeSkill));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnStartTurn_BeforeLog))]
        [HarmonyPostfix]
        private static void StartTurn_BeforeLog(BattleActionModel action, BattleUnitModel __instance)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                if (scriptName.Contains("IfStackAlliesDiedChangeSkill_"))
                {
                    // warning: horrible painful bullshit
                    // "regex at home" but dogshit. 
                    // search through the specific string in Scriptname, get it, cut off the script name, keep the ID number
                    // then convert it to an actual skillID through fucking magic. fuck.
                    // horrible implementation but I need to do it, else the game can't find this specific skill.
                    var newskillID = Convert.ToInt32(scriptName.Replace("IfStackAlliesDiedChangeSkill_", ""));
                    var whae = delegate (SkillModel x) { return x.GetID() == newskillID; };
                    var naenae = __instance.GetSkillList().Find(whae);

                    // at the input players need to specify the conditional they wan
                    var dead_ally_amounts_needed = ability.buffData.stack;

                    var deadallycount = action.Model.deadAllyCount;

                    if (deadallycount >= dead_ally_amounts_needed)
                    {
                        action.ChangeSkill(naenae);
                    }
                };
            }
        }
    }
}
