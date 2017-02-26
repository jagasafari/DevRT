module DevRT.Refactor

open System
open System.Collections.Generic
open DataTypes
open RefactorLine
open StringWrapper

let notNewLine = (<>) Environment.NewLine

let notEmptyLine line =
    (line |> notNewLine) && (line |> isNullOrWhiteSpace |> not)

let notEmptyPairOfLines (l1, l2) = l1 |> notEmptyLine || l2 |> notEmptyLine

let processLines lines =
    let last = lines |> Array.last
    let trimed =
        lines
        |> Array.toSeq
        |> Seq.pairwise
        |> Seq.filter notEmptyPairOfLines
        |> Seq.map (fun (p, _) -> p |> removeTrailingWhiteSpaces)
    seq {
        yield! trimed
        if last |> notEmptyLine then yield last |> removeTrailingWhiteSpaces}

let processFile readLines = readLines >> processLines >> Seq.toArray

let refactor processFile writeLines exists (files: HashSet<string>) =
    files
    |> Seq.filter exists
    |> Seq.iter(fun f -> writeLines f (processFile f))

let handle
    refactor
    (modifiedFilesSet: HashSet<string>) = function
    | RefactorModifiedFiles ->
        modifiedFilesSet |> refactor
        modifiedFilesSet.Clear()
    | QueueModifiedFile file -> file |> modifiedFilesSet.Add |> ignore
