namespace DevRT

type TestProjects = | RunTestsOn of (string * string list) | RunTestsOff

[<NoComparison>]
type FileWatchConfig = {
    SleepMilliseconds: int
    MsBuildWorkingDir: string
    ExcludedDirectories: seq<string> }

type NUnitConfig = {
    TestProjects: TestProjects
    NUnitConsole: string
    DeploymentDir: string }

type MsBuildConfig = {
    MsBuildPath: string
    MsBuildWorkingDir: string
    SolutionOrProjectFile: string
    OptionArgs: string}
