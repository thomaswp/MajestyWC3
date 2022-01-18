gg_trg_Untitled_Trigger_001 = nil
gg_trg_Untitled_Trigger_002 = nil
function InitGlobals()
end

function CreateBuildingsForPlayer0()
    local p = Player(0)
    local u
    local unitID
    local t
    local life
    u = BlzCreateUnitWithSkin(p, FourCC("h001"), 512.0, -1216.0, 270.000, FourCC("h001"))
    u = BlzCreateUnitWithSkin(p, FourCC("n003"), -448.0, -1600.0, 270.000, FourCC("n003"))
    u = BlzCreateUnitWithSkin(p, FourCC("h002"), -64.0, -1152.0, 270.000, FourCC("h002"))
end

function CreateUnitsForPlayer0()
    local p = Player(0)
    local u
    local unitID
    local t
    local life
    u = BlzCreateUnitWithSkin(p, FourCC("e000"), 156.3, -1099.8, 173.479, FourCC("e000"))
    u = BlzCreateUnitWithSkin(p, FourCC("h000"), 360.8, -1547.0, 290.828, FourCC("h000"))
    u = BlzCreateUnitWithSkin(p, FourCC("h000"), 445.8, -1566.9, 216.196, FourCC("h000"))
    u = BlzCreateUnitWithSkin(p, FourCC("h003"), 52.4, -1746.7, 2.033, FourCC("h003"))
    u = BlzCreateUnitWithSkin(p, FourCC("h000"), 207.1, -1500.9, 179.236, FourCC("h000"))
    u = BlzCreateUnitWithSkin(p, FourCC("h000"), 280.7, -1524.1, 238.477, FourCC("h000"))
end

function CreateUnitsForPlayer8()
    local p = Player(8)
    local u
    local unitID
    local t
    local life
    u = BlzCreateUnitWithSkin(p, FourCC("H002"), 256.6, -1837.4, 296.321, FourCC("H002"))
    u = BlzCreateUnitWithSkin(p, FourCC("n001"), 130.2, -1822.8, 254.056, FourCC("n001"))
    u = BlzCreateUnitWithSkin(p, FourCC("n00M"), 226.8, -1921.7, 76.687, FourCC("n00M"))
    u = BlzCreateUnitWithSkin(p, FourCC("H004"), 33.4, -1915.8, 177.292, FourCC("H004"))
end

function CreatePlayerBuildings()
    CreateBuildingsForPlayer0()
end

function CreatePlayerUnits()
    CreateUnitsForPlayer0()
    CreateUnitsForPlayer8()
end

function CreateAllUnits()
    CreatePlayerBuildings()
    CreatePlayerUnits()
end

function Trig_Untitled_Trigger_002_Actions()
    CreateTextTagUnitBJ("TRIGSTR_081", GetKillingUnitBJ(), 0, 10, 100, 100, 100, 0)
end

function InitTrig_Untitled_Trigger_002()
    gg_trg_Untitled_Trigger_002 = CreateTrigger()
    TriggerRegisterAnyUnitEventBJ(gg_trg_Untitled_Trigger_002, EVENT_PLAYER_UNIT_DEATH)
    TriggerAddAction(gg_trg_Untitled_Trigger_002, Trig_Untitled_Trigger_002_Actions)
end

function Trig_Untitled_Trigger_001_Actions()
    SetPlayerStateBJ(Player(0), PLAYER_STATE_RESOURCE_GOLD, 1000)
    SetPlayerStateBJ(Player(0), PLAYER_STATE_RESOURCE_LUMBER, 1000)
end

function InitTrig_Untitled_Trigger_001()
    gg_trg_Untitled_Trigger_001 = CreateTrigger()
    TriggerAddAction(gg_trg_Untitled_Trigger_001, Trig_Untitled_Trigger_001_Actions)
end

function InitCustomTriggers()
    InitTrig_Untitled_Trigger_002()
    InitTrig_Untitled_Trigger_001()
end

