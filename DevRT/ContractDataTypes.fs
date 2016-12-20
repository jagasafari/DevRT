namespace DevRT

type EnvironmentConfig = {
    MsBuildPath: string
    NUnitConsole: string
    DeploymentDir: string }

[<NoComparison>]
type SolutionConfig = {
    MsBuildWorkingDir: string
    TestProjects: (string * string list)
    SolutionFile: string
}

[<NoComparison>]
type FileWatchConfig = {
    ChangeWithinPastSeconds: int
    ExcludedDirectories: seq<string> }
