#tool "nuget:?package=JetBrains.ReSharper.GlobalTools&version=2022.1.1"
///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
var target = Argument<string>("target", "Default");
var projectFile = "../sample/Sample.csproj";


///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Test-Inspect-Code-Default")
    .Does(() =>
    {
        ReSharperInspectCode(projectFile);
    });

Task("Test-Inspect-Code-Output-Html")
    .Does(() =>
    {
        ReSharperInspectCode(
            projectFile,
            new ReSharperInspectCodeSettings
            {
                OutputFileFormat = ReSharperInspectCodeReportFormat.Html,
                OutputFile = "./build/inspect-code.html",
                HandleExitCode = val => true,
            });
    });

Task("Test-Inspect-Code-Output-Json")
    .Does(() =>
    {
        ReSharperInspectCode(
            projectFile,
            new ReSharperInspectCodeSettings
            {
                OutputFileFormat = ReSharperInspectCodeReportFormat.Json,
                OutputFile = "./build/inspect-code-json",
            });
    });

Task("Test-Inspect-Code-Output-Text")
    .Does(() =>
    {
        ReSharperInspectCode(
            projectFile,
            new ReSharperInspectCodeSettings
            {
                OutputFileFormat = ReSharperInspectCodeReportFormat.Text,
                OutputFile = "./build/inspect-code.txt",
            });
    });


Task("Test-Inspect-Code")
    .IsDependentOn("Test-Inspect-Code-Default")
    .IsDependentOn("Test-Inspect-Code-Output-Html")
    .IsDependentOn("Test-Inspect-Code-Output-Json")
    .IsDependentOn("Test-Inspect-Code-Output-Text");

Task("Default")
    .IsDependentOn("Test-Inspect-Code");


///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////
RunTarget(target);