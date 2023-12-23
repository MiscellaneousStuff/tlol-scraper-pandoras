local hero = Hero
local camera = GameCamera
local time = 0
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

function OnLoad()

end

function OnUnload()

end

function OnUpdate(deltaTime)
    
end


local function drawObjects(gameObjects)
    if gameObjects == nil then
        return
    end
    
    for gameObject in luanet.each(gameObjects) do
        if gameObject ~= nil then
            Renderer:Circle3D(gameObject.Position, gameObject.CollisionRadius, Color(0.0, 0.0, 1.0, 1.0), 1, time, 0.5, 1)
            local screenPosition = camera:WorldToScreen(gameObject.Position)
            --Renderer:Text(gameObject.Name .. ' - ' .. gameObject.ObjectName, screenPosition, 21, Color(1.0, 1.0, 1.0, 1.0))
            --Renderer:Text(gameObject.NetworkId, screenPosition, 21, Color(1.0, 1.0, 1.0, 1.0))
            Renderer:Text(tostring(gameObject.NetworkId), screenPosition, 21, Color(1.0, 1.0, 1.0, 1.0))
        end
    end
end

local function drawGameObjects()
    local gameObjects = objectManager:GetGameObjects()
    drawObjects(gameObjects)
end

local function drawEnemyMinions()
    local gameObjects = minionManager:GetEnemyMinions()
    drawObjects(gameObjects)
end

local function drawAllayMinions()
    local gameObjects = minionManager:GetAllyMinions()
    drawObjects(gameObjects)
end

local function drawMonsters()
    local gameObjects = monsterManager:GetMonsters()
    drawObjects(gameObjects)
end

local function drawPlants()
    local gameObjects = plantManager:GetPlants()
    drawObjects(gameObjects)
end

local function drawAllayWards()
    local gameObjects = wardManager:GetAllyWards()
    drawObjects(gameObjects)
end

local function drawEnemyWards()
    local gameObjects = wardManager:GetEnemyWards()
    drawObjects(gameObjects)
end

local function drawAllayTraps()
    local gameObjects = trapManager:GetAllyTraps()
    drawObjects(gameObjects)
end

local function drawEnemyTraps()
    local gameObjects = trapManager:GetEnemyTraps()
    drawObjects(gameObjects)
end

local function drawAllayTurrets()
    local gameObjects = turretManager:GetAllTurrets()
    drawObjects(gameObjects)
end

local function drawEnemyTurrets()
    local gameObjects = turretManager:GetEnemyTurrets()
    drawObjects(gameObjects)
end

local function drawAllayInhibitors()
    local gameObjects = inhibitorManager:GetAllInhibitors()
    drawObjects(gameObjects)
end

local function drawEnemyInhibitors()
    local gameObjects = inhibitorManager:GetEnemyInhibitors()
    drawObjects(gameObjects)
end

local lastPos = nil
local function drawMissiles()
    local missiles = missileManager:GetMissiles()
    
    for missile in luanet.each(missiles) do
        if missile ~= nil then
            Renderer:Circle3D(missile.Position, 100, Color(0.0, 0.0, 1.0, 1.0), 1, time, 0.5, 1)
           
            --Renderer:Circle3D(missile.StartPosition, 50, Color(0.0, 0.0, 1.0, 1.0), 1, time, 0.5, 1)
            --Renderer:Circle3D(missile.EndPosition, 50, Color(0.0, 0.0, 1.0, 1.0), 1, time, 0.5, 1)
            --local pos = missile.Position;
            --pos.Y = missile.EndPosition.Y;
            --Renderer:Rect3D(pos, missile.EndPosition, 60, Color(0.0, 0.0, 1.0, 1.0))
            --Renderer:Rect3D(Math.MidPoint(missile.StartPosition, missile.EndPosition), Vector2(20, distance), Vector3(angle, 0, 0), Color(0.0, 0.0, 1.0, 1.0))
        end
    end
end

local function drawLocalPlayer()
    Renderer:Circle3D(hero.Position, hero.CollisionRadius, Color(0.0, 0.0, 1.0, 1.0), 1, time, 0.5, 2);
end

function debugSpell(item, pos)
    local localPos = Vector2(pos.X, pos.Y)
    local size = Vector2(60, 20)
    Renderer:Rect(Vector2(localPos.X, localPos.Y), size, Color(0, 0, 0, 0.7))
    Renderer:Text(tostring(item.Cooldown), localPos, 21, Color(1.0, 1.0, 1.0, 1.0))
    pos.X = pos.X + 150
    return pos
end

function OnRender(deltaTime)
   
end
