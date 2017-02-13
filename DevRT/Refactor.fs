module DevRT.Refactor

open System

let notEmptyLine line = line = Environment.NewLine |> not

let trimEndEmptyLines lines =
    lines
    |> Seq.pairwise
    |> Seq.filter (fun (p, n) -> p |> notEmptyLine || n |> notEmptyLine)
    |> Seq.map (fun (p, _) -> p)

