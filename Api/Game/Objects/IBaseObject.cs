namespace Api.Game.Objects;

public interface IBaseObject
{
    public IntPtr Pointer { get; set; }
    bool RequireFullUpdate { get; set; }
    bool IsValid { get; set; }
    public int NetworkId { get; set; }
}