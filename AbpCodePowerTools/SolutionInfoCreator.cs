using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AbpDtoGenerator;
using AbpDtoGenerator.CodeAnalysis;
using AbpDtoGenerator.Enums;
using AbpDtoGenerator.GeneratorModels;
using AbpDtoGenerator.Models;
using Microsoft.CodeAnalysis;

namespace AbpCodePowerTools;

public static class SolutionInfoCreator
{
	public static SolutionInfoModel Create(Document currentSelectedDocument)
	{
		bool flag = false;
		Solution solution = currentSelectedDocument.Project.Solution;
        currentSelectedDocument.GetPossibleProjects();
		List<Project> list = solution.Projects.ToList();
		string text = currentSelectedDocument.Project.Name;
		if (text.Contains("("))
		{
			text = text.Substring(0, text.IndexOf("("));
		}
		string projectStartName = currentSelectedDocument.Project.Name.Substring(0, text.LastIndexOf("."));
		if (list.Exists((Project p) => p.Name.Contains(".Shared")))
		{
			flag = true;
		}
		Project val = null;
		Project val2 = null;
		Project val3 = null;
		Project val4 = null;
		Project val5 = null;
		Project val6 = null;
		Project val7 = null;
		Project val8 = null;
		Project val9 = null;
		Project val10 = null;
		if (flag)
		{
			val = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".Application") && !item.Name.Contains(".Shared"));
			val2 = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".Application.Shared"));
			val3 = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".Core") && !item.Name.Contains(".Shared"));
			val4 = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".Core.Shared"));
			val5 = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".EntityFrameworkCore") && !item.Name.Contains(".Shared"));
			val7 = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Mvc"));
			val8 = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Portal"));
			val9 = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Host"));
			val10 = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Core"));
		}
		else
		{
			val = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".Application"));
			val3 = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".Core"));
			val5 = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".EntityFrameworkCore"));
			val7 = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Mvc"));
			val8 = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Portal"));
			val9 = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Host"));
			val10 = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Core"));
			if (val5 == null)
			{
				"未能找到实体对应的.EntityFrameworkCore类库！".ErrorMsg();
				return null;
			}
		}
		val6 = list.Find((Project item) => item.Name.StartsWith(projectStartName + ".Tests"));
		if (val == null || val3 == null || val5 == null)
		{
			"当前项目不是标准的ABP框架结构！请检查框架是否正确！".ErrorMsg();
			return null;
		}
		string empty = string.Empty;
		string empty2 = string.Empty;
		ProjectPathInfo application = new ProjectPathInfo
		{
			PType = ProjectType.Application,
			BasePath = val.GetProjectPath(),
			ProjectName = val.Name
		};
		ProjectPathInfo core = new ProjectPathInfo
		{
			PType = ProjectType.Core,
			BasePath = val3.GetProjectPath(),
			ProjectName = val3.Name
		};
		ProjectPathInfo eF = new ProjectPathInfo
		{
			PType = ProjectType.Ef,
			BasePath = val5.GetProjectPath(),
			ProjectName = val5.Name
		};
		string[] array = text.Split('.');
		if (array.Length < 3)
		{
			empty = array[0];
			empty2 = string.Empty;
		}
		else
		{
			if (array.Length <= 2)
			{
				throw new Exception("解决方案项目的多层架构不符合52ABP规范，建议名称为Company.Project的形式。");
			}
			empty = array[1];
			empty2 = array[0] + ".";
		}
		SolutionInfoModel solutionInfoModel = new SolutionInfoModel();
		solutionInfoModel.IsAbpZero = flag;
		solutionInfoModel.Application = application;
		solutionInfoModel.Core = core;
		solutionInfoModel.EF = eF;
		solutionInfoModel.SolutionNamespace = empty;
		solutionInfoModel.CompanyNamespace = empty2;
		solutionInfoModel.CurrentProjectName = text;
		solutionInfoModel.CurrentSelectFilePath = ((TextDocument)currentSelectedDocument).FilePath;
		solutionInfoModel.SolutionPath = Path.GetDirectoryName(solution.FilePath);
		if (val9 != null)
		{
			ProjectPathInfo host = new ProjectPathInfo
			{
				PType = ProjectType.Host,
				BasePath = val9.GetProjectPath(),
				ProjectName = val9.Name
			};
			solutionInfoModel.Host = host;
		}
		if (val10 != null)
		{
			ProjectPathInfo webCore = new ProjectPathInfo
			{
				PType = ProjectType.WebCore,
				BasePath = val10.GetProjectPath(),
				ProjectName = val10.Name
			};
			solutionInfoModel.WebCore = webCore;
		}
		if (val7 != null)
		{
			ProjectPathInfo mVC = new ProjectPathInfo
			{
				PType = ProjectType.Mvc,
				BasePath = val7.GetProjectPath(),
				ProjectName = val7.Name
			};
			solutionInfoModel.MVC = mVC;
		}
		if (val6 != null)
		{
			ProjectPathInfo tests = new ProjectPathInfo
			{
				PType = ProjectType.XUnitTests,
				BasePath = val6.GetProjectPath(),
				ProjectName = val6.Name
			};
			solutionInfoModel.Tests = tests;
		}
		if (val8 != null)
		{
			ProjectPathInfo portal = new ProjectPathInfo
			{
				PType = ProjectType.Portal,
				BasePath = val8.GetProjectPath(),
				ProjectName = val8.Name
			};
			solutionInfoModel.Portal = portal;
		}
		if (flag)
		{
			ProjectPathInfo core_Shared = (solutionInfoModel.Application_Shared = new ProjectPathInfo
			{
				PType = ProjectType.Application_Shared,
				BasePath = val2.GetProjectPath(),
				ProjectName = val2.Name
			});
			new ProjectPathInfo
			{
				PType = ProjectType.Core_Shared,
				BasePath = val4.GetProjectPath(),
				ProjectName = val4.Name
			};
			solutionInfoModel.Core_Shared = core_Shared;
		}
		return solutionInfoModel;
	}
}
