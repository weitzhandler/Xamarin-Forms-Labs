New-Item ..\artifacts -ItemType Directory -ErrorAction SilentlyContinue
Remove-Item ..\artifacts\*.* -ErrorAction SilentlyContinue

$version = $null; #"2.2.0000-pre02"

$rootPath = Resolve-Path ..\..

gci -Recurse *.nuspec | Where-Object { $_.PSIsContainer -eq $False -and $_.Name -match ".nuspec$" } | % {
	$f = $_.FullName
	$s = $_.Name
	Write-Host "`tProcssing and Updating: $s";
	Copy-Item -Force $f ..\artifacts
	
	#Write-Host "Reading XML"
	[string]$xf = (Resolve-Path "..\artifacts\$s").Path
	[xml]$xmlContent = Get-Content -Path $xf
	
	if ($version -ne $null) {
		$xmlContent.package.metadata.version = "$version"
	}
	
	$xmlContent.package.files.file | % {
		$s1 = $_.src.Replace($rootPath, "..\..")
		$_.SetAttribute("src", $s1)
		#Write-Host $_.src
	}
	
	#Write-Host "Updating XML File: $xf"
	$xmlContent.Save($xf)
}

