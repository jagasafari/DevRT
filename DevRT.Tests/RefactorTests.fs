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
    let len = rnd.Next(1, max)
    len |> randomString

type TestFile() =
    let filePath = Guid.NewGuid() |> sprintf "%A.txt"
    let file = filePath |> File.CreateText
    do 255 |> randomStringRandomLength |> file.WriteLine
    do Environment.NewLine |> file.WriteLine
    do file.Close()
    member x.GetLines() = filePath |> File.ReadLines
    interface IDisposable with
        member x.Dispose() =
            filePath |> File.Exists =! true
            filePath |> File.Delete
            filePath |> File.Exists =! false
[<Test>]
let ``Remove trailing blank lines: n blank lines -> file trimmed`` () =
    use tf = new TestFile()
    let allLinesTrimmed = tf.GetLines() |> trimEndEmptyLines
    allLinesTrimmed |> Seq.length =! 1

[<Test>]
let ``pairwise: result -> size 2 gives size 1`` () =
    let s = seq { yield "abc"; yield Environment.NewLine }
    s |> Seq.pairwise |> Seq.toList =! [("abc",Environment.NewLine)]
