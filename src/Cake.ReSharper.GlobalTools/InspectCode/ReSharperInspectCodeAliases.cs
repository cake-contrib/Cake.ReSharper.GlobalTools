// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.ReSharper.GlobalTools.InspectCode;

/// <summary>
/// <para>Contains functionality related to ReSharper's <see href="https://www.jetbrains.com/help/resharper/InspectCode.html">InspectCode</see> tool.</para>
/// <para>
/// In order to use the commands for this alias, include the following in your build.cake file to download and
/// install from nuget.org, or specify the ToolPath within the <see cref="ReSharperInspectCodeSettings" /> class:
/// <code>
/// #tool "nuget:?package=JetBrains.ReSharper.GlobalTools"
/// </code>
/// </para>
/// </summary>
[CakeAliasCategory(nameof(ReSharper))]
[CakeNamespaceImport("Cake.ReSharper.GlobalTools.InspectCode")]
[CakeNamespaceImport("Cake.Core.IO")]
public static class ReSharperInspectCodeAliases
{
    /// <summary>
    /// Analyzes the specified solution with ReSharper's InspectCode.
    /// Stores result in file ./build/inspect-code.xml.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="solution">The solution.</param>
    /// <example>
    /// <code>
    /// ReSharperInspectCode("./src/MySolution.sln");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory(nameof(ReSharperInspectCode))]
    public static void ReSharperInspectCode(
        this ICakeContext context,
        FilePath solution)
    {
        ReSharperInspectCode(
            context,
            solution,
            new ReSharperInspectCodeSettings
            {
                OutputFile = "./build/inspect-code.xml",
            });
    }

    /// <summary>
    /// Analyzes the specified solution with ReSharper's InspectCode,
    /// using the specified settings.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="solution">The solution.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// var buildOutputDirectory = Directory("./.build");
    /// var resharperReportsDirectory = buildOutputDirectory + Directory("_ReSharperReports");
    ///
    /// var msBuildProperties = new Dictionary&lt;string, string&gt;();
    /// msBuildProperties.Add("configuration", configuration);
    /// msBuildProperties.Add("platform", "AnyCPU");
    ///
    /// ReSharperInspectCode("./MySolution.sln", new InspectCodeSettings {
    ///     SolutionWideAnalysis = true,
    ///     Profile = "./MySolution.sln.DotSettings",
    ///     MsBuildProperties = msBuildProperties,
    ///     OutputFile = resharperReportsDirectory + File("inspectcode-output.xml"),
    ///     ThrowExceptionOnFindingViolations = true
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory(nameof(ReSharperInspectCode))]
    public static void ReSharperInspectCode(
        this ICakeContext context,
        FilePath solution,
        ReSharperInspectCodeSettings settings)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var runner = new ReSharperInspectCodeRunner(
            context.FileSystem,
            context.Environment,
            context.ProcessRunner,
            context.Tools,
            context.Log);
        runner.Run(solution, settings);
    }

    /// <summary>
    /// Runs ReSharper's InspectCode using the specified config file.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="configFile">The config file.</param>
    /// <example>
    /// <code>
    /// ReSharperInspectCodeFromConfig("./src/inspectcode.config");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory(nameof(ReSharperInspectCode))]
    public static void ReSharperInspectCodeFromConfig(
        this ICakeContext context,
        FilePath configFile)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var runner = new ReSharperInspectCodeRunner(
            context.FileSystem,
            context.Environment,
            context.ProcessRunner,
            context.Tools,
            context.Log);
        runner.RunFromConfig(configFile);
    }
}
