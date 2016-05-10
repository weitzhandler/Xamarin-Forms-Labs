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
	$nuproj_folder = $null
    $projects = $null
	$updateVersion = $true
	$updateNuspecVersion = $true
	$updateNuspecFile = $true
	$unittest_framework = "nunit"
	$macAgentServerAddress = $null
	$macAgentUser = $null
	$baseNamespace = $null
	$processNuProjOutput = $false
}

Task Default -Depends DisplayParams,Build

Task DisplayParams -Depends Get-Version {
    Write-Host "`tconfiguration : $configuration"
	Write-Host "`tversion : $version"
	Write-Host "`tpreRelease : $preRelease"
    Write-Host "`tsolution : $solution"
	Write-Host "`tbaseNamespace: $baseNamespace"
    Write-Host "`tsource_folder : $source_folder"
	Write-Host "`ttest_folder : $test_folder"
    Write-Host "`tdeploy_folder : $deploy_folder"
	Write-Host "`tnuget_folder : $nuget_folder"
	Write-Host "`tnuspec_folder : $nuspec_folder"
	Write-Host "`tnuproj_folder : $nuproj_folder"
	Write-Host "`tupdateVersion : $updateVersion"
	Write-Host "`tupdateNuspecVersion : $updateNuspecVersion"
	Write-Host "`tupdateNuspecFile : $updateNuspecFile"
	Write-Host "`tunittest_framework : $unittest_framework"
	Write-Host "`tmacAgentServerAddress : $macAgentServerAddress"
	Write-Host "`tmacAgentUser : $macAgentUser"
    Write-Host "`tprojects : $projects"
}

Task Publish -Depends Get-Version,DisplayParams,Package {
	$projects | % {
		Get-ChildItem -Path $deploy_folder | Where-Object -FilterScript {
			($_.Name.Contains("$project.$version")) -and !($_.Name.Contains(".symbols")) -and ($_.Extension -eq '.nupkg')    
		} | % {
			exec { & "$nuget_folder\nuget.exe" "push" "$($_.Fullname)" }
		}
	}
}

Task Package -Depends Get-Version,DisplayParams,RestoreDependencies,ProcessNuProjNuSpecFiles { #-Depends Test {
	$ver = $version
	
	if ($preRelease)
	{
		$ver = "$ver-$preRelease"
	}

#	$projects | % {
#		Get-ChildItem -Path "$_\*.csproj" | % {
#			exec { $nuget_folder\nuget.exe pack -sym $_.Fullname -OutputDirectory $deploy_folder -Version $ver -Prop Configuration=$configuration }
#		}        
#	}
	
	$path = Resolve-Path $deploy_folder
	
	Get-ChildItem -Path "$nuspec_folder\*.nuspec" -ErrorAction SilentlyContinue | % {
		$nuspecFile = $_;		
		$nuSpecFilePathTmp = $null;
		
		if ($updateNuspecVersion) 
		{ 
			if (-Not ($updateNuspecFile)) { $nuSpecFilePathTmp = "$nuspecFile.tmp.NuSpec"; }
			
			ChangeNuSpecVersion $baseNamespace $nuSpecFile $ver $nuSpecFilePathTmp $true
			
			if (-Not ($updateNuspecFile)) { $nuspecFile = $nuSpecFilePathTmp; }
		}

		Write-Host "`tnuspecFile: $nuSpecFile"
		Write-Host "`tpath: $path"
		Write-Host "`tver: $ver"
		Write-Host "`tconfiguration: $configuration"
		
		exec { & "$nuget_folder\nuget.exe" pack $nuspecFile -OutputDirectory "$path" -MSBuildVersion 14 -Version $ver -Symbols -Prop Configuration=$configuration -Verbosity detailed }

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

Task Build -Depends Get-Version,DisplayParams,Set-Versions,RestorePackages,RestoreDependencies {
	Exec { msbuild "$source_folder\$solution" /t:Build /p:Configuration=$configuration /consoleloggerparameters:"ErrorsOnly;WarningsOnly" /p:ServerAddress=$macAgentServerAddress /p:ServerUser=$macAgentUser } 
}

Task Clean -Depends DisplayParams {
	Exec { msbuild "$source_folder\$solution" /t:Clean /p:Configuration=$configuration /consoleloggerparameters:"ErrorsOnly" /p:ServerAddress=$macAgentServerAddress /p:ServerUser=$macAgentUser } 
	
	gci -Path $source_folder,$test_folder,$deploy_folder,$nuspec_folder,$nuproj_folder -Recurse -include 'bin','obj' -ErrorAction SilentlyContinue | % {
		remove-item $_ -recurse -force
		write-host deleted $_
	}
}

Task RestorePackages {
	New-Item -ItemType Directory  "$source_folder\packages" -ErrorAction SilentlyContinue
	$pathToPackages = Resolve-Path "$source_folder\packages"
	$nugetConfig = Resolve-Path "$nuget_folder\nuget.config"
	Exec { & "$nuget_folder\nuget.exe" restore "$source_folder\$solution" -MSBuildVersion 14 -PackagesDirectory $pathToPackages -ConfigFile $nugetConfig  }
	Exec { & "$nuget_folder\nuget.exe" install NuProj -OutputDirectory $pathToPackages -ConfigFile $nugetConfig -Prerelease }
}

Task RestoreDependencies {
#	switch ($unittest_framework)
#	{
#		"nunit" { choco install -y nunit }
#	}
}

Task ProcessNuProjNuSpecFiles -Precondition { return $processNuProjOutput } {
	pushd
	cd $nuproj_folder
	Exec { & ".\process.ps1" }
	popd
}

Task Set-Versions -Depends Get-Version {
	if (-Not $updateVersion) { return }
	
	Get-ChildItem -Recurse -Force | ? { $_.Name -eq "AssemblyInfo.cs" -or $_.Name -eq "AssemblyInfo.Shared.cs" } | % {
		(Get-Content $_.FullName) | % {
			($_ -replace 'AssemblyVersion\(.*\)', ('AssemblyVersion("' + $version + '")')) -replace 'AssemblyFileVersion\(.*\)', ('AssemblyFileVersion("' + $version + '")')
		} | Set-Content $_.FullName -Encoding UTF8
	}    
}

Task Get-Version {
	if ($version -eq $null) {
		$version = getVersionBase
	}
}

#######################################################################################################################

function getVersionBase {
	if ($version -eq $null) {
		$versionInfo = (Get-Content "version.json") -join "`n" | ConvertFrom-Json
		"$($versionInfo.major).$($versionInfo.minor).$($versionInfo.build)";    
	} else {
		$version
	}
}

function ChangeNuSpecVersion([string] $baseNamespace, [string] $nuSpecFilePath, [string] $version="0.0.0.0", [string] $nuSpecFilePathTmp = $null, [bool] $dependencyupdateVersion = $false, [string] $dependencyVersion = $null)
{
	Write-Host "Dynamically setting NuSpec version: $Version" -ForegroundColor Yellow
	Write-Host "`tNuspec File: $nuSpecFilePath"
	Write-Host "`tTemp Nuspec File: $nuSpecFilePathTmp"
	Write-Host "`tBase Namespace: $baseNamespace"
	Write-Host "`tUpdate Dependency Version: $dependencyupdateVersion"
	
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
	if ($dependencyupdateVersion)
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