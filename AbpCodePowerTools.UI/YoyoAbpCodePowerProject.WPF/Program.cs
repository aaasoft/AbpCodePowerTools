using System;
using System.IO;
using System.Windows;
using AbpDtoGenerator;

namespace YoyoAbpCodePowerProject.WPF;

internal class Program
{
	[STAThread]
	private static void Main(string[] args)
	{
		if (args != null && args != null && args.Length != 0)
		{
			string text = args[0];
			if (!Directory.Exists(text))
			{
				MessageBox.Show("初始化失败！初始化数据不正确！");
				return;
			}
			Global.SolutionPath = Path.Combine(text, "solutionInfo.json");
			if (File.Exists(Global.SolutionPath))
			{
				try
				{
					Global.InitApplication(Global.SolutionPath, null);
					new App().Run(new MainWindow());
					return;
				}
				catch (Exception arg)
				{
					$"初始化UI错误，错误信息:{arg}".ErrorMsg();
					MessageBox.Show("初始化UI错误");
					return;
				}
			}
			MessageBox.Show("初始化失败！未能成功创建启动数据！");
		}
		else
		{
			MessageBox.Show("初始化失败！初始化参数不正确！");
		}
	}
}
