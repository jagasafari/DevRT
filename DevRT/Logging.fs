module DevRT.Logging

open NLog
open DataTypes

let private logger = LogManager.GetLogger("Logging")

let logMsg logLevel = sprintf "%A" >> logLevel

let info msg = msg |> logMsg logger.Info

let error msg =
    msg |> printfn "%A"
    msg |> logMsg logger.Error

let log = function
    | Info msg -> msg |> info | Error msg -> msg |> error
