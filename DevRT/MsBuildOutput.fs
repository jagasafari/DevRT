module DevRT.MsBuildOutput 

open DataTypes
open ConsoleOutput

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
    let isMsBuildSuccess() = handler.BuildStatus = BuildSucceeded
    handleMsBuildOutput, isMsBuildSuccess 
