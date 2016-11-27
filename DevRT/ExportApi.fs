module ci.ExportApi 
    
open DevRT
open NUnitRunner

[<NoComparison; NoEquality>]
type PathsForTestRunner = 
    { Runner: unit -> string; DeploymentDir: string; BuildDirectory: string}

let runMsBuild = MsBuildRunner.runMsBuild 

let runNUnit paths previousStepSuccess =
    let outputHandler = NUnitOutputHandler()
    let handle = outputHandler.Handle NUnitOutput.getUpdatedStatus NUnitOutput.filterOutput

    let testRunningFlow = 
        stopNunitProcess (getProcesses "nunit-agent") (killProcess (sleep 500))
        >> cleanDeploymentDir paths.DeploymentDir  
        >> getDebugDirectories paths.BuildDirectory 
        >> copyBuildOutput paths.DeploymentDir
        >> runTests handle paths.Runner

    testRunningFlow previousStepSuccess
