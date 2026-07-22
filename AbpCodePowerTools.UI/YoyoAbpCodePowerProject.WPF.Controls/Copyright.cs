using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;
using AbpCodePowerTools.Model;

namespace YoyoAbpCodePowerProject.WPF.Controls;

public partial class Copyright : UserControl, IComponentConnector
{
	public Copyright()
	{
		InitializeComponent();
		txtVersion.Text = AppConsts.Version;
		txtTime.Text = AppConsts.StartAppTime;
	}
}
