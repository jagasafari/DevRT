module DevRT.MsBuild

open DataTypes
open StringWrapper

type private MsBuildStatus() =
    let mutable buildStatus = Starting
    member x.Update newStatus = buildStatus <- newStatus
    member x.BuildStatus with get() = buildStatus

let createMsBuildStatus () =
    let handler = MsBuildStatus()
    handler.Update, fun () -> handler.BuildStatus

let getContinuationStatus = function | Starting -> Building | x -> x

let getUpdatedStatus contains getContinuationStatus = function
    | d when contains "Build succeeded" d -> BuildSucceeded
    | d when contains "Build FAILED" d -> BuildFailed
    | d when contains "MSBUILD: error" d -> MSBuildError
    | _ -> () |> getContinuationStatus

let processOutput handleStarting handleSuccess handleFailure updateStatus = function
    | Starting -> () |> handleStarting
    | Building -> updateStatus
    | BuildFailed | MSBuildError -> handleFailure
    | BuildSucceeded -> handleSuccess

let getRunArgsString slnFile = ("%s /m" %% slnFile)

let handleStarting log getNow updateStatus () =
    "%O" %% (() |> getNow) |> log;  updateStatus

let runMSBuild processOutput run getStatus post =
    let processOutput' output = output |> ((getStatus()) |> processOutput)
    run processOutput'
    getStatus() |> post
