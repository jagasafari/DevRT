module DevRT.Tests.NUnitRunnerTest

open System
open NUnitRunner
open Swensen.Unquote
open NUnit.Framework


[<Test>]
let ``stop nunit process when the previous step completed`` () =
    let testResult = Common.TestResult()
    let getProcesses() = testResult.Add "getProcesses"; [1..3]
    let killProcess p = testResult.Add "killProcess" 
    let result = stopNunitProcess getProcesses killProcess
    testResult.Result 
    =! ["getProcesses";"killProcess";"killProcess";"killProcess"]    
