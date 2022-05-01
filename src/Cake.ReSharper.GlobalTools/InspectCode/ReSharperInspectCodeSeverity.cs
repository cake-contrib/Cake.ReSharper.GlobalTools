// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.ReSharper.GlobalTools.InspectCode;

/// <summary>
/// Represents InspectCode's minimal severity report.
/// </summary>
public enum ReSharperInspectCodeSeverity
{
    /// <summary>
    /// Severity: INFO.
    /// </summary>
    Info = 1,

    /// <summary>
    /// Severity: HINT.
    /// </summary>
    Hint = 2,

    /// <summary>
    /// Severity: SUGGESTION.
    /// </summary>
    Suggestion = 3,

    /// <summary>
    /// Severity: WARNING.
    /// </summary>
    Warning = 4,

    /// <summary>
    /// Severity: ERROR.
    /// </summary>
    Error = 5,
}
