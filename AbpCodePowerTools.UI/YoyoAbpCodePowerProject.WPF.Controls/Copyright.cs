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
		txtVersion.Text = "5.0.1";
		txtTime.Text = AppConsts.StartAppTime;
		txtDateYear.Text = DateTime.Now.Year.ToString();
	}

	private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
	{
		Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
		e.Handled = true;
	}
}
