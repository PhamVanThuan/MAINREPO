
$baseFileFilter = "DEVC"

Get-ChildItem -Path . -Filter "*.$baseFileFilter*" -Recurse | 
	% {
	
		$original = $_.FullName
		$new = $original -replace ".$baseFileFilter", ".SYSC"
	
		if (($_.Name -match $baseFileFilter+"LOCAL") -or ($_.Name -match $baseFileFilter+"_LOCAL")) {
			Write-Host "Skipping $original" -foregroundcolor "Magenta"
			return
		}
	
		Write-Host $original
		Write-Host $new
		Copy-Item $original $new
		Write-Host
	}
	


