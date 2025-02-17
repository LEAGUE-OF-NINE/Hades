using System;
using System.Diagnostics;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Il2CppDumper;
using Il2CppSystem.IO;
using Il2CppSystem.Security.Cryptography;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using Random = System.Random;
using Cpp2IL.Core;

namespace BaseMod;

[BepInPlugin(GUID, NAME, VERSION)]
public class FrogMainClass : BasePlugin
{
    public const string NAME = "FroggoScripts";
    public const string VERSION = "0.0.12";
    public const string AUTHOR = "Froggo";
    public const string GUID = AUTHOR + "." + NAME;

    public override void Load()
	{
        FrogMainClass.Logg = new ManualLogSource(NAME);
        BepInEx.Logging.Logger.Sources.Add(FrogMainClass.Logg);

        var harmony = new Harmony(NAME);

        // multiconditional
        ChangeSkillOnMultiConditional.Setup(harmony);
        ChangeSkillOnConditionalTarget.Setup(harmony);
        AtLessThanConditionalChangeSkill.Setup(harmony);

        // casetti skills
        LowerBuffIfGreaterByStack.Setup(harmony);
        LowerBuffIfGreaterByTurn.Setup(harmony);

        // factioncheck skills
        OnSAGiveBuffFactionCheck.Setup(harmony);
        GiveBuffOnUseFactionCheck.Setup(harmony);
        GiveBuffOnUseFactionCheckReson.Setup(harmony);
        GiveBuffOnUseFactionCheckPerfectReson.Setup(harmony);

        // LCCCB Ryoshu Coin Effect
        CustomReloadScript.Setup(harmony);
        SPDrainCustomReloadScript.Setup(harmony);

        // Speed/HP Dependant Change Skill
        AtValueSpeedChangeSkill.Setup(harmony);
        WhenBelowValueHPPercentageChangeSkill.Setup(harmony);

        // reuse coin on buff check self
        ReuseAllCoinsOnBuffCheckSelf.Setup(harmony);
        ReuseCoin1andCoin2OnBuffCheckSelf.Setup(harmony);

        // reuse coin on buff check target
        ReuseAllCoinsOnBuffCheckTarget.Setup(harmony);

        // dead ally scripts
        IfStackAlliesDiedChangeSkill.Setup(harmony);
        GainDeadAlliesMultiplyBuffs.Setup(harmony);

        // new passives
        CapSPAtStack.Setup(harmony);
        IfDmgTakenGreaterThan48GainBuff.Setup(harmony);
        GainPowerOnFactionCheck.Setup(harmony);

        // gauntlet
        HealSPOnEvadeEveryAlly.Setup(harmony);
        AtStackMPCheckChangeSkill.Setup(harmony);
        AtStackMPCheckTargetChangeSkill.Setup(harmony);
        AtLessThanStackMPCheckTargetChangeSkill.Setup(harmony);
        AtLessThanStackMPCheckChangeSkill.Setup(harmony);
        IfEveryEnemyMeetsConditionalChangeSkill.Setup(harmony);

        // racism passives
        // OnSaDealAbsHpDmgIfFactionCheck.Setup(harmony);
        // CoinRollAllHeads.Setup(harmony);
    }

    public static ManualLogSource Logg;
}