namespace DevRT

type EnvironmentConfig = {
    MsBuildPath: string
    NUnitConsole: string
    DeploymentDir: string }

type TestProjects = | RunTestsOn of (string * string list) | RunTestsOff

[<NoComparison>]
type SolutionConfig = {
    MsBuildWorkingDir: string
    TestProjects: TestProjects
    SolutionFile: string
}

[<NoComparison>]
type FileWatchConfig = {
    ChangeWithinPastSeconds: int
    ExcludedDirectories: seq<string> }
