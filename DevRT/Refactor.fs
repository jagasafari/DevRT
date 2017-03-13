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

let processLines processLine lines =
    let last = lines |> Array.last
    let trimed =
        lines
        |> Array.toSeq
        |> Seq.pairwise
        |> Seq.filter notEmptyPairOfLines
        |> Seq.map (fun (p1, _) -> p1 |> processLine )
    seq {
        yield! trimed
        if last |> notEmptyLine then
            yield last |> processLine }

let processLineFsFile rules =
    removeTrailingWhiteSpaces
    >> replaceLine
        replaceRegex (rules |> List.map matchLineRefactorRule)

let processLineCsFile = removeTrailingWhiteSpaces

let difference (original, processed) =
    let setDiff = (set original) - (set processed)
    let lineNumDiff =
        (original.Count()) <> (processed.Count())
    setDiff, lineNumDiff

let processLine rules = function
    | file when file |> contains ".fs" ->
        processLineFsFile rules
    | _ -> processLineCsFile

let processFile processLine read processLines file =
    let original = file |> read
    let processed =
        original
        |> processLines (file |> processLine)
        |> Seq.toArray
    match (original, processed) |> difference with
    | (s, false) when s.IsEmpty -> None
    | sprintf -> Some processed

let getRules read csvPath =
    IOWrapper.combine csvPath "lineRefactor.csv"
    |> read
    |> Array.map (split ',' >> Array.toList)
    |> Array.toList

let fileFilter exists file =
    (exists file) && (isRegexMatch ".fs$" file || isRegexMatch ".cs$" file)

let refactor processFile writeLines file =
    file
    |> processFile
    |> Option.fold(fun _ lines -> lines |> writeLines file) ()

let handle refactor fileFilter = function
    | file when fileFilter file -> refactor file | _ -> ()
