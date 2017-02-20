module DevRT.Tests.StringWrapperTests

open NUnit.Framework
open Swensen.Unquote
open DevRT
open StringWrapper

[<TestCase("abc", "abc", true)>]
[<TestCase("", "", true)>]
[<TestCase("dfgh", "gh", true)>]
[<TestCase("dfgh", "", true)>]
[<TestCase("dfgh", "f", true)>]
[<TestCase("dfgh", "FGh", true)>]
[<TestCase("D fgh", "d F", true)>]
[<TestCase("Df gh", "df", true)>]
[<TestCase("abc", "abcabc", false)>]
[<TestCase("", "ujsd jk", false)>]
[<TestCase("dfgh", "ghh", false)>]
[<TestCase("dfgh", "z", false)>]
[<TestCase("dfgh", "fh", false)>]
[<TestCase("dfgh", "FADDGh", false)>]
[<TestCase("D fgh", "d F ", false)>]
[<TestCase("Df gh", "daf", false)>]
let ``containsCaseInsensitive`` str1 str2 expected =
    containsCaseInsensitive str2 str1 =! expected
