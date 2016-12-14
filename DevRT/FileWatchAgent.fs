module FileWatchAgent

let getFiles extensions dir =
    seq {
        for ext in extensions do
            let filePattern = sprintf "*.%s" ext
            yield! System.IO.Directory.GetFiles(
                dir, filePattern, System.IO.SearchOption.AllDirectories) 
    }

let getNow() = System.DateTime.Now

let getTimeLine (getNow: unit -> System.DateTime) forThePastSeconds =
    getNow().AddSeconds( (-forThePastSeconds) |> float)

let getLastWriteTime filePath =
    let fileInfo = System.IO.FileInfo filePath
    fileInfo.LastWriteTime

let isModified getTimeLine getLastWriteTime filePath =
    (getLastWriteTime filePath) > (getTimeLine())

let handle getFiles isModified post () = 
    let isPost = getFiles() |> Seq.exists isModified
    
    if isPost then () |> post
