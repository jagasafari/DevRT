module DevRT.ExportApi 
  
let getPostToFileWatchAgent envConfig fileWatchConfig slnConfig = 

    let runMsBuild' () = 
        MsBuildRunner.run 
            envConfig.MsBuildPath 
            slnConfig.SolutionFile 
            slnConfig.MsBuildWorkingDir 

    let runNUnit' = 
        NUnitRunner.run 
            slnConfig.TestProjects
            envConfig.NUnitConsole 
            envConfig.DeploymentDir 

    let ciStepsRunHandle = 
        CiStepsRunAgent.handle runMsBuild' runNUnit'

    let stepsRunAgent = Agent.createAgent ciStepsRunHandle ()
    stepsRunAgent.Error.Add(fun exn -> printfn "%A" exn)

    let isBaseCase' = 
        FileWatchAgent.isBaseCase fileWatchConfig.ExcludedDirectories

    let getFiles() =
        FileWatchAgent.getFiles isBaseCase' slnConfig.MsBuildWorkingDir

    let getTimeLine' () = 
        FileWatchAgent.getTimeLine 
            FileWatchAgent.getNow
            fileWatchConfig.ChangeWithinPastSeconds

    let isModified' = 
        FileWatchAgent.isModified 
            getTimeLine' FileWatchAgent.getLastWriteTime
        
    let fileWatchHandle =
        FileWatchAgent.handle 
            getFiles
            isModified' 
            stepsRunAgent.Post

    let fileWatchAgent = Agent.createAgent fileWatchHandle () 

    fileWatchAgent.Post
