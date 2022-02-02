// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;
using Cake.ReSharper.GlobalTools.InspectCode;
using Cake.ReSharper.GlobalTools.Tests.Fixtures.InspectCode;
using Cake.Testing;
using Xunit;

namespace Cake.ReSharper.GlobalTools.Tests.Unit.InspectCode;

public sealed class ReSharperInspectCodeRunnerTests
{
    public sealed class TheRunMethod
    {
        [Fact]
        public void Should_Throw_If_Solution_Is_Null()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
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
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings = null!,
            };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Find_Inspect_Code_Runner_NonWindows()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture(isWindows: false);

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("/Working/tools/inspectcode.sh", result.Path.FullPath);
        }

        [Fact]
        public void Should_Find_Inspect_Code_Runner()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture(isWindows: true);

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("/Working/tools/inspectcode.exe", result.Path.FullPath);
        }

        [Fact]
        public void Should_Find_Inspect_Code_Runner_X86()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture(isWindows: true, useX86: true)
            {
                Settings =
                {
                    UseX86Tool = true,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("/Working/tools/inspectcode.x86.exe", result.Path.FullPath);
        }

        [Fact]
        public void Should_Use_Provided_Solution_In_Process_Arguments()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--build \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture();
            fixture.GivenProcessCannotStart();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("InspectCode: Process was not started.", result.Message);
        }

        [Fact]
        public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("InspectCode: Process returned an error (exit code 1).", result.Message);
        }

        [Fact]
        public void Should_Set_Caches_Home()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
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
                "--caches-home=\"/Working/caches\" --build \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Config_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
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
                "--config=\"/Working/config.xml\" --build \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Config_Create_File()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
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
            var fixture = new ReSharperInspectCodeRunFixture
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
                "--disable-settings-layers=GlobalAll;GlobalPerProduct;SolutionShared;SolutionPersonal;ProjectShared;ProjectPersonal --build \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Debug_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    Debug = true,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--debug --build \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_DotNetCorePath_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    DotNetCorePath = "/usr/local/share/dotnet/dotnet",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--dotnetcore=\"/usr/local/share/dotnet/dotnet\" --build \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_DotNetCoreSdk_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    DotNetCoreSdk = "6.0.101",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--dotnetcoresdk=6.0.101 --build \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Exclude_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    Exclude = new[] { "*.bat", "*.cmd" },
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--exclude=\"*.bat;*.cmd\" --build \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_ReSharper_Plugins()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
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
                "-x=\"ReSharper.AgentSmith;X.Y\" --build \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Extension_Sources_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
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
                "--source=\"https://resharper-plugins.jetbrains.com/api/v2/curated-feeds/Wave_v213.0/;https://api.nuget.org/v3/index.json\" --build \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Help()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
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
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    Include = new[] { "*.bat", "*.cmd" },
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--include=\"*.bat;*.cmd\" --build \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Mono_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    MonoPath = "/Library/Frameworks/Mono.framework/Versions/Current/bin/mono",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--mono=\"/Library/Frameworks/Mono.framework/Versions/Current/bin/mono\" --build \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_No_Buildin_Settings_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    NoBuildInSettings = true,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--no-buildin-settings --build \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Profile_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
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
                "--profile=\"/Working/profile.DotSettings\" --build \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_MsBuild_Properties_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
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
                "--properties:TreatWarningsAsErrors=\"true\" --properties:Optimize=\"false\" --build \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Targets_For_Items_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
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
                "--targets-for-items=BeforeMainBuildTask --build \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Targets_For_References_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
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
                "--targets-for-references=BeforeDownload --build \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Toolset_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
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
                "--toolset=17.0 --build \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Toolset_Path_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
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
                "--toolset-path=\"/usr/local/msbuild/bin/current/MSBuild.exe\" --build \"/Working/Test.sln\"",
                result.Args);
        }

        [Fact]
        public void Should_Set_Verbosity()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    Verbosity = ReSharperVerbosity.Error,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--verbosity=ERROR --build \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Version_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
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
        public void Should_Set_Build_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    Build = false,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--no-build \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Dump_Issue_Types_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    DumpIssueTypes = true,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--build --dumpIssuesTypes \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Measure_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    Measure = ReSharperInspectCodeMeasure.Timeline,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--build --measure=TIMELINE \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Output()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    OutputFile = "build/inspect_code.xml",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--build --output=\"/Working/build/inspect_code.xml\" \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Throw_If_OutputFile_Contains_Violations_And_Set_To_Throw()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    OutputFile = new FilePath("build/violations.xml"),
                    ThrowExceptionOnFindingViolations = true,
                },
            };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsCakeException(result, "Code Inspection Violations found in code base.");
        }

        [Fact]
        public void Should_Set_Output_File_Format_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    OutputFileFormat = ReSharperInspectCodeReportFormat.Html,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--build --format=Html \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Parallel_Jobs_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    ParallelJobs = 4,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "--build --jobs=4 \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Project_Filter()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    ProjectFilter = "Test.*",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--build --project=\"Test.*\" \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Severity()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    Severity = ReSharperInspectCodeSeverity.Hint,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--build --severity=HINT \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Analyze_Output()
        {
            var log = new FakeLog();

            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Log = log,
                Settings =
                {
                    OutputFile = new FilePath("build/violations.xml"),
                },
            };

            // When
            fixture.Run();

            // Then
            var logContainsInspectionResults =
                log.Entries.Any(p => p.Message.StartsWith("Code Inspection Error(s) Located.", StringComparison.Ordinal));

            Assert.True(logContainsInspectionResults);
        }

        [Fact]
        public void Should_Not_Analyze_Output()
        {
            var log = new FakeLog();

            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Log = log,
                Settings =
                {
                    OutputFile = new FilePath("build/violations.xml"),
                    SkipOutputAnalysis = true,
                },
            };

            // When
            fixture.Run();

            // Then
            var logContainsInspectionResults =
                log.Entries.Any(p => p.Message.StartsWith("Code Inspection Error(s) Located.", StringComparison.Ordinal));

            Assert.False(logContainsInspectionResults);
        }

        [Fact]
        public void Should_Set_Solution_Wide_Analysis_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    SolutionWideAnalysis = true,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--build --swea \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_No_Solution_Wide_Analysis_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    SolutionWideAnalysis = false,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--build --no-swea \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Target_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    Target = "Build",
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--build --target=Build \"/Working/Test.sln\"", result.Args);
        }

        [Fact]
        public void Should_Set_Use_Absolute_Paths_Switch()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    UseAbsolutePaths = true,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--build --absolute-paths \"/Working/Test.sln\"", result.Args);
        }
    }

    public sealed class TheRunFromConfigMethod
    {
        [Fact]
        public void Should_Throw_If_Config_File_Is_Null()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFromConfigFixture
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
            var fixture = new ReSharperInspectCodeRunFromConfigFixture
            {
                Config = "config.xml",
            };

            // Then
            var result = fixture.Run();

            // Then
            Assert.Equal("--config=\"/Working/config.xml\"", result.Args);
        }

        [Fact]
        public void Should_Contain_Build_If_Build_Is_Not_Set()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture();

            // When
            var result = fixture.Run();

            // Then
            Assert.Contains("--build", result.Args, StringComparison.Ordinal);
            Assert.DoesNotContain("--no-build", result.Args, StringComparison.Ordinal);
        }

        [Fact]
        public void Should_Contain_Build_If_Build_Is_Set_To_True()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    Build = true,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Contains("--build", result.Args, StringComparison.Ordinal);
        }

        [Fact]
        public void Should_Contain_NoBuild_If_Build_Is_Set_To_False()
        {
            // Given
            var fixture = new ReSharperInspectCodeRunFixture
            {
                Settings =
                {
                    Build = false,
                },
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Contains("--no-build", result.Args, StringComparison.Ordinal);
        }
    }
}
