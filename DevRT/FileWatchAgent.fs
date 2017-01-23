module DevRT.FileWatchAgent

open System
open DevRT.IOWrapper

let contains (str: string) substr = str.Contains(substr)

let isBaseCase excludedDirs contains =
    excludedDirs |> Seq.exists contains

let rec getFiles isBaseCase dir =
    let bc = dir |> contains |> isBaseCase
    seq {
        if bc |> not then
            yield! dir |> enumerateFiles
            for d in dir |> enumerateDirectories do
                yield! getFiles isBaseCase d }

let getNow() = DateTime.Now

let getTimeLine (getNow: unit -> DateTime) secondsInPast =
    let now = getNow()
    now.AddSeconds( (-secondsInPast) |> float)

let getLastWriteTime filePath =
    let fileInfo = IO.FileInfo filePath
    fileInfo.LastWriteTime

let isModified getTimeLine getLastWriteTime filePath =
    (getLastWriteTime filePath) > (getTimeLine())

let handle getFiles isModified post () = 
    let isPost = getFiles() |> Seq.exists isModified
    if isPost then () |> post
