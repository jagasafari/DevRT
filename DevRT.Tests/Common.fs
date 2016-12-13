module DevRT.Tests.Common

type TestResult() =
    let mutable result = []
    member x.Add str = result <- Seq.append result [str] |> Seq.toList
    member x.Result with get() = result
