module DevRT.ShortsDict

let getShort = function
    | (word: string) when word.Length < 4 -> None
    | _ -> None
