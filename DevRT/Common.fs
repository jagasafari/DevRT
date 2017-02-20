module DevRT.Common

open System

let getNow() = DateTime.Now

let createStatus initialStatus =
    let mutable status = initialStatus
    let update newStatus = status <- newStatus
    update, fun () -> status
