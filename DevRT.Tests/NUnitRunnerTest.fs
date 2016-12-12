module NUnitRunnerTest

open System
open NUnitRunner
open Swensen.Unquote
open NUnit.Framework

let mutable testList = []
let add str = testList <- testList |> List.append [str] 
let getProcesses() = add "getProcesses"; [1..3]
let killProcess p = add "killProcess" 

[<TestCase(false)>]
let ``stop nunit process when the previous step failed`` previousStep =
    testList <- []
    let result = stopNunitProcess getProcesses killProcess previousStep
    result =! previousStep
    testList =! []    


[<TestCase(true)>]
let ``stop nunit process when the previous step completed`` previousStep =
    testList <- []
    let result = stopNunitProcess getProcesses killProcess previousStep
    result =! previousStep
    testList |> List.rev =! ["getProcesses";"killProcess";"killProcess";"killProcess"]    
