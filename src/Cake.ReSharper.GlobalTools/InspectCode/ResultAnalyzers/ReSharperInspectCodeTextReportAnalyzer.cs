using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.ReSharper.GlobalTools.InspectCode.ResultAnalyzers;

public class ReSharperInspectCodeTextReportAnalyzer
    : ReSharperInspectCodeReportAnalyzerBase
{
    public ReSharperInspectCodeTextReportAnalyzer(
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
