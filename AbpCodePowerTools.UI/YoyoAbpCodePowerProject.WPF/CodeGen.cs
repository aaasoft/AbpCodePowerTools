using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AbpCodePowerTools.Common;
using AbpDtoGenerator;
using AbpDtoGenerator.CodeAnalysis.GeneratorCodeServices;
using AbpDtoGenerator.GeneratorModels;
using AbpDtoGenerator.Models;
using AbpDtoGenerator.ViewModels;
using YoyoAbpCodePowerProject.WPF.CodeGens;

namespace YoyoAbpCodePowerProject.WPF;

public static class CodeGen
{
	public static void Gen()
	{
		try
		{
			ViewModel viewModel = new ViewModel
			{
				MainWindowsOptionCfg = Global.Option
			};
			GeneratorCodeReplaceInfo replaceInfo = GeneratorCodeService.Create(Global.SolutionInfo, Global.Option, Global.Entity);
			ServerModel serverModel = new ServerModel
			{
				ReplaceInfo = replaceInfo,
				Entity = Global.Entity
			};
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (EntityFieldModel property in serverModel.Entity.Properties)
			{
				if (property.EditChecked)
				{
					property.AppendFieldCode(stringBuilder);
				}
				if (property.ListChecked)
				{
					property.AppendFieldCode(stringBuilder2);
				}
			}
			serverModel.EditDtoFieldCode = stringBuilder.ToString();
			serverModel.ListDtoFieldCode = stringBuilder2.ToString();
			viewModel.Server = serverModel;
			SPAModel sPAClient = new SPAModel
			{
				Entity = Global.Entity,
				UsePermission = Global.Option.UseDomainAuthorizeCode
			};
			viewModel.SPAClient = sPAClient;
			List<CodeTemplateInfo> list = CreateCodeTemplates();
			foreach (CodeTemplateInfo item in list)
			{
				if (!File.Exists(item.BuildPath) || Global.Option.IsOverrideFile)
				{
					item.BuildCode = item.Path.GeneratorCode(viewModel, typeof(ViewModel), item.OldCustomCode);
				}
			}
			foreach (CodeTemplateInfo item2 in list)
			{
				if (!File.Exists(item2.BuildPath) || Global.Option.IsOverrideFile)
				{
					item2.BuildPath.CreateFile(item2.BuildCode);
				}
			}
			"所有代码已经生成完毕，第一次使用可阅读生成的Readme.md文件!".InfoMsg();
		}
		catch (Exception ex)
		{
			("代码生成出错! \r\n" + ex.ToString()).ErrorMsg();
		}
	}

