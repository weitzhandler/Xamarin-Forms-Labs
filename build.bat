del /F /S /Q  build
msbuild XLabs.sln /property:Configuration=Release;platform=ARM /t:Clean
msbuild XLabs.sln /property:Configuration=Release;platform=iPhone /t:Clean
msbuild XLabs.sln /property:Configuration=Release;platform="Any CPU" /t:Clean
msbuild src\Serialization\XLabs.Serialization.AspNet\XLabs.Serialization.AspNet.sln /property:Configuration=Release;platform="Any CPU" /t:Clean
msbuild XLabs.sln /property:Configuration=Release;platform=ARM
msbuild XLabs.sln /property:Configuration=Release;platform=iPhone
msbuild XLabs.sln /property:Configuration=Release;platform="Any CPU"
msbuild src\Serialization\XLabs.Serialization.AspNet\XLabs.Serialization.AspNet.sln /property:Configuration=Release;platform="Any CPU"