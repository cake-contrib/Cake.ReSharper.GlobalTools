// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.ReSharper.GlobalTools.InspectCode.ResultAnalyzers;

public abstract class ReSharperInspectCodeReportAnalyzerBase
    : IReSharperInspectCodeReportAnalyzer
{
    protected ReSharperInspectCodeReportAnalyzerBase(
        ICakeLog log,
        IFileSystem fileSystem)
    {
        Log = log ?? throw new ArgumentNullException(nameof(log));
        FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public abstract void AnalyzeResultsFile(FilePath resultsFilePath, bool throwOnViolations);

    protected ICakeLog Log { get; }

    protected IFileSystem FileSystem { get; }
}
