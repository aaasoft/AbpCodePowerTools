using System.Collections.Generic;
using AbpDtoGenerator.Enums;

namespace AbpDtoGenerator.GeneratorModels;

public class ProjectPathInfo
{
	public ProjectType PType { get; set; }

	public string BasePath { get; set; }

	public string ProjectName { get; set; }

	public List<CodeTemplateInfo> CodeTemplates { get; set; }

	public ProjectPathInfo()
	{
		CodeTemplates = new List<CodeTemplateInfo>();
	}
}
