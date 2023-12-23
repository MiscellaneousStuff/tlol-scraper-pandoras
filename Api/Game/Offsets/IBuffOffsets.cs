﻿namespace Api.Game.Offsets;


public interface IBuffOffsets
{
    OffsetData BuffEntryBuffStartTime { get; }
    OffsetData BuffEntryBuffEndTime { get; }
    OffsetData BuffEntryBuffCount { get; }
    OffsetData BuffEntryBuffCountAlt1 { get; }
    OffsetData BuffEntryBuffCountAlt2 { get; }
    OffsetData BuffInfo { get; }
    OffsetData BuffInfoName { get; }
    OffsetData BuffType { get; set; }
    IEnumerable<OffsetData> GetOffsets();
}