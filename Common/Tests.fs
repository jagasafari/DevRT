module Common.Tests

open NUnit.Framework
open Swensen.Unquote

[<Test>]
let ``function throws, then can access stack trace from fauilure result`` () =
    let result = Bind.bindResult (fun () -> failwith "xyz") ()
    match result with
    | Success _ -> failwith ""
    | Failure e -> e.Message =! "xyz"