module DevRT.Tests.NUnitTests

open NUnit.Framework
open Swensen.Unquote
open NUnit
open Common
open DevRT.DataTypes
open DevRT.NUnit

[<Test>]
let ``handleFailureLineNumberInfo: parse failure output -> only line number info`` () =
    let output = "w DevRT.Tests.RefactorTests.Remove trailing blank lines: n blank lines -> file trimmed() w c:\DevRT\DevRT.Tests\RefactorTests.fs:wiersz 37"
    output |> parseFailureLineNumberInfo =! " c:\DevRT\DevRT.Tests\RefactorTests.fs:wiersz 37"

[<Test>]
let ``handleNUnitOutput: failure line number -> output parsed`` () =
    let mockAdd, mockResult = mock()
    let parseLineInfo _ = mockAdd "parsing"
    handleOutput
        shouldNotBeCalled shouldNotBeCalled parseLineInfo FailureLineInfo ""
    mockResult() =! ["parsing"]
