using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;
using Cake.ReSharper.GlobalTools.InspectCode;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .InstallTool(new Uri("nuget:?package=JetBrains.ReSharper.GlobalTools&version=2022.1.1"))
            .Run(args);
    }
}

public class BuildContext : FrostingContext
{
    public FilePath ProjectFile { get; set; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        ProjectFile = "../../sample/Sample.csproj";
    }
}

[TaskName("TestInspectCodeDefault")]
public sealed class TestInspectCodeDefaultTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.ReSharperInspectCode(context.ProjectFile);
    }
}

[TaskName("TestInspectCodeOutputXml")]
public sealed class TestInspectCodeOutputXmlTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.ReSharperInspectCode(
            context.ProjectFile,
            new ReSharperInspectCodeSettings
            {
                OutputFileFormat = ReSharperInspectCodeReportFormat.Xml,
                OutputFile = "./build/inspect-code2.xml",
                HandleExitCode = val => true,
            });
    }
}

[TaskName("TestInspectCodeOutputHtml")]
public sealed class TestInspectCodeOutputHtmlTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.ReSharperInspectCode(
            context.ProjectFile,
            new ReSharperInspectCodeSettings
            {
                OutputFileFormat = ReSharperInspectCodeReportFormat.Html,
                OutputFile = "./build/inspect-code.html",
                HandleExitCode = val => true,
            });
    }
}


[TaskName("TestInspectCode")]
[IsDependentOn(typeof(TestInspectCodeDefaultTask))]
//[IsDependentOn(typeof(TestInspectCodeOutputHtmlTask))]
public sealed class TestInspectCodeTask : FrostingTask<BuildContext>
{
}


[TaskName("Default")]
[IsDependentOn(typeof(TestInspectCodeTask))]
public class DefaultTask : FrostingTask
{
}
