module Configuration

open DevRT

type Config = FSharp.Configuration.YamlConfig<"config.yaml">

let initConfig() = Config()

let getTestProjectConfig (config: Config.NUnit_Type.TestProjects_Type) =
    match config.TestsOn with
    | false -> RunTestsOff
    | true -> (config.Directory, config.Dlls |> Seq.toList) |> RunTestsOn

let getFileWatchConfig log (config: Config) =
    log config.FileWatch.ExcludedDirectories
    {
    SleepMilliseconds = config.FileWatch.SleepMiliseconds
    ExcludedDirectories = config.FileWatch.ExcludedDirectories
    MsBuildWorkingDir = config.Solution.MsBuildWorkingDir}

let getNUnitConfig (config: Config) =
    {
    TestProjects = config.NUnit.TestProjects |> getTestProjectConfig
    NUnitConsole = config.Environment.NUnitConsole
    DeploymentDir = config.Environment.DeploymentDir }

let getMsBuildConfig (config: Config) =
    {
    MsBuildPath = config.Environment.MsBuildPath
    MsBuildWorkingDir = config.Solution.MsBuildWorkingDir
    SolutionFile = config.Solution.SolutionFile}


