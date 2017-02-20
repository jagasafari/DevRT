module DevRT.StringWrapper

open System

let contains substr (str: string) = str.Contains(substr)

let containsCaseInsensitive (substr:string) (str: string) =
    str.ToLower().Contains(substr.ToLower())

let split c (str: string) = str.Split [|c|]
let (%%) pattern value = sprintf pattern value
let splitByString (pattern: string) (str: string) = str.Split([|pattern|], StringSplitOptions.None)
let startsWith pattern (str: string) = str.StartsWith pattern
let isNullOrWhiteSpace str = String.IsNullOrWhiteSpace str
