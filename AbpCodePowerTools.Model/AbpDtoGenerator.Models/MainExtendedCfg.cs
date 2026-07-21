using AbpDtoGenerator.Base;

namespace AbpDtoGenerator.Models;

public class MainExtendedCfg : BaseViewModel
{
	private bool isXstSolution;

	private bool isGctMedProNdo;

	private bool isGctMedProRdo;

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

	public bool IsGctMedProNdo
	{
		get
		{
			return isGctMedProNdo;
		}
		set
		{
			isGctMedProNdo = value;
			InvokePropertyChanged("IsGctMedProNdo");
		}
	}

	public bool IsGctMedProRdo
	{
		get
		{
			return isGctMedProRdo;
		}
		set
		{
			isGctMedProRdo = value;
			InvokePropertyChanged("IsGctMedProRdo");
		}
	}
}
