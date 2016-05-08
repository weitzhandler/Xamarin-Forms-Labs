properties {
    $configuration = "Release"
	$version = $null
	$preRelease = $null
    $solution = $null
    $source_folder = "."
	$test_folder = "."
    $deploy_folder = "deploy"
	$nuget_folder = ".nuget"
	$nuspec_folder = $null
    $projects = $null
	$versionUpdate = $true
	$updateNuspecVersion = $true
	$updateNuspecFile = $true
	$unittest_framework = "nunit"
}

Task Default -Depends Build

Task RestorePackages {
	Exec { $nuget_folder\nuget.exe restore -PackagesDirectory $source_folder\packages }
}

Task RestoreDependencies {
	switch ($unittest_framework)
	{
		"nunit" { choco install -y nunit }
		"xunit" { choco install -y xunit }
	}
}

Task Publish -Depends Package {
	$version = getVersionBase
	
	$projects | % {
		Get-ChildItem | Where-Object -FilterScript {
			($_.Name.Contains("$project.$version")) -and !($_.Name.Contains(".symbols")) -and ($_.Extension -eq '.nupkg')    
		} | % {
			exec { $nuget_folder\nuget.exe push $_.Fullname }
		}
	}
}

Task Package -Depends RestoreDependencies { #-Depends Test {
	$version = getVersionBase
	
	if ($preRelease) {
		$version = $version+"-"+$preRelease;
	}

#	$projects | % {
#		Get-ChildItem -Path "$_\*.csproj" | % {
#			exec { $nuget_folder\nuget.exe pack -sym $_.Fullname -OutputDirectory $deploy_folder -Version $version -Prop Configuration=$configuration }
#		}        
#	}
	
	Get-ChildItem -Path "$nuspec_folder\*.nuspec" -ErrorAction SilentlyContinue | % {
		$nuspecFile = $_;		
		$nuSpecFilePathTmp = $null;
		
		if ($updateNuspecVersion) 
		{ 
			if (-Not ($updateNuspecFile)) { $nuSpecFilePathTmp = "$nuspecFile.tmp.NuSpec"; }
			
			ChangeNuSpecVersion $baseNamespace $nuSpecFile $version $nuSpecFilePathTmp $true
			
			if (-Not ($updateNuspecFile)) { $nuspecFile = $nuSpecFilePathTmp; }
		}
				
		exec { $nuget_folder\nuget.exe pack $nuspecFile -OutputDirectory $deploy_folder -Version $version -Symbols -Prop Configuration=$configuration }

		if (-Not ($updateNuspecFile)) { Remove-Item $nuSpecFilePathTmp -ErrorAction SilentlyContinue }
	}
}

Task Test -Depends Build {
	Get-ChildItem $source_folder -Recurse -Include *NUnit.csproj,*Test.csproj | % {
		switch ($unittest_framework) {
			"nunit" {
				Exec { & "xunit.console.exe" "$($_.DirectoryName)\bin\$configuration\$($_.BaseName).dll" }
			}
			"xunit" {
				Exec { & "nunit.console.exe" "$($_.DirectoryName)\bin\$configuration\$($_.BaseName).dll" }
			}
		}
	}
}

Task Build -Depends Clean,Set-Versions,RestorePackages,RestoreDependencies {
	Exec { msbuild "$solution" /t:Build /p:Configuration=$configuration } 
}

Task Clean {
	Exec { msbuild "$solution" /t:Clean /p:Configuration=$configuration } 
	
	gci -Path $source_folder,$test_folder,$nuspec_folder -Recurse -include 'bin','obj' | % {
		remove-item $_ -recurse -force
		write-host deleted $_
	}
}

Task Set-Versions {
	if (-Not $versionUpdate) return
	
	$version = getVersionBase

	Get-ChildItem -Recurse -Force | Where-Object { $_.Name -eq "AssemblyInfo.cs" } | ForEach-Object {
		(Get-Content $_.FullName) | ForEach-Object {
			($_ -replace 'AssemblyVersion\(.*\)', ('AssemblyVersion("' + $version + '")')) -replace 'AssemblyFileVersion\(.*\)', ('AssemblyFileVersion("' + $version + '")')
		} | Set-Content $_.FullName -Encoding UTF8
	}    
}

#######################################################################################################################

function getVersionBase {
	if ($version -eq $null) {
		$versionInfo = (Get-Content "version.json") -join "`n" | ConvertFrom-Json
		"$($versionInfo.major).$($versionInfo.minor).$($versionInfo.patch)";    
	} else {
		$version
	}
}

function ChangeNuSpecVersion([string] $baseNamespace, [string] $nuSpecFilePath, [string] $version="0.0.0.0", [string] $nuSpecFilePathTmp = $null, [bool] $dependencyVersionUpdate = $false, [string] $dependencyVersion = $null)
{
	Write-Host "Dynamically setting NuSpec version: $Version" -ForegroundColor Yellow
	Write-Host "`tNuspec File: $nuSpecFilePath"
	Write-Host "`tTemp Nuspec File: $nuSpecFilePathTmp"
	Write-Host "`tBase Namespace: $baseNamespace"
	Write-Host "`tUpdate Dependency Version: $dependencyVersionUpdate"
	
	# Get full path or save operation fails when launched in standalone powershell
	$nuSpecFile = Get-Item $nuSpecFilePath | Select-Object -First 1
	
	# Bring the XML Linq namespace in
	[Reflection.Assembly]::LoadWithPartialName( "System.Xml.Linq" ) | Out-Null
	
	# Update the XML document with the new version
	$xmlns = [System.Xml.Linq.XNamespace] "http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd";
	$xDoc = [System.Xml.Linq.XDocument]::Load( $nuSpecFile.FullName )
	$versionNode = $xDoc.Descendants( $xmlns + "version" ) | Select-Object -First 1
	if ($versionNode -ne $null)
	{
		Write-Host "`t`tUpdating Version: $version" -ForegroundColor Green
		$versionNode.SetValue($version)
	}
	
	# Update the XML document dependencies with the new version
	if ($dependencyVersionUpdate)
	{
		Write-Host "Updating Dependencies" -ForegroundColor Yellow
		$dependencies = $xDoc.Descendants( $xmlns + "dependency" )
		foreach( $dependency in $dependencies )
		{
			$idAttribute = $dependency.Attributes( "id" ) | Select-Object -First 1
			if ( $idAttribute -ne $null )
			{
				Write-Host "`tChecking Dependency: $idAttribute.Value" -ForegroundColor Green
				if ($baseNamespace -ne $null -and $idAttribute.Value -like "$baseNamespace.*" )
				{
					Write-Host "`t`tUpdating Dependency Version: $version" -ForegroundColor Green

					if ($dependencyVersion -ne $null)					
					{
						$dependency.SetAttributeValue( "version", "$dependencyVersion" )
					} 
					else
					{
					$dependency.SetAttributeValue( "version", "[$version]" )
					}
				}
			}
		}
	}
	
	# Save file
	if ($nuSpecFilePathTmp -ne $null -and $nuSpecFilePathTmp.Lengt -gt 0) 
	{ 
		Write-Host "Creating a temporary NuSpec file with the new version" 
		$xDoc.Save( $nuSpecFilePathTmp, [System.Xml.Linq.SaveOptions]::None )
	} else {
		$xDoc.Save( $nuSpecFile.FullName, [System.Xml.Linq.SaveOptions]::None )
	}
}