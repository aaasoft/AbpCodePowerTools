using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AbpDtoGenerator;

public static class StringEx
{
	private static Regex R = new Regex("[A-Z]");

	public static string FirstCharToLower(this string str)
	{
		string text = str.Substring(0, 1).ToLower();
		string text2 = str.Substring(1, str.Length - 1);
		return text + text2;
	}

	public static List<string> ConvertLowerSplitArray(this string str)
	{
		List<string> list = new List<string>();
		char[] array = str.ToCharArray();
		bool flag = true;
		string text = string.Empty;
		char[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			char c = array2[i];
			if (R.IsMatch(c.ToString()) && !flag)
			{
				list.Add(text);
				text = string.Empty;
			}
			text += c.ToString().ToLower();
			flag = false;
		}
		list.Add(text);
		return list;
	}
}
