module DevRT.FileUtil

let doCopy createPath get copy =
    get() |> Seq.iter (fun x -> x |> createPath |> copy x |> ignore)

let copyAllFiles createDirectory copyFiles copySubdirectories source dest =
    let rec copyAllFiles' source dest =
        dest |> createDirectory
        copyFiles source dest
        copySubdirectories copyAllFiles' source dest
    copyAllFiles' source dest

let deleteAllFiles exists deleteRecursive target =
    if target |> exists then target |> deleteRecursive

let cleanDirectory deleteAllFiles createDirectory deploymentDir =
    deploymentDir |> deleteAllFiles
    deploymentDir |> createDirectory


