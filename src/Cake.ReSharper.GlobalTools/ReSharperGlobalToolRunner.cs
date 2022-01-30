// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.ReSharper.GlobalTools;

public abstract class ReSharperGlobalToolRunner<TSettings>
    : Tool<TSettings>
    where TSettings : ToolSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReSharperGlobalToolRunner{TSettings}"/> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="processRunner">The process runner.</param>
    /// <param name="tools">The tool locator.</param>
    /// <param name="log">The logger.</param>
    protected ReSharperGlobalToolRunner(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        IProcessRunner processRunner,
        IToolLocator tools,
        ICakeLog log)
        : base(fileSystem, environment, processRunner, tools)
    {
        Environment = environment;
        FileSystem = fileSystem;
        Log = log;
    }

    protected ICakeEnvironment Environment { get; }

    protected IFileSystem FileSystem { get; }

    protected ICakeLog Log { get; }

#pragma warning disable MA0051 // Method is too long
    protected bool FillArguments(
        ProcessArgumentBuilder builder,
        ReSharperSettings settings)
#pragma warning restore MA0051 // Method is too long
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        if (settings.Help)
        {
            builder.Append("--help");
            return true;
        }

        if (settings.Version)
        {
            builder.Append("--version");
            return true;
        }

        if (settings.ConfigCreateFile != null)
        {
            builder.AppendSwitchQuoted(
                "--config-create",
                "=",
                settings.ConfigCreateFile.MakeAbsolute(Environment).FullPath);
            return true;
        }

        if (settings.CachesHome != null)
        {
            builder.AppendSwitchQuoted(
                "--caches-home",
                "=",
                settings.CachesHome.MakeAbsolute(Environment).FullPath);
        }

        if (settings.ConfigFile != null)
        {
            builder.AppendSwitchQuoted(
                "--config",
                "=",
                settings.ConfigFile.MakeAbsolute(Environment).FullPath);
        }

        if (settings.DisabledSettingsLayers?.Any() == true)
        {
            var layers = settings.DisabledSettingsLayers
                .Select(s => s.ToString("G"));
            builder.AppendSwitch(
                "--disable-settings-layers",
                "=",
                string.Join(";", layers));
        }

        if (settings.Debug)
        {
            builder.Append("--debug");
        }

        if (settings.DotNetCorePath != null)
        {
            builder.AppendSwitchQuoted(
                "--dotnetcore",
                "=",
                settings.DotNetCorePath);
        }

        if (!string.IsNullOrEmpty(settings.DotNetCoreSdk))
        {
            builder.AppendSwitch(
                "--dotnetcoresdk",
                "=",
                settings.DotNetCoreSdk);
        }

        if (settings.Exclude != null)
        {
            builder.AppendSwitchQuoted(
                "--exclude",
                "=",
                string.Join(";", settings.Exclude));
        }

        if (settings.Extensions != null)
        {
            builder.AppendSwitchQuoted(
                "-x",
                "=",
                string.Join(";", settings.Extensions));
        }

        if (settings.ExtensionSources != null)
        {
            builder.AppendSwitchQuoted(
                "--source",
                "=",
                string.Join(";", settings.ExtensionSources));
        }

        if (settings.Include != null)
        {
            builder.AppendSwitchQuoted(
                "--include",
                "=",
                string.Join(";", settings.Include));
        }

        if (settings.MonoPath != null)
        {
            builder.AppendSwitchQuoted(
                "--mono",
                "=",
                settings.MonoPath);
        }

        if (settings.NoBuildInSettings)
        {
            builder.Append("--no-buildin-settings");
        }

        if (settings.Profile != null)
        {
            builder.AppendSwitchQuoted(
                "--profile",
                "=",
                settings.Profile.MakeAbsolute(Environment).FullPath);
        }

        if (settings.MsBuildProperties != null)
        {
            foreach (var property in settings.MsBuildProperties)
            {
                builder.AppendSwitch(
                    "--properties",
                    ":",
                    $"{property.Key}=\"{property.Value}\"");
            }
        }

        if (settings.TargetsForItems != null)
        {
            builder.AppendSwitch(
                "--targets-for-items",
                "=",
                string.Join(";", settings.TargetsForItems));
        }

        if (settings.TargetsForReferences != null)
        {
            builder.AppendSwitch(
                "--targets-for-references",
                "=",
                string.Join(";", settings.TargetsForReferences));
        }

        if (!string.IsNullOrEmpty(settings.Toolset))
        {
            builder.AppendSwitch(
                "--toolset",
                "=",
                settings.Toolset);
        }

        if (settings.ToolsetPath != null)
        {
            builder.AppendSwitchQuoted(
                "--toolset-path",
                "=",
                settings.ToolsetPath.MakeAbsolute(Environment).FullPath);
        }

        if (settings.Verbosity.HasValue)
        {
            builder.AppendSwitch(
                "--verbosity",
                "=",
                settings.Verbosity.Value.ToString("G").ToUpper(CultureInfo.InvariantCulture));
        }

        return false;
    }

    protected ProcessArgumentBuilder GetConfigArgument(FilePath configFile)
    {
        var builder = new ProcessArgumentBuilder();
        builder.AppendSwitchQuoted(
            "--config",
            "=",
            configFile.MakeAbsolute(Environment).FullPath);
        return builder;
    }
}
