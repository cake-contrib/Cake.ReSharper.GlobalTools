// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.ReSharper.GlobalTools.InspectCode;

internal interface IReSharperInspectCodeReportAnalyzer
{
    void AnalyzeResults(string resultsPath, bool throwOnViolations);
}
