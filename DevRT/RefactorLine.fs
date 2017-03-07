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

let getRegExReplacementForFSharp () =
    [
        ("(^| )l ", "let ")
        ("(^| )o ", "open ")
        ("(^| )t ", "type ")
 //       ("(?<m>[a-zA-Z]) p ", "${m} |> ")
    ]
