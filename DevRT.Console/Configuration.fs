module Configuration

open DevRT

type Config = FSharp.Configuration.YamlConfig<"config.yaml">

let initConfig() = Config()

let getEnvConfig (config: Config.Environment_Type) = {
    MsBuildPath = config.MsBuildPath 
    NUnitConsole = config.NUnitConsole
    DeploymentDir = config.DeploymentDir }

let getSlnConfig (config: Config.Solution_Type)  = {
    MsBuildWorkingDir = config.MsBuildWorkingDir
    TestProjects = 
       (config.TestProjects.Directory
       , config.TestProjects.Dlls |> Seq.toList)
    SolutionFile = config.SolutionFile }

let getFileWatchConfig 
    (config: Config.Solution_Type.FileWatch_Type) = { 
    ChangeWithinPastSeconds = config.ChangeWithinPastSeconds
    ExcludedDirectories = config.ExcludedDirectories }
