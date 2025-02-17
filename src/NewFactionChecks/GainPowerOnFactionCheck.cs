using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class GainPowerOnFactionCheck : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<GainPowerOnFactionCheck>();
            harmony.PatchAll(typeof(GainPowerOnFactionCheck));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleActionModel.OnStartTurn_AfterLog))]
        [HarmonyPostfix]
        private static void OnStartTurn_AfterLog(BattleActionModel action, BattleUnitModel __instance, BATTLE_EVENT_TIMING timing)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                // checks if our script contains the right Script Name
                if (scriptName.Contains("GainPowerOnFactionCheck_"))
                {
                    FrogMainClass.Logg.LogInfo("Successfully Detected: " + scriptName);
                    // if buffdata is empty, skip this.
                    if (ability.buffData == null) continue;

                    // removes the first part of our script name and only takes the faction name.
                    // then filters through buff data and collects all the data we need
                    var factionname = scriptName.Substring("GainPowerOnFactionCheck_".Length);
                    var parsed_association = Enum.Parse<UNIT_KEYWORD>(factionname);

                    var value = ability.buffData.value;
                    var final_power_added = 0;

                    UNIT_FACTION thisFaction = __instance.Faction;
                    foreach (BattleUnitModel model in BattleObjectManager.Instance.GetAliveList(false, thisFaction))
                    {
                        // if the alive and player controlled character also belongs to the right association/faction/whatever the fuck, apply a status effect.
                        if (model.AssociationList.Contains(parsed_association) || model.UnitKeywordList.Contains(parsed_association))
                        {
                            final_power_added++;
                        }
                    }
                    int balls = (int)value;
                    var adder = final_power_added * balls;
                    action._skill.skillData.defaultSkillPower += adder;
                    action._skillPowerResultValue += adder;
                };
            }
        }
    }
}
