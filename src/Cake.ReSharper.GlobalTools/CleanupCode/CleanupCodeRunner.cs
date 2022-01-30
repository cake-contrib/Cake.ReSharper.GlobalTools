// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.ReSharper.GlobalTools.CleanupCode;

public sealed class CleanupCodeRunner
    : ReSharperGlobalToolRunner<CleanupCodeSettings>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CleanupCodeRunner"/> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="processRunner">The process runner.</param>
    /// <param name="tools">The tool locator.</param>
    /// <param name="log">The logger.</param>
    public CleanupCodeRunner(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        IProcessRunner processRunner,
        IToolLocator tools,
        ICakeLog log)
        : base(fileSystem, environment, processRunner, tools, log)
    {
    }

    /// <summary>
    /// Analyzes the specified solution, using the specified settings.
    /// </summary>
    /// <param name="solution">The solution.</param>
    /// <param name="settings">The settings.</param>
    public void Run(FilePath solution, CleanupCodeSettings settings)
    {
        if (solution == null)
        {
            throw new ArgumentNullException(nameof(solution));
        }

        if (settings == null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        Run(settings, GetArguments(settings, solution));
    }

    /// <summary>
    /// Runs ReSharper's CleanupCode using the provided config file.
    /// </summary>
    /// <param name="configFile">The config file.</param>
    public void RunFromConfig(FilePath configFile)
    {
        if (configFile == null)
        {
            throw new ArgumentNullException(nameof(configFile));
        }

        Run(new CleanupCodeSettings(), GetConfigArgument(configFile));
    }

    /// <summary>
    /// Gets the name of the tool.
    /// </summary>
    /// <returns>The name of the tool.</returns>
    protected override string GetToolName() => nameof(CleanupCode);

    /// <summary>
    /// Gets the possible names of the tool executable.
    /// </summary>
    /// <returns>The tool executable name.</returns>
    protected override IEnumerable<string> GetToolExecutableNames() => GetToolExecutableNames(settings: null);

    /// <summary>
    /// Gets the possible names of the tool executable.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <returns>The tool executable name.</returns>
    protected override IEnumerable<string> GetToolExecutableNames(CleanupCodeSettings? settings)
    {
        if (!Environment.Platform.IsWindows())
        {
            return new[] { "./cleanupcode.sh" };
        }

        return new[] { settings?.UseX86Tool == true ? "cleanupcode.x86.exe" : "cleanupcode.exe" };
    }

    private ProcessArgumentBuilder GetArguments(CleanupCodeSettings settings, FilePath solution)
    {
        var builder = new ProcessArgumentBuilder();

        if (FillArguments(builder, settings))
        {
            return builder;
        }

        if (settings.SettingsPath != null)
        {
            builder.AppendSwitchQuoted("--settings", "=", settings.SettingsPath.MakeAbsolute(Environment).FullPath);
        }

        builder.AppendQuoted(solution.MakeAbsolute(Environment).FullPath);

        return builder;
    }
}
