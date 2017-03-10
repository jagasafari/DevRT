module DevRT.Console

open System
open Configuration

[<EntryPoint>]
let main argv =
    let config = initConfig()
    config.Load(@"c:/DevRT/DevRT.Console/config.yaml")
    let fileWatchConfig = getFileWatchConfig config
    let nUnitConfig = getNUnitConfig config
    let msBuildConfig = getMsBuildConfig config
    let refactorConfig = getRefactorConfig config

    let run =
        ExportApi.getPostToFileWatchAgent fileWatchConfig nUnitConfig msBuildConfig refactorConfig

    () |> run |> ignore
    Console.ReadKey() |> ignore
    0
