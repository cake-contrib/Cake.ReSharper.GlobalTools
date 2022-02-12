// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.ReSharper.GlobalTools.InspectCode;
using Cake.Testing;
using NSubstitute;

namespace Cake.ReSharper.GlobalTools.Tests.Fixtures.InspectCode;

internal sealed class ReSharperInspectCodeRunFixture
    : ReSharperInspectCodeFixture
{
    public ReSharperInspectCodeRunFixture(
        bool isWindows = false,
        bool useX86 = false)
        : base(isWindows, useX86)
    {
        Solution = new FilePath("./Test.sln");

        Log = Substitute.For<ICakeLog>();

        FileSystem.CreateFile("build/resharper_inspect_code.xml")
            .SetContent(Resources.ReSharperInspectCodeXmlReportNoViolations.NormalizeLineEndings());
        FileSystem.CreateFile("build/resharper_inspect_code_violations.xml")
            .SetContent(Resources.ReSharperInspectCodeXmlReportWithViolations.NormalizeLineEndings());
        FileSystem.CreateFile("build/resharper_inspect_code_violations.txt")
            .SetContent(Resources.ReSharperInspectCodeTextReportWithViolations.NormalizeLineEndings());
        FileSystem.CreateFile("build/resharper_inspect_code_violations.html")
            .SetContent(Resources.ReSharperInspectCodeHtmlReportWithViolations.NormalizeLineEndings());
        FileSystem.CreateDirectory("build/reports");
        FileSystem.CreateFile("build/reports/resharper_inspect_code_violations1.json")
            .SetContent(Resources.ReSharperInspectCodeJsonReport1WithViolations.NormalizeLineEndings());
        FileSystem.CreateFile("build/reports/resharper_inspect_code_violations2.json")
            .SetContent(Resources.ReSharperInspectCodeJsonReport2WithViolations.NormalizeLineEndings());
        FileSystem.CreateFile("build/reports/resharper_inspect_code_violations3.json")
            .SetContent(Resources.ReSharperInspectCodeJsonReport3WithViolations.NormalizeLineEndings());
        FileSystem.CreateFile("build/reports/resharper_inspect_code_violations4.json")
            .SetContent(Resources.ReSharperInspectCodeJsonReport4WithViolations.NormalizeLineEndings());
    }

    public ICakeLog Log { get; set; }

    public FilePath? Solution { get; set; }

    protected override void RunTool()
    {
        var tool = new ReSharperInspectCodeRunner(FileSystem, Environment, ProcessRunner, Tools, Log);
        tool.Run(Solution!, Settings);
    }
}
