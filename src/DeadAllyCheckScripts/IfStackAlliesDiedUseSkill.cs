using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class IfStackAlliesDiedUseSkill : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<IfStackAlliesDiedUseSkill>();
            harmony.PatchAll(typeof(IfStackAlliesDiedUseSkill));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnSucceedAttack))]
        [HarmonyPostfix]
        private static void OnSucceedAttack(BattleActionModel action, CoinModel coin, BattleUnitModel target, BattleUnitModel __instance)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                if (scriptName.Contains("IfStackAlliesDiedUseSkill"))
                {
                    //temporary action
                        var model = __instance;
                        var actionSlot = model._actionSlotDetail;
                        var sinActionModel = actionSlot.CreateSinActionModel(true);
                        actionSlot.AddSinActionModelToSlot(sinActionModel);

                        var skillID = Convert.ToInt32(scriptName.Replace("IfStackAlliesDiedUseSkill_", ""));
                        var unitView = SingletonBehavior<BattleObjectManager>.Instance.GetView(model);
                        var unitDataModel = model._unitDataModel;

                        //funny action stuff
                        var sinModel = new UnitSinModel(skillID, model, sinActionModel, false);
                        var battleActionModel = new BattleActionModel(sinModel, model, sinActionModel, -1);
                        battleActionModel._targetDataDetail.ReadyOriginTargeting(battleActionModel);
                        model.CutInDefenseActionForcely(battleActionModel);
                        var enemyBattleAction = action;
                        battleActionModel.ChangeMainTargetSinAction(enemyBattleAction._sinAction, enemyBattleAction, true);

                        //change skill and sinModel?
                        battleActionModel._skill = new SkillModel(Singleton<StaticDataManager>.Instance._skillList.GetData(skillID), unitDataModel.Level, unitDataModel.SyncLevel)
                        {
                            _skillData =  {
                                _targetType = (int)SKILL_TARGET_TYPE.FRONT,
                            }
                        };
                        sinModel._skillId = skillID;

                        //add BattleSkillViewer
                        var skillViewer = new BattleSkillViewer(unitView, skillID.ToString(), battleActionModel._skill);
                        unitView._battleSkillViewers.TryAdd(skillID.ToString(), skillViewer);
                        break;
                };
            }
        }
    }
}
