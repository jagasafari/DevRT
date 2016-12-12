module ProcessStartInfoProvider

open System.Diagnostics
open System
open Common

let getProcessStartInfo fileName arguments workingDir =
    let psi = ProcessStartInfo()
    psi.FileName <- fileName
    psi.Arguments <- arguments
    psi.WorkingDirectory <- workingDir
    psi.UseShellExecute <- false
    psi.RedirectStandardOutput <- true
    psi.RedirectStandardError <- true
    psi.CreateNoWindow <- true
    psi.WindowStyle <- ProcessWindowStyle.Hidden
    psi

