module DevRT.Logging

open NLog
open DataTypes

let private logger = LogManager.GetLogger("Logging")

let private logMsg logLevel = sprintf "%A" >> logLevel

let info msg = msg |> logMsg logger.Info

let private error msg =
    msg |> printfn "%A"
    msg |> logMsg logger.Error

let log = function
    | Info msg -> msg |> info | Error msg -> msg |> error
