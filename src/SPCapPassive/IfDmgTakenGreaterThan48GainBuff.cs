using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class IfDmgTakenGreaterThan48GainBuff : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<IfDmgTakenGreaterThan48GainBuff>();
            harmony.PatchAll(typeof(IfDmgTakenGreaterThan48GainBuff));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnStartTurn_BeforeLog))]
        [HarmonyPostfix]
        private static void StartTurn_BeforeLog(BattleActionModel action, BattleUnitModel __instance)
        {
            // checks if our script contains the right Script Name
            var data = __instance._unitDataModel._classInfo;
            if (data == null) return;
            if (data.PassiveSetInfo.PassiveIdList.Contains(2188844))
            {
                //FrogMainClass.Logg.LogInfo("Successfully Detected Passive ID");
                if (action.Model.GetHitAttackDamagePrevRound() >= 48)
                {
                    FrogMainClass.Logg.LogInfo("Successfully Detected And Met Conditionals");
                    __instance.AddBuff_Giver(BUFF_UNIQUE_KEYWORD.Vulnerable, 1, __instance, BATTLE_EVENT_TIMING.ON_START_TURN, 0, 1, ABILITY_SOURCE_TYPE.PASSIVE, action, 1, 0);
                    __instance.AddBuff_Giver(BUFF_UNIQUE_KEYWORD.Enhancement, 1, __instance, BATTLE_EVENT_TIMING.ON_START_TURN, 0, 1, ABILITY_SOURCE_TYPE.PASSIVE, action, 1, 0);
                }
            }
        }
    }
}
