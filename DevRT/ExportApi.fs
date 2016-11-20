module ci.ExportApi 
    
open NUnitRunner

[<NoComparison; NoEquality>]
type PathsForTestRunner = 
    { Runner: unit -> string; DeploymentDir: string; BuildDirectory: string}

let runMsBuild = MsBuildRunner.runMsBuild 

let runNUnit paths previousStepSuccess =
    
    let testRunningFlow = 
        stopNunitProcess (getProcesses "nunit-agent") (killProcess (sleep 500))
        >> cleanDeploymentDir paths.DeploymentDir  
        >> getDebugDirectories paths.BuildDirectory 
        >> copyBuildOutput paths.DeploymentDir
        >> runTests paths.Runner

    testRunningFlow previousStepSuccess
