module DevRT.Console

open System
open Configuration

[<EntryPoint>]
let main argv =
    let config, project = initConfig()
    let fileWatchConfig = getFileWatchConfig config project
    let nUnitConfig = getNUnitConfig config project
    let msBuildConfig = getMsBuildConfig config project
    let refactorConfig = getRefactorConfig config

    let run =
        ExportApi.getPostToFileWatchAgent 
            fileWatchConfig 
            nUnitConfig 
            msBuildConfig 
            refactorConfig

    () |> run |> ignore
    Console.ReadKey() |> ignore
    0
