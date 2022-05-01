// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.ReSharper.GlobalTools;

/// <summary>
/// Contains common settings used by ReSharper Global Tools.
/// </summary>
public abstract class ReSharperSettings
    : ToolSettings
{
    /// <summary>
    /// Gets or sets the directory where caches will be stored.
    /// The default is %TEMP%.
    /// </summary>
    /// <value>The directory where caches will be stored.</value>
    public DirectoryPath? CachesHome { get; set; }

    /// <summary>
    /// Gets or sets file path in which command line parameters will be stored.
    /// </summary>
    public FilePath? ConfigCreateFile { get; set; }

    /// <summary>
    /// Gets or sets path to configuration file where parameters are specified (use 'config-create' option to create sample file).
    /// </summary>
    public FilePath? ConfigFile { get; set; }

    /// <summary>
    /// Gets or sets a list of <see cref="ReSharperSettingsLayer">settings layer</see> which will be disabled.
    /// </summary>
    public ReSharperSettingsLayer[]? DisabledSettingsLayers { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the debug output should be enabled.
    /// </summary>
    /// <value>
    ///     <c>true</c> if the debug output should be enabled; otherwise, <c>false</c>.
    /// </value>
    public bool Debug { get; set; }

    /// <summary>
    /// Gets or sets .NET Core path.
    /// Empty to ignore .NET Core.
    /// Not specified for autodetect.
    /// Example: /usr/local/share/dotnet/dotnet.
    /// </summary>
    public string? DotNetCorePath { get; set; }

    /// <summary>
    /// Gets or sets .NET Core SDK version.
    /// Example: 3.0.100.
    /// </summary>
    public string? DotNetCoreSdk { get; set; }

    /// <summary>
    /// Gets or sets files excluded by provided wildcards from processing.
    /// If defined along with 'Included' takes higher priority.
    /// </summary>
    public string[]? Exclude { get; set; }

    /// <summary>
    /// Gets or sets a list of ReSharper extensions which will be used.
    /// </summary>
    public string[]? Extensions { get; set; }

    /// <summary>
    /// Gets or sets sources from which extensions should be installed.
    /// </summary>
    public string[]? ExtensionSources { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether help should be written then process should exit.
    /// </summary>
    public bool Help { get; set; }

    /// <summary>
    /// Gets or sets files included by provided wildcards in processing.
    /// </summary>
    public string[]? Include { get; set; }

    /// <summary>
    /// Gets or sets Mono path.
    /// Empty to ignore Mono.
    /// Not specified for autodetect.
    /// Example: /Library/Frameworks/Mono.framework/Versions/Current/bin/mono.
    /// </summary>
    public string? MonoPath { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether all global, solution and project settings should be ignored.
    /// Alias for disabling the settings layers GlobalAll, GlobalPerProduct, SolutionShared, SolutionPersonal,
    /// ProjectShared and ProjectPersonal.
    /// </summary>
    public bool NoBuildInSettings { get; set; }

    /// <summary>
    /// Gets or sets the path to the file to use custom settings from.
    /// </summary>
    public FilePath? Profile { get; set; }

    /// <summary>
    /// Gets or sets MSBuild properties.
    /// </summary>
    /// <value>The MSBuild properties to override.</value>
    public IDictionary<string, string>? MsBuildProperties { get; set; } =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets or sets MSBuild targets.
    /// These targets will be executed to get other items (e.g. Compile item) of projects.
    /// </summary>
    public string[]? TargetsForItems { get; set; }

    /// <summary>
    /// Gets or sets MSBuild targets.
    /// These targets will be executed to get referenced assemblies of projects.
    /// </summary>
    public string[]? TargetsForReferences { get; set; }

    /// <summary>
    /// Gets or sets MsBuild toolset version.
    /// Highest available is used by default.
    /// Example: 12.0.
    /// </summary>
    public string? Toolset { get; set; }

    /// <summary>
    /// Gets or sets MsBuild toolset (exe/dll) path.
    /// Example: /usr/local/msbuild/bin/current/MSBuild.exe.
    /// </summary>
    public FilePath? ToolsetPath { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use x86 tool.
    /// </summary>
    public bool UseX86Tool { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ReSharperVerbosity">verbosity</see> level of the log messages.
    /// </summary>
    public ReSharperVerbosity? Verbosity { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether show tool version and exit.
    /// </summary>
    public bool Version { get; set; }
}
