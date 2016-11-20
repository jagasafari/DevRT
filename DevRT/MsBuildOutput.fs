module MsBuildOutput 
    open DevRT
    open Common.ConsoleUtil

    let getUpdatedStatus currentStatus (data: string) =
        match data with
        | d when d.Contains("Build succeeded") -> BuildSucceeded
        | d when d.Contains("Build FAILED") -> BuildFailed
        | d when d.Contains("MSBUILD: error") -> MSBuildError
        | _ -> currentStatus
        
    let processOutput updateStatus = function
        | Building -> updateStatus 
        | BuildFailed | MSBuildError -> writelineRed 
        | BuildSucceeded -> appendToLinePurple 

    let isBuildSuccess = function | BuildSucceeded -> true | _ -> false

    let createMsBuildHandle() =
        let handler = MsBuildOutputHandler()
        let handleMsBuildOutput = handler.Handle getUpdatedStatus processOutput
        let isMsBuildSuccess() = isBuildSuccess (handler.GetBuildStatus())
        handleMsBuildOutput, isMsBuildSuccess 
