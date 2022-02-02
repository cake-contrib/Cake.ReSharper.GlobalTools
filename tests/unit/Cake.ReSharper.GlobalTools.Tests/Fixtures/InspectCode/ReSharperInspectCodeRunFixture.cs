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

        FileSystem.CreateFile("build/inspect_code.xml").SetContent(Resources.InspectCodeReportNoViolations.NormalizeLineEndings());
        FileSystem.CreateFile("build/violations.xml").SetContent(Resources.InspectCodeReportWithViolations.NormalizeLineEndings());
    }

    public ICakeLog Log { get; set; }

    public FilePath? Solution { get; set; }

    protected override void RunTool()
    {
        var tool = new ReSharperInspectCodeRunner(FileSystem, Environment, ProcessRunner, Tools, Log);
        tool.Run(Solution!, Settings);
    }
}
