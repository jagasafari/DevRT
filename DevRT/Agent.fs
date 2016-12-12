module DevRT.Agent

let createAgent handle initState = 
    MailboxProcessor.Start(fun inbox -> 
        let rec messageLoop state = async {
            let! msg = inbox.Receive()
            let state = msg |> handle
            return! messageLoop state
        }
        messageLoop initState
    )
