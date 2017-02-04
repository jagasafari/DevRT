module DevRT.Tests.FileUtilTests

open NUnit.Framework
open Swensen.Unquote
open Common
open DevRT.FileUtil

let doCopyTest elements =
    let result = TestResult()
    let createPath x =
        result.Add (sprintf "createPath %s" x)
        "y"
    let copy x y = result.Add (sprintf "copy %s %s" x y)
    let get = fun () -> elements
    doCopy createPath get copy
    result.Result

[<Test>]
let ``doCopy when nothing to copy`` () =
    [] |> doCopyTest =! []

[<Test>]
let ``doCopy when one element`` () =
    ["1"] |> doCopyTest =! ["createPath 1"; "copy 1 y"]

let copyAllFilesTest recLevel =
    let mutable rl = recLevel
    let result = TestResult()
    let createDirectory dest = result.Add (sprintf "createdirectory %d" dest)
    let copyFiles source dest = result.Add (sprintf "copyFiles %d %d" source dest)
    let copySubdirectories cf source dest =
        match rl with | l when l > 0 -> rl <- (rl - 1); cf rl rl | _ -> ()
    copyAllFiles createDirectory copyFiles copySubdirectories 44 66
    result.Result
    
[<Test>]
let ``copyAllFiles when rec level 0`` () =
    copyAllFilesTest 0 =! ["createdirectory 66"; "copyFiles 44 66"]

[<Test>]
let ``copyAllFiles when rec level 1`` () =
    copyAllFilesTest 1 =!
        [
            "createdirectory 66"; "copyFiles 44 66";
            "createdirectory 0"; "copyFiles 0 0"
        ]

[<Test>]
let ``deleteAllFiles when target does not exists`` () =
    let exists _ = false
    let deleteRecursive _ = failwith "test fails"
    deleteAllFiles exists deleteRecursive ()

[<Test>]
let ``deleteAllFiles when target does exists`` () =
    let result = TestResult()
    let exists _ = true
    let deleteRecursive _ = result.Add "delete"
    deleteAllFiles exists deleteRecursive ()
    result.Result =! ["delete"]
