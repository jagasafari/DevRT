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
    let postToRefactor key =
        key |> sprintf "posting key: %A" |> add
    run 1 post postToRefactor |> Seq.take n |> Seq.toList |> ignore
    getResult().Length =! n

[<Test>]
let ``action: checking key -> post to run ci called`` () =
    let add, getResult = mock()
    let postToRunCi() = add  "running ci"
    action postToRunCi shouldNotBeCalled ()
    getResult() =! ["running ci"]
