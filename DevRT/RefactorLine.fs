module DevRT.RefactorLine

open DataTypes
open StringWrapper

let removeTrailingWhiteSpaces = trimEnd ' '

let replaceLine replace rules line =
    let rec accLine rules line =
        match rules with
        | (pattern, replacement)::tl ->
            accLine tl (replace pattern replacement line)
        | [] -> line
    accLine rules line

let likeLetRule short key =
    sprintf "(?<m1>^| )%s(?<m2> |$)" short,
    sprintf "${m1}%s${m2}" key

let likeFunRule short key =
    sprintf "\(%s " short, sprintf "(%s " key

let likeSeqRule short key =
    sprintf "(?<m1>^| )%s(?<m2> |$|\()" short,
    sprintf "${m1}%s${m2}" key

let likeType () =
    "(?<m1>[a-zA-Z]+):(?<m2>[a-zA-Z]+)",
    "${m1}: ${m2}"

let likeTupleType() =
    "(?<m1>[a-zA-Z]+)\*(?<m2>[a-zA-Z]+)",
    "${m1} * ${m2}"
let likeStringType() =
    "(?<m1>^| |<)st(?<m2> |$|\)|>)", "${m1}string${m2}"

let likeType2 () =
    "(?<m1>\S) (?<m2>[a-zA-Z]+)(?<m3>:) (?<m4>[a-zA-Z]+) ",
    "${m1} (${m2}${m3} ${m4}) "

let matchLineRefactorRule = function
    | ["LikeLet";short;key;_] -> likeLetRule short key
    | ["LikeFun";short;key;_] -> likeFunRule short key
    | ["LikeSeq";short;key;_] -> likeSeqRule short key
    | ["LikeType";_;_;_] -> likeType()
    | ["LikeTupleType";_;_;_] -> likeTupleType()
    | ["LikeStringType";_;_;_] -> likeStringType()
    | ["LikeType2";_;_;_] -> likeType2()
    | _ -> failwith "not known line refactor rule"
