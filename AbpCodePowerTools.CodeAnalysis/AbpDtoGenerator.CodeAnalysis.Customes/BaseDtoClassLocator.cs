using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AbpDtoGenerator.CodeAnalysis.Customes;

public class BaseDtoClassLocator : CSharpSyntaxWalker
{
	public string BaseDtoName { get; set; }

	public override void VisitClassDeclaration(ClassDeclarationSyntax node)
	{
		if (!node.Identifier.ToString().Contains("Mapper") && node != null && (node.BaseList?.Types).HasValue)
		{
			BaseDtoName = node.BaseList.Types[0].Type.ToString();
		}
		base.VisitClassDeclaration(node);
	}
}