	private static void AppendFieldCode(this EntityFieldModel entityField, StringBuilder code)
	{
		string newValue = entityField.FieldName;
		if (!string.IsNullOrWhiteSpace(entityField.FieldDisplayName))
		{
			code.AppendLine("");
			code.AppendLine("\t\t/// <summary>");
			code.AppendLine("\t\t/// {{PropAnnotation}} " + entityField.FieldDescription);
			code.AppendLine("\t\t/// </summary>");
			newValue = entityField.FieldDisplayName;
		}
		if (entityField.FieldTypeStr.Contains("ICollection<"))
		{
			entityField.FieldTypeStr = entityField.FieldTypeStr.Replace("ICollection<", "List<");
		}
		entityField.FieldTypeStrFirstCharToLower = entityField.FieldTypeStr.FirstCharToLower();
		if (entityField.MaxLength.HasValue)
		{
			string text = entityField.AttributesList.Find((string o) => o.StartsWith("[MaxLength("));
			if (text != null)
			{
				code.AppendLine("\t\t" + text);
			}
			else
			{
				code.AppendLine("\t\t[MaxLength({{MaxLength}}, ErrorMessage=\"{{PropAnnotation}}超出最大长度\")]");
			}
		}
		if (entityField.MinLength.HasValue)
		{
			string text2 = entityField.AttributesList.Find((string o) => o.StartsWith("[MinLength("));
			if (text2 != null)
			{
				code.AppendLine("\t\t" + text2);
			}
			else
			{
				code.AppendLine("\t\t[MinLength({{MinLength}}, ErrorMessage=\"{{PropAnnotation}}小于最小长度\")]");
			}
		}
		if (!string.IsNullOrWhiteSpace(entityField.RegularExpression))
		{
			code.AppendLine("\t\t[RegularExpression(\"{{RegularExpression}}\",ErrorMessage=\"{{PropAnnotation}}格式错误\")]");
		}
		if (entityField.Required)
		{
			code.AppendLine("\t\t[Required(ErrorMessage=\"{{PropAnnotation}}不能为空\")]");
		}
		if (entityField.FieldName == "Id")
		{
			code.AppendLine("\t\tpublic {{PropType}}? {{PropName}} { get; set; }");
		}
		else
		{
			code.AppendLine("\t\tpublic {{PropType}} {{PropName}} { get; set; }");
		}
		code = code.Replace("{{PropAnnotation}}", newValue).Replace("{{MaxLength}}", (entityField.MaxLength ?? 0).ToString()).Replace("{{MinLength}}", (entityField.MinLength ?? 0).ToString())
			.Replace("{{RegularExpression}}", entityField.RegularExpression)
			.Replace("{{PropType}}", entityField.FieldTypeStr)
			.Replace("{{PropName}}", entityField.FieldName);
		code.Append("\r\n\r\n");
	}

	private static List<CodeTemplateInfo> CreateCodeTemplates()
	{
		BasicOptionCfg option = Global.Option;
		EntityModel entity = Global.Entity;
		SolutionInfoModel solutionInfo = Global.SolutionInfo;
		string directoryName = Path.GetDirectoryName(typeof(CommonConsts).Assembly.Location);
		string directoryName2 = Path.GetDirectoryName(Global.SolutionInfo.CurrentSelectFilePath);
		string text = string.Empty;
		string str = directoryName2.Split(new char[2] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
		string uiParentDirName = string.Join("-", str.ConvertLowerSplitArray());
		if (directoryName2.StartsWith(Global.SolutionInfo.Application.BasePath))
		{
			text = directoryName2.Replace(Global.SolutionInfo.Application.BasePath + "\\", string.Empty);
		}
		if (directoryName2.StartsWith(Global.SolutionInfo.Core.BasePath))
		{
			text = directoryName2.Replace(Global.SolutionInfo.Core.BasePath + "\\", string.Empty);
		}
		if (directoryName2.StartsWith(Global.SolutionInfo.EF.BasePath))
		{
			text = directoryName2.Replace(Global.SolutionInfo.EF.BasePath + "\\", string.Empty);
		}
		if (option.IsAbpZero)
		{
			if (directoryName2.StartsWith(Global.SolutionInfo.Application_Shared.BasePath))
			{
				text = directoryName2.Replace(Global.SolutionInfo.Application_Shared.BasePath + "\\", string.Empty);
			}
			if (directoryName2.StartsWith(Global.SolutionInfo.Core_Shared.BasePath))
			{
				text = directoryName2.Replace(Global.SolutionInfo.Core_Shared.BasePath + "\\", string.Empty);
			}
		}
		string.IsNullOrWhiteSpace(text);
		CodeGenDto dto = new CodeGenDto
		{
			EntityDir = text,
			TemplateBasePath = directoryName,
			Option = option,
			UiParentDirName = uiParentDirName,
			Entity = entity,
			SolutionInfoModel = solutionInfo
		};
		if (Global.MainViewModel.MainExtendedCfg.IsXstSolution)
		{
			return GenAbpZeroCodeTemplate.GetMoonsXStCodeTemplates(dto);
		}
		return Gen52AbpProCodeTemplate.Get52abpDefaultCodeTemplates(dto);
	}
}
