module DevRT.Tests.ProcessWrapperTests

open System
open Swensen.Unquote
open System
open Common
open NUnit.Framework
open DevRT.ProcessWrapper

[<Test>]
let ``stop nunit process when the previous step completed`` () =
    let testResult = TestResult()
    let getProcesses() = testResult.Add "getProcesses"; [1..3]
    let killProcess p = testResult.Add "killProcess"
    stopNunitProcess getProcesses killProcess |> ignore
    testResult.Result
    =! ["getProcesses";"killProcess";"killProcess";"killProcess"]
