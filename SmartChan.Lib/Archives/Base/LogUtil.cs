// Deci SmartChan.Lib LogUtil.cs
// $File.CreatedYear-$File.CreatedMonth-8 @ 1:25

using Microsoft.Extensions.Logging;

namespace SmartChan.Lib.Archives.Base;

internal static class LogUtil
{

	internal static readonly ILoggerFactory Factory =
		LoggerFactory.Create(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Trace));

}