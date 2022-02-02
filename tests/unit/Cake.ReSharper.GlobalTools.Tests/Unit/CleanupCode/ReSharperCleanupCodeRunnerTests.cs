// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.ReSharper.GlobalTools.Tests.Fixtures.CleanupCode;
using Cake.Testing;
using Xunit;

namespace Cake.ReSharper.GlobalTools.Tests.Unit.CleanupCode;

public sealed class ReSharperCleanupCodeRunnerTests
{
    public sealed class TheRunMethod
    {
        [Fact]
        public void Should_Throw_If_Solution_Is_Null()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Solution = null,
            };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "solution");
        }

        [Fact]
        public void Should_Throw_If_Settings_Is_Null()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings = null!,
            };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Find_Cleanup_Code_Runner_NonWindows()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture(isWindows: false);

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("/Working/tools/cleanupcode.sh", result.Path.FullPath);
        }

        [Fact]
        public void Should_Find_Cleanup_Code_Runner()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture(isWindows: true);

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("/Working/tools/cleanupcode.exe", result.Path.FullPath);
        }

        [Fact]
        public void Should_Find_Cleanup_Code_Runner_X86()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture(isWindows: true, useX86: true)
            {
                Settings =
                {
                    UseX86Tool = true,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("/Working/tools/cleanupcode.x86.exe", result.Path.FullPath);
        }

        [Fact]
        public void Should_Use_Provided_Solution_In_Process_Arguments()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture();
            fixture.GivenProcessCannotStart();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("CleanupCode: Process was not started.", result.Message);
        }

        [Fact]
        public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("CleanupCode: Process returned an error (exit code 1).", result.Message);
        }

        [Fact]
        public void Should_Set_Caches_Home()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    CachesHome = "caches/",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--caches-home=\"/Working/caches\" \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Config_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    ConfigFile = "config.xml",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--config=\"/Working/config.xml\" \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Config_Create_File()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    ConfigCreateFile = "create.xml",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--config-create=\"/Working/create.xml\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Disabled_Settings_Layers()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    DisabledSettingsLayers = new[]
                    {
                        ReSharperSettingsLayer.GlobalAll,
                        ReSharperSettingsLayer.GlobalPerProduct,
                        ReSharperSettingsLayer.SolutionShared,
                        ReSharperSettingsLayer.SolutionPersonal,
                        ReSharperSettingsLayer.ProjectShared,
                        ReSharperSettingsLayer.ProjectPersonal,
                    },
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--disable-settings-layers=GlobalAll;GlobalPerProduct;SolutionShared;SolutionPersonal;ProjectShared;ProjectPersonal \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Debug_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    Debug = true,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--debug \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_DotNetCorePath_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    DotNetCorePath = "/usr/local/share/dotnet/dotnet",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--dotnetcore=\"/usr/local/share/dotnet/dotnet\" \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_DotNetCoreSdk_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    DotNetCoreSdk = "6.0.101",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--dotnetcoresdk=6.0.101 \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Exclude_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    Exclude = new[] { "*.bat", "*.cmd" },
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--exclude=\"*.bat;*.cmd\" \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_ReSharper_Plugins()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    Extensions = new[]
                    {
                        "ReSharper.AgentSmith",
                        "X.Y",
                    },
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "-x=\"ReSharper.AgentSmith;X.Y\" \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Extension_Sources_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    ExtensionSources = new[]
                    {
                        "https://resharper-plugins.jetbrains.com/api/v2/curated-feeds/Wave_v213.0/",
                        "https://api.nuget.org/v3/index.json",
                    },
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--source=\"https://resharper-plugins.jetbrains.com/api/v2/curated-feeds/Wave_v213.0/;https://api.nuget.org/v3/index.json\" \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Help()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    Help = true,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--help", result.Args);
        }

        [Fact]
        public void Should_Set_Include_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    Include = new[] { "*.bat", "*.cmd" },
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--include=\"*.bat;*.cmd\" \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Mono_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    MonoPath = "/Library/Frameworks/Mono.framework/Versions/Current/bin/mono",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--mono=\"/Library/Frameworks/Mono.framework/Versions/Current/bin/mono\" \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_No_Buildin_Settings_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    NoBuildInSettings = true,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--no-buildin-settings \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Profile_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    Profile = "profile.DotSettings",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--profile=\"/Working/profile.DotSettings\" \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_MsBuild_Properties_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    MsBuildProperties = new Dictionary<string, string>(StringComparer.Ordinal)
                    {
                        ["TreatWarningsAsErrors"] = "true",
                        ["Optimize"] = "false",
                    },
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--properties:TreatWarningsAsErrors=\"true\" --properties:Optimize=\"false\" \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Targets_For_Items_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    TargetsForItems = new[] { "BeforeMainBuildTask" },
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--targets-for-items=BeforeMainBuildTask \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Targets_For_References_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    TargetsForReferences = new[] { "BeforeDownload" },
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--targets-for-references=BeforeDownload \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Toolset_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    Toolset = "17.0",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--toolset=17.0 \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Toolset_Path_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    ToolsetPath = "/usr/local/msbuild/bin/current/MSBuild.exe",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--toolset-path=\"/usr/local/msbuild/bin/current/MSBuild.exe\" \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Verbosity()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    Verbosity = ReSharperVerbosity.Error,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--verbosity=ERROR \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Version_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    Version = true,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--version", result.Args);
        }

        [Fact]
        public void Should_Set_Settings_Path_Switch()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFixture
            {
                Settings =
                {
                    SettingsPath = "Test.sln.DotSettings",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--settings=\"/Working/Test.sln.DotSettings\" \"/Working/Test.sln\"", result.Args);
        }
    }

    public sealed class TheRunFromConfigMethod
    {
        [Fact]
        public void Should_Throw_If_Config_File_Is_Null()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFromConfigFixture
            {
                Config = null,
            };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "configFile");
        }

        [Fact]
        public void Should_Use_Provided_Config_File()
        {
            // Given
            var fixture = new ReSharperCleanupCodeRunFromConfigFixture
            {
                Config = "config.xml",
            };

            // Then
            var result = fixture.Run();

            // Then
            Assert.Equal("--config=\"/Working/config.xml\"", result.Args);
        }
    }
}
