module FileWatchAgent

open System

let getFiles extensions dir =
    seq {
        for ext in extensions do
            let filePattern = sprintf "*.%s" ext
            yield! IO.Directory.GetFiles(
                dir, filePattern, IO.SearchOption.AllDirectories) 
    }

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
