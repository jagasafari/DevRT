module Configuration

open DevRT

type Config = FSharp.Configuration.YamlConfig<"config.yaml">

let config = Config()

let envConfig = {
    MsBuildPath = config.Environment.MsBuildPath 
    NUnitConsole = config.Environment.NUnitConsole
    DeploymentDir = config.Environment.DeploymentDir 
    }

let slnConfig = {
    FileChangeWithinLastSeconds 
        = config.Solution.FileChangeWithinLastSeconds
    WatchedFilesExtenstions = config.Solution.WatchedFilesExtenstions
    MsBuildWorkingDir = config.Solution.MsBuildWorkingDir
    TestProjects = 
       (config.Solution.TestProjectDirectory
       , config.Solution.TestProjects |> Seq.toList)
    SolutionFile = config.Solution.SolutionFile
    }
