module DevRT.Agent

let createAgentWithErrorHandling log handle initState =
    let agent =
        MailboxProcessor.Start(fun inbox ->
            let rec messageLoop state = async {
                try
                    let! msg = inbox.Receive()
                    let state = msg |> handle
                    return! messageLoop state
                with exn -> exn |> log
            }
            messageLoop initState
        )
    agent.Error.Add(fun exn -> exn |> log)
    agent

let createAgent logError handle initState =
    createAgentWithErrorHandling logError handle initState