function RunInitializationTriggers()
    ConditionalTriggerExecute(gg_trg_Untitled_Trigger_001)
end

function InitCustomPlayerSlots()
    SetPlayerStartLocation(Player(0), 0)
    ForcePlayerStartLocation(Player(0), 0)
    SetPlayerColor(Player(0), ConvertPlayerColor(0))
    SetPlayerRacePreference(Player(0), RACE_PREF_HUMAN)
    SetPlayerRaceSelectable(Player(0), true)
    SetPlayerController(Player(0), MAP_CONTROL_USER)
    SetPlayerStartLocation(Player(1), 1)
    ForcePlayerStartLocation(Player(1), 1)
    SetPlayerColor(Player(1), ConvertPlayerColor(1))
    SetPlayerRacePreference(Player(1), RACE_PREF_HUMAN)
    SetPlayerRaceSelectable(Player(1), true)
    SetPlayerController(Player(1), MAP_CONTROL_COMPUTER)
    SetPlayerStartLocation(Player(8), 2)
    ForcePlayerStartLocation(Player(8), 2)
    SetPlayerColor(Player(8), ConvertPlayerColor(8))
    SetPlayerRacePreference(Player(8), RACE_PREF_UNDEAD)
    SetPlayerRaceSelectable(Player(8), true)
    SetPlayerController(Player(8), MAP_CONTROL_COMPUTER)
end

function InitCustomTeams()
    SetPlayerTeam(Player(0), 0)
    SetPlayerState(Player(0), PLAYER_STATE_ALLIED_VICTORY, 1)
    SetPlayerTeam(Player(1), 0)
    SetPlayerState(Player(1), PLAYER_STATE_ALLIED_VICTORY, 1)
    SetPlayerAllianceStateAllyBJ(Player(0), Player(1), true)
    SetPlayerAllianceStateAllyBJ(Player(1), Player(0), true)
    SetPlayerAllianceStateVisionBJ(Player(0), Player(1), true)
    SetPlayerAllianceStateVisionBJ(Player(1), Player(0), true)
    SetPlayerTeam(Player(8), 1)
end

function main()
    SetCameraBounds(-5888.0 + GetCameraMargin(CAMERA_MARGIN_LEFT), -6144.0 + GetCameraMargin(CAMERA_MARGIN_BOTTOM), 8960.0 - GetCameraMargin(CAMERA_MARGIN_RIGHT), 8704.0 - GetCameraMargin(CAMERA_MARGIN_TOP), -5888.0 + GetCameraMargin(CAMERA_MARGIN_LEFT), 8704.0 - GetCameraMargin(CAMERA_MARGIN_TOP), 8960.0 - GetCameraMargin(CAMERA_MARGIN_RIGHT), -6144.0 + GetCameraMargin(CAMERA_MARGIN_BOTTOM))
    SetDayNightModels("Environment\\DNC\\DNCLordaeron\\DNCLordaeronTerrain\\DNCLordaeronTerrain.mdl", "Environment\\DNC\\DNCLordaeron\\DNCLordaeronUnit\\DNCLordaeronUnit.mdl")
    NewSoundEnvironment("Default")
    SetAmbientDaySound("LordaeronSummerDay")
    SetAmbientNightSound("LordaeronSummerNight")
    SetMapMusic("Music", true, 0)
    CreateAllUnits()
    InitBlizzard()
    InitGlobals()
    InitCustomTriggers()
    RunInitializationTriggers()
end

function config()
    SetMapName("TRIGSTR_001")
    SetMapDescription("TRIGSTR_003")
    SetPlayers(3)
    SetTeams(3)
    SetGamePlacement(MAP_PLACEMENT_USE_MAP_SETTINGS)
    DefineStartLocation(0, 512.0, -1216.0)
    DefineStartLocation(1, 2816.0, 320.0)
    DefineStartLocation(2, -3456.0, -4928.0)
    InitCustomPlayerSlots()
    InitCustomTeams()
end

