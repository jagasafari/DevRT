module DevRT.FileWatchAgent

open System
open Common
open IOWrapper
open StringWrapper

let isBaseCase excludedDirs dir =
    excludedDirs |> Seq.exists(fun x -> contains x dir)

let rec getFiles isBaseCase dir =
    let bc = dir |> isBaseCase
    seq {
        if bc |> not then
            yield! dir |> enumerateFiles
            for d in dir |> enumerateDirectories do
                yield! getFiles isBaseCase d }


let getTimeLine (getNow: unit -> DateTime) secondsInPast =
    let now = getNow()
    now.AddSeconds( (-secondsInPast) |> float)

let getLastWriteTime filePath =
    let fileInfo = IO.FileInfo filePath
    fileInfo.LastWriteTime

let isModified getTimeLine getLastWriteTime filePath =
    let modified = (getLastWriteTime filePath) > (getTimeLine())
    if modified then
        "modification found %s" %% filePath |> Logging.info
    modified

let handle getFiles isModified post () =
    let isPost = getFiles() |> Seq.exists isModified
    if isPost then () |> post
