namespace DevRT

type TestProjects =
    | RunTestsOn of string * string list | RunTestsOff

type RefactorConfig = {
    DevRTDeploymentDir: string }

[<NoComparison>]
type FileWatchConfig = {
    SleepMilliseconds: int
    FileChangeWatchDir: string
    ExcludedDirectories: seq<string> }

type NUnitConfig = {
    TestProjects: TestProjects
    NUnitConsole: string
    NUnitDeploymentDir: string }

type MsBuildConfig = {
    MsBuildPath: string
    MsBuildWorkingDir: string
    SolutionOrProjectFile: string
    OptionArgs: string }
