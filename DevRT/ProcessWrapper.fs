module DevRT.ProcessWrapper

open System
open System.Diagnostics
open System.Threading

let killProcess sleep (p: Diagnostics.Process) = p.Kill(); sleep()

let getProcesses name () = Process.GetProcessesByName name

let sleep (miliseconds:int) () = Thread.Sleep miliseconds

let stopNunitProcess getProcesses killProcess = getProcesses() |> Seq.iter killProcess

