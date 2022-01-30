// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.ReSharper.GlobalTools.InspectCode;
using NSubstitute;

namespace Cake.ReSharper.GlobalTools.Tests.Fixtures.InspectCode;

internal sealed class InspectCodeRunFromConfigFixture
    : InspectCodeFixture
{
    public InspectCodeRunFromConfigFixture(
        bool isWindows = true,
        bool useX86 = false)
        : base(isWindows, useX86)
    {
        Log = Substitute.For<ICakeLog>();
    }

    public ICakeLog Log { get; set; }

    public FilePath? Config { get; set; }

    protected override void RunTool()
    {
        var tool = new InspectCodeRunner(FileSystem, Environment, ProcessRunner, Tools, Log);
        tool.RunFromConfig(Config!);
    }
}
