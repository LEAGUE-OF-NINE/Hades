using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;
using System.Linq;

namespace BaseMod
{
    internal class CoinRollAllHeads : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<CoinRollAllHeads>();
            harmony.PatchAll(typeof(CoinRollAllHeads));
        }

        [HarmonyPatch(typeof(CoinModel), nameof(CoinModel.Roll))]
        [HarmonyPrefix]
        private static void Roll(CoinModel __instance, float prob, BattleActionModel action)
        {
            var id = action.GetSkillID();
            if (id.Equals(2080611) || id.Equals(2020211) || id.Equals(2040711) || id.Equals(2050411) || id.Equals(1110603) || id.Equals(1110604))
            {
                __instance._result = COIN_RESULT.HEAD;
            }
            else if (id.Equals(2080211) || id.Equals(91004006))
            {
                __instance._result = COIN_RESULT.TAIL;
            }
        }
    }
}
