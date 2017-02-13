module DevRT.Tests.ProcessWrapperTests

open System
open Swensen.Unquote
open System
open Common
open NUnit.Framework
open DevRT.ProcessWrapper

[<Test>]
let ``stop nunit process when the previous step completed`` () =
    let mockAdd, mockResult = mock()
    let getProcesses() = mockAdd "getProcesses"; [1..3]
    let killProcess p = mockAdd "killProcess"
    stopNunitProcess getProcesses killProcess |> ignore
    mockResult()
    =! ["getProcesses";"killProcess";"killProcess";"killProcess"]
