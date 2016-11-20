module ProcessRunner

open System.Diagnostics
open Common.ConsoleUtil

let run processStartInfo handleOutput =
    use proc = new Process(StartInfo = processStartInfo)
    proc.ErrorDataReceived.Add(fun d ->
        if d.Data <> null then writelineRed (d.Data))
    proc.OutputDataReceived.Add(fun d ->
        if d.Data <> null then handleOutput (d.Data))
    proc.EnableRaisingEvents <- true
    proc.Start() |> ignore
    proc.BeginOutputReadLine()
    proc.BeginErrorReadLine()
    proc.WaitForExit() |> ignore

