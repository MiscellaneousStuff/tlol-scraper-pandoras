using System.Numerics;

namespace Api.Game.Objects;

public interface IAiManager
{
    public IntPtr Pointer { get; set; }
    public Vector3 TargetPosition { get; set; }
    public Vector3 PathStart { get; set; }
    public Vector3 PathEnd { get; set; }
    public int CurrentPathSegment { get; set; }
    public List<Vector3> PathSegments { get; set; }
    public int PathSegmentsCount { get; set; }
    public Vector3 CurrentPosition { get; set; }
    public bool IsDashing { get; set; }
    public float DashSpeed { get; set; }
    public bool IsMoving { get; set; }
    public float MovementSpeed { get; set; }
    public IEnumerable<Vector3> RemainingPath { get; }
    public IEnumerable<Vector3> GetRemainingPath();
}