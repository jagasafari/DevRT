module DevRT.Tests.NUnitRunnerTest

open System
open Swensen.Unquote
open NUnit.Framework
open DevRT
open NUnitRunner

[<Test>]
let ``stop nunit process when the previous step completed`` () =
    let testResult = Common.TestResult()
    let getProcesses() = testResult.Add "getProcesses"; [1..3]
    let killProcess p = testResult.Add "killProcess" 
    stopNunitProcess getProcesses killProcess |> ignore
    testResult.Result 
    =! ["getProcesses";"killProcess";"killProcess";"killProcess"]    
