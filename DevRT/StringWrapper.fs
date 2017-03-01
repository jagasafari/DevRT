module DevRT.StringWrapper

open System
open System.Text.RegularExpressions

let replaceRegex pattern (replacement: string) (str: string) =
    let regex = Regex(pattern)
    regex.Replace(str, replacement)

let isRegexMatch pattern (str: string) =
    let regex = Regex(pattern)
    regex.IsMatch(str)
let contains substr (str: string) = str.Contains(substr)
let replace (pattern: string) replacement (str: string) = 
    str.Replace(pattern, replacement)

let containsCaseInsensitive (substr:string) (str: string) =
    str.ToLower().Contains(substr.ToLower())

let split c (str: string) = str.Split [|c|]
let (%%) pattern value = sprintf pattern value
let splitByString (pattern: string) (str: string) = str.Split([|pattern|], StringSplitOptions.None)
let startsWith pattern (str: string) = str.StartsWith pattern
let isNullOrWhiteSpace str = String.IsNullOrWhiteSpace str
let trimEnd c (str: string) = str.TrimEnd([|c|])
