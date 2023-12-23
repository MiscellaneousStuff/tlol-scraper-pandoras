local hero = Hero
local camera = GameCamera
local damageCalculator = DamageCalculator
local damagePrediction = DamagePrediction
local inputManager = InputManager
local gameInput = GameInput
local objectManager = ObjectManager
local minionManager = MinionManager
local monsterManager = MonsterManager
local plantManager = PlantManager
local wardManager = WardManager
local trapManager = TrapManager
local turretManager = TurretManager
local inhibitorManager = InhibitorManager
local missileManager = MissileManager

local time = 0
local humanizer = nil
local extraWindup = nil
local ping = nil
local stoppingDistance = nil

local humanizeTimer = 0
local attackTimer = 0
local moveTimer = 0

local comboHotkey = nil
local farmHotkey = nil
local clearHotkey = nil

local function updateTimers(deltaTime)
    humanizeTimer = humanizeTimer - deltaTime
    attackTimer = attackTimer - deltaTime
    moveTimer = moveTimer - deltaTime
end

local function getAttackTime()
    return math.max(1.0 / hero.AttackSpeed, ping.Value / 100)
end

local function getWindupTime()
    return (1.0 / getAttackTime()) * (hero.BasicAttackWindup / hero.AttackSpeedRatio)
end

local function move(position)
    if moveTimer > 0 then
        return
    end
    
    local screenPosition = camera:WorldToScreen(hero.Position)
    local mousePosition = gameInput.MousePosition

    if Math.Distance(screenPosition, mousePosition) <= stoppingDistance.Value then
        return
    end

    if gameInput:IssueOrder(position, IssueOrderType.Move) == true then
        humanizeTimer = humanizer.Value / 1000
    end
end

local function move2(position)
    if moveTimer > 0 then
        return
    end

    if gameInput:IssueOrder(position, IssueOrderType.Move) == true then
        humanizeTimer = humanizer.Value / 1000
    end
end

local function attackTarget(target)
    if attackTimer > 0 then
        return false
    end
    
    if gameInput:IssueOrder(target.Position, IssueOrderType.Attack) then
        attackTimer = getAttackTime()
        moveTimer = getWindupTime() + extraWindup.Value / 1000
        humanizeTimer = humanizer.Value / 1000

        return true
    end
    
    return false
end

local function getBestMinion()
    local minions = minionManager:GetEnemyMinions(hero.AttackRange)
    if minions == nil then
        return
    end

    local targetHp = 9999
    local target = nil
    for minion in luanet.each(minions) do
        local health = damagePrediction:PredictHealth(minion, 100)
        if minion ~= nil and health > 0 and health < targetHp then
            targetHp = minion.Health
            target = minion
        end
    end

    return target
end

local function getLasthitableMinion()
    local minion = getBestMinion()

    if minion ~= nil and minion.Health > damageCalculator:GetDamage(DamageType.Physical, hero, minion, hero.TotalAttackDamage) then
        return nil
    end
    return minion
end


function OnLoad()
    local menu = CreateMenu("Test Lua menu", ScriptType.OrbWalker)
    humanizer = menu:AddValueSlider("Humanizer", "Delay between actions is ms", 25, 25, 300)

    extraWindup = menu:AddValueSlider("Extra windup", "", 10, 0, 80)
    ping = menu:AddValueSlider("Ping", "", 35, 0, 250)

    stoppingDistance = menu:AddValueSlider("Stopping distance", "No move action will be taken when mouse is in range", 80, 0, 250)

    local hotkeysSubmenu = menu:AddSubMenu("Hotkeys", "You can setup hotkeys for orbwalker")
    
    comboHotkey = hotkeysSubmenu:AddHotkey("Combo", "Combo mode. Will attack enemy heroes.", VirtualKey.Spacebar, HotkeyType.Press)

    farmHotkey = hotkeysSubmenu:AddHotkey("Farm", "Farm mode. Will farm minions.", VirtualKey.X, HotkeyType.Press)

    clearHotkey = hotkeysSubmenu:AddHotkey("Clear", "Clear mode. Will attack minions.", VirtualKey.V, HotkeyType.Press)
    
end

function OnUnload()
end


function OnUpdate(deltaTime)

    updateTimers(deltaTime)

    if humanizeTimer > 0 then
        return
    end

    local target = nil
    local moveAction = false
    if clearHotkey.Enabled then
        target = getBestMinion()
        moveAction = true
    end

    if farmHotkey.Enabled then
        target = getLasthitableMinion()
        moveAction = true
    end

    if comboHotkey.Enabled then
        target = nil
        moveAction = true
    end

    if target ~= nil then
        if(attackTarget(target)) then
            return
        end
    end
    
    if moveAction == true then
        move(gameInput.MousePosition)
    end
end

function drawMinions()
    local minions = minionManager:GetEnemyMinions(hero.AttackRange)
    if minions == nil then
        return
    end

    for minion in luanet.each(minions) do
        if minion ~= nil then
            Renderer:Circle3D(minion.Position, minion.CollisionRadius, Color(0.0, 0.0, 1.0, 1.0), 1, time, 0.5, 1)
            local screenPosition = camera:WorldToScreen(minion.Position)
            --local damage = damageCalculator:GetDamage(DamageType.Physical, hero, minion, hero.TotalAttackDamage)
            --Renderer:Text(tostring(math.ceil(damage)) .. ' ' .. tostring(math.ceil(minion.Health)), screenPosition, 21, Color(1.0, 1.0, 1.0, 1.0))
        end
    end
end 

function OnRender(deltaTime)
    time = time + deltaTime
    return
    Renderer:Circle3D(hero.Position, hero.AttackRange, Color(0.0, 0.0, 1.0, 1.0), 1, time, 0.5, 2);
end
