// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.ReSharper.GlobalTools.InspectCode;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.ReSharper.GlobalTools.Tests.Fixtures.InspectCode;

internal abstract class ReSharperInspectCodeFixture
    : ToolFixture<ReSharperInspectCodeSettings>
{
    protected ReSharperInspectCodeFixture(bool isWindows, bool useX86)
#pragma warning disable S3358 // Ternary operators should not be nested
        : base(!isWindows ? "./inspectcode.sh" : useX86 ? "inspectcode.x86.exe" : "inspectcode.exe")
#pragma warning restore S3358 // Ternary operators should not be nested
    {
        Environment = !isWindows ? FakeEnvironment.CreateUnixEnvironment() : FakeEnvironment.CreateWindowsEnvironment(!useX86);
    }
}
