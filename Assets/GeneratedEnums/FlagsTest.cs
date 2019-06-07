using System;
namespace GeneratedEnums
{
    [Flags]
    public enum FlagsTest
    {
        None = 0,
        Ground = 1,
        Road =   1 << 1,//DO NOT USE DIRECTLY
        Road2Lane = Road | 1 << 2,
        Road4Lane = Road | 1 << 3,
        RoadBus = Road | 1 << 4,
        Road6Lane = Road | 1 << 5,
        RoadTram = Road | 1 << 6,
        Rail =   1 << 7,
        Invalid =   1 << 8,
        Road2LaneTo4Lane = Road2Lane | Road4Lane,
        AllInfrastructure = Road2Lane | Road4Lane | Road6Lane | RoadBus | RoadTram | Rail,
    }
}
