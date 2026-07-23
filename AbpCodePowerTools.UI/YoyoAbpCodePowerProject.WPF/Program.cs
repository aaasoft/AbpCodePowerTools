using AbpDtoGenerator;
using AbpDtoGenerator.Models;
using System;
using System.IO;
using System.Windows;

namespace YoyoAbpCodePowerProject.WPF;

internal class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        var solutionInfoModelKey = $"{nameof(AbpCodePowerTools)}_{nameof(SolutionInfoModel)}";
        var solutionInfoModelContent = Environment.GetEnvironmentVariable(solutionInfoModelKey);
        if (string.IsNullOrEmpty(solutionInfoModelContent))
        {
            MessageBox.Show($"初始化失败！未传入环境变量：{solutionInfoModelKey}");
        }
        else
        {
            try
            {
                Global.InitApplication(solutionInfoModelContent);
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
    }
}
