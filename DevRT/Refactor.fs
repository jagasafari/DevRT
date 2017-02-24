module DevRT.Refactor

open System
open System.Collections.Generic
open DataTypes
open StringWrapper

let notNewLine = (<>) Environment.NewLine

let notEmptyLine line =
    (line |> notNewLine) && (line |> isNullOrWhiteSpace |> not)

let notEmptyPairOfLines (l1, l2) = l1 |> notEmptyLine || l2 |> notEmptyLine

let trimEmptyLines lines =
    let last = lines |> Array.last
    let trimed =
        lines
        |> Array.toSeq
        |> Seq.pairwise
        |> Seq.filter notEmptyPairOfLines
        |> Seq.map (fun (p, _) -> p)
    seq { yield! trimed; if last |> notEmptyLine then yield last }

let refactor readLines writeLines (files: HashSet<string>) =
    files
    |> Seq.iter(fun f ->
                let lines = f |> readLines
                let refactored = lines |> trimEmptyLines |> Seq.toArray
                writeLines f refactored)
let handle
    (modifiedFilesSet: HashSet<string>) = function
    | RefactorModifiedFiles ->
        modifiedFilesSet
        |> refactor IOWrapper.readAllLines IOWrapper.writeAllLines
        modifiedFilesSet.Clear()
    | QueueModifiedFile file -> file |> modifiedFilesSet.Add |> ignore