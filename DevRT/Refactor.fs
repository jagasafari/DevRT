module DevRT.Refactor

open System
open System.Collections.Generic
open System.Linq
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
    let originalLines = file |> readLines
    let processedLines = originalLines |> processLines (getProcessSingleLine file) |> Seq.toArray
    match ((set originalLines) - (set processedLines), ((originalLines.Count()) <> (processedLines.Count()))) with
    | (s, false) when s.IsEmpty -> None
    | s -> Some processedLines

let fileFilter exists file =
    (exists file) && (isRegexMatch ".fs$" file || isRegexMatch ".cs$" file)

let refactor processFile writeLines file =
    file
    |> processFile
    |> Option.fold(
        fun _ lines -> lines |> writeLines file) ()

let handle refactor fileFilter = function
    | file when fileFilter file -> refactor file | _ -> ()
