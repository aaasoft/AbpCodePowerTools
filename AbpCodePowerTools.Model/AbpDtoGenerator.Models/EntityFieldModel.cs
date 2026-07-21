using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AbpDtoGenerator.Base;
using Newtonsoft.Json;

namespace AbpDtoGenerator.Models;

public class EntityFieldModel : BaseViewModel
{
	private bool @checked;

	private bool editChecked;

	private bool listChecked;

	private string fieldName;

	private string fieldDisplayName;

	private string fieldDescription;

	private bool required;

	private int? minLength;

	private int? maxLength;

	private string regularExpression;

	private string fieldTypeStr;

	private string fieldTypeStrFirstCharToLower;

	private Type fieldType;

	private ObservableCollection<string> ctrlTypes;

	private int ctrlTypeIndex;

	public bool Checked
	{
		get
		{
			return @checked;
		}
		set
		{
			@checked = value;
			InvokePropertyChanged("Checked");
		}
	}

	public bool EditChecked
	{
		get
		{
			return editChecked;
		}
		set
		{
			editChecked = value;
			InvokePropertyChanged("EditChecked");
		}
	}

	public bool ListChecked
	{
		get
		{
			return listChecked;
		}
		set
		{
			listChecked = value;
			InvokePropertyChanged("ListChecked");
		}
	}

	public string FieldName
	{
		get
		{
			return fieldName;
		}
		set
		{
			fieldName = value;
			InvokePropertyChanged("FieldName");
		}
	}

	public string FieldDisplayName
	{
		get
		{
			return fieldDisplayName;
		}
		set
		{
			fieldDisplayName = value;
			InvokePropertyChanged("FieldDisplayName");
		}
	}

	public string FieldDescription
	{
		get
		{
			return fieldDescription;
		}
		set
		{
			fieldDescription = value;
			InvokePropertyChanged("FieldDescription");
		}
	}

	public bool Required
	{
		get
		{
			return required;
		}
		set
		{
			required = value;
			InvokePropertyChanged("Required");
		}
	}

	public int? MinLength
	{
		get
		{
			return minLength;
		}
		set
		{
			minLength = value;
			InvokePropertyChanged("MinLength");
		}
	}

	public int? MaxLength
	{
		get
		{
			return maxLength;
		}
		set
		{
			maxLength = value;
			InvokePropertyChanged("MaxLength");
		}
	}

	public string RegularExpression
	{
		get
		{
			return regularExpression;
		}
		set
		{
			regularExpression = value;
			InvokePropertyChanged("RegularExpression");
		}
	}

	public string FieldTypeStr
	{
		get
		{
			return fieldTypeStr;
		}
		set
		{
			fieldTypeStr = value;
			InvokePropertyChanged("FieldTypeStr");
		}
	}

	public string FieldTypeStrFirstCharToLower
	{
		get
		{
			return fieldTypeStrFirstCharToLower;
		}
		set
		{
			fieldTypeStrFirstCharToLower = value;
			InvokePropertyChanged("FieldTypeStrFirstCharToLower");
		}
	}

	public Type FieldType
	{
		get
		{
			return fieldType;
		}
		set
		{
			fieldType = value;
			InvokePropertyChanged("FieldType");
		}
	}

	public ObservableCollection<string> CtrlTypes
	{
		get
		{
			return ctrlTypes;
		}
		set
		{
			ctrlTypes = value;
			InvokePropertyChanged("CtrlTypes");
		}
	}

	public int CtrlTypeIndex
	{
		get
		{
			return ctrlTypeIndex;
		}
		set
		{
			ctrlTypeIndex = value;
			InvokePropertyChanged("CtrlTypeIndex");
		}
	}

	public bool IsCollection { get; set; }

	public string FieldNameFirstLower { get; set; }

	public string ContrlType => CtrlTypes[CtrlTypeIndex];

	public List<string> AttributesList { get; set; }

	public bool IsSimpleProperty { get; set; }

	public bool IsRelation { get; set; }

	public string RelatedEntityName { get; set; }

	[JsonIgnore]
	public EntityModel RelationMetadata { get; set; }

	public EntityFieldModel()
	{
		CtrlTypeIndex = 0;
	}

	public void HasAttributes()
	{
		if (AttributesList == null)
		{
			AttributesList = new List<string>();
			return;
		}
		foreach (string attributes in AttributesList)
		{
			if (attributes.StartsWith("[Required"))
			{
				Required = true;
			}
			else if (attributes.StartsWith("[MaxLength("))
			{
				MaxLength = 64;
			}
			else if (attributes.StartsWith("[MinLength("))
			{
				MinLength = 0;
			}
		}
	}

	public EntityFieldModel Clone()
	{
		return DClone<EntityFieldModel>();
	}
}
