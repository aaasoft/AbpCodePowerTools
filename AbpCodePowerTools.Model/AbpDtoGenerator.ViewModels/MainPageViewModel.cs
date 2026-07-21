using AbpDtoGenerator.Base;
using AbpDtoGenerator.Models;

namespace AbpDtoGenerator.ViewModels;

public class MainPageViewModel : BaseViewModel
{
	private BasicOptionCfg optionCfg;

	private MainExtendedCfg mainExtendedViewModel;

	private string versionInfo;

	public BasicOptionCfg OptionCfg
	{
		get
		{
			return optionCfg;
		}
		set
		{
			optionCfg = value;
			InvokePropertyChanged("OptionCfg");
		}
	}

	public MainExtendedCfg MainExtendedCfg
	{
		get
		{
			return mainExtendedViewModel;
		}
		set
		{
			mainExtendedViewModel = value;
			InvokePropertyChanged("MainExtendedCfg");
		}
	}

	public string VersionInfo
	{
		get
		{
			return versionInfo;
		}
		set
		{
			versionInfo = value;
			InvokePropertyChanged("VersionInfo");
		}
	}

	public static MainPageViewModel Create(SolutionInfoModel model)
	{
		MainPageViewModel mainPageViewModel = new MainPageViewModel();
		mainPageViewModel.OptionCfg = new BasicOptionCfg
		{
			IsAbpZero = model.IsAbpZero
		};
		if (mainPageViewModel.OptionCfg.IsAbpZero)
		{
			mainPageViewModel.VersionInfo = "当前项目为ABP Zero";
		}
		return mainPageViewModel;
	}
}
