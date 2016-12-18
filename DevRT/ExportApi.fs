module DevRT.ExportApi 
  
let getPostToFileWatchAgent envConfig slnConfig = 

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
            slnConfig.MsBuildWorkingDir 

    let ciStepsRunHandle = 
        CiStepsRunAgent.handle runMsBuild' runNUnit'

    let stepsRunAgent = Agent.createAgent ciStepsRunHandle ()
    stepsRunAgent.Error.Add(fun exn -> printfn "%A" exn)

    let getFiles() = 
        FileWatchAgent.getFiles 
            slnConfig.WatchedFilesExtenstions slnConfig.MsBuildWorkingDir

    let getTimeLine' () = 
        FileWatchAgent.getTimeLine 
            FileWatchAgent.getNow
            slnConfig.FileChangeWithinLastSeconds

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
