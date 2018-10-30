///////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Filename: SpawnEnums.cs
//  
// Author: Garth de Wet <garthofhearts@gmail.com>
// Website: http://corruptedsmilestudio.blogspot.com/
// Date Modified: 04 Feb 2012
//
// Copyright (c) 2012 Garth de Wet
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace CorruptedSmileStudio.Spawn
{
    /// <summary>
    /// The different spawn modes supported.
    /// </summary>
    public enum SpawnModes
    {
        /// <summary>
        /// This will continually spawn units until the limit, then spawn whenever a unit dies.
        /// Use this mode with a trigger that disables the spawner.
        /// </summary>
        Normal,
        /// <summary>
        /// This will spawn units till the limit is hit then stop.
        /// </summary>
        Once,
        /// <summary>
        /// This will spawn units in waves. Once all units are dead it will spawn a new wave.
        /// </summary>
        Wave,
        /// <summary>
        /// This will spawn units in waves, once the specified time has passed a new wave will spawn
        /// regardless of whether there were still units left.
        /// </summary>
        TimedWave,
        /// <summary>
        /// This will spawn units in waves, once all units are dead it will, after the specified time,
        /// spawn a new wave.
        /// </summary>
        TimeSplitWave
    }
    /// <summary>
    /// The unit levels allowed.
    /// </summary>
    public enum UnitLevels
    {
        /// <summary>
        /// Easy unit
        /// </summary>
        Easy = 0,
        /// <summary>
        /// Medium unit
        /// </summary>
        Medium,
        /// <summary>
        /// Hard unit
        /// </summary>
        Hard,
        /// <summary>
        /// Boss unit
        /// </summary>
        Boss
    }
}