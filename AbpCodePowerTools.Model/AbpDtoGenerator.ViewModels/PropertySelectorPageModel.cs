using System.Collections.Generic;
using System.Collections.ObjectModel;
using AbpDtoGenerator.Base;
using AbpDtoGenerator.Models;

namespace AbpDtoGenerator.ViewModels;

public class PropertySelectorPageModel : BaseViewModel
{
	private string entityDisplayName;

	private double dtoWidth;

	private double editListDtoWidth;

	private double ctrlWidth;

	public ObservableCollection<EntityFieldModel> EntityFields { get; set; }

	public string EntityDisplayName
	{
		get
		{
			return entityDisplayName;
		}
		set
		{
			entityDisplayName = value;
			InvokePropertyChanged("EntityDisplayName");
		}
	}

	public double DtoWidth
	{
		get
		{
			return dtoWidth;
		}
		set
		{
			InvokePropertyChanged("DtoWidth");
		}
	}

	public double EditListDtoWidth
	{
		get
		{
			return editListDtoWidth;
		}
		set
		{
			InvokePropertyChanged("EditListDtoWidth");
		}
	}

	public double CtrlWidth
	{
		get
		{
			return ctrlWidth;
		}
		set
		{
			InvokePropertyChanged("CtrlWidth");
		}
	}

	public bool IsBackMain { get; set; }

	public void OnlyShowEditAndListDtoCheckbox(bool useCtrl = false)
	{
		if (useCtrl)
		{
			ctrlWidth = 140.0;
		}
		editListDtoWidth = 48.0;
		dtoWidth = 0.0;
	}

	public void OnlyShowDtoCheckbox()
	{
		ctrlWidth = 0.0;
		editListDtoWidth = 0.0;
		dtoWidth = 30.0;
	}

	public static PropertySelectorPageModel Create(List<EntityFieldModel> entityFields)
	{
		PropertySelectorPageModel propertySelectorPageModel = new PropertySelectorPageModel();
		propertySelectorPageModel.EntityFields = new ObservableCollection<EntityFieldModel>();
		foreach (EntityFieldModel entityField in entityFields)
		{
			propertySelectorPageModel.EntityFields.Add(entityField);
		}
		return propertySelectorPageModel;
	}
}
