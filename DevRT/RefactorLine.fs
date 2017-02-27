module DevRT.RefactorLine

open DataTypes
open StringWrapper

let removeTrailingWhiteSpaces = trimEnd ' '

let getSubstringReplacmentForFSharp () =
    []
    |> List.map ReplaceSubstr
