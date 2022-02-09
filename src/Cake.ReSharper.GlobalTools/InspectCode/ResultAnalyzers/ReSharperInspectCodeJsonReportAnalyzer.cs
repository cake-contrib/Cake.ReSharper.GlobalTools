using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.ReSharper.GlobalTools.InspectCode.ResultAnalyzers;

public class ReSharperInspectCodeJsonReportAnalyzer
    : ReSharperInspectCodeReportAnalyzerBase
{
    public ReSharperInspectCodeJsonReportAnalyzer(
        ICakeLog log,
        IFileSystem fileSystem)
        : base(log, fileSystem)
    {
    }

    public override void AnalyzeResultsFile(FilePath resultsFilePath, bool throwOnViolations)
    {
        throw new NotSupportedException();
    }
}
