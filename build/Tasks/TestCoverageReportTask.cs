using Build.Utils;
using Cake.Common.Tools.ReportGenerator;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build.Tasks;

[TaskName("TestCoverageReport")]
public class TestCoverageReportTask
    : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var unitReportPath = new DirectoryPath(Constants.BackendUnitTestPath).MakeAbsolute(context.Environment);

        GenerateReport(context, unitReportPath, ReportGeneratorReportType.HtmlInline_AzurePipelines);
    }

    private void GenerateReport(
        BuildContext context,
        DirectoryPath unitReportPath,
        ReportGeneratorReportType reportType)
    {
        var outputPath = unitReportPath.Combine(new DirectoryPath(reportType.ToString("G")));
        var reportGeneratorSettings = new ReportGeneratorSettings
        {
           ReportTypes = new[] { reportType },
        };

        context.ReportGenerator(
            new GlobPattern($"{unitReportPath.FullPath}/**/{Constants.CoverageCoberturaFileName}"),
            outputPath,
            reportGeneratorSettings);
    }
}
