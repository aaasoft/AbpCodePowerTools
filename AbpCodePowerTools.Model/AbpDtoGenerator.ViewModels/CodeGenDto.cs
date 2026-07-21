using AbpDtoGenerator.Models;

namespace AbpDtoGenerator.ViewModels;

public class CodeGenDto
{
	public BasicOptionCfg Option { get; set; }

	public string UiParentDirName { get; set; }

	public SolutionInfoModel SolutionInfoModel { get; set; }

	public string EntityDir { get; set; }

	public EntityModel Entity { get; set; }

	public string TemplateBasePath { get; set; }
}
