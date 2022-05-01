// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Xml.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.ReSharper.GlobalTools.InspectCode.ResultAnalyzers;

internal sealed class ReSharperInspectCodeXmlReportAnalyzer
    : ReSharperInspectCodeReportAnalyzerBase
{
    internal ReSharperInspectCodeXmlReportAnalyzer(
        ICakeLog log,
        IFileSystem fileSystem)
        : base(log, fileSystem)
    {
    }

    public override void AnalyzeResults(string resultsPath, bool throwOnViolations)
    {
        var anyFailures = false;
        var resultsFile = FileSystem.GetFile(resultsPath);
        if (!resultsFile.Exists)
        {
            return;
        }

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
}
