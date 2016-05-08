Param(
    [Parameter(Position=1,Mandatory=0)]
    [string[]]$task_list = @(),

	[Parameter()]
    [string]$version = $null,
	
	[Parameter()]
    [string]$preRelease = $null,
	
	[Parameter()]
    [string]$configuration = "Release",
	
	[Parameter()]
	[bool]$updateVersion = $false,
		
	[Parameter()]
	[bool]$updateNuspecFile = $true,
	
	[Parameter()]
	[bool]$updateNuspecVersion = $true
)

$build_file = 'psake.default.ps1'

# Properties for the psake build script
$properties = @{

    # Build configuration to use
    "configuration" = $configuration;

    # Version number to use if running the Publish build task.
    # This will be read from the command line args
    "version"       = $version;

	# Is the Nuget package a pre-release version?
	"preRelease" = $preRelease;

    # Path to the solution file
    "solution"      = 'XLabs.sln';

    # Folder containing source code
    "source_folder" = '.\source';
	
	# Folder container unit tests
	"test_folder" = '.\tests';

    # Folder to output deployable packages to. This folder should be ignored
    # from any source control, as we dont commit build artifacts to source
    # control
    "deploy_folder" = '.\artifacts';

	# Folder that contains nuget files (nuget.exe, nuget.config)
	"nuget_folder" = '.\.nuget';

	# Folder that contains nuspec files
	"nuspec_folder" = '.\.nuget\artifacts';
	
	# Folder that contains nuspec files
	"nuproj_folder" = '.\.nuget\source';
	
	# List of projects to use when building NuGet Packages (Note: Not used for XLabs)
    "projects" = @(
	);
	
	# Unit Test Framework (nunit, xunit)
	"unittest_framework" = "nunit";
	
	# Update the version numbers automatically
	"updateVersion" = $updateVersion;
	
	"updateNuspecVersion" = $updateNuspecVersion;
	
	"updateNuspecFile" = $updateNuspecFile;
	
	# The name or ip address of the Mac that is running the Xamarin build agent
	"macAgentServerAddress" = "10.0.1.139"
	
	# The user name to use to authentice for the Xamarin build agent
	"macAgentUser" = "Shawn Anderson"
}

#if (!(Get-Module -Name psake -ListAvailable)) { Install-Module -Name psake -Scope CurrentUser }

#import-module .\packages\psake.4.4.2\tools\psake.psm1
import-module C:\ProgramData\chocolatey\lib\psake\tools\psake.psm1

invoke-psake $build_file $task_list -Properties $properties -Framework "4.6"
