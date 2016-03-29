cd C:\git\blue\dev\Build\Scripts\SetConfig
get-childitem "C:\git\blue\dev" *.config -rec | ? { ($_.FullName -notmatch "Binaries\\?") -and    ($_.FullName -notmatch "Build\\ConfigTransform\\?") } | 
Format-Table FullName

