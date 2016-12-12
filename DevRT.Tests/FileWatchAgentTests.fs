module FileWatchAgentTests

open NUnit.Framework
open Swensen.Unquote
open FileWatchAgent

let getNow () = System.DateTime(2016, 11, 21, 13, 10, 55)

[<TestCase(20, 10, 35)>]
[<TestCase(55, 10, 0)>]
[<TestCase(57, 9, 58)>]
let ``get time line, last n seconds, seconds deducted`` 
    deduction minutes seconds =
    let result = getTimeLine getNow deduction 
    result.Minute =! minutes
    result.Second =! seconds



