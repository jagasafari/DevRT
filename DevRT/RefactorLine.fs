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

let likeLetRule short key =
    sprintf "(?<m1>^| )%s(?<m2> |$)" short,
    sprintf "${m1}%s${m2}" key

let getRegExReplacementForFSharp rules () =
    rules
    |> List.map (fun rule ->
        match rule with
        | ["LikeLet";short;key;_] -> likeLetRule short key
        // like (fun ; like Array.m
        | _ -> failwith "not known line refactor rule")
