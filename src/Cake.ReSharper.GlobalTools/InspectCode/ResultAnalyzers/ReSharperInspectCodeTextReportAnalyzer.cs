using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.ReSharper.GlobalTools.InspectCode.ResultAnalyzers;

internal sealed class ReSharperInspectCodeTextReportAnalyzer
    : ReSharperInspectCodeReportAnalyzerBase
{
    private readonly Regex _reSharperTextReportRegex;

    internal ReSharperInspectCodeTextReportAnalyzer(
        ICakeLog log,
        IFileSystem fileSystem)
        : base(log, fileSystem)
    {
        _reSharperTextReportRegex = new Regex(
            ".*(?<file>[^:]+)[:](?<line>\\d+)\\s(?<message>.*(?=\\r?\\n))",
            RegexOptions.Compiled,
            TimeSpan.FromSeconds(30));
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
        using (var reader = new StreamReader(stream))
        {
            var text = reader.ReadToEnd();
            var matches = _reSharperTextReportRegex.Matches(text);
            if (matches.Count > 0)
            {
                anyFailures = true;
                Log.Warning("Code Inspection Error(s) Located.");
            }

            foreach (Match? match in matches)
            {
                if (match == null)
                {
                    continue;
                }

                var file = match.Groups["file"].Value;
                var line = match.Groups["line"].Value;
                var message = match.Groups["message"].Value;

                Log.Warning("File Name: {0} Line Number: {1} Message: {2}", file, line, message);
            }
        }

        if (anyFailures && throwOnViolations)
        {
            throw new CakeException("Code Inspection Violations found in code base.");
        }
    }
}
