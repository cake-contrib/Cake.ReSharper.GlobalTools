// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.ReSharper.GlobalTools.InspectCode;

public sealed class InspectCodeSettings
    : ReSharperSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether to build or not-build the
    /// sources before running the tool. Setting this value is only valid
    /// for InspectCode version 2021.2.0 and later.
    /// <para>
    /// <list type="bullet">
    /// <item><description>
    /// Setting this property to <c>true</c> will result in passing the '--build' option.
    /// </description></item>
    /// <item><description>
    /// Setting this property to <c>false</c> will result in passing the '--no-build' option.
    /// </description></item>
    /// </list>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Starting from version 2021.2, InspectCode builds the target solution before starting the analysis
    /// to make sure it only finds relevant code issues.
    /// To explicitly accept the new behavior and suppress this warning, use the '--build' option.
    /// To match the behavior in previous versions and skip the build, use '--no-build'.
    /// </remarks>
    public bool Build { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether dump issues types (default: False).
    /// </summary>
    public bool DumpIssueTypes { get; set; }

    /// <summary>
    /// Gets or sets <see cref="InspectCodeMeasure">measure</see> indicating if tool should
    /// measure its performance [NONE, MEMORY, SAMPLING, TIMELINE].
    /// </summary>
    public InspectCodeMeasure Measure { get; set; }

    /// <summary>
    /// Gets or sets the location InspectCode should write its output.
    /// </summary>
    /// <value>The location that InspectCode should write its output.</value>
    public FilePath? OutputFile { get; set; }

    /// <summary>
    /// Gets or sets <see cref="InspectCodeReportFormat">format</see> in which write
    /// inspections report [Xml, Html, Text, Json] (default: Xml).
    /// </summary>
    public InspectCodeReportFormat OutputFileFormat { get; set; }

    /// <summary>
    /// Gets or sets value indicating how many jobs should be run in parallel. 0 means as many as possible (default: 0).
    /// </summary>
    public int ParallelJobs { get; set; }

    /// <summary>
    /// Gets or sets a filter to analyze only particular project(s) instead of the whole solution.
    /// Supports wildcards.
    /// </summary>
    /// <value>The filter to analyze only particular projects(s).</value>
    public string? ProjectFilter { get; set; }

    /// <summary>
    /// Gets or sets the minimal <see cref="InspectCodeSeverity">severity</see> of issues to report.
    /// </summary>
    public InspectCodeSeverity? Severity { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to skip analysis of the file
    /// that was output by the command line tool or not.
    /// </summary>
    public bool SkipOutputAnalysis { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether enable solution-wide analysis should be forced.
    /// <para>
    /// <list type="bullet">
    /// <item><description>
    /// Setting this property to <c>null</c> will result in no changes to the options. This is the default setting.
    /// </description></item>
    /// <item><description>
    /// Setting this property to <c>true</c> will result in passing the '--swea' option.
    /// </description></item>
    /// <item><description>
    /// Setting this property to <c>false</c> will result in passing the '--no-swea' option.
    /// </description></item>
    /// </list>
    /// </para>
    /// </summary>
    /// <value>
    ///     <c>null</c> if solution-wide analysis should be neither enabled nor disabled by force;
    ///     <c>true</c> if solution-wide analysis should be enabled by force;
    ///     <c>false</c> if solution-wide analysis should be disabled by force.
    /// </value>
    public bool? SolutionWideAnalysis { get; set; }

    /// <summary>
    /// Gets or sets MsBuild target to execute before processing. (default: Build).
    /// </summary>
    public string? Target { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to throw an exception on finding violations.
    /// </summary>
    public bool ThrowExceptionOnFindingViolations { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether use absolute paths in inspections report (default: False).
    /// </summary>
    public bool UseAbsolutePaths { get; set; }
}
