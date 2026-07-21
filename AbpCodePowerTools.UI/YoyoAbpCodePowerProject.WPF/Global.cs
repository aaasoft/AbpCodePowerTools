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
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(SolutionInfo.CurrentSelectFilePath);
		EntityModel entityModel = LoadEntityService.LoadEntityInfoInCurrentSelectFile(SolutionInfo.CurrentSelectFilePath, fileNameWithoutExtension);
		string parentDirName = Path.GetDirectoryName(SolutionInfo.CurrentSelectFilePath)?.Split(new char[2] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
		Entity = entityModel;
		Entity.ParentDirName = parentDirName;
	}

	private static void CreateViewModels()
	{
		MainViewModel = MainPageViewModel.Create(SolutionInfo);
		MainViewModel.MainExtendedCfg = new MainExtendedCfg();
		PropertyViewModel = PropertySelectorPageModel.Create(Entity.Properties);
		PropertyViewModel.EntityDisplayName = Entity.EntityDisplayName;
		Option = MainViewModel.OptionCfg;
	}
}
