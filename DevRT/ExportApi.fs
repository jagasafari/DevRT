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

    let getFsFiles() = FileWatchAgent.getFiles config.MsBuildWorkingDir

    let getTimeLine' () = 
        FileWatchAgent.getTimeLine 
            FileWatchAgent.getNow
            config.FileChangeWithinLastSeconds

    let isModified' = 
        FileWatchAgent.isModified 
            getTimeLine' FileWatchAgent.getLastWriteTime
        
    let fileWatchHandle =
        FileWatchAgent.handle 
            getFsFiles
            isModified' 
            stepsRunAgent.Post


    let fileWatchAgent = Agent.createAgent fileWatchHandle () 

    fileWatchAgent.Post
