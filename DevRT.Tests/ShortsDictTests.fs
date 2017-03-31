module DevRT.Tests.ShortsDictTests

open NUnit.Framework
open Swensen.Unquote
open DevRT.ShortsDict

[<TestCase("a")>]
[<TestCase("let")>]
let ``getShort: less than 4 letter word -> not added`` word =
    word |> getShort =! None

[<TestCase()>]

let ``getShort: one part word -> mapped to first letter``
    word expected =
    word |> getShort =! (Some expected)
