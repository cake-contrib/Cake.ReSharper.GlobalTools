// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.ReSharper.GlobalTools.InspectCode;

/// <summary>
/// Represents InspectCode's report format.
/// </summary>
public enum ReSharperInspectCodeReportFormat
{
    /// <summary>
    /// Report format: Xml.
    /// </summary>
    Xml,

    /// <summary>
    /// Report format: Html.
    /// </summary>
    Html,

    /// <summary>
    /// Report format: Text.
    /// </summary>
    Text,

    /// <summary>
    /// Report format: Json.
    /// </summary>
    Json,
}
