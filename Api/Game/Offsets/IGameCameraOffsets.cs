namespace Api.Game.Offsets;

public interface IGameCameraOffsets
{
    public OffsetData ViewProjMatrix { get; }
    public OffsetData Renderer { get; }
    public OffsetData RendererWidth { get; }
    public OffsetData RendererHeight { get; }
}