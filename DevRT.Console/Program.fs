module DevRT.Console

open System
open Configuration

[<EntryPoint>]
let main argv =
    let config = initConfig()
    let fileWatchConfig = getFileWatchConfig config
    let nUnitConfig = getNUnitConfig config
    let msBuildConfig = getMsBuildConfig Logging.info config

    let run =
        ExportApi.getPostToFileWatchAgent fileWatchConfig nUnitConfig msBuildConfig

    () |> run |> ignore
    Console.ReadKey() |> ignore
    0
