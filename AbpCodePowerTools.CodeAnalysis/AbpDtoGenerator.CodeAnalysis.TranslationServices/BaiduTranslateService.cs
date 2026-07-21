using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace AbpDtoGenerator.CodeAnalysis.TranslationServices;

public static class BaiduTranslateService
{
	public static TransRoot GetTranslate(string text, BaiduTrranslateEnum source, BaiduTrranslateEnum target)
	{
		string text2 = "20200221000386710";
		string text3 = "6DXjXjhzXX6PdF07jwrP";
		string text4 = source.ToString();
		string text5 = target.ToString();
		string text6 = new Random().Next(100000).ToString();
		string text7 = EncryptString(text2 + text + text6 + text3);
		HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat("http://api.fanyi.baidu.com/api/trans/vip/translate?" + "q=" + HttpUtility.UrlEncode(text), "&from=", text4), "&to=", text5), "&appid=", text2), "&salt=", text6), "&sign=", text7));
		obj.Method = "GET";
		obj.ContentType = "text/html;charset=UTF-8";
		obj.UserAgent = null;
		obj.Timeout = 6000;
		HttpWebResponse httpWebResponse = (HttpWebResponse)obj.GetResponse();
		if (httpWebResponse.StatusCode == HttpStatusCode.OK)
		{
			Stream responseStream = httpWebResponse.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
			string text8 = streamReader.ReadToEnd();
			streamReader.Close();
			responseStream.Close();
			var transRoot = JsonConvert.DeserializeObject<TransRoot>(text8);
			if (string.IsNullOrEmpty(transRoot.to))
			{
				throw new Exception("翻译错误！百度api错误信息：" + text8.ToString());
			}
			return transRoot;
		}
		return null;
	}

	private static string EncryptString(string str)
	{
		MD5 mD = MD5.Create();
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		byte[] array = mD.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			stringBuilder.Append(b.ToString("x2"));
		}
		return stringBuilder.ToString();
	}
}
