module MsBuildRunner

let run msBuild workingDirectory =
    let msBuildStartInfo = 
        ProcessStartInfoProvider.getProcessStartInfo 
            msBuild 
            "/m" 
            workingDirectory

    let msBuildOutputHandle, isMsBuildSuccessfull = 
        MsBuildOutput.createMsBuildHandle()

    ProcessRunner.run msBuildStartInfo msBuildOutputHandle

    isMsBuildSuccessfull()
