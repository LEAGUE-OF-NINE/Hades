using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;
using BepInEx.Logging;

namespace BaseMod
{
    internal class SPDrainCustomReloadScript : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<SPDrainCustomReloadScript>();
            harmony.PatchAll(typeof(SPDrainCustomReloadScript));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleActionModel.OnStartTurn_AfterLog))]
        [HarmonyPostfix]
        private static void OnStartTurn_AfterLog(BattleActionModel action, BattleUnitModel __instance, BATTLE_EVENT_TIMING timing)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                // checks if our script contains the right Script Name
                if (scriptName.Contains("SPDrainCustomReloadScript_"))
                {
                    FrogMainClass.Logg.LogInfo("Successfully Detected: " + scriptName);
                    // if buffdata is empty, skip this.
                    if (ability.buffData == null) continue;

                    var name_keyword = scriptName.Replace("SPDrainCustomReloadScript_", "");
                    var parsed_name_keyword = Enum.Parse<BUFF_UNIQUE_KEYWORD>(name_keyword);

                    // parses keyword data from "buffdata keyword" and makes it useable.
                    var keyword_one = ability.buffData.buffKeyword;
                    var keyword_status_one = Enum.Parse<BUFF_UNIQUE_KEYWORD>(keyword_one);

                    // grabs how much value (potency) was input
                    var potency_check_one = ability.buffData.value;
                    // grabs how much potency we currenty have
                    var current_potency_one = __instance.GetActivatedBuffStack(keyword_status_one);

                    // Grabs the target potency, and lowers it by the current potency.
                    // so if we have a target Potency of 6 [Bullets], we lower it by the amount of Potency on Self.
                    // if this is equal to or less than 0, end this script.

                    var increase_amount_stack_one = Convert.ToInt32(potency_check_one - current_potency_one);
                    if (increase_amount_stack_one <= 0) continue;

                    // grabs how much stack and count was input into buffdata
                    var potency_check_two = ability.buffData.stack;
                    var count_check_two = ability.buffData.turn;

                    // used to determine the amount of SP lost per bullet regained
                    var final_sp_loss = increase_amount_stack_one * (ability.buffData.limit);

                    // if our current potency is lower than our input potency, continue 
                    if (current_potency_one < potency_check_one)
                    {
                        // if our current potency was 0 at the time of activation, gain an extra status effect
                        if (current_potency_one <= 0)
                        {
                            FrogMainClass.Logg.LogInfo("Successfully Activated Secondary Effect Of: " + scriptName);
                            __instance.AddBuff_Giver(parsed_name_keyword, potency_check_two, __instance, BATTLE_EVENT_TIMING.ON_START_TURN, count_check_two, 0, ABILITY_SOURCE_TYPE.BUFF, null, potency_check_two, count_check_two);
                        }
                        FrogMainClass.Logg.LogInfo("Successfully Activated: " + scriptName);
                        __instance.AddBuff_Giver(keyword_status_one, increase_amount_stack_one, __instance, BATTLE_EVENT_TIMING.ON_START_TURN, 0, 0, ABILITY_SOURCE_TYPE.BUFF, null, increase_amount_stack_one, 0);
                        __instance.GiveAbsMpDamage(__instance, final_sp_loss, BATTLE_EVENT_TIMING.ON_START_TURN, DAMAGE_SOURCE_TYPE.SKILL);
                    }
                    //  [WhenUse] This unit triggers a Reload\n -
                    //  (Reload: If this unit has less than [value] [keyword], regain([value] - (Current [keyword] Amount)) [keyword], then lose [limit] SP for every [keyword] regained
                    //  \n - If this unit's [keyword] was at 0 upon triggering a Reload, gain [stack] [parsed_name_keyword] and +[turn] [parsed_name_keyword] Count
                };
            }
        }
    }
}
