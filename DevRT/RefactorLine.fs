module DevRT.RefactorLine

open DataTypes
open StringWrapper

let removeTrailingWhiteSpaces = trimEnd ' '

let replaceLine replace getDefinitions line =
    let rec accLine definitionList line =
        match definitionList with
        | (pattern, replacement)::tl ->
            accLine tl (replace pattern replacement line)
        | [] -> line
    accLine (getDefinitions()) line

let  getRegExReplacementForFSharp read () =
    @"c:\run\console\lineRefactor.csv" |> read
    |> Array.map (split ',' >> Array.toList)
    |> Array.map (fun rule ->
        match rule with
        | ["Shorten";short;key] ->
            (sprintf "(^| )%s " short, sprintf "%s " key)
        | _ -> failwith "not known line refactor rule")
    |> Array.toList
 //       ("(?<m>[a-zA-Z]) p ", "${m} |> ")
