module DevRT.Tests.RefactorLineTests

open System
open System.Configuration
open NUnit.Framework
open Swensen.Unquote
open DevRT.IOWrapper
open DevRT.RefactorLine
open DevRT.StringWrapper

[<TestCase("", "")>]
[<TestCase("    ", "")>]
[<TestCase("ssds    ", "ssds")>]
[<TestCase(" a    ", " a")>]
let ``removeTrailingWhitespaces: cases`` line expected =
    line |> removeTrailingWhiteSpaces =! expected

[<TestCase("d DevRT.FileUtil", "d, ,DevRT,.,FileUtil")>]
[<TestCase("","")>]
[<TestCase("l doCopy createPath get copy e",
    "l, ,doCopy, ,createPath, ,get, ,copy, ,e")>]
[<TestCase("    get g",", ,, ,, ,, ,get, ,g")>]
let ``chopLine: cases`` ( line: string ) chops =
    let expected = ( chops |> split ',' )
    let result = line |> chopLine
    result =! expected

let getRules() =
    ConfigurationManager.AppSettings.["rules"]
    |> getRules readAllLines

[<TestCase("d DevRT.FileUtil", "module DevRT.FileUtil")>]
[<TestCase("","")>]
[<TestCase("l doCopy createPath get copy e",
    "let doCopy createPath get copy =")>]
[<TestCase("    get g","    get ()")>]
let ``replaceAbrev: cases`` line resultedLine =
    line |> replaceAbrev ( getRules() ) =! resultedLine

[<TestCase ("gf", " let f = ()","gf" ) >]
[<TestCase ("", " let f = ()","" ) >]
[<TestCase ("", "let f = ()","" ) >]
[<TestCase ("", "l f = ()", "") >]
let ``appendIfMostOuterLet : no append`` prev next expeted =
    appendIfMostOuterLet ( prev, next ) =! [expeted]

[<TestCase ("gfr", "let f = ()","gfr" ) >]
[<TestCase ("heu", "l f = ()", "heu") >]
let ``appendIfMostOuterLet : append`` prev next expeted =
    appendIfMostOuterLet ( prev, next ) =! ""::[expeted]
