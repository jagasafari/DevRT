module DevRT.Tests.RunTests

open System
open NUnit.Framework
open Swensen.Unquote
open Common
open DevRT.Run

[<TestCase(0)>]
[<TestCase(1)>]
[<TestCase(9)>]
let ``run: n times -> file watch executed n times`` n =
    let mockAdd, mockResult = mock()
    let post() = mockAdd "posting"
    let postToRefactor key =
        key |> sprintf "posting key: %A" |> mockAdd
    run 1 post postToRefactor |> Seq.take n |> Seq.toList |> ignore
    mockResult().Length =! n

[<Test>]
let ``action: checking key -> post to run ci called`` () =
    let mockAdd, mockResult = mock()
    let postToRunCi() = mockAdd  "running ci"
    action postToRunCi shouldNotBeCalled ()
    mockResult() =! ["running ci"]
