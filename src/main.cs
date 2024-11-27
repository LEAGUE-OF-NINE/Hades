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
using UnityEngine;
using Random = System.Random;

namespace BaseMod;

[BepInPlugin(GUID, NAME, VERSION)]
public class Main : BasePlugin
{
    public const string NAME = "FroggoScripts";
    public const string VERSION = "0.0.4";
    public const string AUTHOR = "Froggo";
    public const string GUID = AUTHOR + "." + NAME;

    public override void Load()
	{
        var harmony = new Harmony(NAME);

        // multiconditional
        ChangeSkillOnMultiConditional.Setup(harmony);
        ChangeSkillOnConditionalTarget.Setup(harmony);

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

        // Speed/HP Dependant Change Skill
        AtValueSpeedChangeSkill.Setup(harmony);
        WhenBelowValueHPPercentageChangeSkill.Setup(harmony);

        // reuse coin on buff check self
        ReuseAllCoinsOnBuffCheckSelf.Setup(harmony);
        ReuseCoin1andCoin2OnBuffCheckSelf.Setup(harmony);

        // reuse coin on buff check target
        ReuseAllCoinsOnBuffCheckTarget.Setup(harmony);
    }
}