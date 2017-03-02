module DevRT.Tests.RefactorTests

open System
open System.IO
open NUnit.Framework
open Swensen.Unquote
open DevRT.DataTypes
open DevRT.Refactor
open DevRT.RefactorLine
open DevRT.StringWrapper

let randomString n =
    let rnd = Random()
    let getLetter() = (char)((int)'a' + rnd.Next(0, 26))
    [1..n]
    |> List.map (fun _ -> getLetter() |> string)
    |> List.fold (fun acc el -> acc + el) String.Empty

let randomStringRandomLength max =
    let rnd = Random()
    rnd.Next(2, max) |> randomString

let getLine = function
    | "randomString"|" randomString" -> 5 |> randomStringRandomLength
    | "emptyLine" -> String.Empty
    | _ -> String.Empty

let getTestLines = Array.map getLine

[<TestCase("", false)>]
[<TestCase("  ", false)>]
[<TestCase("hvgh", true)>]
let ``notEmptyLine: cases`` line expectedResult =
    line |> notEmptyLine =! expectedResult

[<Test>]
let ``notEmptyLine: new line`` () =
    (Environment.NewLine |> notEmptyLine) =! false

[<TestCase("", "", false)>]
[<TestCase("fschs", "fjhskhf sk", true)>]
[<TestCase("", "fklhss", true)>]
[<TestCase("dgfhs  ha", "", true)>]
[<TestCase("      ", "", false)>]
[<TestCase("      ", "    ", false)>]
let ``notEmptyPairsOfLines: cases`` l1 l2 expected =
    (l1, l2) |> notEmptyPairOfLines =! expected

let pl = processLines removeTrailingWhiteSpaces
[<TestCase("randomString, emptyLine", 1)>]
[<TestCase("randomString, emptyLine, randomString", 3)>]
[<TestCase("randomString, emptyLine, emptyLine", 1)>]
[<TestCase("randomString, emptyLine, emptyLine, emptyLine", 1)>]
let ``Remove trailing blank lines: n blank lines -> file trimmed`` lines expected =
    lines |> split ',' |> getTestLines |> pl |> Seq.length =! expected

[<TestCase("abc", "abc")>]
[<TestCase("abc  ", "abc")>]
[<TestCase("a bc  ", "a bc")>]
[<TestCase("   abc", "   abc")>]
let ``processLines: cases -> trailing whitespaces removed`` line expected =
    [|line|] |> pl |> Seq.toList =! [expected]

[<TestCase("filtering")>]
let ``refactor: none existing files -> noting processed`` expected =
    let add, getResult = Common.mock()
    handle
        (fun f -> add (sprintf "processing %s" f))
        (fun _ -> add "filtering"; false)
        "efefw"
    getResult() =! [expected]

[<TestCase(false, "whatever", false)>]
[<TestCase(false, "abc.fs", false)>]
[<TestCase(false, "abc.fs.xyz", false)>]
[<TestCase(true, "whatever", false)>]
[<TestCase(true, "abc.fs", true)>]
[<TestCase(true, "abc.fs.xyz", false)>]
let ``fileFilter: cases`` exists file expected =
    fileFilter (fun _ -> exists) file =! expected
