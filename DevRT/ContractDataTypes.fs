namespace DevRT

type DevRTConfiguration = {
    MsBuildPath: string
    DeploymentDir: string
    NUnitConsole: string
    MsBuildWorkingDir: string
    FileChangeWithinLastSeconds: int
    TestProjects: string list
}
