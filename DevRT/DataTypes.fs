module DevRT.DataTypes

type MsBuildStatus =
    Starting | Building | BuildFailed | BuildSucceeded | MSBuildError

type NUnitOutputStatus = Noise | Summary | Failure
