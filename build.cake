#tool nuget:?package=NUnit.ConsoleRunner&version=3.2.1
#tool "nuget:?package=GitReleaseNotes"
#tool "nuget:?package=GitVersion.CommandLine"
#addin "Cake.Git"
#addin "Cake.Json"
#addin "Cake.Xamarin"
//#addin "Cake.Slack"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var settings = DeserializeJsonFromFile<BuildSettings>("build.settings.json");
var versionInfo = DeserializeJsonFromFile<VersionInfo>("version.json");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var sourceDir = MakeAbsolute(Directory("./source"));
var solutionFile =  sourceDir + File("/XLabs.sln");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
	.IsDependentOn("DisplaySettings")
    .Does(() =>
{
	string binFolderSpec = sourceDir.ToString() + "/**/bin/" + configuration;
	string objFolderSpec = sourceDir.ToString() + "/**/obj";
	
	Information("Cleaning Bin Folders by spec: {0}", binFolderSpec);
    CleanDirectories(binFolderSpec);
	Information("Cleaning Obj Folders by spec: {0}", objFolderSpec);
    CleanDirectories(objFolderSpec);
});

//////////////////////////////////////////////////////////////////////
Task("CleanPackages")
	.IsDependentOn("DisplaySettings")
	.Does(() =>
{
	var packageFolderSpec = sourceDir.ToString() + "/artifacts/Packages/*";

	Information("Cleaning Artifacts Folders by spec: {0}", packageFolderSpec);
    CleanDirectory(packageFolderSpec);
});

//////////////////////////////////////////////////////////////////////

Task("Restore")
	.IsDependentOn("DisplaySettings")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionFile);
});

//////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("DisplaySettings")
    .IsDependentOn("Restore")
    .Does(() =>
{
	DotNetBuild(solutionFile, settings => 
		settings.SetConfiguration(configuration)
			.SetVerbosity(Cake.Core.Diagnostics.Verbosity.Minimal)
			.WithTarget("Build")
			.WithProperty("TreatWarningsAsErrors","true"));
		
});

//////////////////////////////////////////////////////////////////////

Task("Publish")
	.IsDependentOn("DisplaySettings")
	.IsDependentOn("Build")
	.Does(() =>
{

});

//////////////////////////////////////////////////////////////////////

Task("UnitTests")
	.IsDependentOn("DisplaySettings")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("./src/**/bin/" + configuration + "/*.UnitTests.dll", new NUnit3Settings {
        NoResults = true
        });
});

//////////////////////////////////////////////////////////////////////

Task("UpdateAssemblyInfo")
	.IsDependentOn("DisplaySettings")
	.IsDependentOn("GetVersionInfo")
    .Does(() =>
{
    GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true
    });
});

Task("GetVersionInfo")
	.IsDependentOn("DisplaySettings")
    .Does(() =>
{
    var result = GitVersion(new GitVersionSettings {
        UserName = settings.git.userName,
        Password = settings.git.password,
        Url = settings.git.url,
        Branch = settings.git.branch
        //,Commit = EnviromentVariable("MY_COMMIT")
    });
    // Use result for building nuget packages, setting build server version, etc...
});

//////////////////////////////////////////////////////////////////////

Task("DisplaySettings")
	.Does(() => 
{
	Information(logAction=>logAction("Target: {0}", target));
	Information(logAction=>logAction("Configuration: {0}", configuration));

	Information(logAction=>logAction("Settings: \r\n\t{0}", settings.Dump()));
	Information(logAction=>logAction("Version: {0}", versionInfo.ToString()));
});

//////////////////////////////////////////////////////////////////////

Task("Blank")
	.Does(() =>
	{
	});

//////////////////////////////////////////////////////////////////////
// TASK SETUP/TEARDOWN
//////////////////////////////////////////////////////////////////////

TaskSetup((context, task) =>
{
	// custom logging
	//Verbose(logAction=>logAction("Task: {0} - Start", task.Task.Name));
});

TaskTeardown((context, task) =>
{
	//Verbose(logAction=>logAction("Task: {0} - Finish", task.Task.Name));
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Blank");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////
// Helper classes for settings and Versioning (wish cake supported dynamic types out of json)
// then these would not be needed
//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////
public class BuildSettings
{
	public GitSettings git {get; set;}
	public NugetSetings nuget {get;set;}
	
	public string Dump()
	{
		var message = git.Dump() + "\r\n\t" + nuget.Dump();
		
		return message;
	}
}

public class GitSettings
{
	public string url {get;set;}
	public string apiKey {get;set;}
	public string userName {get;set;}
	public string password {get;set;}
	public string branch {get;set;}
	
	public string Dump()
	{
		var message = string.Format("Git Settings: [Url={0}] APIKey={1}, branch={2}, User={3}, Password={4}", url, 
																											apiKey,
																											branch,																											
																											userName, 
																											string.IsNullOrEmpty(password) ? "Not Specificed" : "Specified");
		return message;
	}
}

public class NugetSetings
{
	public string url {get;set;}
	public string apiKey {get;set;}
	
	public string Dump()
	{
		var message = string.Format("NuGet Settings: [Url={0}] APIKey={1}", url, apiKey);
		
		return message;
	}
}

public class VersionInfo
{
	public VersionInfo()
	{
		major = 1; minor = 0; build = 0; preRelease = null;
	}
	
	public int major {get;set;} 
	public int minor {get;set;} 
	public int build {get;set;} 

	public int? preRelease {get;set;}
	public string commit {get;set;}
	
	public override string ToString()
	{
		var str = string.Format("{0:#0}.{1:#0}.{2:#0}", major, minor, build);
		
		if (preRelease != null) str += string.Format(".pre{0:00}", preRelease);
		
		return str;
	}
}
//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////
