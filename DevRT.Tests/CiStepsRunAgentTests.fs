module DevRT.CiStepsRunAgentTests

open NUnit.Framework
open Swensen.Unquote
open DevRT.CiStepsRunAgent

type TestResult() =
    let mutable result = []
    member x.Add str = result <- Seq.append result [str] |> Seq.toList
    member x.Result with get() = result

[<Test>]
let ``handle, unit as an input, all steps executed`` () =
    let tr = TestResult()
    let runMsBuild() = tr.Add "run ms build" 
    let runNUnit() = tr.Add "run nunit"
    let handle' = handle runMsBuild runNUnit
    //act
    () |> handle'
    //assert
    tr.Result =! ["run ms build"; "run nunit"]
