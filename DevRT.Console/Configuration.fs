module Configuration

open DevRT

type Config = FSharp.Configuration.YamlConfig<"config.yaml">

let initConfig() = Config()

let getEnvConfig (config: Config.Environment_Type) = {
    MsBuildPath = config.MsBuildPath 
    NUnitConsole = config.NUnitConsole
    DeploymentDir = config.DeploymentDir }

let getTestProjectConfig (config: Config.Solution_Type.TestProjects_Type) =
    match config.TestsOn with
    | true -> RunTestsOff 
    | false -> (config.Directory, config.Dlls |> Seq.toList) |> RunTestsOn
    
let getSlnConfig (config: Config.Solution_Type) =
    let slnConfig = {
        MsBuildWorkingDir = config.MsBuildWorkingDir
        TestProjects = getTestProjectConfig config.TestProjects 
        SolutionFile = config.SolutionFile }
    slnConfig |> Logging.info
    slnConfig

let getFileWatchConfig 
    (config: Config.Solution_Type.FileWatch_Type) = { 
    ChangeWithinPastSeconds = config.ChangeWithinPastSeconds
    ExcludedDirectories = config.ExcludedDirectories }
