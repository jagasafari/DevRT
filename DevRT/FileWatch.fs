module DevRT.FileWatch

open System
open DataTypes
open Common
open IOWrapper
open StringWrapper

let isBaseCase excludedDirs dir =
    excludedDirs
    |> Seq.exists(fun excludedDir ->
        containsCaseInsensitive excludedDir dir)

let rec getFiles isBaseCase dir =
    let bc = dir |> isBaseCase
    seq {
        if bc |> not then
            yield! dir |> enumerateFiles
            for directory in dir |> enumerateDirectories do
                yield! getFiles isBaseCase directory }

let getTimeLine (getNow: unit -> DateTime) sleepMiliseconds =
    let now = getNow()
    let delay = 100
    (-(sleepMiliseconds + delay)) |> float |> now.AddMilliseconds

let getLastWriteTime filePath = (IO.FileInfo filePath).LastWriteTime

let isNewModification getTimeLine getLastWriteTime filePath =
    (filePath |> getLastWriteTime) > (getTimeLine())

let publishAboutModification log post postModifiedFile =
    function
    | None -> ()
    | Some file ->
        file |> log; post(); file |> postModifiedFile

let handle getFiles isNewModification publish () =
    getFiles() |> Seq.tryFind isNewModification |> publish
