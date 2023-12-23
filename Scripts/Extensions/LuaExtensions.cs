using NLua;

namespace Scripts.Extensions;

public static class LuaExtensions
{
    public static void RegisterEnum<T>(this Lua lua) where T : struct, Enum
    {
        var name = typeof(T).Name;
        lua.DoString($"{name} = {{}}");
        foreach (var enumValue in Enum.GetValues<T>())
        {
            lua[$"{name}.{enumValue}"] = enumValue;
        }
    }
}