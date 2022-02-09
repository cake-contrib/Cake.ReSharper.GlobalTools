using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.ReSharper.GlobalTools.InspectCode.ResultAnalyzers;

public class ReSharperInspectCodeHtmlReportAnalyzer
    : ReSharperInspectCodeReportAnalyzerBase
{
    private readonly Regex _reSharperHtmlReportRegex;

    public ReSharperInspectCodeHtmlReportAnalyzer(
        ICakeLog log,
        IFileSystem fileSystem)
        : base(log, fileSystem)
    {
        _reSharperHtmlReportRegex =
            new Regex(
                ".*[<]dt[>](?<file>[^:]+)[:][<][^>]+[>](?<line>\\d+).*\\n\\s+[<]dd[>](?<message>.*(?=[<][/]dd))",
                RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline,
                TimeSpan.FromSeconds(30));
    }

    public override void AnalyzeResultsFile(FilePath resultsFilePath, bool throwOnViolations)
    {
        var anyFailures = false;
        var resultsFile = FileSystem.GetFile(resultsFilePath);

        using (var stream = resultsFile.OpenRead())
        using (var reader = new StreamReader(stream))
        {
            var text = reader.ReadToEnd();
            var matches = _reSharperHtmlReportRegex.Matches(text);
            if (matches.Count > 0)
            {
                anyFailures = true;
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
