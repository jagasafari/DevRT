module DevRT.Console

open System

[<EntryPoint>]
let main argv =

    let post = 
        ExportApi.getPostToFileWatchAgent 
            Configuration.envConfig Configuration.slnConfig

    while true do () |> post; Threading.Thread.Sleep 1000
    Console.ReadKey() |> ignore
    0
