using System;
using System.IO;
using AbpCodePowerTools.Common;

namespace AbpDtoGenerator;

public static class TypeEx
{
	public static string ReadTemplate(this Type type, string templatePath)
	{
		using Stream stream = type.Assembly.GetManifestResourceStream(templatePath);
		return new StreamReader(stream, EncodingEx.Utf8WithoutBom).ReadToEnd();
	}
}
