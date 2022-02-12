using Cake.ReSharper.GlobalTools.InspectCode.ResultAnalyzers;
using Cake.Testing;
using Xunit;

namespace Cake.ReSharper.GlobalTools.Tests.Unit.InspectCode.ResultAnalyzers;

public class ReSharperInspectCodeXmlReportAnalyzerTests
{
    private readonly ReSharperInspectCodeXmlReportAnalyzer _sut;
    private readonly FakeLog _fakeLog;
    private readonly FakeFileSystem _fakeFileSystem;
    public ReSharperInspectCodeXmlReportAnalyzerTests()
    {
        _fakeLog = new FakeLog();
        var fakeEnvironment = FakeEnvironment.CreateUnixEnvironment();
        _fakeFileSystem = new FakeFileSystem(fakeEnvironment);
        _sut = new ReSharperInspectCodeXmlReportAnalyzer(
            _fakeLog,
            _fakeFileSystem);
    }

    [Fact]
    public void Should_Not_Throw_If_Result_File_Not_Found()
    {
        // Given
        var resultFileName = "./non-existent-result.xml";
        var shouldThrowException = true;

        // When
        var result = Record.Exception(() =>
            _sut.AnalyzeResults(resultFileName, shouldThrowException));

        // Then
        Assert.Null(result);
    }
}
