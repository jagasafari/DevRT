module Configuration

open DevRT

type Config = FSharp.Configuration.YamlConfig<"config.yaml">

let initConfig() = Config()

let getTestProjectConfig (config: Config.NUnit_Type.TestProjects_Type) =
    match config.TestsOn with
    | false -> RunTestsOff
    | true -> (config.Directory, config.Dlls |> Seq.toList) |> RunTestsOn

let getFileWatchConfig (config: Config) =
    {
    SleepMilliseconds = config.FileWatch.SleepMiliseconds
    ExcludedDirectories = config.FileWatch.ExcludedDirectories
    FileChangeWatchDir = config.FileWatch.FileChangeWatchDir }

let getRefactorConfig (config: Config) =
    {
    DevRTDeploymentDir = config.Environment.DevRTDeploymentDir }

let getNUnitConfig (config: Config) =
    {
    TestProjects = config.NUnit.TestProjects |> getTestProjectConfig
    NUnitConsole = config.Environment.NUnitConsole
    DeploymentDir = config.Environment.DeploymentDir }

let getMsBuildConfig log (config: Config) =
    log config.Solution.OptionArgs
    {
    MsBuildPath = config.Environment.MsBuildPath
    MsBuildWorkingDir = config.Solution.MsBuildWorkingDir
    SolutionOrProjectFile = config.Solution.SolutionOrProjectFile
    OptionArgs = config.Solution.OptionArgs}
