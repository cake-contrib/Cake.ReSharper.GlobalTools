using Cake.ReSharper.GlobalTools.InspectCode.ResultAnalyzers;
using Cake.Testing;
using Xunit;

namespace Cake.ReSharper.GlobalTools.Tests.Unit.InspectCode.ResultAnalyzers;

public class ReSharperInspectCodeHtmlReportAnalyzerTests
{
    private readonly ReSharperInspectCodeHtmlReportAnalyzer _sut;
    private readonly FakeLog _fakeLog;
    private readonly FakeFileSystem _fakeFileSystem;

    public ReSharperInspectCodeHtmlReportAnalyzerTests()
    {
        _fakeLog = new FakeLog();
        var fakeEnvironment = FakeEnvironment.CreateUnixEnvironment();
        _fakeFileSystem = new FakeFileSystem(fakeEnvironment);
        _sut = new ReSharperInspectCodeHtmlReportAnalyzer(
            _fakeLog,
            _fakeFileSystem);
    }

    [Fact]
    public void Should_Not_Throw_If_Result_File_Not_Found()
    {
        // Given
        var resultFileName = "./non-existent-result.html";
        var shouldThrowException = true;

        // When
        var result = Record.Exception(() =>
            _sut.AnalyzeResults(resultFileName, shouldThrowException));

        // Then
        Assert.Null(result);
    }

    [Fact]
    public void Should_Process_Html_Violation_File()
    {
        // Given
        var resultFileName = "./resharper_inspect_code_violations.html";
        var shouldThrowException = true;
        _fakeFileSystem.CreateFile(resultFileName)
            .SetContent(Resources.ReSharperInspectCodeHtmlReportWithViolations);

        // When
        var result = Record.Exception(() =>
            _sut.AnalyzeResults(resultFileName, shouldThrowException));

        // Then
        AssertEx.IsCakeException(result, "Code Inspection Violations found in code base.");
        Assert.NotEmpty(_fakeLog.Entries);
        Assert.Contains(
            "Code Inspection Error(s) Located.",
            _fakeLog.Entries[0].Message,
            StringComparison.OrdinalIgnoreCase);
    }
}
