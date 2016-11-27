module MsBuildOutput 
    open DevRT
    open Common

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

    let createMsBuildHandle() =
        let handler = MsBuildOutputHandler()
        let handleMsBuildOutput = handler.Handle getUpdatedStatus processOutput
        let isMsBuildSuccess() = handler.GetBuildStatus() = BuildSucceeded
        handleMsBuildOutput, isMsBuildSuccess 
