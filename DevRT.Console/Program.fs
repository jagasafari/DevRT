module DevRT.Console

open System
open Configuration

[<EntryPoint>]
let main argv =
    DevRT.Logging.info "kfj"
    let config = initConfig()
    let fileWatchConfig = getFileWatchConfig Logging.info config
    let nUnitConfig = getNUnitConfig config
    let msBuildConfig = getMsBuildConfig config

    let run =
        ExportApi.getPostToFileWatchAgent fileWatchConfig nUnitConfig msBuildConfig

    () |> run |> ignore
    Console.ReadKey() |> ignore
    0
