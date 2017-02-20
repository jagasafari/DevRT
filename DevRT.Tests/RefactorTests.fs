module DevRT.Tests.RefactorTests

open System
open System.IO
open NUnit.Framework
open Swensen.Unquote
open DevRT.Refactor

let randomString n =
    let rnd = Random()
    let getLetter() = (char)((int)'a' + rnd.Next(0, 26))
    [1..n]
    |> List.map (fun _ -> getLetter() |> string)
    |> List.fold (fun acc el -> acc + el) String.Empty

let randomStringRandomLength max =
    let rnd = Random()
    rnd.Next(1, max) |> randomString

let getLine = function
    | "t" -> 255 |> randomStringRandomLength | _ -> String.Empty

let getLines = Seq.map (fun l -> l |> getLine)

[<TestCase("", false)>]
[<TestCase("  ", false)>]
[<TestCase("hvgh", true)>]
let ``notEmptyLine: cases`` line expectedResult =
    line |> notEmptyLine =! expectedResult

[<Test>]
let ``notEmptyLine: new line`` () =
    (Environment.NewLine |> notEmptyLine) =! false

[<TestCase("","",false)>]
[<TestCase("fschs","fjhskhf sk",true)>]
[<TestCase("","fklhss",true)>]
[<TestCase("dgfhs  ha","",true)>]
[<TestCase("      ","",false)>]
[<TestCase("      ","    ",false)>]
let ``notEmptyPairsOfLines: cases`` l1 l2 expected =
    (l1, l2) |> notEmptyPairOfLines =! expected

[<TestCase("t,e", 1)>]
[<TestCase("t,e,t", 3)>]
[<TestCase("t,e,e", 1)>]
[<TestCase("t,e,e,e", 1)>]
let ``Remove trailing blank lines: n blank lines -> file trimmed`` lines expected =
    let lines' = DevRT.StringWrapper.split ',' lines |> Array.toSeq |> getLines
    lines' |> trimEmptyLines |> Seq.length =! expected

[<Test>]
let ``pairwise: result -> size 2 gives size 1`` () =
    let s = seq { yield "abc"; yield Environment.NewLine }
    s
    |> trimEmptyLines
    |> List.length =! 1

