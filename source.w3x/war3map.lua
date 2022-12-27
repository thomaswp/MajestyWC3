gg_trg_Untitled_Trigger_001 = nil
function InitGlobals()
end

--[[ UnitInfoPanels V4b by Tasyen
AddUnitInfoPanel(frame, update[, condition])
    frame is the containerPanel, update a function(unit) that runs every updateTick, condition is a function(unit) that returns true or false it is used to prevent rotating to this panel.
AddUnitInfoPanelEx(update, condition)
    wrapper for AddUnitInfoPanel, it creates and returns a new frame

SetUnitInfoPanelFrame(frame)
    makes a non SimpleFrame share visibility with the last Added InfoPanel, supports only one Frame per InfoPanel
SetUnitInfoPanelFrameEx()
    wrapper SetUnitInfoPanelFrame creates and returns a new empty Frame

UnitInfoPanelAddTooltipListener(frame, code)
    detects if this frame is visible if it is call the given function which should return a text which is the now wanted tooltipText.

UnitInfoCreateCustomInfo(parent, label, texture, tooltipCode)
    return createContext, infoFrame, iconFrame, labelFrame, textFrame
UnitInfoGetUnit([player])
    returns & recacls the current selected Unit
    without a player it will use the local player.
    
function UnitInfoPanelSetPage(newPage, updateWanted)
    ignores conditions, newPage can be "+" or "-" or a frame
--]]


