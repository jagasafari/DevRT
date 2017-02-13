module DevRT.Run

open System

type RunAction = | CheckKeyPressed | HandleKeyPressed | RunCi | RunRefactor

let infiniteLoop action =
    let rec infinite() =
        seq { yield action(); yield! infinite() }
    infinite()

let action postRunCi postRefactor () =
    let rec actionRec = function
        | RunCi -> postRunCi(); Threading.Thread.Sleep 1000
        | RunRefactor -> postRefactor(); Threading.Thread.Sleep 2000
        | CheckKeyPressed ->
            match Console.KeyAvailable with
            | true -> HandleKeyPressed |> actionRec
            | false -> RunCi |> actionRec
        | HandleKeyPressed ->
            let key = Console.ReadKey(true)
            match key.Key with
            | ConsoleKey.R -> RunRefactor |> actionRec
            | _ -> RunCi |> actionRec
    actionRec CheckKeyPressed


let run postRunCi postRefactor = action postRunCi postRefactor |> infiniteLoop
