module DevRT.Agent

let createAgent log handle =
    let agent =
        MailboxProcessor.Start(fun inbox ->
            let rec messageLoop() = async {
                try
                    let! msg = inbox.Receive()
                    msg |> handle
                    return! messageLoop()
                with exn ->
                    exn |> log
                    return! messageLoop()
            }
            messageLoop()
        )
    agent.Error.Add(fun exn -> exn |> log)
    agent
