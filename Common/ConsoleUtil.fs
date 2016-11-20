module Common.ConsoleUtil

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
