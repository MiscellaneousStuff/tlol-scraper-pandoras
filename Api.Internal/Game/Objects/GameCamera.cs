using System.Numerics;
using Api.Game.Objects;
using Newtonsoft.Json;

namespace Api.Internal.Game.Objects;

internal class GameCamera : IGameCamera
{
    public bool RequireFullUpdate { get; set; } = true;
    public bool IsValid { get; set; }
    public Matrix4x4 ViewProjMatrix { get; set; }
    public int RendererWidth { get; set; }
    public int RendererHeight { get; set; }

    public Vector2 WorldToScreen(Vector3 worldPosition)
    {
        // Transform to clip space
        var clipSpace = Vector4.Transform(worldPosition, ViewProjMatrix);
    
        // Perspective divide
        var ndc = new Vector3(clipSpace.X, clipSpace.Y, clipSpace.Z) / clipSpace.W;

        // Convert to screen coordinates
        var x = (ndc.X + 1.0f) * 0.5f * RendererWidth;
        var y = (1.0f - ndc.Y) * 0.5f * RendererHeight; 
        
        /*
         	out.x = (screen.x / 2.f * M.x) + (M.x + screen.x / 2.f);
	out.y = -(screen.y / 2.f * M.y) + (M.y + screen.y / 2.f);
         */

        return new Vector2(x, y);
    }
    
    public bool WorldToScreen(Vector3 worldPosition, out Vector2 screenPosition)
    {
        // Transform to clip space
        Vector4 clipSpace = Vector4.Transform(worldPosition, ViewProjMatrix);
    
        // Check Z-value for visibility
        if (clipSpace.Z < 0 || clipSpace.Z > clipSpace.W)
        {
            screenPosition = default;
            return false; // Point is not visible
        }

        // Perspective divide
        var ndc = new Vector3(clipSpace.X, clipSpace.Y, clipSpace.Z) / clipSpace.W;

        // Check NDC for visibility
        if (ndc.X < -1 || ndc.X > 1 || ndc.Y < -1 || ndc.Y > 1)
        {
            screenPosition = default;
            return false; // Point is outside of the visible frustum
        }

        // Convert to screen coordinates
        var x = (ndc.X + 1.0f) * 0.5f * RendererWidth;
        var y = (1.0f - ndc.Y) * 0.5f * RendererHeight; // Y is inverted as screen's Y grows downwards

        screenPosition = new Vector2(x, y);

        return true; // Point is visible on screen
    }
    
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}