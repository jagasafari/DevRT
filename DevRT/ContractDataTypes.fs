namespace DevRT

type EnvironmentConfiuration = {
    MsBuildPath: string
    NUnitConsole: string
    DeploymentDir: string
}

[<NoComparison>]
type SolutionConfiguration = {
    FileChangeWithinLastSeconds: int
    WatchedFilesExtenstions: seq<string>
    MsBuildWorkingDir: string
    TestProjects: (string * string list)
    SolutionFile: string
}
