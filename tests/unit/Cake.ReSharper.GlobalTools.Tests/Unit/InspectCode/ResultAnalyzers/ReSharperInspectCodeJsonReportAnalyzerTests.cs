using Cake.ReSharper.GlobalTools.InspectCode.ResultAnalyzers;
using Cake.Testing;
using Xunit;

namespace Cake.ReSharper.GlobalTools.Tests.Unit.InspectCode.ResultAnalyzers;

public class ReSharperInspectCodeJsonReportAnalyzerTests
{
    private readonly ReSharperInspectCodeJsonReportAnalyzer _sut;
    private readonly FakeLog _fakeLog;
    private readonly FakeFileSystem _fakeFileSystem;

    public ReSharperInspectCodeJsonReportAnalyzerTests()
    {
        _fakeLog = new FakeLog();
        var fakeEnvironment = FakeEnvironment.CreateUnixEnvironment();
        _fakeFileSystem = new FakeFileSystem(fakeEnvironment);
        _sut = new ReSharperInspectCodeJsonReportAnalyzer(
            _fakeLog,
            _fakeFileSystem);
    }

    [Fact]
    public void Should_Not_Throw_If_Result_File_Not_Found()
    {
        // Given
        var resultFileName = "./non-existent-result.json";
        var shouldThrowException = true;

        // When
        var result = Record.Exception(() =>
            _sut.AnalyzeResults(resultFileName, shouldThrowException));

        // Then
        Assert.Null(result);
    }

    [Fact]
    public void Should_Process_Json_Violation_File()
    {
        // Given
        var reportFolder = "./reports";
        var resultFileName = $"{reportFolder}/resharper_inspect_code_violations.json";
        var shouldThrowException = true;
        _fakeFileSystem.CreateDirectory("reports");
        _fakeFileSystem.CreateFile(resultFileName)
            .SetContent(Resources.ReSharperInspectCodeJsonReport1WithViolations);

        // When
        var result = Record.Exception(() =>
            _sut.AnalyzeResults(reportFolder, shouldThrowException));

        // Then
        AssertEx.IsCakeException(result, "Code Inspection Violations found in code base.");
        Assert.NotEmpty(_fakeLog.Entries);
        Assert.Contains(
            "Code Inspection Error(s) Located.",
            _fakeLog.Entries[0].Message,
            StringComparison.OrdinalIgnoreCase);
    }
}
