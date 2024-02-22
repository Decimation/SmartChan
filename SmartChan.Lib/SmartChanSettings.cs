// Deci SmartChan.Lib SmartChanSettings.cs
// $File.CreatedYear-$File.CreatedMonth-8 @ 1:22

using System.Runtime.CompilerServices;
using SmartChan.Lib.Archives.Base;

namespace SmartChan.Lib;

public static class SmartChanSettings
{

    [ModuleInitializer]
    public static void Init()
    {
        // Global.Setup();
        RuntimeHelpers.RunClassConstructor(typeof(BaseArchiveEngine).TypeHandle);
    }

}