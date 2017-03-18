module DevRT.MsBuild

open DataTypes
open StringWrapper

let createMsBuildStatus () =
    let mutable status = Starting
    let update newStatus = status <- newStatus
    update, fun () -> status

let getContinuationStatus = function
    | Starting -> Building | currentStatus -> currentStatus

let isMsBuildError = isRegexMatch "MSBUILD[\s]*:[\s]*error"

let isBuildSuccessful = contains "Build succeeded"

let isBuildError = contains "Build FAILED"

let getUpdatedStatus getContinuationStatus = function
    | output when isBuildSuccessful output -> BuildSucceeded
    | output when isBuildError output -> BuildFailed
    | output when isMsBuildError output -> MSBuildError
    | _ -> () |> getContinuationStatus

let processOutput
    handleStarting
    handleSuccess
    handleFailure
    updateStatus = function
    | Starting -> handleStarting()
    | Building -> updateStatus
    | BuildFailed | MSBuildError -> handleFailure
    | BuildSucceeded -> handleSuccess

let getRunArgsString args slnOrProjectFile =
    sprintf "%s %s" slnOrProjectFile args

let handleStarting output getNow updateStatus () =
    sprintf "%O" (getNow()) |> output;  updateStatus

let handle processOutput run getStatus post =
    (fun output -> (getStatus() |> processOutput) output)
    |> run
    getStatus() |> post
