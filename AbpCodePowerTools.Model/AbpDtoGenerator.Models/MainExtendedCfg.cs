using AbpDtoGenerator.Base;

namespace AbpDtoGenerator.Models;

public class MainExtendedCfg : BaseViewModel
{
	private bool isXstSolution;

	public bool IsXstSolution
	{
		get
		{
			return isXstSolution;
		}
		set
		{
			isXstSolution = value;
			InvokePropertyChanged("IsXstSolution");
		}
	}
}
