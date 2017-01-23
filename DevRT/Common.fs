module DevRT.Common

type Result<'a, 'b> = | Success of 'a | Failure of 'b

let bindResult f input =
    try input |> f |> Success with ex -> Failure ex
