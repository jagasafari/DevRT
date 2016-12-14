namespace DevRT

type DevRTConfiguration = {
    MsBuildPath: string
    DeploymentDir: string
    NUnitConsole: string
    MsBuildWorkingDir: string
    WatchedFilesExtenstions: string list
    FileChangeWithinLastSeconds: int
    TestProjects: (string * string list)
}
