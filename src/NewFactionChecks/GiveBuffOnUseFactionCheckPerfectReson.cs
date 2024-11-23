﻿using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class GiveBuffOnUseFactionCheckPerfectReson : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<GiveBuffOnUseFactionCheckPerfectReson>();
            harmony.PatchAll(typeof(GiveBuffOnUseFactionCheckPerfectReson));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleActionModel.OnStartTurn_AfterLog))]
        [HarmonyPostfix]
        private static void OnStartTurn_AfterLog(BattleActionModel action, BattleUnitModel __instance, BATTLE_EVENT_TIMING timing)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                // checks if our script contains the right Script Name
                if (scriptName.Contains("GiveBuffOnUseFactionCheckPerfectReson_"))
                {
                    // if buffdata is empty, skip this.
                    if (ability.buffData == null) continue;

                    // removes the first part of our script name and only takes the faction name.
                    // then filters through buff data and collects all the data we need
                    var factionname = scriptName.Replace("GiveBuffOnUseFactionCheckPerfectReson_", "");
                    var parsed_association = Enum.Parse<UNIT_KEYWORD>(factionname);

                    var keyword = ability.buffData.buffKeyword;
                    var keyword_status = Enum.Parse<BUFF_UNIQUE_KEYWORD>(keyword);

                    var potency_check = ability.buffData.stack;
                    var count_check = ability.buffData.turn;
                    var active_round = ability.buffData.activeRound;
                    var resonance_check = ability.buffData.value;

                    // checks through every living unit onthe battle, and if they're both alive AND player controlled, proceed
                    foreach (BattleUnitModel model in BattleObjectManager.Instance.GetAliveList(true))
                    {
                        var current_resonance = action.GetPerfectResonanceStack();
                        // if the alive and player controlled character also belongs to the right association/faction/whatever the fuck, apply a status effect.
                        if ((model.AssociationList.Contains(parsed_association) || model.UnitKeywordList.Contains(parsed_association)) && current_resonance >= resonance_check)
                        {
                            model.AddBuff_Giver(keyword_status, potency_check, model, BATTLE_EVENT_TIMING.ON_START_TURN, count_check, active_round, ABILITY_SOURCE_TYPE.BUFF, null, potency_check, count_check);

                        }
                    }
                };
            }
        }
    }
}