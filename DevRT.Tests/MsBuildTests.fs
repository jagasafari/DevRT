module DevRT.Tests.MsBuildTests

open NUnit.Framework
open Swensen.Unquote
open DevRT.MsBuild

[<TestCase("msbuild success", false)>]
[<TestCase("MSBUILD: error", true)>]
[<TestCase("MSBUILD : error", true)>]
let ``isMsBuildError: cases`` output expected =
    isMsBuildError output =! expected
