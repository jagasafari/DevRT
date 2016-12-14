module DevRT.ExportApi 
  
let getPostToFileWatchAgent config = 

    let runMsBuild' () = 
        MsBuildRunner.run config.MsBuildPath config.MsBuildWorkingDir 

    let runNUnit' = 
        NUnitRunner.run 
            config.TestProjects
            config.NUnitConsole 
            config.DeploymentDir 
            config.MsBuildWorkingDir 

    let ciStepsRunHandle = CiStepsRunAgent.handle runMsBuild' runNUnit'

    let stepsRunAgent = Agent.createAgent ciStepsRunHandle ()
    stepsRunAgent.Error.Add(fun exn -> printfn "%A" exn)

    let getFiles() = 
        FileWatchAgent.getFiles 
            config.WatchedFilesExtenstions config.MsBuildWorkingDir

    let getTimeLine' () = 
        FileWatchAgent.getTimeLine 
            FileWatchAgent.getNow
            config.FileChangeWithinLastSeconds

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
