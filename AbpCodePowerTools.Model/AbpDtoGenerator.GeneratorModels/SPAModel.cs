using System.Collections.Generic;
using System.Linq;
using AbpDtoGenerator.Models;

namespace AbpDtoGenerator.GeneratorModels;

public class SPAModel
{
	public EntityModel Entity { get; set; }

	public bool UsePermission { get; set; }

	public string[] GetListFieldsDisplayName()
	{
		return (from o in Entity.Properties
			where o.ListChecked
			select o.FieldDisplayName).ToArray();
	}

	public string[] GetListFieldsName()
	{
		return (from o in Entity.Properties
			where o.ListChecked
			select o.FieldName).ToArray();
	}

	public string[] GetEditFieldsDisplayName()
	{
		return (from o in Entity.Properties
			where o.EditChecked
			select o.FieldDisplayName).ToArray();
	}

	public string[] GetListFieldsNameFirstLower()
	{
		return (from o in Entity.Properties
			where o.ListChecked
			select o.FieldNameFirstLower).ToArray();
	}

	public string[] GetEditFieldsNameFirstLower()
	{
		return (from o in Entity.Properties
			where o.EditChecked
			select o.FieldNameFirstLower).ToArray();
	}

	public string[] GetListFieldsType()
	{
		return (from o in Entity.Properties
			where o.ListChecked
			select o.FieldTypeStr).ToArray();
	}

	public List<EntityFieldModel> GetListDtoFields()
	{
		return Entity.Properties.Where((EntityFieldModel o) => o.ListChecked).ToList();
	}

	public List<EntityFieldModel> GetEditCheckedDtoFields()
	{
		return Entity.Properties.Where((EntityFieldModel o) => o.EditChecked).ToList();
	}

	public string[] GetListDtoControlNames()
	{
		return (from o in Entity.Properties
			where o.ListChecked
			select o.ContrlType).ToArray();
	}
}
