module DevRT.ConsoleOutput

open System
open System.IO

let writeEmptyLine () = Console.WriteLine ()

let write (str: string) = Console.Write str

let writeline (str: string) = Console.WriteLine str

let writeColor write color (str:string) =
    Console.ForegroundColor <- color; write str; Console.ResetColor()
    
let writeColorLine = writeColor writeline 

let writelineRed = writeColorLine ConsoleColor.Red 

let writelinePurple = writeColorLine ConsoleColor.DarkMagenta 

let writelineYellow = writeColorLine ConsoleColor.Yellow 

let appendToLinePurple = writeColor write ConsoleColor.DarkMagenta 
