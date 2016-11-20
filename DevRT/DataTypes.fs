namespace DevRT

open Common.ConsoleUtil
open System

type MsBuildStatus = | Building | BuildFailed | BuildSucceeded | MSBuildError

type MsBuildOutputHandler() =
    let mutable buildStatus = Building
    
    let updateStatus getUpdatedStatus data =
        let updatedStatus = getUpdatedStatus buildStatus data
        buildStatus <- updatedStatus
        
    member x.Handle getUpdatedStatus processOutput data = 
        let updateStatus' = updateStatus getUpdatedStatus
        processOutput updateStatus' buildStatus data
    
    member x.GetBuildStatus() = buildStatus

type NUnitOutputHandler() =
    member x.Handle data = ()
