module DevRT.Tests.CiStepsRunAgentTests

open NUnit.Framework
open Swensen.Unquote
open DevRT.CiStepsRunAgent

[<Test>]
let ``handle, unit as an input, all steps executed`` () =
    let tr = Common.TestResult()
    let runMsBuild() = tr.Add "run ms build" 
    let runNUnit() = tr.Add "run nunit"
    handle runMsBuild runNUnit ()
    tr.Result =! ["run ms build"; "run nunit"]
