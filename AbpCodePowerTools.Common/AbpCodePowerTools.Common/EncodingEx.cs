using System.Text;

namespace AbpCodePowerTools.Common;

public static class EncodingEx
{
	private static Encoding utf8WithoutBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

	public static Encoding Utf8WithoutBom => utf8WithoutBom;
}
