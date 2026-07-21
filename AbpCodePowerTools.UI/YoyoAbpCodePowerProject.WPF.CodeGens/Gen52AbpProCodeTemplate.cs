using System.Collections.Generic;
using System.IO;
using AbpDtoGenerator.Enums;
using AbpDtoGenerator.GeneratorModels;
using AbpDtoGenerator.Models;
using AbpDtoGenerator.ViewModels;

namespace YoyoAbpCodePowerProject.WPF.CodeGens;

public static class Gen52AbpProCodeTemplate
{
	public static List<CodeTemplateInfo> Get52abpDefaultCodeTemplates(CodeGenDto dto)
	{
		List<CodeTemplateInfo> list = new List<CodeTemplateInfo>();
		BasicOptionCfg option = dto.Option;
		string entityDir = dto.EntityDir;
		string templateBasePath = dto.TemplateBasePath;
		EntityModel entity = dto.Entity;
		string uiParentDirName = dto.UiParentDirName;
		SolutionInfoModel solutionInfoModel = dto.SolutionInfoModel;
		if (option.IsAllGeneratorCode || option.UseApplicationServiceCode)
		{
			string basePath = Global.SolutionInfo.Application.BasePath;
			string buildPath = Path.Combine(basePath, entityDir, "Mapper", entity.Name + "DtoAutoMapper.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Mapper\\EntityDtoAutoMapper.txt"), buildPath));
			if (option.UseExportExcel)
			{
				buildPath = Path.Combine(basePath, entityDir, "Exporting", "I" + entity.Name + "ListExcelExporter.cs");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Exporting\\IEntityListExcelExporter.txt"), buildPath));
				buildPath = Path.Combine(basePath, entityDir, "Exporting", entity.Name + "ListExcelExporter.cs");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Exporting\\EntityListExcelExporter.txt"), buildPath));
			}
			buildPath = Path.Combine(basePath, entityDir, "I" + entity.Name + "AppService.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\IEntityApplicationService.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, entity.Name + "AppService.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\EntityApplicationService.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Dtos", "CreateOrUpdate" + entity.Name + "Input.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Dtos\\CreateOrUpdateEntityInput.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Dtos", "Get" + entity.Name + "ForEditOutput.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Dtos\\GetEntityForEditOutput.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Dtos", "Get" + entity.Name + "sInput.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Dtos\\GetEntitysInput.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Dtos", entity.Name + "EditDto.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Dtos\\EntityEditDto.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Dtos", entity.Name + "ListDto.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Dtos\\EntityListDto.txt"), buildPath));
			buildPath = Path.Combine(basePath, entityDir, "Readme.md");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Readme.txt"), buildPath));
			if (option.UseNgZorro)
			{
				buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, "Readme.md");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\Client\\NGZorro\\Readme.txt"), buildPath));
				buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, "create-or-edit-" + entity.SplitName, "create-or-edit-" + entity.SplitName + ".component.less");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\Client\\NGZorro\\EntityEditViewCss.txt"), buildPath));
				buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, "create-or-edit-" + entity.SplitName, "create-or-edit-" + entity.SplitName + ".component.html");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\Client\\NGZorro\\EntityEditViewHtml.txt"), buildPath));
				buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, "create-or-edit-" + entity.SplitName, "create-or-edit-" + entity.SplitName + ".component.ts");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\Client\\NGZorro\\EntityEditViewTs.txt"), buildPath));
				buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, entity.SplitName + ".component.less");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\Client\\NGZorro\\EntityListViewCss.txt"), buildPath));
				buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, entity.SplitName + ".component.html");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Client\\NGZorro\\EntityListViewHtml.txt"), buildPath));
				buildPath = Path.Combine(basePath, entityDir, "Client", "NGZorro", uiParentDirName, entity.SplitName + ".component.ts");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Client\\NGZorro\\EntityListViewTs.txt"), buildPath));
			}
		}
		if (option.IsAllGeneratorCode || option.UseDomainAuthorizeCode || option.UseDomainManagerCode)
		{
			string basePath2 = Global.SolutionInfo.Core.BasePath;
			string empty = string.Empty;
			if (option.UseDomainAuthorizeCode)
			{
				empty = Path.Combine(basePath2, entityDir, "Authorization", entity.Name + "AuthorizationProvider.cs");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Domain\\Authorization\\EntityAuthorizationProvider.txt"), empty));
				empty = Path.Combine(basePath2, entityDir, "Authorization", entity.Name + "Permissions.cs");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Domain\\Authorization\\EntityPermissions.txt"), empty));
			}
			if (option.UseDomainManagerCode)
			{
				empty = Path.Combine(basePath2, entityDir, "DomainService", "I" + entity.Name + "Manager.cs");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Domain\\DomainService\\IEntityManager.txt"), empty));
				empty = Path.Combine(basePath2, entityDir, "DomainService", entity.Name + "Manager.cs");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Domain\\DomainService\\EntityManager.txt"), empty));
				empty = Path.Combine(basePath2, entityDir, entity.Name + "多语言.md");
				list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Domain\\Duoyuyan.txt"), empty));
			}
			empty = Path.Combine(basePath2, entityDir, "Readme.md");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Readme.txt"), empty));
		}
		string buildPath2 = Path.Combine(Global.SolutionInfo.EF.BasePath, "EntityMapper", entity.Name + "s", entity.Name + "Cfg.cs");
		list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\EntityFrameworkCore\\EntityMapper\\EntityCfg.txt"), buildPath2));
		if (option.UseXUnitTests && Global.SolutionInfo.Tests != null)
		{
			string buildPath3 = Path.Combine(Global.SolutionInfo.Tests.BasePath, entity.Name + "s", entity.Name + "AppService_Tests.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Tests\\EntityAppService_Tests.txt"), buildPath3));
		}
		if (option.InitGeneratorCode)
		{
			string basePath3 = Global.SolutionInfo.Application.BasePath;
			string buildPath4 = Path.Combine(basePath3, "Dtos", "PagedAndFilteredInputDto.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\Dtos\\PagedAndFilteredInputDto.txt"), buildPath4));
			buildPath4 = Path.Combine(basePath3, "Dtos", "PagedAndSortedInputDto.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\Dtos\\PagedAndSortedInputDto.txt"), buildPath4));
			buildPath4 = Path.Combine(basePath3, "Dtos", "PagedInputDto.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\Dtos\\PagedInputDto.txt"), buildPath4));
			buildPath4 = Path.Combine(basePath3, "Dtos", "PagedSortedAndFilteredInputDto.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\Dtos\\PagedSortedAndFilteredInputDto.txt"), buildPath4));
			basePath3 = Global.SolutionInfo.Core.BasePath;
			buildPath4 = Path.Combine(basePath3, "Authorization", "AppLtmPermissions.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\AppLtmPermissions.txt"), buildPath4));
			buildPath4 = Path.Combine(basePath3, solutionInfoModel.SolutionNamespace + "DomainServiceBase.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\SolutionNameDomainServiceBase.txt"), buildPath4));
			buildPath4 = Path.Combine(basePath3, "AppLtmConsts.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\AppLtmConsts.txt"), buildPath4));
			buildPath4 = Path.Combine(basePath3, "YoYoAbpefCoreConsts.cs");
			list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\YoYoAbpefCoreConsts.txt"), buildPath4));
		}
		return list;
	}
}
