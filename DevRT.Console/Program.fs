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

    let post = 
        ExportApi.getPostToFileWatchAgent 
            envConfig fileWatchConfig slnConfig 

    while true do () |> post; Threading.Thread.Sleep 1000
    Console.ReadKey() |> ignore
    0
