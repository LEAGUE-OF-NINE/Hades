using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;
using System.Linq;

namespace BaseMod
{
    internal class OnSAGiveBuffFactionCheck : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<OnSAGiveBuffFactionCheck>();
            harmony.PatchAll(typeof(OnSAGiveBuffFactionCheck));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleActionModel.OnAttackConfirmed))]
        [HarmonyPostfix]
        private static void OnAttackConfirmed(CoinModel coin, BattleUnitModel target, BATTLE_EVENT_TIMING timing)
        {
            foreach (var abilitydata in coin._classInfo.GetAbilityScript())
            {
                var scriptName = abilitydata.scriptName;
                if (scriptName.Contains("OnSAGiveBuffFactionCheck_"))
                {
                    FrogMainClass.Logg.LogInfo("Successfully Detected: " + scriptName);
                    if (abilitydata.buffData == null) continue;

                    var factionname = scriptName.Substring("OnSAGiveBuffFactionCheck_".Length);
                    var parsed_association = Enum.Parse<UNIT_KEYWORD>(factionname);

                    var keyword = abilitydata.buffData.buffKeyword;
                    var keyword_status = Enum.Parse<BUFF_UNIQUE_KEYWORD>(keyword);

                    var potency_check = abilitydata.buffData.stack;
                    var count_check = abilitydata.buffData.turn;
                    var active_round = abilitydata.buffData.activeRound;

                    foreach (BattleUnitModel model in BattleObjectManager.Instance.GetAliveList(true))
                    {
                        if (model.AssociationList.Contains(parsed_association) || model.UnitKeywordList.Contains(parsed_association))
                        {
                            FrogMainClass.Logg.LogInfo("Successfully Activated: " + scriptName);
                            model.AddBuff_Giver(keyword_status, potency_check, model, BATTLE_EVENT_TIMING.BEFORE_GIVE_ATTACK, count_check, active_round, ABILITY_SOURCE_TYPE.UNIT, null, potency_check, count_check);
                        }
                    }
                }
            }
        }
    }
}
