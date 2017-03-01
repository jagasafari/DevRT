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
    let add, getResult = mock()
    let post() = add "posting"
    run 1 post |> Seq.take n |> Seq.toList |> ignore
    getResult().Length =! n

[<Test>]
let ``action: checking key -> post to run ci called`` () =
    let add, getResult = mock()
    let postToRunCi() = add  "running ci"
    action postToRunCi ()
    getResult() =! ["running ci"]
