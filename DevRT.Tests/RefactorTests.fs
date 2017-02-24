module DevRT.Tests.RefactorTests

open System
open System.IO
open NUnit.Framework
open Swensen.Unquote
open DevRT.DataTypes
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

let getLines = Array.map (fun l -> l |> getLine)

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
    let lines' = DevRT.StringWrapper.split ',' lines |> getLines
    lines' |> trimEmptyLines |> Seq.length =! expected

[<Test>]
let ``experiment: laziness of seq`` () =
    let add, getResult = Common.mock()
    let nl l = add l; l
    let s = seq{yield nl "a"; yield nl "b"; yield nl "c"; yield nl "d"; yield nl "e"}
    let last = s|>Seq.last
    s
    |> Seq.map (fun l -> nl ("1" + l))
    |> Seq.pairwise
    |> Seq.map(fun (l,r)-> nl (l+r))
    |> Seq.toList
    |> ignore
    getResult() =! ["a";"b";"c";"d";"e";"a";"1a";"b";"1b";"1a1b";"c";"1c";"1b1c";"d";"1d";"1c1d";"e";"1e";"1d1e"]

[<Test>]
let ``experiment: laziness of seq when from array`` () =
    let add, getResult = Common.mock()
    let nl l = add l; l
    let s = [|nl "a"; nl "b"; nl "c"; nl "d"; nl "e"|]
    s|>Array.last|>ignore
    s|>Array.last|>ignore

    s
    |> Array.toSeq
    |> Seq.map (fun l -> nl ("1" + l))
    |> Seq.pairwise
    |> Seq.map(fun (l,r)-> nl (l+r))
    |> Seq.toList
    |> ignore
    getResult() =! ["a";"b";"c";"d";"e";"1a";"1b";"1a1b";"1c";"1b1c";"1d";"1c1d";"1e";"1d1e"]

let getNonEmptySet() =
    let nonEmptySet = Collections.Generic.HashSet<string>()
    nonEmptySet.Add "kdj" |> ignore
    nonEmptySet

[<Test>]
let ``handle: Refactor msg -> clears set of modified files`` () =
    let testSet = (getNonEmptySet())
    handle testSet RefactorModifiedFiles
    testSet.Count =! 0

[<Test>]
let ``handle: queue msg -> adds file`` () =
    let testSet = (getNonEmptySet())
    handle testSet (QueueModifiedFile "adding")
    testSet.Count =! 2
    testSet |> Seq.toList =! ["kdj";"adding"]