do
    local real = MarkGameStarted
    local wantedIndex = 1
    local panels, panelsCondition, panelFrame, updates
    local tooltipListener, tooltipBox, tooltipText
    local isReforged = (BlzFrameGetChild ~= nil)    
    local UnitInfoPanelUnit = nil
    local group, timer, trigger
    local unitInfo, parent, pageUp, pageDown, pageUpBig, pageSwaps, createContext, activeIndex
    local HAVE_BIG_PAGE_BUTTON = false --places a big unseeable Button over the UnitInfo, this button lies below all content of the Panels. But it breaks interactive non-SimpleFrames.

    function UnitInfoGetUnit(player)
        if not player then player = GetLocalPlayer() end
        GroupEnumUnitsSelected(group, player, nil)
        UnitInfoPanelUnit = FirstOfGroup(group)
        GroupClear(group)
        return UnitInfoPanelUnit
    end
    -- frame is the containerPanel, update a function(unit) that runs every updateTick, condition is a function that returns true or false it is used to prevent showing this panel currently.
    function AddUnitInfoPanel(frame, update, condition)
        BlzFrameSetParent(frame, BlzGetFrameByName("SimpleInfoPanelUnitDetail", 0))
        table.insert(panels, frame)
        panelsCondition[#panels] = condition
        updates[#panels] = update
        BlzFrameSetVisible(frame, false)
    end
    function AddUnitInfoPanelEx(update, condition)
        local frame = BlzCreateFrameByType("SIMPLEFRAME", "", BlzGetFrameByName("SimpleInfoPanelUnitDetail", 0), "", 0)
        AddUnitInfoPanel(frame, update, condition)
        return frame
    end
    function SetUnitInfoPanelFrame(frame)
        panelFrame[#panels] = frame
        BlzFrameSetVisible(frame, false)
    end
    function SetUnitInfoPanelFrameEx()
        local frame = BlzCreateFrameByType("FRAME", "", BlzGetOriginFrame(ORIGIN_FRAME_GAME_UI, 0), "", 0)
        SetUnitInfoPanelFrame(frame)
        return frame
    end
    -- detects if this frame is visible if it is call the given function which should return a text which is the now wanted tooltipText.
    function UnitInfoPanelAddTooltipListener(frame, code)
        if not tooltipListener[frame] then
            table.insert(tooltipListener, frame)
            tooltipListener[frame] = code
        end
    end

    if not isReforged then
        function UnitInfoAddTooltip(parent, frame)
            -- pre reforged one can not use the auto generated mouse listener, hence create an empty button
            local button = BlzCreateSimpleFrame("EmptySimpleButton", parent, 0)
            local toolTip = BlzCreateFrameByType("SIMPLEFRAME", "", button, "", 0)
            BlzFrameSetAllPoints(button, frame)
            BlzFrameSetTooltip(button, toolTip)
            BlzFrameSetLevel(button, 9)
            BlzFrameSetVisible(toolTip, false)
            return toolTip
        end
    else
        function UnitInfoAddTooltip(parent, frame)
            -- the used simpleFrame autogenerates a mouselistener, lets use it
            local button = BlzFrameGetChild(parent, 0)
            local toolTip = BlzCreateFrameByType("SIMPLEFRAME", "", parent, "", 0)
            BlzFrameSetTooltip(button, toolTip)
            BlzFrameSetVisible(toolTip, false)
            return toolTip
        end
    end

    function UnitInfoAddTooltipEx(parent, frame, code)
        UnitInfoPanelAddTooltipListener(UnitInfoAddTooltip(parent, frame), code)
    end

    function UnitInfoCreateCustomInfo(parent, label, texture, tooltipCode)
        createContext = createContext + 1
        local infoFrame = BlzCreateSimpleFrame("SimpleInfoPanelIconRank", parent, createContext)
        local iconFrame = BlzGetFrameByName("InfoPanelIconBackdrop", createContext)
        local labelFrame = BlzGetFrameByName("InfoPanelIconLabel", createContext) 
        local textFrame = BlzGetFrameByName("InfoPanelIconValue", createContext) 
        BlzFrameSetText(labelFrame, label)
        BlzFrameSetText(textFrame, "xxx")
        BlzFrameSetTexture(iconFrame, texture, 0, false)
        BlzFrameClearAllPoints(iconFrame)
        BlzFrameSetSize(iconFrame, 0.028, 0.028)
        if tooltipCode then
            UnitInfoAddTooltipEx(infoFrame, iconFrame, tooltipCode)
        end
        return createContext, infoFrame, iconFrame, labelFrame, textFrame
    end
    local function PageSwapCheck()
        pageSwaps = pageSwaps - 1
        if pageSwaps < 0 then
            print("Unit Info Panel - NO VALID PANEL", GetUnitName(UnitInfoPanelUnit))
            return false
        end
        return true
    end
    local function nextPanel()
        activeIndex = activeIndex + 1
        if activeIndex > #panels then
           activeIndex = 1
        end
        if PageSwapCheck() and not panelsCondition[activeIndex](UnitInfoPanelUnit) then
            nextPanel()
        end
    end

    local function prevPanel()
        activeIndex = activeIndex - 1
        if activeIndex < 1 then
           activeIndex = #panels
        end
        if PageSwapCheck() and not panelsCondition[activeIndex](UnitInfoPanelUnit) then
            prevPanel()
        end
    end
    local function makeSub(frame)
        BlzFrameSetParent(frame, parent)
    end
    function UnitInfoPanelSetPage(newPage, updateWanted)
        if string.sub(tostring(newPage), 1, 12 ) == "framehandle:" then
            local found = false
            for i, v in ipairs(panels) do 
                if v == newPage or panelFrame[i] == newPage then
                    newPage = i
                    found = true
                    break
                end
            end
            if not found then return end
        end
        BlzFrameSetVisible(panels[activeIndex], false)
        BlzFrameSetVisible(panelFrame[activeIndex], false)
        if newPage == "+" then
            pageSwaps = #panels
            nextPanel()
        elseif newPage == "-" then
            pageSwaps = #panels
            prevPanel()
        else
            activeIndex = math.min(#panels, math.max(1, newPage))
        end
        if updateWanted then wantedIndex = activeIndex end
        BlzFrameSetVisible(panels[activeIndex], true)
        BlzFrameSetVisible(panelFrame[activeIndex], true)
    end
    local function defaultCondition() return true end
    local function Init()
        BlzLoadTOCFile("war3mapImported\\UnitInfoPanels.toc")
        tooltipListener = {}
        panelsCondition = __jarray(defaultCondition)
        panels = {}
        panelFrame = {}
        updates = {}
        activeIndex = 1
        createContext = 1000
        unitInfo = BlzGetFrameByName("SimpleInfoPanelUnitDetail", 0)
        parent = BlzCreateFrameByType("SIMPLEFRAME", "", unitInfo, "", 0)
        pageUp = BlzCreateSimpleFrame("UnitInfoSimpleIconButtonUp", unitInfo, 0)
        pageDown = BlzCreateSimpleFrame("UnitInfoSimpleIconButtonDown", unitInfo, 0)
        --BlzFrameSetAbsPoint(pageUp, FRAMEPOINT_TOPRIGHT, 0.51, 0.136)
        BlzFrameSetAbsPoint(pageUp, FRAMEPOINT_BOTTOMRIGHT, 0.51, 0.08)

        if HAVE_BIG_PAGE_BUTTON then
            pageUpBig = BlzCreateSimpleFrame("EmptySimpleButton", unitInfo, 0)
            BlzFrameSetAllPoints(pageUpBig, unitInfo)
            BlzFrameSetLevel(pageUpBig, 0)
            BlzTriggerRegisterFrameEvent(trigger, pageUpBig, FRAMEEVENT_CONTROL_CLICK)
        end

        BlzTriggerRegisterFrameEvent(trigger, pageUp, FRAMEEVENT_CONTROL_CLICK)
        BlzTriggerRegisterFrameEvent(trigger, pageDown, FRAMEEVENT_CONTROL_CLICK)
        panels[1] = parent
        
        makeSub(BlzGetFrameByName("SimpleInfoPanelIconDamage", 0))
        makeSub(BlzGetFrameByName("SimpleInfoPanelIconDamage", 1))
        makeSub(BlzGetFrameByName("SimpleInfoPanelIconArmor", 2))
        makeSub(BlzGetFrameByName("SimpleInfoPanelIconRank", 3))
        makeSub(BlzGetFrameByName("SimpleInfoPanelIconFood", 4))
        makeSub(BlzGetFrameByName("SimpleInfoPanelIconGold", 5))
        makeSub(BlzGetFrameByName("SimpleInfoPanelIconHero", 6))
        makeSub(BlzGetFrameByName("SimpleInfoPanelIconAlly", 7))
        if isReforged then makeSub(BlzGetOriginFrame(ORIGIN_FRAME_UNIT_PANEL_BUFF_BAR, 0)) end

        -- tooltip handling
        if isReforged then parent = BlzGetFrameByName("ConsoleUIBackdrop", 0) else parent = BlzGetOriginFrame(ORIGIN_FRAME_GAME_UI, 0) end
        tooltipBox = BlzCreateFrame("CustomUnitInfoTextBox", parent, 0, 0)
        tooltipText = BlzCreateFrame("CustomUnitInfoText", tooltipBox, 0, 0)
        BlzFrameSetAbsPoint(tooltipText, FRAMEPOINT_BOTTOMRIGHT, 0.79, 0.18)
        BlzFrameSetSize(tooltipText, 0.275, 0)
        BlzFrameSetPoint(tooltipBox, FRAMEPOINT_TOPLEFT, tooltipText, FRAMEPOINT_TOPLEFT, -0.01, 0.01)
        BlzFrameSetPoint(tooltipBox, FRAMEPOINT_BOTTOMRIGHT, tooltipText, FRAMEPOINT_BOTTOMRIGHT, 0.005, -0.01)
        BlzFrameSetVisible(tooltipBox, false)

                
        TimerStart(timer, 0.05, true, function()
            xpcall(function()
            local found = false
            if BlzFrameIsVisible(unitInfo) then
                
                UnitInfoGetUnit(GetLocalPlayer())
                for int = 1, #tooltipListener, 1 do
                    if BlzFrameIsVisible(tooltipListener[int]) then
                        BlzFrameSetText(tooltipText, tooltipListener[tooltipListener[int]](UnitInfoPanelUnit))
                        found = true
                        break
                    end
                end

                local useAblePages = 0
                for i, v in ipairs(panels) do 
                    if not panels[v] or panels[v](UnitInfoPanelUnit) then
                        useAblePages = useAblePages + 1
                    end
                end
                BlzFrameSetVisible(pageUp, useAblePages > 1)
                BlzFrameSetVisible(pageDown, useAblePages > 1)

                if wantedIndex ~= activeIndex and panelsCondition[wantedIndex](UnitInfoPanelUnit) then
                    UnitInfoPanelSetPage(wantedIndex)
                end

                if not panelsCondition[activeIndex](UnitInfoPanelUnit) then
                    UnitInfoPanelSetPage("+")
                end
                --for i, v in ipairs(updates) do v(UnitInfoPanelUnit) end
                if updates[activeIndex] then updates[activeIndex](UnitInfoPanelUnit) end
                BlzFrameSetVisible(panelFrame[activeIndex], true)
            else
                BlzFrameSetVisible(panelFrame[activeIndex], false)
            end
            BlzFrameSetVisible(tooltipBox, found)
            end, print)
        end)

    end
    function MarkGameStarted()
        real()
        
        group = CreateGroup()
        timer = CreateTimer()
        trigger = CreateTrigger()
        TriggerAddAction(trigger, function()
            if GetTriggerPlayer() == GetLocalPlayer() then
                if BlzGetTriggerFrame() == pageDown then UnitInfoPanelSetPage("-", true) else UnitInfoPanelSetPage("+", true) end
            end
        end)
        Init()
        if FrameLoaderAdd then FrameLoaderAdd(Init) end
    end
end
-- in 1.31 and upto 1.32.9 PTR (when I wrote this). Frames are not correctly saved and loaded, breaking the game.
-- This runs all functions added to it with a 0s delay after the game was loaded.
do
    local data = {}
    local real = MarkGameStarted
    local timer
    
    function FrameLoaderAdd(func)
        table.insert(data, func)
    end
    function MarkGameStarted()
        real()
        local trigger = CreateTrigger()
        timer = CreateTimer()
        TriggerRegisterGameEvent(trigger, EVENT_GAME_LOADED)
        TriggerAddAction(trigger, function()
            TimerStart(timer, 0, false, function()
                for _,v in ipairs(data) do v() end
            end)
            
        end)
    end
end

-- Examle for a non SimpleFrame Button
do
    local realFunction = MarkGameStarted
    local parent, frameParent, textArea
    local function Init()
        BlzLoadTOCFile("war3mapImported\\Templates.toc")
        parent =  AddUnitInfoPanelEx(function(unit)
            BlzFrameSetText(textArea, BlzGetAbilityExtendedTooltip(GetUnitTypeId(unit), 0))
        end)
        frameParent = SetUnitInfoPanelFrameEx()
        textArea = BlzCreateFrameByType("TEXTAREA", "", frameParent, "EscMenuTextAreaTemplate", 0)
        BlzFrameSetPoint(textArea, FRAMEPOINT_TOP, BlzGetFrameByName("SimpleHeroLevelBar", 0), FRAMEPOINT_BOTTOM, 0, -0.001)
	    BlzFrameSetSize(textArea, 0.18, 0.08)
    end
    function MarkGameStarted()
        realFunction()
        realFunction = nil

        Init()
        if FrameLoaderAdd then FrameLoaderAdd(Init) end

    end

end
function CreateUnitsForPlayer0()
local p = Player(0)
local u
local unitID
local t
local life

u = BlzCreateUnitWithSkin(p, FourCC("e000"), 156.3, -1099.8, 173.479, FourCC("e000"))
u = BlzCreateUnitWithSkin(p, FourCC("n00O"), -54.2, -1522.1, 119.139, FourCC("n00O"))
end

function CreateUnitsForPlayer1()
local p = Player(1)
local u
local unitID
local t
local life

u = BlzCreateUnitWithSkin(p, FourCC("h000"), 379.4, -1551.0, 82.081, FourCC("h000"))
u = BlzCreateUnitWithSkin(p, FourCC("h000"), 509.4, -1562.9, 346.399, FourCC("h000"))
u = BlzCreateUnitWithSkin(p, FourCC("h000"), 218.8, -1442.5, 272.535, FourCC("h000"))
u = BlzCreateUnitWithSkin(p, FourCC("h000"), 285.8, -1487.0, 65.920, FourCC("h000"))
end

function CreatePlayerBuildings()
end

function CreatePlayerUnits()
CreateUnitsForPlayer0()
CreateUnitsForPlayer1()
end

function CreateAllUnits()
CreatePlayerBuildings()
CreatePlayerUnits()
end

function Trig_Untitled_Trigger_001_Actions()
CreateNUnitsAtLoc(2, FourCC("hfoo"), Player(0), GetRectCenter(GetPlayableMapRect()), bj_UNIT_FACING)
SetPlayerStateBJ(Player(0), PLAYER_STATE_RESOURCE_GOLD, 1000)
SetPlayerStateBJ(Player(0), PLAYER_STATE_RESOURCE_LUMBER, 1000)
end

function InitTrig_Untitled_Trigger_001()
gg_trg_Untitled_Trigger_001 = CreateTrigger()
TriggerAddAction(gg_trg_Untitled_Trigger_001, Trig_Untitled_Trigger_001_Actions)
end

function InitCustomTriggers()
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

function InitAllyPriorities()
SetStartLocPrioCount(2, 1)
SetStartLocPrio(2, 0, 0, MAP_LOC_PRIO_HIGH)
end

function main()
SetCameraBounds(-29952.0 + GetCameraMargin(CAMERA_MARGIN_LEFT), -30720.0 + GetCameraMargin(CAMERA_MARGIN_BOTTOM), 29952.0 - GetCameraMargin(CAMERA_MARGIN_RIGHT), 30720.0 - GetCameraMargin(CAMERA_MARGIN_TOP), -29952.0 + GetCameraMargin(CAMERA_MARGIN_LEFT), 30720.0 - GetCameraMargin(CAMERA_MARGIN_TOP), 29952.0 - GetCameraMargin(CAMERA_MARGIN_RIGHT), -30720.0 + GetCameraMargin(CAMERA_MARGIN_BOTTOM))
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
InitAllyPriorities()
end

