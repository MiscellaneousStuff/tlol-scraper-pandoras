namespace Api.Game.Offsets;

public interface IGameObjectOffsets
{
    OffsetData NetworkId { get; }
    OffsetData Name { get; }
    OffsetData Team { get; }
    OffsetData IsVisible { get; }
    OffsetData Position { get; }
    OffsetData ObjectName { get; }
    IEnumerable<OffsetData> GetOffsets();
}