// 警告：某些程序集引用无法自动解析。这可能会导致某些部分反编译错误，
// 例如属性 getter/setter 访问。要获得最佳反编译结果，请手动将缺少的引用添加到加载的程序集列表中。
// AbpCodePowerTools.CodeAnalysis, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ab311aa47caa24fa
// AbpDtoGenerator.CodeAnalysis.SolutionEx
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;

public static class SolutionEx
{
	public static Project GetProjectByName(this Solution solution, string domainProjectName)
	{
		return (from a in solution.Projects
			where a.Name.Contains(domainProjectName)
			orderby a.Name
			select a).FirstOrDefault();
	}

	public static Document GetDocumentForSymbol(this Compilation compilation, Solution solution, string name)
	{
		List<ISymbol> list = compilation.GetSymbolsWithName((string p) => p == name, SymbolFilter.Type).ToList();
		if (list.Count != 1)
		{
			return null;
		}
		if ((list.First() as ITypeSymbol).TypeKind == TypeKind.Enum)
		{
			return null;
		}
		Location location = list.Select((ISymbol p) => p.Locations.FirstOrDefault()).FirstOrDefault();
		DocumentId val = solution.GetDocumentIdsWithFilePath(location.SourceTree.FilePath).FirstOrDefault();
		return solution.GetDocument(val);
	}

	public static Document GetDocumentByFilePath(this Solution solution, string fullName)
	{
		return (from p in solution.GetDocumentIdsWithFilePath(fullName)
			select solution.GetDocument(p)).FirstOrDefault();
	}

	public static List<string> GetPossibleProjects(this Document doc)
	{
		return ((TextDocument)doc).Project.Solution.Projects.Select((Project p) => p.Name).ToList();
	}

	public static List<DocumentId> GetDocumentIdsToOpen(this Solution newSolution, Solution oldSolution)
	{
		try
		{
			SolutionChanges changes = newSolution.GetChanges(oldSolution);
			IEnumerable<DocumentId> first = changes.GetProjectChanges().SelectMany((ProjectChanges p) => p.GetAddedDocuments());
			IEnumerable<DocumentId> second = changes.GetProjectChanges().SelectMany((ProjectChanges p) => p.GetChangedDocuments());
			return first.Concat(second).ToList();
		}
		catch (Exception)
		{
			return new List<DocumentId>();
		}
	}

	public static string GetProjectPath(this Project project)
	{
		return Path.GetDirectoryName(project.FilePath);
	}
}
