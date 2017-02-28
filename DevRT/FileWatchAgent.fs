module DevRT.FileWatchAgent

open System
open DataTypes
open Common
open IOWrapper
open StringWrapper

let isBaseCase excludedDirs dir =
    excludedDirs |> Seq.exists(fun x -> containsCaseInsensitive x dir)

let rec getFiles isBaseCase dir =
    let bc = dir |> isBaseCase
    seq {
        if bc |> not then
            yield! dir |> enumerateFiles
            for d in dir |> enumerateDirectories do
                yield! getFiles isBaseCase d }

let getTimeLine (getNow: unit -> DateTime) sleepMiliseconds =
    let now = getNow()
    let delay = 100
    (-(sleepMiliseconds + delay)) |> float |> now.AddMilliseconds

let getLastWriteTime filePath = (IO.FileInfo filePath).LastWriteTime

let isNewModification getTimeLine getLastWriteTime filePath =
    (filePath |> getLastWriteTime) > (getTimeLine())

let handle getFiles isNewModification post postModifiedFile () =
    match getFiles() |> Seq.tryFind isNewModification with
    | None -> ()
    | Some file ->
        "modification found %s" %% file |> Logging.info
        post()
        file |> QueueModifiedFile |> postModifiedFile
