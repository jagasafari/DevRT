module DevRT.Agent

let createAgent logHandledError logFatalError handle =
    let agent =
        MailboxProcessor.Start(fun inbox ->
            let rec messageLoop() = async {
                try
                    let! msg = inbox.Receive()
                    msg |> handle
                    return! messageLoop()
                with exn ->
                    exn |> logHandledError
                    return! messageLoop()
            }
            messageLoop()
        )
    agent.Error.Add(fun exn -> exn |> logFatalError)
    agent
