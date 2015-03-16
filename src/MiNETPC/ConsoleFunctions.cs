using System;

namespace MiNETPC
{
	class ConsoleFunctions
	{
		public static void WriteInfoLine(string Text)
		{
			PluginGlobals.Log.Info(Text);
		}

		public static void WriteErrorLine(string Text)
		{
			PluginGlobals.Log.Error(Text);
		}

		public static void WriteWarningLine(string Text)
		{
			PluginGlobals.Log.Warn(Text);
		}

		public static void WriteServerLine(string Text)
		{
			WriteInfoLine(Text);
		}

		public static void WriteDebugLine(string Text)
		{
			WriteWarningLine(Text);
		}
	}
}
