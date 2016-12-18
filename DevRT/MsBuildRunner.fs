module MsBuildRunner

let run msBuild sln workingDirectory =
    let msBuildStartInfo = 
        ProcessStartInfoProvider.getProcessStartInfo 
            msBuild 
            (sprintf "%s /m" sln) 
            workingDirectory

    let msBuildOutputHandle, isMsBuildSuccessfull = 
        MsBuildOutput.createMsBuildHandle()

    ProcessRunner.run msBuildStartInfo msBuildOutputHandle

    isMsBuildSuccessfull()
