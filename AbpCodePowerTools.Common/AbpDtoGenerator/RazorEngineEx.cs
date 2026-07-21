using System;
using System.IO;
using AbpCodePowerTools.Common;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace AbpDtoGenerator;

public static class RazorEngineEx
{
	private static bool CreateEngineAfter { get; set; }

	public static string GeneratorCode(this Type type, string tempaltePath, object viewModel)
	{
		if (!CreateEngineAfter)
		{
			CreateEngine();
		}
		string templateSource = type.ReadTemplate(tempaltePath);
		return Engine.Razor.RunCompile(templateSource, tempaltePath, null, viewModel).Replace("&#39;", "'").Replace("&gt;", ">")
			.Replace("&quot;", "\"")
			.Replace("&lt;", "<");
	}

	public static string GeneratorCode(this string tempaltePath, object viewModel, Type viewModelType, string OldCustomCode)
	{
		try
		{
			DynamicViewBag dynamicViewBag = new DynamicViewBag();
			dynamicViewBag.AddValue("OldCustomCode", OldCustomCode);
			if (!CreateEngineAfter)
			{
				CreateEngine();
			}
			string templateSource = File.ReadAllText(tempaltePath, EncodingEx.Utf8WithoutBom);
			return Engine.Razor.RunCompile(templateSource, tempaltePath, viewModelType, viewModel, dynamicViewBag).Replace("&#39;", "'").Replace("&gt;", ">")
				.Replace("&quot;", "\"")
				.Replace("&lt;", "<")
				.Replace("<pre>", "")
				.Replace("</pre>", "");
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static void CreateEngine()
	{
		Engine.Razor = RazorEngineService.Create(new TemplateServiceConfiguration
		{
			Language = Language.CSharp,
			DisableTempFileLocking = true,
			CachingProvider = new DefaultCachingProvider(delegate(string t)
			{
				Directory.Delete(t, recursive: true);
			})
		});
		CreateEngineAfter = true;
	}
}
