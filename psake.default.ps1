properties {
    [string]$configuration = "Release"
	[string]$packageVersion = $null
	[string]$preReleaseNumber = $null
    [string]$solution = $null
    [string]$source_folder = "."
	[string]$test_folder = "."
    [string]$deploy_folder = "deploy"
	[string]$nuget_folder = ".nuget"
	[string]$nuspec_folder = $null
	[string]$nuproj_folder = $null
    $projects = $null
	[bool]$updateVersion = $true
	[bool]$updateNuspecVersion = $true
	[bool]$updateNuspecFile = $true
	[string]$unittest_framework = "nunit"
	[string]$macAgentServerAddress = $null
	[string]$macAgentUser = $null
	[string]$baseNamespace = $null
	[bool]$processNuProjOutput = $false
	[bool]$autoIncrementVersion = $false
	[string]$nugetServerUrl = "https://nuget.org"
	[string]$nugetAPIKey = $null
}

Task Default -Depends DisplayParams,Build

Task DisplayParams -Depends Get-Version {
    Write-Host "`tconfiguration : $configuration"
	Write-Host "`tpackageVersion : $script:packageVersion"
	Write-Host "`tpreReleaseNumber : $script:preReleaseNumber"
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
    Write-Host "`tnugetServerUrl : $nugetServerUrl"
    Write-Host "`tnugetAPIKey : $nugetAPIKey"
    Write-Host "`tprojects : $projects"
}

Task Publish -Depends Get-Version,DisplayParams {
	$nugetConfig = Resolve-Path "$nuget_folder\nuget.config"

	Get-ChildItem -Path $deploy_folder | Where-Object -FilterScript {
		($_.Name.Contains("$project.$script:packageVersion")) -and !($_.Name.Contains(".symbols")) -and ($_.Extension -eq '.nupkg')    
	} | % {
		Write-Host "Publishing $($_.Fullname)"
		
		if ($nugetAPIKey)
		{
			exec { & "$nuget_folder\nuget.exe" "push" "$($_.Fullname)" -ConfigFile $nugetConfig -Source $nugetServerUrl -ApiKey $nugetAPIKey }
		} else {
			exec { & "$nuget_folder\nuget.exe" "push" "$($_.Fullname)" -ConfigFile $nugetConfig }
		}
	}
}

Task UnPublish -Depends Get-Version,DisplayParams {
	$ver = $script:packageVersion
	
	if ($script:preReleaseNumber)
	{
		$ver = "$ver-pre$($script:preReleaseNumber)"
	}

	$nugetConfig = Resolve-Path "$nuget_folder\nuget.config"

	Get-ChildItem -Path "$nuspec_folder\*.nuspec" -ErrorAction SilentlyContinue | % {
		Write-Host "`tUnPublishing $($_.BaseName) $ver"
		
		if ($nugetAPIKey)
		{
			exec { & "$nuget_folder\nuget.exe" "delete" "$($_.BaseName)" $ver -ConfigFile $nugetConfig -Source $nugetServerUrl -ApiKey $nugetAPIKey -NonInteractive }
		} else {
			exec { & "$nuget_folder\nuget.exe" "delete" "$($_.BaseName)" $ver -ConfigFile $nugetConfig -NonInteractive }
		}
	}
}

