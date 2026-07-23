using System;
using AbpDtoGenerator.Base;

namespace AbpDtoGenerator.Models;

public class BasicOptionCfg : BaseViewModel
{
	private bool isAllGeneratorCode;

	private bool isOverrideFile;

	private bool isOnlyDto;

	private bool useApplicationServiceCode;

	private bool useNgZorro;

	private bool useDomainAuthorizeCode;

	private bool useDomainManagerCode;

	private bool isRepositoryExtendCode;

	private bool _initGeneratorCode;

	private bool isAbpZero;

	public bool IsAllGeneratorCode
	{
		get
		{
			return isAllGeneratorCode;
		}
		set
		{
			isAllGeneratorCode = value;
			SetAll();
		}
	}

	public bool IsOverrideFile
	{
		get
		{
			return isOverrideFile;
		}
		set
		{
			isOverrideFile = value;
			InvokePropertyChanged("IsOverrideFile");
		}
	}

	public Action<bool> OnOnlyCustomDtoChanged { get; set; }

	public bool IsOnlyDto
	{
		get
		{
			return isOnlyDto;
		}
		set
		{
			isOnlyDto = value;
		}
	}

	public bool UseApplicationServiceCode
	{
		get
		{
			return useApplicationServiceCode;
		}
		set
		{
			useApplicationServiceCode = value;
			CheckIsAllGeneratorCode();
			InvokePropertyChanged("UseApplicationServiceCode");
		}
	}

	public bool UseNgZorro
	{
		get
		{
			return useNgZorro;
		}
		set
		{
			useNgZorro = value;
			CheckIsAllGeneratorCode();
			InvokePropertyChanged("UseNgZorro");
		}
	}

	public bool UseDomainAuthorizeCode
	{
		get
		{
			return useDomainAuthorizeCode;
		}
		set
		{
			useDomainAuthorizeCode = value;
			CheckIsAllGeneratorCode();
			InvokePropertyChanged("UseDomainAuthorizeCode");
		}
	}

	public bool UseDomainManagerCode
	{
		get
		{
			return useDomainManagerCode;
		}
		set
		{
			useDomainManagerCode = value;
			CheckIsAllGeneratorCode();
			InvokePropertyChanged("UseDomainManagerCode");
		}
	}

	public bool IsRepositoryExtendCode
	{
		get
		{
			return isRepositoryExtendCode;
		}
		set
		{
			isRepositoryExtendCode = value;
			CheckIsAllGeneratorCode();
			InvokePropertyChanged("IsRepositoryExtendCode");
		}
	}

	public bool InitGeneratorCode
	{
		get
		{
			return _initGeneratorCode;
		}
		set
		{
			_initGeneratorCode = value;
			InvokePropertyChanged("InitGeneratorCode");
		}
	}

	public bool IsAbpZero
	{
		get
		{
			return isAbpZero;
		}
		set
		{
			isAbpZero = value;
			InvokePropertyChanged("IsAbpZero");
		}
	}

	public BasicOptionCfg()
	{
		IsAllGeneratorCode = true;
		IsOverrideFile = true;
	}

	private void SetAll()
	{
		useDomainAuthorizeCode = isAllGeneratorCode;
		useDomainManagerCode = isAllGeneratorCode;
		useApplicationServiceCode = isAllGeneratorCode;
		useNgZorro = isAllGeneratorCode;
		InvokePropertyChanged("IsAllGeneratorCode");
		InvokePropertyChanged("UseDomainAuthorizeCode");
		InvokePropertyChanged("UseDomainManagerCode");
		InvokePropertyChanged("UseApplicationServiceCode");
		InvokePropertyChanged("UseNgZorro");
	}

	private void CheckIsAllGeneratorCode()
	{
		isAllGeneratorCode = useDomainAuthorizeCode && useDomainManagerCode && useApplicationServiceCode && useNgZorro;
		InvokePropertyChanged("IsAllGeneratorCode");
	}

	public bool HasChecked()
	{
		if (!InitGeneratorCode && !UseDomainAuthorizeCode && !UseDomainManagerCode)
		{
			return UseApplicationServiceCode;
		}
		return true;
	}

	public bool DoWithoutSelectProperty()
	{
		if (InitGeneratorCode || UseDomainAuthorizeCode || UseDomainManagerCode)
		{
			return !UseApplicationServiceCode;
		}
		return false;
	}
}
