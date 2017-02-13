module DevRT.Console

open System
open Configuration

[<EntryPoint>]
let main argv =
    DevRT.Logging.info "kfj"
    let config = initConfig()
    let envConfig = getEnvConfig (config.Environment)
    let slnConfig = getSlnConfig (config.Solution)
    let fileWatchConfig = getFileWatchConfig (config.Solution.FileWatch)

    let run =
        ExportApi.getPostToFileWatchAgent
            envConfig fileWatchConfig slnConfig

    () |> run |> ignore
    Console.ReadKey() |> ignore
    0
