// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.ReSharper.GlobalTools.InspectCode;

public interface IReSharperInspectCodeReportAnalyzer
{
    void AnalyzeResultsFile(FilePath resultsFilePath, bool throwOnViolations);
}
