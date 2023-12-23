local menu = nil
function OnLoad()
    menu = CreateMenu("Hello lua", ScriptType.Utility)
    
end

function OnUnload()
    RemoveMenu(menu)
end


function OnUpdate(deltaTime)

end

function OnRender(deltaTime)
end
