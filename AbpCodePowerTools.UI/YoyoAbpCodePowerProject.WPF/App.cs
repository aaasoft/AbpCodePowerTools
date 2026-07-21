using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;

namespace YoyoAbpCodePowerProject.WPF;

public class App : Application
{
	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		base.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
	}
}
