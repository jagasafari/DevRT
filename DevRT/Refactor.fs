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

let processLines processSingleLine lines =
    let last = lines |> Array.last
    let trimed =
        lines
        |> Array.toSeq
        |> Seq.pairwise
        |> Seq.filter notEmptyPairOfLines
        |> Seq.map (fun (p, _) -> p |> processSingleLine )
    seq {
        yield! trimed
        if last |> notEmptyLine then yield last |> processSingleLine }

let getProcessSingleLine file =
    match file |> contains ".fs" with
    | true ->
        removeTrailingWhiteSpaces
        >> replaceLine replaceRegex getRegExReplacementForFSharp
    | false -> removeTrailingWhiteSpaces

let processFile readLines file =
    file |> readLines |> processLines (getProcessSingleLine file) |> Seq.toArray

let fileFilter exists file =
    (exists file) && (contains ".fs" file || contains ".cs" file)

let refactor processFile writeLines fileFilter (files: HashSet<string>) =
    files
    |> Seq.filter fileFilter //nothing processed don't rewrite the file
    |> Seq.iter(fun f -> writeLines f (processFile f))

let handle
    refactor
    (modifiedFilesSet: HashSet<string>) = function
    | RefactorModifiedFiles ->
        modifiedFilesSet |> refactor
        modifiedFilesSet.Clear()
    | QueueModifiedFile file -> file |> modifiedFilesSet.Add |> ignore