Task Package -Depends Get-Version,DisplayParams,RestoreDependencies { #-Depends Test {
	$ver = $script:packageVersion
	
	if ($script:preReleaseNumber)
	{
		$ver = "$ver-pre$($script:preReleaseNumber)"
	}
	
	New-Item $deploy_folder -ItemType Directory -ErrorAction SilentlyContinue	
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
		
		Try {
			exec { & "$nuget_folder\nuget.exe" pack $nuspecFile -OutputDirectory "$path" -MSBuildVersion 14 -Version $ver -Symbols -Prop Configuration=$configuration -Verbosity detailed }
		}
		Catch 
		{
			Write-Host "`tFailed generating package: $nuspecFile" -ForegroundColor Red
			Write-Host "`tError: $($_.Exception.Message)" -ForegroundColor Red
		}

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

Task Build -Depends DisplayParams,Set-Versions,RestorePackages,RestoreDependencies {
	Exec { msbuild "$source_folder\$solution" /t:Build /p:Configuration=$configuration /consoleloggerparameters:"ErrorsOnly;WarningsOnly" /p:ServerAddress=$macAgentServerAddress /p:ServerUser=$macAgentUser } 
}

Task Clean -Depends DisplayParams {
	Exec { msbuild "$source_folder\$solution" /t:Clean /p:Configuration=$configuration /consoleloggerparameters:"ErrorsOnly" /p:ServerAddress=$macAgentServerAddress /p:ServerUser=$macAgentUser } 
	
	gci -Path $source_folder,$test_folder,$deploy_folder,$nuspec_folder,$nuproj_folder -Recurse -include 'bin','obj' -ErrorAction SilentlyContinue | % {
		remove-item $_ -recurse -force
		write-host deleted $_
	}
}

Task CleanPackages -Depends DisplayParams {
	$ver = "$project.$script:packageVersion"
	
	if ($script:preReleaseNumber)
	{
		$ver = "$ver-pre$($script:preReleaseNumber)"
	}

	Get-ChildItem -Path $deploy_folder | Where-Object -FilterScript {
		($_.Name.Contains($ver)) -and ($_.Extension -eq '.nupkg')    
	} | % {
		remove-item $_.FullName -force
		write-host deleted $_
	}
}

Task RestorePackages {
	New-Item -ItemType Directory  "$source_folder\packages" -ErrorAction SilentlyContinue
	$pathToPackages = Resolve-Path "$source_folder\packages"
	$nugetConfig = Resolve-Path "$nuget_folder\nuget.config"
	
	$packages = @( 
		"Microsoft.Bcl", "Microsoft.Bcl.Async", "Microsoft.Bcl.Build", "Microsoft.CSharp", "Microsoft.Net.Http", "Microsoft.NETCore",
		"Microsoft.NETCore.Platforms", "Microsoft.NETCore.Portable.Compatibility", "Microsoft.NETCore.Runtime", "Microsoft.NETCore.Runtime.CoreCLR",
		"Microsoft.NETCore.Runtime.Native", "Microsoft.NETCore.Targets", "Microsoft.NETCore.Targets.UniversalWindowsPlatform", "Microsoft.NETCore.UniversalWindowsPlatform",
		"Microsoft.NETCore.Windows.ApiSets-x64", "Microsoft.NETCore.Windows.ApiSets-x86", "Microsoft.VisualBasic", "Microsoft.Win32.Primitives",
		"runtime.any.System.Private.DataContractSerialization", "runtime.aot.System.Private.DataContractSerialization",
		"runtime.win7-x64.Microsoft.NETCore.Runtime.CoreCLR", "runtime.win7-x86.Microsoft.NETCore.Runtime.CoreCLR",
		"runtime.win8-arm.Microsoft.NETCore.Runtime.CoreCLR", "System.AppContext",
		"System.Collections", "System.Collections.Concurrent", "System.Collections.Immutable",
		"System.Collections.NonGeneric", "System.Collections.Specialized", "System.ComponentModel",
		"System.ComponentModel.Annotations", "System.ComponentModel.EventBasedAsync", "System.Data.Common",
		"System.Diagnostics.Contracts", "System.Diagnostics.Debug", "System.Diagnostics.StackTrace",
		"System.Diagnostics.Tools", "System.Diagnostics.Tracing", "System.Dynamic.Runtime",
		"System.Globalization", "System.Globalization.Calendars", "System.Globalization.Extensions",
		"System.IO", "System.IO.Compression", "System.IO.Compression.clrcompression-arm",
		"System.IO.Compression.clrcompression-x64", "System.IO.Compression.clrcompression-x86",
		"System.IO.Compression.ZipFile", "System.IO.FileSystem", "System.IO.FileSystem.Primitives",
		"System.IO.IsolatedStorage", "System.IO.UnmanagedMemoryStream", "System.Linq",
		"System.Linq.Expressions", "System.Linq.Parallel", "System.Linq.Queryable",
		"System.Net.Http", "System.Net.Http.Rtc", "System.Net.NetworkInformation",
		"System.Net.Primitives", "System.Net.Requests", "System.Net.Sockets",
		"System.Net.WebHeaderCollection", "System.Numerics.Vectors", "System.Numerics.Vectors.WindowsRuntime",
		"System.ObjectModel", "System.Private.DataContractSerialization", "System.Private.Networking",
		"System.Private.ServiceModel", "System.Private.Uri", "System.Reflection",
		"System.Reflection.Context", "System.Reflection.DispatchProxy", "System.Reflection.Emit",
		"System.Reflection.Emit.ILGeneration", "System.Reflection.Emit.Lightweight", "System.Reflection.Extensions",
		"System.Reflection.Metadata", "System.Reflection.Primitives", "System.Reflection.TypeExtensions",
		"System.Resources.ResourceManager", "System.Runtime", "System.Runtime.Extensions",
		"System.Runtime.Handles", "System.Runtime.InteropServices", "System.Runtime.InteropServices.WindowsRuntime",
		"System.Runtime.Numerics", "System.Runtime.Serialization.Json", "System.Runtime.Serialization.Primitives",
		"System.Runtime.Serialization.Xml", "System.Runtime.WindowsRuntime", "System.Runtime.WindowsRuntime.UI.Xaml",
		"System.Security.Claims", "System.Security.Principal", "System.ServiceModel.Duplex",
		"System.ServiceModel.Http", "System.ServiceModel.NetTcp", "System.ServiceModel.Primitives",
		"System.ServiceModel.Security", "System.Text.Encoding", "System.Text.Encoding.CodePages",
		"System.Text.Encoding.Extensions", "System.Text.RegularExpressions", "System.Threading",
		"System.Threading.Overlapped", "System.Threading.Tasks", "System.Threading.Tasks.Dataflow",
		"System.Threading.Tasks.Parallel", "System.Threading.Timer", "System.Xml.ReaderWriter",
		"System.Xml.XDocument", "System.Xml.XmlDocument", "System.Xml.XmlSerializer", "PropertyChanged.Fody", "Autofac", "Newtonsoft.Json", 
		"NUnit"
	)

	Write-Host "`tRestoring Packages to: $pathToPackages" -ForegroundColor Yellow
	Write-Host "`tUsing Nuget Config File: $nugetConfig" -ForegroundColor Yellow

	#Exec { & "$nuget_folder\nuget.exe" restore -SolutionDirectory $source_folder -PackagesDirectory $pathToPackages -ConfigFile $nugetConfig }
	Exec { & "$nuget_folder\nuget.exe" restore "$source_folder\$solution" -PackagesDirectory $pathToPackages -ConfigFile $nugetConfig }
	
	$packages | % { 
		Write-Host "`t`tInstalling Package: $_" -ForegroundColor Yellow
		Exec { & "$nuget_folder\nuget.exe" install $_ -OutputDirectory $pathToPackages -ConfigFile $nugetConfig }
	}
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
	#exec { & ".\process.ps1" }
	popd
}

Task Set-Versions -Depends Get-Version {
	if (-Not $updateVersion) { return }
	
	Get-ChildItem -Recurse -Force | ? { $_.Name -eq "AssemblyInfo.cs" -or $_.Name -eq "AssemblyInfo.Shared.cs" } | % {
		(Get-Content $_.FullName) | % {
			($_ -replace 'AssemblyVersion\(.*\)', ('AssemblyVersion("' + $script:packageVersion + '")')) -replace 'AssemblyFileVersion\(.*\)', ('AssemblyFileVersion("' + $script:packageVersion + '")')
		} | Set-Content $_.FullName -Encoding UTF8
	}    
}

Task Get-Version {
	if (-Not $script:packageVersion -and -Not $packageVersion) {
		$versionInfo = (Get-Content "version.json") -join "`n" | ConvertFrom-Json
		$script:packageVersion = "$($versionInfo.major).$($versionInfo.minor).$($versionInfo.build)";
		if ($versionInfo.preRelease) {
			$script:preReleaseNumber = "{0:00}" -f $versionInfo.preRelease
		}
		
		Write-Host "`tVersion Loaded: $script:packageVersion $script:preReleaseNumber"
	} else {
		$script:packageVersion = $packageVersion
		
		if ($preReleaseNumber) {
			$script:preReleaseNumber = "{0:00}" -f $preReleaseNumber
		}
		
		Write-Host "`tVersion Passed in: $script:packageVersion $script:preReleaseNumber"
	}
}

Task Increment-Version {
	if (-Not (Test-Path "version.json")) {
		$versionInfo = '{ "major":  2, "minor":  2, "build":  0, "preRelease":  1 }' | ConvertFrom-Json
	} else {
		$versionInfo = (Get-Content "version.json") -join "`n" | ConvertFrom-Json
	}

	$preVersion = "$($versionInfo.major).$($versionInfo.minor).$($versionInfo.build)";	
	
	if ($versionInfo.preRelease) {
		$preVersion = "$preVersion-pre" + "{0:00}" -f $versionInfo.preRelease
		$versionInfo.preRelease = [int]$versionInfo.preRelease + 1
		$newVersion = "-pre" + "{0:00}" -f $versionInfo.preRelease
	} else {
		$versionInfo.build = [int]$versionInfo.build + 1
	}
		
	$newVersion = "$($versionInfo.major).$($versionInfo.minor).$($versionInfo.build)" + $newVersion;	
	
	Write-Host "`tIncrementing Version from $preVersion to $newVersion"
	$versionInfo | ConvertTo-Json | Out-File "version.json"
}

#######################################################################################################################

function ChangeNuSpecVersion([string] $baseNamespace, [string] $nuSpecFilePath, [string] $versionNumber="0.0.0.0", [string] $nuSpecFilePathTmp = $null, [bool] $dependencyupdateVersion = $false, [string] $dependencyVersion = $null)
{
	Write-Host "`tDynamically setting NuSpec version: $versionNumber" -ForegroundColor Yellow
	Write-Host "`t`tBase Namespace: $baseNamespace" -ForegroundColor Yellow
	Write-Host "`t`tNuspec File: $nuSpecFilePath" -ForegroundColor Yellow
	Write-Host "`t`tTemp Nuspec File: $nuSpecFilePathTmp" -ForegroundColor Yellow
	Write-Host "`t`tDependency Version: $dependencyVersion" -ForegroundColor Yellow
	Write-Host "`t`tUpdate Dependency Version: $dependencyupdateVersion" -ForegroundColor Yellow
	
	# Get full path or save operation fails when launched in standalone powershell
	$nuSpecFile = Get-Item $nuSpecFilePath | Select-Object -First 1
	
	# Bring the XML Linq namespace in
	[Reflection.Assembly]::LoadWithPartialName( "System.Xml.Linq" ) | Out-Null
	
	# Update the XML document with the new version	
	$xmlns = [System.Xml.Linq.XNamespace] "http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd";
	$xDoc = [System.Xml.Linq.XDocument]::Load( $nuSpecFile.FullName )
	$versionNode = $xDoc.Descendants( $xmlns + "version" ) | Select-Object -First 1
	if ($versionNode -ne $null)
	{
		Write-Host "`t`t`tUpdating Version: $versionNumber" -ForegroundColor Green
		$versionNode.SetValue($versionNumber)
	}
	
	# Update the XML document dependencies with the new version
	if ($dependencyupdateVersion)
	{
		Write-Host "`tUpdating Dependencies" -ForegroundColor Yellow
		$dependencies = $xDoc.Descendants( $xmlns + "dependency" )
		foreach( $dependency in $dependencies )
		{
			$idAttribute = $dependency.Attributes( "id" ) | Select-Object -First 1
			Write-Host "`t`tDependency: $idAttribute"
			if ( $idAttribute -ne $null -or $idAttribute)
			{
				if ($baseNamespace -and $idAttribute.Value -like "$baseNamespace.*" )
				{
					Write-Host "`t`t`tUpdating Dependency Version: $versionNumber" -ForegroundColor Green

					if ($dependencyVersion)					
					{
						$dependency.SetAttributeValue( "version", "$dependencyVersion" )
					} 
					else
					{
					$dependency.SetAttributeValue( "version", "[$versionNumber]" )
					}
				}
			}
		}
	}
	
	# Save file
	if ($nuSpecFilePathTmp) 
	{ 
		Write-Host "`tCreating a temporary NuSpec file with the new version" -ForegroundColor Yellow
		$xDoc.Save( $nuSpecFilePathTmp, [System.Xml.Linq.SaveOptions]::None )
	} else {
		$xDoc.Save( $nuSpecFile.FullName, [System.Xml.Linq.SaveOptions]::None )
	}
}