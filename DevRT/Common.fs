module Common

open System

let writeline (str: string) = Console.WriteLine(str)
let writeEmptyLine() = Console.WriteLine()
let write (str: string) = Console.Write(str)
let writeColor write color (str:string) =
    Console.ForegroundColor <- color; write str; Console.ResetColor()
let writelineColor = writeColor writeline 
let writelineRed = writelineColor ConsoleColor.Red 
let writelinePurple = writelineColor ConsoleColor.DarkMagenta 
let writelineYellow = writelineColor ConsoleColor.Yellow 
let appendToLinePurple = writeColor write ConsoleColor.DarkMagenta 

open System.IO

let rec copyAllFiles source target =
    Directory.CreateDirectory(target) |> ignore
    Directory.GetFiles(source)
    |> Seq.iter( fun x -> File.Copy(x, Path.Combine(target, Path.GetFileName(x))))

    Directory.GetDirectories(source)
    |> Seq.iter(fun x -> copyAllFiles x (Path.Combine(target, (Path.GetFileName(x)))))

let deleteAllFiles target = 
    if Directory.Exists(target) then Directory.Delete(target, true)

type Result<'a, 'b> = | Success of 'a | Failure of 'b

let bindResult func input =
    try
        let result = func input
        Success result
    with | ex -> Failure ex

