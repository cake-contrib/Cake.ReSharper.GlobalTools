// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.ReSharper.GlobalTools.CleanupCode;
using Cake.Testing.Fixtures;

namespace Cake.ReSharper.GlobalTools.Tests.Fixtures.CleanupCode;

internal abstract class CleanupCodeFixture
    : ToolFixture<CleanupCodeSettings>
{
    protected CleanupCodeFixture(bool isWindows, bool useX86)
#pragma warning disable S3358 // Ternary operators should not be nested
        : base(!isWindows ? "./cleanupcode.sh" : useX86 ? "cleanupcode.x86.exe" : "cleanupcode.exe")
#pragma warning restore S3358 // Ternary operators should not be nested
    {
    }
}
