using System;
using System.Collections.Generic;
using System.Text;
using XLabs.Ioc;

namespace XLabs.Test.NuGet.Shared
{
    public class SharedCode
    {
        public static void UnamedMethodOnPurpose()
        {
            System.Diagnostics.Debug.WriteLine(Resolver.IsSet);
        }
    }
}
