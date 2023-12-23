namespace Api.Game.Offsets;

public interface IAiManagerOffsets
{
    public OffsetData TargetPosition { get; }
    public OffsetData PathStart { get; }
    public OffsetData PathEnd { get; }
    public OffsetData CurrentPathSegment { get; }
    public OffsetData PathSegments { get; }
    public OffsetData PathSegmentsCount { get; }
    public OffsetData CurrentPosition { get; }
    public OffsetData IsDashing { get; }
    public OffsetData DashSpeed { get; }
    public OffsetData IsMoving { get; }
    public OffsetData MovementSpeed { get; }
    IEnumerable<OffsetData> GetOffsets();
}