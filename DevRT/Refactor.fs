module DevRT.Refactor

open System
open DataTypes
open StringWrapper

let notNewLine = (<>) Environment.NewLine

let notEmptyLine line =
    (line |> notNewLine) && (line |> isNullOrWhiteSpace |> not)

let notEmptyPairOfLines (l1, l2) = l1 |> notEmptyLine || l2 |> notEmptyLine

let handleLastLine last lines =
    match last |> notEmptyLine with
    | true -> lines @ [last] | false -> lines

let trimEmptyLines lines =
    let last = lines |> Seq.last
    lines
    |> Seq.pairwise
    |> Seq.filter notEmptyPairOfLines
    |> Seq.map (fun (p, _) -> p)
    |> Seq.toList
    |> handleLastLine last

let getRefactorHandle = function
    | RefactorModifiedFiles -> () | QueueModifiedFile file -> ()
