using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class IfEveryEnemyMeetsConditionalChangeSkill : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<IfEveryEnemyMeetsConditionalChangeSkill>();
            harmony.PatchAll(typeof(IfEveryEnemyMeetsConditionalChangeSkill));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnStartTurn_BeforeLog))]
        [HarmonyPostfix]
        private static void StartTurn_BeforeLog(BattleActionModel action, BattleUnitModel __instance)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                if (scriptName.Contains("IfEveryEnemyMeetsConditionalChangeSkill_"))
                {
                    FrogMainClass.Logg.LogInfo("Successfully Detected: " + scriptName);
                    var newskillID = Convert.ToInt32(scriptName.Substring("IfEveryEnemyMeetsConditionalChangeSkill_".Length));
                    var whae = delegate (SkillModel x) { return x.GetID() == newskillID; };
                    var naenae = __instance.GetSkillList().Find(whae);

                    var keyword_one = ability.buffData.buffKeyword;
                    var keyword_status = Enum.Parse<BUFF_UNIQUE_KEYWORD>(keyword_one);

                    var potency_check = ability.buffData.stack;
                    var count_check = ability.buffData.turn;

                    UNIT_FACTION thisFaction = __instance.Faction;
                    UNIT_FACTION enemyFaction = thisFaction == UNIT_FACTION.PLAYER ? UNIT_FACTION.ENEMY : UNIT_FACTION.PLAYER;
                    var changeSkill = true;

                    foreach (BattleUnitModel model in BattleObjectManager.Instance.GetAliveList(false, enemyFaction))
                    {
                        int count_status = model.GetActivatedBuffTurn(keyword_status);
                        //FrogMainClass.Logg.LogInfo(model._originID + "'s Count of " + keyword_status + "is at " +  count_status);
                        if (model.GetActivatedBuffStack(keyword_status) < potency_check || (count_status >= 0 && count_status < count_check))
                        {
                            FrogMainClass.Logg.LogInfo("Did not activate because of " + model._originID);
                            changeSkill = false;
                            break;
                        }
                    }

                    if (changeSkill) {
                        FrogMainClass.Logg.LogInfo("Successfully Activated: " + scriptName);
                        action.ChangeSkill(naenae);
                    }
                };
            }
        }
    }
}
