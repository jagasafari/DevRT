module FileWatchAgent

let getFiles dir =
    System.IO.Directory.GetFiles(
       dir, "*.fs", System.IO.SearchOption.AllDirectories) 

let getNow() = System.DateTime.Now

let getTimeLine (getNow: unit -> System.DateTime) forThePastSeconds =
    getNow().AddSeconds( (-forThePastSeconds) |> float)

let getLastWriteTime filePath =
    let fileInfo = System.IO.FileInfo filePath
    fileInfo.LastWriteTime

let isModified getTimeLine getLastWriteTime filePath =
    (getLastWriteTime filePath) > (getTimeLine())

let handle getFsFiles isModified post () = 
    let isPost = getFsFiles() |> Seq.exists isModified
    
    if isPost then () |> post
