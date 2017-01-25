module DevRT.MsBuildRunner

let run msBuild sln workingDirectory =
    let msBuildStartInfo = 
        ProcessStartInfoProvider.getProcessStartInfo 
            msBuild 
            (sprintf "%s /m" sln) 
            workingDirectory

    let msBuildOutputHandle, isMsBuildSuccessfull = 
        MsBuildOutput.createMsBuildHandle()
    System.DateTime.Now |> sprintf "%O" |> Logging.info
    ProcessRunner.run msBuildStartInfo msBuildOutputHandle

    isMsBuildSuccessfull()
