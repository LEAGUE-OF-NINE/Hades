using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
	internal class ChangeSkillOnConditionalTarget : MonoBehaviour
	{
		public static void Setup(Harmony harmony)
		{
			ClassInjector.RegisterTypeInIl2Cpp<ChangeSkillOnConditionalTarget>();
			harmony.PatchAll(typeof(ChangeSkillOnConditionalTarget));
		}

		[HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnStartTurn_BeforeLog))]
		[HarmonyPostfix]
		private static void StartTurn_BeforeLog(BattleActionModel action, BattleUnitModel __instance)
		{
			foreach (var ability in action._skill.GetSkillAbilityScript())
			{
				var scriptName = ability.scriptName;
				if (scriptName.Contains("ChangeSkillOnConditionalTarget_"))
				{
                    FrogMainClass.Logg.LogInfo("Successfully Detected: " + scriptName);
                    if (ability.buffData == null) continue;

					var newskillID = Convert.ToInt32(scriptName.Replace("ChangeSkillOnConditionalTarget_", ""));
					var whae = delegate (SkillModel x) { return x.GetID() == newskillID; };
					var naenae = __instance.GetSkillList().Find(whae);

					var keyword_one = ability.buffData.buffKeyword;
					var keyword_status_one = Enum.Parse<BUFF_UNIQUE_KEYWORD>(keyword_one);

					var potency_check = ability.buffData.stack;
					var count_check = ability.buffData.turn;
					BattleUnitModel battleUnitModel = (action != null) ? action.GetMainTarget() : null;
					var target_potency = battleUnitModel.GetActivatedBuffStack(keyword_status_one);
					var target_count = 0;

                        if (count_check >= 1) { target_count = battleUnitModel.GetActivatedBuffTurn(keyword_status_one); };

					if (battleUnitModel != null)
					{
						if (target_count >= count_check)
						{
							if (target_potency >= potency_check)
							{
								FrogMainClass.Logg.LogInfo("Successfully Activated: " + scriptName);
								action.ChangeSkill(naenae);
							} else { FrogMainClass.Logg.LogInfo("Failed because of potency check"); };
						} else { FrogMainClass.Logg.LogInfo("Failed because of target_count_check"); };
					} else { FrogMainClass.Logg.LogInfo("Failed because of null check"); }
				};
			}
		}
	}
}
