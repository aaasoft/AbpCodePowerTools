using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AbpCodePowerTools.Common;
using AbpDtoGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace AbpDtoGenerator.CodeAnalysis.GeneratorCodeServices;

public static class LoadEntityService
{
	public static EntityModel LoadEntityInConfigJson(string entityConfigPath, string entityName)
	{
		try
		{
			if (File.Exists(entityConfigPath))
			{
				return JsonConvert.DeserializeObject<EntityModel>(File.ReadAllText(entityConfigPath, EncodingEx.Utf8WithoutBom));
			}
		}
		catch (Exception ex)
		{
			("读取实体配置文件信息出错！" + ex.ToString()).ErrorMsg();
		}
		return null;
	}

	public static MainExtendedCfg LoadMainExtendedInConfigJson(string entityConfigPath)
	{
		try
		{
			if (File.Exists(entityConfigPath))
			{
				return JsonConvert.DeserializeObject<MainExtendedCfg>(File.ReadAllText(entityConfigPath, EncodingEx.Utf8WithoutBom));
			}
		}
		catch (Exception ex)
		{
			("读取实体配置文件信息出错！" + ex.ToString()).ErrorMsg();
		}
		return null;
	}

	public static EntityModel LoadEntityInfoInCurrentSelectFile(string currentSelectFilePath, string entityName)
	{
		EntityModel entityModel = new EntityModel();
		SyntaxTree syntaxTree = File.ReadAllText(currentSelectFilePath, EncodingEx.Utf8WithoutBom).ToCSharpeSyntaxTree();
		ClassDeclarationSyntax firstClassNode = syntaxTree.GetFirstClassNode();
		entityModel.Name = entityName;
		entityModel.LowerName = entityModel.Name.FirstCharToLower();
		entityModel.Namespace = syntaxTree.GetNameSpace().Result;
		GetEntityBaseClassNamesAndEntityKeyTypeStr(entityModel, firstClassNode);
		entityModel.AttributesList = firstClassNode.AttributeLists.GetFilteredAttributeList();
		entityModel.Properties = firstClassNode.GetProperties().Select(delegate(PropertyDeclarationSyntax p)
		{
			Regex reg = new Regex("^Description\\(\"(.*)\"\\)$", RegexOptions.IgnorePatternWhitespace);
			string fieldDescription = (from o in p.AttributeLists.GetStringAttributes()
				where reg.IsMatch(o)
				select reg.Match(o).Groups[1].Value).FirstOrDefault();
			return new EntityFieldModel
			{
				FieldTypeStr = p.Type.ToString(),
				FieldName = p.Identifier.Text,
				FieldDisplayName = p.GetAnnotationStr(),
				FieldNameFirstLower = p.Identifier.Text.FirstCharToLower(),
				FieldDescription = fieldDescription,
				IsSimpleProperty = p.IsSimpleProperty(),
				IsRelation = p.IsRelation(),
				IsCollection = p.IsCollection(),
				AttributesList = p.AttributeLists.GetFilteredAttributeStringList(),
				CtrlTypes = new ObservableCollection<string>
				{
					"Text", "bool", "Checkbox", "Textarea", "DatePicker", "DateTimePicker", "TimePicker", "DropdownList", "Enums", "Radio",
					"double", "float", "int", "long"
				},
				CtrlTypeIndex = 0,
				Checked = true,
				EditChecked = true,
				ListChecked = true
			};
		}).ToList();
		foreach (EntityFieldModel property in entityModel.Properties)
		{
			string fieldTypeStr = property.FieldTypeStr;
			if (fieldTypeStr.Contains("ICollection<") || fieldTypeStr.Contains("List<"))
			{
				property.CtrlTypeIndex = 7;
			}
			if (fieldTypeStr == "bool")
			{
				property.CtrlTypeIndex = 1;
			}
			if (fieldTypeStr == "DateTime")
			{
				property.CtrlTypeIndex = 5;
			}
			if (fieldTypeStr.Contains("Type") || fieldTypeStr.Contains("Enum"))
			{
				property.CtrlTypeIndex = 8;
			}
			if (fieldTypeStr == "float" || fieldTypeStr == "double")
			{
				property.CtrlTypeIndex = 10;
			}
			if (fieldTypeStr == "long" || fieldTypeStr == "int")
			{
				property.CtrlTypeIndex = 12;
			}
			property.HasAttributes();
		}
		entityModel.NameSplit = entityModel.Name.ConvertLowerSplitArray();
		entityModel.SplitName = string.Join("-", entityModel.NameSplit);
		return entityModel;
	}

	private static void GetEntityBaseClassNamesAndEntityKeyTypeStr(EntityModel entityMeta, ClassDeclarationSyntax classNode)
	{
		if (classNode.BaseList == null || classNode.BaseList.Types.Count <= 0)
		{
			return;
		}
		List<string> list = classNode.BaseList.Types.Select((BaseTypeSyntax a) => a.ToString()).ToList();
		string text = list.First();
		list.RemoveRange(0, 1);
		if (text.Length <= 2 || !text.StartsWith("I") || !char.IsUpper(text[1]))
		{
			entityMeta.BaseClassName = text;
			string empty = string.Empty;
			Match match = Regex.Match(text, ".*?<(.*?)>");
			string entityKeyName;
			if (match != null && match.Success)
			{
				entityKeyName = match.Groups[1].Value;
				empty = text.Replace("<", "Dto<");
			}
			else
			{
				entityKeyName = "int";
				empty = text + "Dto";
			}
			entityMeta.BaseClassNameList = string.Join(",", list);
			entityMeta.BaseClassDtoName = empty;
			entityMeta.EntityKeyName = entityKeyName;
			entityMeta.EntityDisplayName = classNode.GetAnnotationStr();
        }
	}
}
