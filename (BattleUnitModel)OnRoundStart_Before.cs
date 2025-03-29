static void Postfix(BattleUnitModel __instance)
{
    try {
        UNIT_FACTION thisFaction = UNIT_FACTION.PLAYER;
        
        if (__instance._faction == thisFaction)
        {
            int potency_check = 15;
            int count_check = 0;
            int active_round = 0;
            var keyword_string_type_here = "Enhancement";
            BUFF_UNIQUE_KEYWORD keyword = Enum.Parse<BUFF_UNIQUE_KEYWORD>(keyword_string_type_here);
            int MP = 45;
            // int healingUnused = 0;
            __instance.RecoverMp(MP, out MP);
            // __instance.TryRecoverHp(__instance, null, 50, ABILITY_SOURCE_TYPE.SKILL, BATTLE_EVENT_TIMING.ON_START_ROUND, out healingUnused);

            __instance.AddBuff_Giver(keyword, potency_check, __instance, BATTLE_EVENT_TIMING.ON_START_ROUND, count_check, active_round, ABILITY_SOURCE_TYPE.BUFF, null, out potency_check, out count_check);
        };

        UNIT_FACTION enemyFaction = UNIT_FACTION.ENEMY;
        if (__instance._faction == enemyFaction )
        {
            int potency_check_enemy = 25;
            int count_check_enemy = 5;
            var active_round_enemy = 0;
            var keyword_one_enemy = "Combustion";
            BUFF_UNIQUE_KEYWORD keyword_status_enemy = Enum.Parse<BUFF_UNIQUE_KEYWORD>(keyword_one_enemy);
            __instance.AddBuff_Giver(keyword_status_enemy, potency_check_enemy, __instance, BATTLE_EVENT_TIMING.ON_START_ROUND, count_check_enemy, active_round_enemy, ABILITY_SOURCE_TYPE.NONE, null, out potency_check_enemy, out count_check_enemy);
        }

    }
    catch (System.Exception ex)
    {
        UnityExplorer.ExplorerCore.LogWarning($"Exception in patch of void BattleUnitModel::OnRoundStart_Before():\n{ex}");
    }
}

        if (__instance.InstanceID == 10508)

static void Postfix(SinManager __instance)
{
    try {
        int amount = 999;

        Singleton<SinManager>.Instance.AddSinStock(UNIT_FACTION.PLAYER, ATTRIBUTE_TYPE.AMBER, amount, null);

        Singleton<SinManager>.Instance.AddSinStock(UNIT_FACTION.PLAYER, ATTRIBUTE_TYPE.AZURE, amount, null);

        Singleton<SinManager>.Instance.AddSinStock(UNIT_FACTION.PLAYER, ATTRIBUTE_TYPE.CRIMSON, amount, null);


        Singleton<SinManager>.Instance.AddSinStock(UNIT_FACTION.PLAYER, ATTRIBUTE_TYPE.SCARLET, amount, null);

        Singleton<SinManager>.Instance.AddSinStock(UNIT_FACTION.PLAYER, ATTRIBUTE_TYPE.INDIGO, amount, null);

        Singleton<SinManager>.Instance.AddSinStock(UNIT_FACTION.PLAYER, ATTRIBUTE_TYPE.VIOLET, amount, null);

        Singleton<SinManager>.Instance.AddSinStock(UNIT_FACTION.PLAYER, ATTRIBUTE_TYPE.SHAMROCK, amount, null);
    }
    catch (System.Exception ex) {
        UnityExplorer.ExplorerCore.LogWarning($"Exception in patch of void SinManager::InitAllies():\n{ex}");
    }
}

            // __instance.GiveBuff_Self(__instance, keyword_status, potency_check, count_check, active_round, BATTLE_EVENT_TIMING.ON_START_ROUND, null);
            // var keyword_one = "Enhancement";
            // var keyword_status = Enum.Parse<BUFF_UNIQUE_KEYWORD>(keyword_one);

        // int MP = 45;
        // __instance.RecoverMp(MP, out MP);
        UNIT_FACTION enemyFaction = UNIT_FACTION.ENEMY;
        if (__instance._faction == enemyFaction )
        {
            var potency_check_enemy = 999;
            var count_check_enemy = 999;
            var active_round_enemy = 0;
            var keyword_one_enemy = "Laceration";
            var keyword_status_enemy = Enum.Parse<BUFF_UNIQUE_KEYWORD>(keyword_one_enemy);
            __instance.AddBuff_Giver(keyword_status_enemy, potency_check_enemy, __instance, BATTLE_EVENT_TIMING.ON_START_ROUND, count_check_enemy, active_round_enemy, ABILITY_SOURCE_TYPE.NONE, null, potency_check_enemy, count_check_enemy);
        }

    foreach (var buff in Singleton<StaticDataManager>.Instance._buffList.list)
    {
        buff.maxStack = 2147483647;
        buff.maxTurn = 2147483647;
    }
    foreach (var stage in Singleton<StaticDataManager>.Instance._storyBattleStageList.list)
    {
        stage.recommendedLevel = 1;
        stage.story = new StoryInfo { };
    }
        foreach (var entry in Singleton<StaticDataManager>.Instance._storyBattleStageList.list)
    {
        stage.recommendedLevel = 1;
        stage.story = new StoryInfo { };
    }
    foreach (var stage in Singleton<StaticDataManager>.Instance._dungeonBattleStageList.list)
    {
        stage.recommendedLevel = 1;
        stage.story = new StoryInfo { };
    }
    foreach (var stage in Singleton<StaticDataManager>.Instance._abBattleStageList.list)
    {
        stage.recommendedLevel = 1;
        stage.story = new StoryInfo { };
    }

static void Postfix(BattleUnitModel __instance, BattleActionModel action)
{
    try
    {

        bool MySkillPredicate(SkillModel x)
        {
            return x.GetID() == 999905;
        };

        Il2CppSystem.Predicate<SkillModel> predicate = new Il2CppSystem.Predicate<SkillModel>(MySkillPredicate);
        var naenae = __instance.GetSkillList().Find(predicate);
        action.ChangeSkill(naenae);
    }
    catch (System.Exception ex)
    {
        UnityExplorer.ExplorerCore.LogWarning($"Exception in patch of virtual void BattleUnitModel::OnStartTurn_BeforeLog(BattleActionModel action, BATTLE_EVENT_TIMING timing):\n{ex}");
    }
}


////if (__instance._faction == enemyFaction)
////{
////    __instance.AddBuff_Giver(BUFF_UNIQUE_KEYWORD.Sinking, 5, __instance, BATTLE_EVENT_TIMING.ON_START_ROUND, 5, 0, ABILITY_SOURCE_TYPE.NONE, null, 5, 5);
////}
////__instance.AddBuff_Giver(BUFF_UNIQUE_KEYWORD.DarkFlame, 5, __instance, BATTLE_EVENT_TIMING.ON_START_ROUND, 0, 0, ABILITY_SOURCE_TYPE.NONE, null, 5, 0);