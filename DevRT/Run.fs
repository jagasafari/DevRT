module DevRT.Run

open System
open DataTypes

let infiniteLoop (sleepMilliseconds: int) action =
    let rec infinite() =
        seq {
            yield action(); Threading.Thread.Sleep sleepMilliseconds; yield! infinite() }
    infinite()

let action postRunCi () =
    let rec actionRec = function
        | RunCi -> postRunCi()
        | RunRefactor -> ()
        | CheckKeyPressed ->
            match Console.KeyAvailable with
            | true -> HandleKeyPressed |> actionRec
            | false -> RunCi |> actionRec
        | HandleKeyPressed ->
            let key = Console.ReadKey(true)
            printfn "key pressed: %A" key.Key
            match key.Key with
            | ConsoleKey.R -> RunRefactor |> actionRec
            | _ -> RunCi |> actionRec
    actionRec CheckKeyPressed

let run sleepMilliseconds postRunCi =
    action postRunCi |> infiniteLoop sleepMilliseconds
