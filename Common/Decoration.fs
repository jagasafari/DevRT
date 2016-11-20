module Common.Decoration

open System.Diagnostics

let timer longCommand =
    let timer = Stopwatch.StartNew()
    longCommand()
    timer.Stop()
    timer.ElapsedMilliseconds
