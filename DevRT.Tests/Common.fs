module DevRT.Tests.Common

let mock() =
    let mutable result = []
    let add str = result <- Seq.append result [str] |> Seq.toList
    let getResult() = result
    (add, getResult)

let shouldNotBeCalled _ = failwith "test fails"
