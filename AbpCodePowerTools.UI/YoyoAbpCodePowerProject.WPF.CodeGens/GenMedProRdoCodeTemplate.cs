using System.Collections.Generic;
using System.IO;
using AbpDtoGenerator.Enums;
using AbpDtoGenerator.GeneratorModels;
using AbpDtoGenerator.Models;
using AbpDtoGenerator.ViewModels;

namespace YoyoAbpCodePowerProject.WPF.CodeGens;

public static class GenMedProRdoCodeTemplate
{
	public static List<CodeTemplateInfo> GetGctMedProRdoCodeTemplates(CodeGenDto dto)
	{
		List<CodeTemplateInfo> list = new List<CodeTemplateInfo>();
		BasicOptionCfg option = dto.Option;
		string entityDir = dto.EntityDir;
		string templateBasePath = dto.TemplateBasePath;
		EntityModel entity = dto.Entity;
		string uiParentDirName = dto.UiParentDirName;
		_ = dto.SolutionInfoModel;
		if (option.IsAllGeneratorCode || option.UseApplicationServiceCode)
		{
			string basePath = Global.SolutionInfo.Application.BasePath;
			string buildPath = Path.Combine(basePath, entityDir, "Mapper", entity.Name + "DtoAutoMapper.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Application\\Mapper\\EntityDtoAutoMapper.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "I" + entity.Name + "AppService.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Application\\IEntityApplicationService.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, entity.Name + "AppService.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Application\\EntityApplicationService.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Dtos", "CreateOrUpdate" + entity.Name + "Input.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Application\\Dtos\\CreateOrUpdateEntityInput.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Dtos", "Get" + entity.Name + "ForEditOutput.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Application\\Dtos\\GetEntityForEditOutput.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Dtos", "Get" + entity.Name + "sInput.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Application\\Dtos\\GetEntitysInput.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Dtos", entity.Name + "EditDto.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Application\\Dtos\\EntityEditDto.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Dtos", entity.Name + "ListDto.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Application\\Dtos\\EntityListDto.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Dtos", entity.Name + "BaseListDto.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Application\\Dtos\\BaseListDto.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Dtos", entity.Name + "BaseEditDto.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Application\\Dtos\\BaseEditDto.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, entity.Name + "DevelopAPI.md");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\DevelopAPI.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Readme.md");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Readme.txt"), buildPath));
			if (option.UseNgZorro)
			{
				buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, "Readme.md");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Client\\NGZorro\\Readme.txt"), buildPath));
				buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, "create-or-edit-" + entity.SplitName, "create-or-edit-" + entity.SplitName + ".component.less");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Client\\NGZorro\\EntityEditViewCss.txt"), buildPath));
				buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, "create-or-edit-" + entity.SplitName, "create-or-edit-" + entity.SplitName + ".component.html");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Client\\NGZorro\\EntityEditViewHtml.txt"), buildPath));
				buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, "create-or-edit-" + entity.SplitName, "create-or-edit-" + entity.SplitName + ".component.ts");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Client\\NGZorro\\EntityEditViewTs.txt"), buildPath));
				buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, entity.SplitName + ".component.less");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Client\\NGZorro\\EntityListViewCss.txt"), buildPath));
				buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, entity.SplitName + ".component.html");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Client\\NGZorro\\EntityListViewHtml.txt"), buildPath));
				buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, entity.SplitName + ".component.ts");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Client\\NGZorro\\EntityListViewTs.txt"), buildPath));
			}
			buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, "g-" + entity.SplitName + "-select-rdo", "g-" + entity.SplitName + "-select-rdo.component.less");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Client\\NGZorro\\Rdo\\GEntityRdoSelectCss.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, "g-" + entity.SplitName + "-select-rdo", "g-" + entity.SplitName + "-select-rdo.component.html");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Client\\NGZorro\\Rdo\\GEntityRdoSelectHtml.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, "g-" + entity.SplitName + "-select-rdo", "g-" + entity.SplitName + "-select-rdo.component.ts");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Client\\NGZorro\\Rdo\\GEntityRdoSelectTs.txt"), buildPath));
		}
		if (option.IsAllGeneratorCode || option.UseDomainAuthorizeCode || option.UseDomainManagerCode)
		{
			string basePath2 = Global.SolutionInfo.Core.BasePath;
			string empty = string.Empty;
			if (option.UseDomainAuthorizeCode)
			{
				empty = Path.Combine(basePath2, entityDir, "Authorization", entity.Name + "AuthorizationProvider.cs");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Domain\\Authorization\\EntityAuthorizationProvider.txt"), empty));
				empty = Path.Combine(basePath2, entityDir, "Authorization", entity.Name + "Permissions.cs");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Domain\\Authorization\\EntityPermissions.txt"), empty));
			}
			if (option.UseDomainManagerCode)
			{
				empty = Path.Combine(basePath2, entityDir, "DomainService", "I" + entity.Name + "Manager.cs");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Domain\\DomainService\\IEntityManager.txt"), empty));
				empty = Path.Combine(basePath2, entityDir, "DomainService", entity.Name + "Manager.cs");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Domain\\DomainService\\EntityManager.txt"), empty));
				empty = Path.Combine(basePath2, entityDir, entity.Name + "多语言.md");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Domain\\Duoyuyan.txt"), empty));
			}
			empty = Path.Combine(basePath2, entityDir, entity.Name + "DevelopAPI.md");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\DevelopAPI.txt"), empty));
			empty = Path.Combine(basePath2, entityDir, "Readme.md");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\MedPro\\Rdo\\Server\\Readme.txt"), empty));
		}
		_ = Global.SolutionInfo.EF.BasePath;
		return list;
	}
}
