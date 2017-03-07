module DevRT.ProcessWrapper

open System
open System.Diagnostics
open System.Threading

let killProcess sleep (p: Diagnostics.Process) =
    p.Kill(); sleep()
let getProcesses = Process.GetProcessesByName
let sleep (miliseconds:int) () = Thread.Sleep miliseconds
let stopProcess sleepMiliseconds name =
    name
    |> getProcesses
    |> Seq.iter (sleepMiliseconds |> sleep |> killProcess)
