module DevRT.RefactorLine

open DataTypes
open StringWrapper

let removeTrailingWhiteSpaces = trimEnd ' '

let removeInnerSpaces line = line

let replaceLine replace getDefinitions line =
    let rec accLine definitionList li =
        match definitionList with
        | (pattern, r)::tl -> accLine tl (replace pattern r li)
        | [] -> li
    accLine (getDefinitions()) line

let getRegExReplacementForFSharp () =
    [
        ("(^| )l ", "let ")
        ("(^| )o ", "open ")
        ("(^| )t ", "type ")
        ("(?<m>\S) -> ", "${m} -> ")
        (" -> (?<m>\S)", " -> ${m}")
        ("(?<m>[a-zA-Z]) p ", "${m} |> ")
    ]
