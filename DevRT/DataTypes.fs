module DevRT.DataTypes

type MsBuildStatus =
    Starting | Building | BuildFailed | BuildSucceeded | MSBuildError

type NUnitOutputStatus = Noise | Summary | Failure | Invalid | FailureLineInfo

type RefactorMessage = RefactorModifiedFiles | QueueModifiedFile of string

type RunAction = | CheckKeyPressed | HandleKeyPressed | RunCi | RunRefactor
