using System.Text.Json.Nodes;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.ReSharper.GlobalTools.InspectCode.ResultAnalyzers;

internal sealed class ReSharperInspectCodeJsonReportAnalyzer
    : ReSharperInspectCodeReportAnalyzerBase
{
    internal ReSharperInspectCodeJsonReportAnalyzer(
        ICakeLog log,
        IFileSystem fileSystem)
        : base(log, fileSystem)
    {
    }

    public override void AnalyzeResults(string resultsPath, bool throwOnViolations)
    {
        var anyFailures = false;

        var outputDirectoryPath = DirectoryPath.FromString(resultsPath);
        var outputDirectory = FileSystem.GetDirectory(outputDirectoryPath);
        if (!outputDirectory.Exists)
        {
            return;
        }

        foreach (var file in outputDirectory.GetFiles("*.json", SearchScope.Current))
        {
            using var stream = file.OpenRead();
            var jsonRootNode = JsonNode.Parse(stream);

            var problems = jsonRootNode?["problems"]?.AsArray();
            if (problems is null)
            {
                return;
            }

            Log.Warning("Code Inspection Error(s) Located.");

            foreach (var problem in problems)
            {
                anyFailures |= AnalyzeProblem(problem);
            }
        }

        if (anyFailures && throwOnViolations)
        {
            throw new CakeException("Code Inspection Violations found in code base.");
        }
    }

    internal bool AnalyzeProblem(JsonNode? problemNode)
    {
        if (problemNode is null)
        {
            return false;
        }

        var problemObj = problemNode.AsObject();
        var problemClass = problemObj["problem_class"]?.AsObject();
        if (problemClass is null)
        {
            return false;
        }

        var severity = problemClass["severity"]?.GetValue<string>();
        if (!string.Equals(severity, "ERROR", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var filePath = problemObj["file"]?.GetValue<string>();
        if (!string.IsNullOrEmpty(filePath))
        {
            filePath = filePath
                .Replace(@"$PROJECT_DIR$\\", string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        var line = problemObj["line"]?.GetValue<string>();
        var message = problemObj["description"]?.GetValue<string>();
        Log.Warning("File Name: {0} Line Number: {1} Message: {2}", filePath, line, message);
        return true;
    }
}
