module MsBuildRunner

let runMsBuild msBuild workingDirectory =
    let msBuildStartInfo = ProcessStartInfoProvider.getProcessStartInfo msBuild "/m" workingDirectory
    let msBuildOutputHandle, isMsBuildSuccessfull = MsBuildOutput.createMsBuildHandle()
    ProcessRunner.run msBuildStartInfo msBuildOutputHandle
    isMsBuildSuccessfull()
