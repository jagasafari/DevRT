module MsBuildRunner

let runMsBuild msBuild =
    let msBuildStartInfo = ProcessStartInfoProvider.getProcessStartInfo msBuild "/m" @"."
    let msBuildOutputHandle, isMsBuildSuccessfull = MsBuildOutput.createMsBuildHandle()
    ProcessRunner.run msBuildStartInfo msBuildOutputHandle
    isMsBuildSuccessfull()
