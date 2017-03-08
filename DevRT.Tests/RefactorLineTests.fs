module DevRT.Tests.RefactorLineTests

open System
open NUnit.Framework
open Swensen.Unquote
open DevRT.RefactorLine
open DevRT.StringWrapper

[<TestCase("", "")>]
[<TestCase("    ", "")>]
[<TestCase("ssds    ", "ssds")>]
[<TestCase(" a    ", " a")>]
let ``removeTrailingWhitespaces: cases`` line expected =
    line |> removeTrailingWhiteSpaces =! expected
