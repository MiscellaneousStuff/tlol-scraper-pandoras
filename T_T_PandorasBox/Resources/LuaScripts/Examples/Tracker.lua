---
--- Basic tracker
---

local hero = Hero
local camera = GameCamera
local heroManager = HeroManager
local gameState = GameState
local renderer = Renderer

local drawEnemyAttackRanges = nil
local drawAllyAttackRanges = nil
local drawEnemyPaths = nil
local drawAllyPaths = nil

function OnLoad()
    local menu = CreateMenu("Tracker", ScriptType.Utility)

    local rangeSubMenu = menu:AddSubMenu("Range display", "")
    drawEnemyAttackRanges = rangeSubMenu:AddToggle("Enemy Attack Range", "Draws circle with enemy attack range", false)
    drawAllyAttackRanges = rangeSubMenu:AddToggle("Ally Attack Range", "Draws circle with ally attack range", false)


    local pathSubMenu = menu:AddSubMenu("Path display", "")
    drawEnemyPaths = pathSubMenu:AddToggle("Enemy Attack Range", "Draws circle with enemy attack range", false)
    drawAllyPaths = pathSubMenu:AddToggle("Ally Attack Range", "Draws circle with ally attack range", false)
end

function OnUnload()
end

function OnUpdate(deltaTime)
end

function drawEnemyRanges()
    drawHeroRange( heroManager:GetEnemyHeroes(), Color(1, 0, 0, 1))
end

function drawAllyRanges()
    drawHeroRange(heroManager:GetAllyHeroes(), Color(0, 1, 0, 1))
end

function drawHeroRange(heroes, color)
    if heroes == nil then
        return
    end

    for target in luanet.each(heroes) do
        if target ~= nil and target.IsVisible and target.IsAlive then
            renderer:Circle3D(target.Position, target.AttackRange, color, 1, gameState.Time, 0.5, 1)
        end
    end
end

function drawEnemyPath()
    local heroes = heroManager:GetEnemyHeroes()
    drawHeroPath(heroes, Color(1, 0, 0, 1))
end

function drawAllyPath()
    local heroes = heroManager:GetAllyHeroes()
    drawHeroPath(heroes, Color(0, 1, 0, 1))
end

function drawHeroPath(heroes, color)
    if heroes == nil then
        return
    end

    for target in luanet.each(heroes) do
        if target ~= nil and target.IsVisible and target.IsAlive then
            --renderer:RenderLines(target.AiManager.RemainingPath, 1, color)
        end
    end
end

function OnRender(deltaTime)
    if drawEnemyAttackRanges.Toggled == true then
        drawEnemyRanges()
    end
    if drawAllyAttackRanges.Toggled == true then
        drawAllyRanges()
    end

    --destroyes preformence
    if drawEnemyPaths.Toggled == true then
        --drawEnemyPath()
    end
    if drawAllyPaths.Toggled == true then
        --drawAllyPath()
    end
end 