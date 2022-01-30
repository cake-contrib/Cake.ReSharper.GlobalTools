// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.ReSharper.GlobalTools.InspectCode;

/// <summary>
/// Represents InspectCode's own performance.
/// </summary>
public enum InspectCodeMeasure
{
    /// <summary>
    /// Measure: NONE.
    /// </summary>
    None,

    /// <summary>
    /// Measure: MEMORY.
    /// </summary>
    Memory,

    /// <summary>
    /// Measure: SAMPLING.
    /// </summary>
    Sampling,

    /// <summary>
    /// Measure: TIMELINE.
    /// </summary>
    Timeline,
}
