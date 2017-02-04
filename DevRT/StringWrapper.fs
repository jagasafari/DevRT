module DevRT.StringWrapper

let contains substr (str: string) = str.Contains(substr)
let split c (str: string) = str.Split [|c|]
let (%%) pattern value = sprintf pattern value
