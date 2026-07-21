using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AbpDtoGenerator.CodeAnalysis.GeneratorCodeServices;
using AbpDtoGenerator.Models;
using AbpDtoGenerator.ViewModels;
using Newtonsoft.Json;

namespace YoyoAbpCodePowerProject.WPF;

public static class Global
{
	public static string SolutionPath { get; set; }

	public static SolutionInfoModel SolutionInfo { get; set; }

	public static MainPageViewModel MainViewModel { get; set; }

	public static PropertySelectorPageModel PropertyViewModel { get; set; }

	public static BasicOptionCfg Option { get; set; }

	public static EntityModel Entity { get; set; }

	public static void InitApplication(string solutionPath, string selectEntityPath)
	{
		SolutionPath = solutionPath;
		SolutionInfo = JsonConvert.DeserializeObject<SolutionInfoModel>(File.ReadAllText(SolutionPath, Encoding.UTF8));
		LoadEntityInfos();
		CreateViewModels();
	}

	private static void LoadEntityInfos()
	{
		string text = Path.Combine(SolutionInfo.SolutionPath, "52abp_code_power");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(SolutionInfo.CurrentSelectFilePath);
		EntityModel entityModel = LoadEntityService.LoadEntityInConfigJson(Path.Combine(text, fileNameWithoutExtension + ".json"), fileNameWithoutExtension);
		EntityModel entityModel2 = LoadEntityService.LoadEntityInfoInCurrentSelectFile(SolutionInfo.CurrentSelectFilePath, fileNameWithoutExtension);
		string parentDirName = Path.GetDirectoryName(SolutionInfo.CurrentSelectFilePath)?.Split(new char[2] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
		if (entityModel == null)
		{
			Entity = entityModel2;
			Entity.ParentDirName = parentDirName;
			return;
		}
		foreach (EntityFieldModel item in entityModel2.Properties)
		{
			if (!entityModel.Properties.Exists((EntityFieldModel o) => o.FieldName == item.FieldName))
			{
				entityModel.Properties.Add(item);
			}
		}
		List<EntityFieldModel> list = new List<EntityFieldModel>();
		foreach (EntityFieldModel item2 in entityModel.Properties)
		{
			if (!entityModel2.Properties.Exists((EntityFieldModel o) => o.FieldName == item2.FieldName))
			{
				list.Add(item2);
			}
		}
		foreach (EntityFieldModel item3 in list)
		{
			entityModel.Properties.Remove(item3);
		}
		entityModel.Namespace = entityModel2.Namespace;
		entityModel.NameSplit = entityModel2.NameSplit;
		entityModel.EntityKeyName = entityModel2.EntityKeyName;
		entityModel.BaseClassDtoName = entityModel2.BaseClassDtoName;
		entityModel.BaseClassName = entityModel2.BaseClassName;
		entityModel.BaseClassNameList = entityModel2.BaseClassNameList;
		Entity = entityModel;
		Entity.ParentDirName = parentDirName;
	}

	private static void CreateViewModels()
	{
		MainViewModel = MainPageViewModel.Create(SolutionInfo);
		string text = Path.Combine(SolutionInfo.SolutionPath, "52abp_code_power");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		MainExtendedCfg mainExtendedCfg = LoadEntityService.LoadMainExtendedInConfigJson(Path.Combine(text, "52ABP_CodePowerExtendedModel.json"));
		if (mainExtendedCfg == null)
		{
			MainViewModel.MainExtendedCfg = new MainExtendedCfg();
		}
		else
		{
			MainViewModel.MainExtendedCfg = mainExtendedCfg;
		}
		PropertyViewModel = PropertySelectorPageModel.Create(Entity.Properties);
		PropertyViewModel.EntityDisplayName = Entity.EntityDisplayName;
		Option = MainViewModel.OptionCfg;
	}
}
