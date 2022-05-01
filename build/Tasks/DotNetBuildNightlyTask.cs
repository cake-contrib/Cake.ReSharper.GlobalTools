using Cake.Frosting;

namespace Build.Tasks;

[TaskName("DotNetBuildNightly")]
[IsDependentOn(typeof(CleanTask))]
[IsDependentOn(typeof(CreateFoldersTask))]
[IsDependentOn(typeof(DotNetRestoreTask))]

// [IsDependentOn(typeof(SonarBeginTask))]
[IsDependentOn(typeof(DotNetBuildTask))]
[IsDependentOn(typeof(UnitTestTask))]
[IsDependentOn(typeof(IntegrationTestTask))]

// [IsDependentOn(typeof(SonarEndTask))]
[IsDependentOn(typeof(UnitTestReportTask))]
[IsDependentOn(typeof(IntegrationTestReportTask))]
[IsDependentOn(typeof(TestCoverageReportTask))]

// [IsDependentOn(typeof(ReSharperInspectCodeTask))]
// [IsDependentOn(typeof(ReSharperInspectCodeReportTask))]
[IsDependentOn(typeof(BuildReportTask))]
[IsDependentOn(typeof(KillDotNetBuildServerTask))]
public class DotNetBuildNightlyTask
    : FrostingTask<BuildContext>
{
}
