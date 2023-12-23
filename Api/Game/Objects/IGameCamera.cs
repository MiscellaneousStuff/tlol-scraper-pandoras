using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Api.Game.Objects
{
    public interface IGameCamera
    {
        bool RequireFullUpdate { get; set; }
        bool IsValid { get; set; }

        Matrix4x4 ViewProjMatrix { get; set; }
        int RendererWidth { get; set; }
        int RendererHeight { get; set; }

        Vector2 WorldToScreen(Vector3 worldPosition);
        bool WorldToScreen(Vector3 worldPosition, out Vector2 screenPosition);
    }
}
