module DevRT.DataTypes

open System

type MsBuildStatus = 
    | Building | BuildFailed | BuildSucceeded | MSBuildError

type NUnitOutputStatus = Noise | Summary | Failure

type MsBuildOutputHandler() =
    let mutable buildStatus = Building
    member x.Handle getUpdatedStatus processOutput data = 
        let updateStatus data = 
            buildStatus <- getUpdatedStatus buildStatus data
        processOutput updateStatus buildStatus data 
    
    member x.BuildStatus with get() = buildStatus

type NUnitOutputHandler() =
    let mutable status = Noise

    member x.Handle getUpdatedStatus filterOutput data =
        status <- getUpdatedStatus status data
        filterOutput data status   

