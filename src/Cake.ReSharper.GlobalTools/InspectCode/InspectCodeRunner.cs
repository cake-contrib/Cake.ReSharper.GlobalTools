// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.ReSharper.GlobalTools.InspectCode;

public sealed class InspectCodeRunner
    : ReSharperGlobalToolRunner<InspectCodeSettings>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InspectCodeRunner"/> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="processRunner">The process runner.</param>
    /// <param name="tools">The tool locator.</param>
    /// <param name="log">The logger.</param>
    public InspectCodeRunner(
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
    public void Run(FilePath solution, InspectCodeSettings settings)
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

        if (settings.SkipOutputAnalysis ||
            settings.OutputFile == null)
        {
            return;
        }

        AnalyzeResultsFile(settings.OutputFile, settings.ThrowExceptionOnFindingViolations);
    }

    /// <summary>
    /// Runs ReSharper's InspectCode using the provided config file.
    /// </summary>
    /// <param name="configFile">The config file.</param>
    public void RunFromConfig(FilePath configFile)
    {
        if (configFile == null)
        {
            throw new ArgumentNullException(nameof(configFile));
        }

        Run(new InspectCodeSettings(), GetConfigArgument(configFile));
    }

    /// <summary>
    /// Gets the name of the tool.
    /// </summary>
    /// <returns>The name of the tool.</returns>
    protected override string GetToolName() => nameof(InspectCode);

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
    protected override IEnumerable<string> GetToolExecutableNames(InspectCodeSettings? settings)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new[] { "./inspectcode.sh" };
        }

        return new[] { settings?.UseX86Tool == true ? "inspectcode.x86.exe" : "inspectcode.exe" };
    }

    private void AnalyzeResultsFile(FilePath resultsFilePath, bool throwOnViolations)
    {
        var anyFailures = false;
        var resultsFile = FileSystem.GetFile(resultsFilePath);

        using (var stream = resultsFile.OpenRead())
        {
            var xmlDoc = XDocument.Load(stream);
            var violations = xmlDoc.Descendants("IssueType")
                .Where(i => string.Equals(
                    i.Attribute("Severity")?.Value,
                    "ERROR",
                    StringComparison.OrdinalIgnoreCase));

            foreach (var violation in violations)
            {
                Log.Warning("Code Inspection Error(s) Located. Description: {0}", violation.Attribute("Description")?.Value);

                var tempViolation = violation; // Copy to temporary variable to avoid side effects.
                var issueLookups = xmlDoc.Descendants("Issue")
                    .Where(i => string.Equals(
                        i.Attribute("TypeId")?.Value,
                        tempViolation.Attribute("Id")?.Value,
                        StringComparison.OrdinalIgnoreCase));

                foreach (var issueLookup in issueLookups)
                {
                    var file = issueLookup.Attribute("File")?.Value ?? string.Empty;
                    var line = issueLookup.Attribute("Line")?.Value ?? string.Empty;
                    var message = issueLookup.Attribute("Message")?.Value ?? string.Empty;

                    Log.Warning("File Name: {0} Line Number: {1} Message: {2}", file, line, message);
                }

                anyFailures = true;
            }
        }

        if (anyFailures && throwOnViolations)
        {
            throw new CakeException("Code Inspection Violations found in code base.");
        }
    }

#pragma warning disable MA0051 // Method is too long
    private ProcessArgumentBuilder GetArguments(InspectCodeSettings settings, FilePath solution)
#pragma warning restore MA0051 // Method is too long
    {
        var builder = new ProcessArgumentBuilder();

        if (FillArguments(builder, settings))
        {
            return builder;
        }

        builder.Append(settings.Build ? "--build" : "--no-build");

        if (settings.DumpIssueTypes)
        {
            builder.Append("--dumpIssuesTypes");
        }

        if (settings.Measure != InspectCodeMeasure.None)
        {
            builder.AppendSwitch(
                "--severity",
                "=",
                settings.Measure.ToString("G").ToUpper(CultureInfo.InvariantCulture));
        }

        if (settings.OutputFile != null)
        {
            builder.AppendSwitchQuoted(
                "--output",
                "=",
                settings.OutputFile.MakeAbsolute(Environment).FullPath);
        }

        if (settings.OutputFileFormat != InspectCodeReportFormat.Xml)
        {
            builder.AppendSwitch(
                "--severity",
                "=",
                settings.OutputFileFormat.ToString("G").ToUpper(CultureInfo.InvariantCulture));
        }

        if (settings.ParallelJobs > 0)
        {
            builder.AppendSwitch(
                "--jobs",
                "=",
                settings.ParallelJobs.ToString(CultureInfo.InvariantCulture));
        }

        if (settings.ProjectFilter != null)
        {
            builder.AppendSwitchQuoted("--project", "=", settings.ProjectFilter);
        }

        if (settings.Severity.HasValue)
        {
            builder.AppendSwitch(
                "--severity",
                "=",
                settings.Severity.Value.ToString("G").ToUpper(CultureInfo.InvariantCulture));
        }

        if (settings.SolutionWideAnalysis == true)
        {
            builder.Append("--swea");
        }
        else if (settings.SolutionWideAnalysis == false)
        {
            builder.Append("--no-swea");
        }

        if (!string.IsNullOrEmpty(settings.Target))
        {
            builder.AppendSwitch(
                "--target",
                "=",
                settings.Target);
        }

        if (settings.UseAbsolutePaths)
        {
            builder.Append("--absolute-paths");
        }

        builder.AppendQuoted(solution.MakeAbsolute(Environment).FullPath);

        return builder;
    }
}
