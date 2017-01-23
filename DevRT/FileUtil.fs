module DevRT.FileUtil

open System
open IOWrapper
open DataTypes

let doCopy createPath get copy =
    get() |> Seq.iter (fun x -> x |> createPath |> copy x |> ignore)

let rec copyAllFiles source dest =
    dest |> createDirectory

    let createPath' = dest |> createPath
    doCopy createPath' (source |> getFiles) copyFile
    doCopy createPath' (source |> getDirectories) copyAllFiles

let deleteAllFiles target = 
    if target |> exists then target |> deleteRecursive
