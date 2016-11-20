module Common.FileUtil
open System
open System.IO

let rec copyAllFiles source target =
    Directory.CreateDirectory(target) |> ignore
    Directory.GetFiles(source)
    |> Seq.iter( fun x -> File.Copy(x, Path.Combine(target, Path.GetFileName(x))))

    Directory.GetDirectories(source)
    |> Seq.iter(fun x -> copyAllFiles x (Path.Combine(target, (Path.GetFileName(x)))))

let deleteAllFiles target = 
    if Directory.Exists(target) then Directory.Delete(target, true)

