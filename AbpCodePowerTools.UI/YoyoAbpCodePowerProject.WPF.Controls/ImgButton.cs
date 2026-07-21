using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace YoyoAbpCodePowerProject.WPF.Controls;

public partial class ImgButton : Button, IComponentConnector
{
	public static readonly DependencyProperty ImageProperty;

	public static readonly DependencyProperty ImageWidthProperty;

	public static readonly DependencyProperty ImageHeightProperty;

	public ImageSource Image
	{
		get
		{
			return GetValue(ImageProperty) as ImageSource;
		}
		set
		{
			SetValue(ImageProperty, value);
		}
	}

	public double ImageWidth
	{
		get
		{
			return (double)GetValue(ImageWidthProperty);
		}
		set
		{
			SetValue(ImageWidthProperty, value);
		}
	}

	public double ImageHeight
	{
		get
		{
			return (double)GetValue(ImageHeightProperty);
		}
		set
		{
			SetValue(ImageHeightProperty, value);
		}
	}

	static ImgButton()
	{
		ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImgButton), new PropertyMetadata(null));
		ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(double), typeof(ImgButton), new PropertyMetadata(double.NaN));
		ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(double), typeof(ImgButton), new PropertyMetadata(double.NaN));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ImgButton), new FrameworkPropertyMetadata(typeof(ImgButton)));
	}
}
