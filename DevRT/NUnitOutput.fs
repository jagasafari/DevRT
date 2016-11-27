module NUnitOutput

open Common
open DevRT

let contains pattern (str: string) = str.Contains pattern
let isSummary data = contains "Duration:" data || contains "Test Count:" data
let isRunSettings = contains "Run Settings"
let isTestFailerInfo = contains ") Failed :"

let getUpdatedStatus currentStatus = function
    | d when isSummary d -> Summary
    | d when isRunSettings d -> Noise
    | d when isTestFailerInfo d -> Failure
    | _ when currentStatus = Failure -> currentStatus
    | _ -> Noise
    
let filterOutput data = function
    | Summary -> writeline data    
    | Failure -> writelineYellow data
    | _ -> ()
