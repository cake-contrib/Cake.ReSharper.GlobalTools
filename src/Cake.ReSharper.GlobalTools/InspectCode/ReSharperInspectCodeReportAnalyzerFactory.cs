// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.ReSharper.GlobalTools.InspectCode.ResultAnalyzers;

namespace Cake.ReSharper.GlobalTools.InspectCode;

internal static class ReSharperInspectCodeReportAnalyzerFactory
{
    internal static IReSharperInspectCodeReportAnalyzer Create(
        ReSharperInspectCodeReportFormat format,
        ICakeLog log,
        IFileSystem fileSystem) =>
        format switch
        {
            ReSharperInspectCodeReportFormat.Html =>
                new ReSharperInspectCodeHtmlReportAnalyzer(log, fileSystem),
            ReSharperInspectCodeReportFormat.Json =>
                new ReSharperInspectCodeJsonReportAnalyzer(log, fileSystem),
            ReSharperInspectCodeReportFormat.Text =>
                new ReSharperInspectCodeTextReportAnalyzer(log, fileSystem),
            ReSharperInspectCodeReportFormat.Xml =>
                new ReSharperInspectCodeXmlReportAnalyzer(log, fileSystem),
            _ => throw new NotSupportedException(),
        };
}
