// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.ReSharper.GlobalTools.CleanupCode;

public sealed class ReSharperCleanupCodeSettings
    : ReSharperSettings
{
    /// <summary>
    /// Gets or sets path to the file to use custom settings from
    /// (default: Use R#'s solution shared settings if exists).
    /// </summary>
    public FilePath? SettingsPath { get; set; }
}
