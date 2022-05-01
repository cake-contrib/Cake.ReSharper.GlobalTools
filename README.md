# Cake.ReSharper.GlobalTools

Contains functionality related to [JetBrains ReSharper Command Line Tools](https://www.jetbrains.com/help/resharper/ReSharper_Command_Line_Tools.html)
which is a set of free cross-platform standalone tools that help you integrate automatic code quality procedures into your CI, version control, or any other server.

## InspectCode

Executes hundreds of ReSharper code inspections.

### ReSharperInspectCode(FilePath)

Analyses the specified solution with ReSharper's InspectCode.
Stores result in file ./build/resharper_inspect_code.xml.

```c#
    ReSharperInspectCode("./src/MySolution.sln");
```

### ReSharperInspectCode(FilePath, InspectCodeSettings)

Analyses the specified solution with ReSharper's InspectCode, using the specified settings.

```c#
    var buildOutputDirectory = Directory("./.build");
    var resharperReportsDirectory = buildOutputDirectory + Directory("_ReSharperReports");
    var msBuildProperties = new Dictionary<string, string>();
    msBuildProperties.Add("configuration", configuration);
    msBuildProperties.Add("platform", "AnyCPU");
    ReSharperInspectCode(
        "./MySolution.sln",
        new InspectCodeSettings
        {
            SolutionWideAnalysis = true,
            Profile = "./MySolution.sln.DotSettings",
            MsBuildProperties = msBuildProperties,
            OutputFile = resharperReportsDirectory + File("resharper_inspectcode_output.xml"),
            ThrowExceptionOnFindingViolations = true,
        });
```

### ReSharperInspectCodeFromConfig(FilePath)

Runs ReSharper's InspectCode using the specified config file.

## CleanupCode

Instantly eliminates code style violations and ensures a uniform code base.

### ReSharperCleanupCode(FilePath)

Cleanups the specified solution with ReSharper's CleanupCode.

### ReSharperCleanupCode(FilePath, ReSharperCleanupCodeSettings)

Cleanups the specified solution with ReSharper's CleanupCode, using the specified settings.

### ReSharperCleanupCodeFromConfig(FilePath)

Runs ReSharper's CleanupCode using the specified config file.

## Special consideration when running on platforms

### Linux

- due to ReSharper dependencies it is needed to install libs: libgdiplus, libc6-dev

```cmd
    sudo apt install libgdiplus libc6-dev
```