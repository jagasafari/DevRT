module Common.Bind

open Common

let bindResult func input =
    try
        let result = func input
        Success result
    with | ex -> Failure ex



