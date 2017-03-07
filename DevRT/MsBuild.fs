module DevRT.MsBuild

open DataTypes
open StringWrapper

let createMsBuildStatus () =
    let mutable status = Starting
    let update newStatus = status <- newStatus
    update, fun () -> status

let getContinuationStatus = function
    | Starting -> Building | x -> x

let getUpdatedStatus getContinuationStatus = function
    | d when contains "Build succeeded" d -> BuildSucceeded
    | d when contains "Build FAILED" d -> BuildFailed
    | d when contains "MSBUILD: error" d -> MSBuildError
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
