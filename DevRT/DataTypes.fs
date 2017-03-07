module DevRT.DataTypes

open System

type MsBuildStatus =
    Starting | Building | BuildFailed | BuildSucceeded | MSBuildError

type NUnitOutputStatus = Noise | Summary | Failure | Invalid | FailureLineInfo

type RunAction = | CheckKeyPressed | HandleKeyPressed | RunCi | RunRefactor

type LogMessageInfo =
    | FileWatchModificationFound of string
    | ProcessRunnerStartInfo of string * string
    | NUnitCopyBuildOutput of string * string

[<NoComparison>]
type LogMessageError =
    | AgentRecoverableError of exn
    | AgentFatalError of exn

[<NoComparison>]
type LogLevel =
    | Info of LogMessageInfo | Error of LogMessageError
