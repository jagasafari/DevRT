module DevRT.Logging

open NLog

let private logger = LogManager.GetLogger("Logging")

let logMsg logLevel = sprintf "%A" >> logLevel

let info msg = msg |> logMsg logger.Info 

let error msg = msg |> logMsg logger.Error

