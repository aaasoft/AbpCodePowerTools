using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;
using YoyoAbpCodePowerProject.WPF.Enums;

namespace YoyoAbpCodePowerProject.WPF;

public partial class PropertySelectorWindow : Window, IComponentConnector
{
	public Action<CallBackTypeEnum> CloseCallBack { get; set; }

	public CallBackTypeEnum BackType { get; set; }

	public PropertySelectorWindow(Action<CallBackTypeEnum> closeCallBack)
	{
		InitializeComponent();
		CloseCallBack = closeCallBack;
		base.DataContext = Global.PropertyViewModel;
		base.ResizeMode = ResizeMode.NoResize;
		base.WindowStyle = WindowStyle.None;
		base.Closing += PropertySelectorWindow_Closing;
		title.MouseDown += Title_MouseDown;
		base.WindowStartupLocation = WindowStartupLocation.CenterScreen;
	}

	private void Title_MouseDown(object sender, MouseButtonEventArgs e)
	{
		DragMove();
	}

	private void PropertySelectorWindow_Closing(object sender, CancelEventArgs e)
	{
		CloseCallBack?.Invoke(BackType);
	}

	private void Prev_Click(object sender, RoutedEventArgs e)
	{
		BackType = CallBackTypeEnum.Prev;
		Close();
	}

	private void Next_Click(object sender, RoutedEventArgs e)
	{
		BackType = CallBackTypeEnum.Enter;
		Close();
	}

	private void Cancel_Click(object sender, RoutedEventArgs e)
	{
		BackType = CallBackTypeEnum.Cancel;
		Close();
	}

	private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
	{
		Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
		e.Handled = true;
	}
}
