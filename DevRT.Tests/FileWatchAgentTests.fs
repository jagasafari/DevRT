module DevRT.Tests.FileWatchTests

open NUnit.Framework
open Swensen.Unquote
open DevRT.FileWatch

let getNow () = System.DateTime(2016, 11, 21, 13, 10, 55)

[<TestCase(20000, 10, 34, 900)>]
[<TestCase(55000, 9, 59, 900)>]
[<TestCase(57000, 9, 57, 900)>]
[<TestCase(20, 10, 54, 880)>]
[<TestCase(55, 10, 54, 845)>]
[<TestCase(57, 10, 54, 843)>]
let ``getTimeLine, last n seconds, seconds deducted``
    deduction minutes seconds milliseconds =
    let result = getTimeLine getNow deduction
    result.Minute =! minutes
    result.Second =! seconds
    result.Millisecond =! milliseconds

[<Test>]
let ``handle: no file modified -> no posts`` () =
    let add, getResult = Common.mock()
    handle
        (fun () -> seq { yield "whatever" })
        (fun _ -> false)
        (fun modification -> add (sprintf "%A" modification))
        ()
    getResult() =! ["<null>"]

[<Test>]
let ``handle: file modifies -> posting`` () =
    let add, getResult = Common.mock()
    let file = "file"
    handle
        (fun () -> seq { yield file })
        (fun _ -> true)
        (fun f -> sprintf "%A" f |> add)
        ()
    getResult() =! ["Some \"file\""]
