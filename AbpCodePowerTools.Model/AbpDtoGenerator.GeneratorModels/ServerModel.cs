using System.Collections.Generic;
using System.Linq;
using AbpDtoGenerator.Models;

namespace AbpDtoGenerator.GeneratorModels;

public class ServerModel
{
	public GeneratorCodeReplaceInfo ReplaceInfo { get; set; }

	public EntityModel Entity { get; set; }

	public string EditDtoFieldCode { get; set; }

	public string ListDtoFieldCode { get; set; }

	public ServerModel()
	{
		EditDtoFieldCode = string.Empty;
		ListDtoFieldCode = string.Empty;
	}

	public List<EntityFieldModel> GetListDtoFields()
	{
		return Entity.Properties.Where((EntityFieldModel o) => o.ListChecked).ToList();
	}

	public List<EntityFieldModel> GetEditCheckedDtoFields()
	{
		return Entity.Properties.Where((EntityFieldModel o) => o.EditChecked).ToList();
	}
}
