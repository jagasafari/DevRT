module DevRT.CiStepsRunAgent

let handle runMsBuild runNUnit = runMsBuild >> runNUnit >> ignore
