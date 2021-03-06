module DevRT.Tests.RefactorTests

open System
open System.Configuration
open System.IO
open NUnit.Framework
open Swensen.Unquote
open DevRT.DataTypes
open DevRT.IOWrapper
open DevRT.Refactor
open DevRT.RefactorLine
open DevRT.StringWrapper

let randomString size =
    let rnd = Random()
    let getLetter() = (char)((int)'a' + rnd.Next(0, 26))
    [1..size]
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
let ``notEmptyPairsOfLines: cases`` let1 let2 expected =
    (let1, let2) |> notEmptyPairOfLines =! expected

let processLines' = processLines removeTrailingWhiteSpaces

[<TestCase("randomString, emptyLine", 1)>]
[<TestCase("randomString, emptyLine, randomString", 3)>]
[<TestCase("randomString, emptyLine, emptyLine", 1)>]
[<TestCase("randomString, emptyLine, emptyLine, emptyLine", 1)>]
let ``Remove trailing blank lines -> file trimmed`` lines expected =
    lines |> split ',' |> getTestLines |> processLines' |> Seq.length =! expected

[<TestCase("abc", "abc")>]
[<TestCase("abc  ", "abc")>]
[<TestCase("a bc  ", "a bc")>]
[<TestCase("   abc", "   abc")>]
let ``processLines: cases -> trailing whitespaces removed`` line expected =
    [|line|] |> processLines' |> Seq.toList =! [expected]

[<TestCase(false, "whatever", false)>]
[<TestCase(false, "abc.fs", false)>]
[<TestCase(false, "abc.fs.xyz", false)>]
[<TestCase(true, "whatever", false)>]
[<TestCase(true, "abc.fs", true)>]
[<TestCase(true, "abc.fs.xyz", false)>]
let ``fileFilter: cases`` exists file expected =
    fileFilter (fun _ -> exists) file =! expected

[<TestCase("filtering")>]
let ``refactor: none existing files -> noting processed`` expected =
    let add, getResult = Common.mock()
    handle
        (fun file outFile ->
            add (sprintf "processing %s" file))
        (fun _ -> add "filtering"; false)
        true
        "efefw"
    getResult() =! [expected]

[<Test>]
let ``emptyLineAbove: no lines`` () =
    emptyLineAbove [] [] =! []

[<Test>]
let ``emptyLineAbove: one line`` () =
    emptyLineAbove ["a"] [] =! ["a"]

[<Test>]
let ``emptyLineAbove: two lines`` () =
    emptyLineAbove ["a";"b"] [] =! ["a";"b"]

[<TestCase("let ", "b")>]
[<TestCase("dfj", "b")>]
[<TestCase("", "l ")>]
[<TestCase("", "sdsl ")>]
[<TestCase("", "")>]
let ``emptyLineAbove: two lines, no append`` inPrev inCurr =
    emptyLineAbove [inPrev; inCurr] [] =! [inPrev; inCurr]

[<TestCase("a","l ")>]
let ``emptyLineAbove: two lines, append`` inPrev inCurr =
    emptyLineAbove [inPrev; inCurr] [] =! [inPrev; ""; inCurr]
