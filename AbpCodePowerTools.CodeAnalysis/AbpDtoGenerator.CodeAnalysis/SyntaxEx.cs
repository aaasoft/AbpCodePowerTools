using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpDtoGenerator.CodeAnalysis.Customes;
using AbpDtoGenerator.CodeAnalysis.Enums;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AbpDtoGenerator.CodeAnalysis;

public static class SyntaxEx
{
	private static char[] BR = new char[2] { '\r', '\n' };

	private static List<Type> _simpleTypes = new List<Type>
	{
		typeof(DateTime),
		typeof(TimeSpan),
		typeof(Guid),
		typeof(byte),
		typeof(byte),
		typeof(sbyte),
		typeof(sbyte),
		typeof(char),
		typeof(char),
		typeof(decimal),
		typeof(decimal),
		typeof(double),
		typeof(double),
		typeof(float),
		typeof(float),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(long),
		typeof(long),
		typeof(ulong),
		typeof(ulong),
		typeof(short),
		typeof(short),
		typeof(ushort),
		typeof(ushort),
		typeof(string),
		typeof(string)
	};

	private static List<string> _classDataAnnotationToPreserve = new List<string> { "MetadataType" };

	private static List<string> _attributDataAnnotationToPreserve = new List<string>
	{
		"Key", "TimeStamp", "ConcurrencyCheck", "MaxLength", "MinLength", "ForeignKey", "DisplayName", "DisplayFormat", "Required", "StringLength",
		"RegularExpression", "Range", "DataType", "Validation"
	};

	public const string NewLine = "\r\n";

	public static SyntaxTrivia EndOfLineTrivia => SyntaxFactory.EndOfLine("\r\n");

	public static async Task<SyntaxNode> GetRootNode(this SyntaxTree syntaxTree)
	{
		return await syntaxTree.GetRootAsync();
	}

	public static async Task<List<PropertyDeclarationSyntax>> GetPropertysAsync(this SyntaxTree syntaxTree)
	{
		return (await syntaxTree.GetRootAsync()).GetClassNodes()[0].GetProperties();
	}

	public static List<ClassDeclarationSyntax> GetClassNodes(this SyntaxTree syntaxTree)
	{
		return syntaxTree.GetRootNode().Result.GetClassNodes();
	}

	public static List<ClassDeclarationSyntax> GetClassNodes(this SyntaxNode root)
	{
		return root.DescendantNodes((SyntaxNode p) => !(p is ClassDeclarationSyntax)).OfType<ClassDeclarationSyntax>().ToList();
	}

	public static ClassDeclarationSyntax GetFirstClassNode(this SyntaxTree syntaxTree)
	{
		return syntaxTree.GetRootNode().Result.GetFirstClassNode();
	}

	public static ClassDeclarationSyntax GetFirstClassNode(this SyntaxNode root)
	{
		return root.DescendantNodes((SyntaxNode p) => !(p is ClassDeclarationSyntax)).OfType<ClassDeclarationSyntax>().FirstOrDefault();
	}

	public static NamespaceDeclarationSyntax GetNamespaceNode(this SyntaxNode root)
	{
		return root.DescendantNodes((SyntaxNode p) => !(p is NamespaceDeclarationSyntax)).OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
	}

	public static List<PropertyDeclarationSyntax> GetProperties(this ClassDeclarationSyntax classNode)
	{
		return (from p in classNode.DescendantNodes((SyntaxNode p) => !(p is PropertyDeclarationSyntax)).OfType<PropertyDeclarationSyntax>()
			where p.Modifiers.Any((SyntaxToken m) => m.Kind() == SyntaxKind.PublicKeyword)
			where p.FirstAncestorOrSelf<ClassDeclarationSyntax>() == classNode
			where p.AccessorList != null
			where p.AccessorList.Accessors.Any((AccessorDeclarationSyntax a) => a.Kind() == SyntaxKind.GetAccessorDeclaration)
			where p.AccessorList.Accessors.Any((AccessorDeclarationSyntax a) => a.Kind() == SyntaxKind.SetAccessorDeclaration)
			select p).ToList();
	}

	public static List<AttributeListSyntax> GetFilteredAttributeList(this SyntaxList<AttributeListSyntax> attributeGroups)
	{
		return (from a in attributeGroups
			where a.Attributes.Any((AttributeSyntax att) => _attributDataAnnotationToPreserve.Contains(att.Name.ToString()))
			select a.RemoveNodes(a.Attributes.Where((AttributeSyntax att) => !_attributDataAnnotationToPreserve.Contains(att.Name.ToString())).ToArray(), SyntaxRemoveOptions.KeepNoTrivia)).ToList();
	}

	public static List<string> GetFilteredAttributeStringList(this SyntaxList<AttributeListSyntax> attributeGroups)
	{
		var list = (from a in attributeGroups
			where a.Attributes.Any((AttributeSyntax att) => _attributDataAnnotationToPreserve.Contains(att.Name.ToString()))
			select a.RemoveNodes(a.Attributes.Where((AttributeSyntax att) => !_attributDataAnnotationToPreserve.Contains(att.Name.ToString())).ToArray(), SyntaxRemoveOptions.KeepNoTrivia)).ToList();
		List<string> list2 = new List<string>();
		foreach (AttributeListSyntax item in list)
		{
			list2.Add(item.ToString());
		}
		return list2;
	}

	public static async Task<string> GetNameSpace(this SyntaxTree syntaxTree)
	{
		return (await syntaxTree.GetRootAsync()).GetNamespaceNode().Name.ToString();
	}

	public static bool IsSimpleProperty(this PropertyDeclarationSyntax propertyNode)
	{
		if (propertyNode.Type is NullableTypeSyntax nullableTypeSyntax)
		{
			return nullableTypeSyntax.ElementType.IsSimpleType();
		}
		if (propertyNode.Type is GenericNameSyntax { Identifier: var identifier } genericNameSyntax && identifier.ToString() == "Nullable")
		{
			return genericNameSyntax.TypeArgumentList.Arguments.First().IsSimpleType();
		}
		return propertyNode.Type.IsSimpleType();
	}

	public static string GetAnnotationStr(this PropertyDeclarationSyntax propertyNode)
	{
		string text = string.Empty;
		string text2 = propertyNode.Modifiers.ToList().FirstOrDefault().LeadingTrivia.ToString();
        //首先尝试读取注释
        if (string.IsNullOrWhiteSpace(text))
        {
            string[] array = text2.Split(BR, StringSplitOptions.RemoveEmptyEntries);
            foreach (string text3 in array)
            {
                if (!string.IsNullOrWhiteSpace(text3) && !text3.Contains("summary"))
                {
                    text = text3.Replace("///", string.Empty).Trim();
                    break;
                }
            }
        }
        //其次尝试读取Description特性
        if (string.IsNullOrWhiteSpace(text))
        {
            var descriptionString = propertyNode.AttributeLists
                .FirstOrDefault(t => t.ToString().Contains("[Description("))?
                .ToString();
            if (!string.IsNullOrWhiteSpace(descriptionString))
            {
                var startIndex = descriptionString.IndexOf('\"') + 1;
                var endIndex = descriptionString.LastIndexOf('\"');
                text = descriptionString.Substring(startIndex, endIndex - startIndex);
            }
        }
		//最后使用属性名称
        if (string.IsNullOrWhiteSpace(text))
		{
			text = propertyNode.Identifier.Text;
		}
		return text;
	}

	public static bool IsRelation(this PropertyDeclarationSyntax p)
	{
		return !p.IsSimpleProperty();
	}

	public static bool IsCollection(this PropertyDeclarationSyntax p)
	{
		if (p.Type is GenericNameSyntax { Identifier: var identifier } && identifier.ToString() != "Nullable")
		{
			return true;
		}
		return false;
	}

	public static bool IsSimpleType(this TypeSyntax type)
	{
		if (_simpleTypes.Select((Type p) => p.Namespace + "." + p.Name).Concat(_simpleTypes.Select((Type p) => p.Name)).ToList()
			.Contains(type.ToString()))
		{
			return true;
		}
		if (type is PredefinedTypeSyntax)
		{
			return true;
		}
		return false;
	}

	public static bool HasDataContract(this SyntaxNode rootNode)
	{
		return rootNode?.ToString().Contains("[DataContract]") ?? false;
	}

	public static bool HasEntities(this SyntaxNode rootNode)
	{
		return rootNode?.GetClassNodes().First().GetProperties()
			.Any((PropertyDeclarationSyntax p) => (p.Type.ToString().Length > 3 && p.Type.ToString().Substring(p.Type.ToString().Length - 3, 3) == "DTO") || (p.Type.ToString().Length > 14 && p.Type.ToString().Substring(0, 11) == "ICollection" && p.Type.ToString().Substring(p.Type.ToString().Length - 4, 3) == "DTO")) ?? false;
	}

	public static bool HasBaseDto(this SyntaxNode rootNode, string baseDtoName)
	{
		if (rootNode == null)
		{
			return false;
		}
		BaseDtoClassLocator baseDtoClassLocator = new BaseDtoClassLocator();
		baseDtoClassLocator.Visit(rootNode);
		return baseDtoClassLocator.BaseDtoName == baseDtoName;
	}

	public static bool HasDataAnnotations(SyntaxNode rootNode)
	{
		if (rootNode == null)
		{
			return false;
		}
		if (rootNode.ToString().Contains("[MetadataType"))
		{
			return true;
		}
		return rootNode.GetClassNodes().First().GetProperties()
			.Any((PropertyDeclarationSyntax p) => p.AttributeLists.Any((AttributeListSyntax a) => a.Attributes.Any((AttributeSyntax att) => _attributDataAnnotationToPreserve.Contains(att.Name.ToString()))));
	}

	public static bool HasStyleCop(SyntaxNode rootNode)
	{
		if (rootNode == null)
		{
			return false;
		}
		if (rootNode.ToString().Contains("#pragma warning disable CS1591"))
		{
			return true;
		}
		return false;
	}

	public static bool HasMapEntitiesById(SyntaxNode rootNode)
	{
		if (rootNode == null)
		{
			return false;
		}
		List<PropertyDeclarationSyntax> properties = rootNode.GetFirstClassNode().GetProperties();
		IEnumerable<string> propname = properties.Select((PropertyDeclarationSyntax p) => p.Identifier.ToString());
		return propname.Any((string p) => propname.Contains(p + "Id") || propname.Contains(p + "Ids"));
	}

	public static TypeSyntax ToCollectionType(this string type, string collectionType)
	{
		return SyntaxFactory.GenericName(SyntaxFactory.Identifier(collectionType)).WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList((TypeSyntax)SyntaxFactory.IdentifierName(type))));
	}

	public static NameSyntax SyntaxNameFromFullName(this string fullName)
	{
		if (fullName.Count((char p) => p == '.') == 0)
		{
			return SyntaxFactory.IdentifierName(fullName);
		}
		string[] source = fullName.Split('.');
		if (fullName.Count((char p) => p == '.') == 1)
		{
			return SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName(source.First()), SyntaxFactory.IdentifierName(source.Last()));
		}
		return SyntaxFactory.QualifiedName(fullName.Substring(0, fullName.LastIndexOf('.')).SyntaxNameFromFullName(), SyntaxFactory.IdentifierName(source.Last()));
	}

	public static CompilationUnitSyntax AppendUsing(this CompilationUnitSyntax node, params string[] usingDirectives)
	{
		List<string> list = (from p in node.DescendantNodes((SyntaxNode p) => !(p is ClassDeclarationSyntax)).OfType<UsingDirectiveSyntax>()
			select p.Name.ToString()).ToList();
		SyntaxList<UsingDirectiveSyntax> usings = node.Usings;
		foreach (string text in usingDirectives)
		{
			if (text != null && !list.Contains(text))
			{
				usings = usings.Add(text.ToUsing());
			}
		}
		return node.WithUsings(usings);
	}

	public static NamespaceDeclarationSyntax AppendUsing(this NamespaceDeclarationSyntax node, params string[] usingDirectives)
	{
		List<string> list = (from p in node.DescendantNodes((SyntaxNode p) => !(p is ClassDeclarationSyntax)).OfType<UsingDirectiveSyntax>()
			select p.Name.ToString()).ToList();
		foreach (string text in usingDirectives)
		{
			if (text != null && !list.Contains(text))
			{
				list.Add(text);
			}
		}
		list.Sort();
		SyntaxList<UsingDirectiveSyntax> syntaxList = node.Usings;
		while (syntaxList.Count > 0)
		{
			syntaxList = syntaxList.RemoveAt(0);
		}
		syntaxList = syntaxList.AddRange(list.Select((string u) => u.ToUsing()));
		return node.WithUsings(syntaxList);
	}

	public static UsingDirectiveSyntax ToUsing(this string @namespace)
	{
		return @namespace.SyntaxNameFromFullName().ToUsing();
	}

	public static UsingDirectiveSyntax ToUsing(this NameSyntax nameSyntaxNode)
	{
		return SyntaxFactory.UsingDirective(nameSyntaxNode.PrependWhitespace()).AppendNewLine();
	}

	public static TNode AppendNewLine<TNode>(this TNode node, bool preserveExistingTrivia = true) where TNode : SyntaxNode
	{
		SyntaxTriviaList trivia = (preserveExistingTrivia ? node.GetTrailingTrivia() : SyntaxFactory.TriviaList()).Add(EndOfLineTrivia);
		return node.WithTrailingTrivia(trivia);
	}

	public static BaseListSyntax ToBaseClassList(this string baseClass)
	{
		return SyntaxFactory.BaseList(SyntaxFactory.SingletonSeparatedList((BaseTypeSyntax)SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName(baseClass).PrependWhitespace()))).AppendNewLine();
	}

	public static FieldDeclarationSyntax DeclareField(string type, bool autoCreateNew)
	{
		string text = "_" + char.ToLower(type[0]) + type.Substring(1);
		return SyntaxFactory.FieldDeclaration(SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName(type)).WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(text)).WithInitializer(SyntaxFactory.EqualsValueClause(SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName(type)).WithArgumentList(SyntaxFactory.ArgumentList())))))).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword))).NormalizeWhitespace("    ", "\r\n", elasticTrivia: true)
			.AppendNewLine();
	}

	public static ExpressionStatementSyntax InvocationStatement(this string member, params string[] args)
	{
		return SyntaxFactory.ExpressionStatement(member.ToMethodInvocation(args.Select((string p) => p.ToMemberAccess()).ToArray()));
	}

	public static ExpressionStatementSyntax AssignmentStatement(this string left, string right)
	{
		return SyntaxFactory.ExpressionStatement(left.AssignmentExpression(right)).NormalizeWhitespace("    ", "\r\n", elasticTrivia: true).AppendNewLine();
	}

	public static InvocationExpressionSyntax ToMethodInvocation(this string method, params ExpressionSyntax[] args)
	{
		return method.ToMemberAccess().ToMethodInvocation(args);
	}

	public static InvocationExpressionSyntax ToMethodInvocation(this ExpressionSyntax methodMember, params ExpressionSyntax[] args)
	{
		IEnumerable<ArgumentSyntax> nodes = args.Select((ExpressionSyntax p) => SyntaxFactory.Argument(p));
		return SyntaxFactory.InvocationExpression(methodMember).WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(nodes)));
	}

	public static SimpleNameSyntax ToName(this string identifier)
	{
		return SyntaxFactory.IdentifierName(identifier);
	}

	public static ExpressionSyntax ToMemberAccess(this string selector)
	{
		string[] source = selector.Split('.');
		if (source.Count() == 1)
		{
			return source.First().ToName();
		}
		if (source.Count() == 2)
		{
			if (source.First() == "this")
			{
				return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ThisExpression(), SyntaxFactory.IdentifierName(source.Last()));
			}
			return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(source.First()), SyntaxFactory.IdentifierName(source.Last()));
		}
		string selector2 = string.Join(".", source.Take(source.Count() - 1));
		return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, selector2.ToMemberAccess(), SyntaxFactory.IdentifierName(source.Last()));
	}

	public static SyntaxList<AttributeListSyntax> CreateAttributes(params string[] attributes)
	{
		return SyntaxFactory.SingletonList(SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(attributes.Select((string p) => SyntaxFactory.Attribute(SyntaxFactory.IdentifierName(p))))));
	}

	public static PropertyDeclarationSyntax DeclareAutoProperty(this TypeSyntax type, string identifier)
	{
		return SyntaxFactory.PropertyDeclaration(type.AppendWhitespace(), SyntaxFactory.Identifier(identifier).AppendWhitespace()).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword).AppendWhitespace())).WithAccessorList(SyntaxFactory.AccessorList(SyntaxFactory.List(new AccessorDeclarationSyntax[2]
		{
			PropertyAccessor(PropertyAccessorType.Get),
			PropertyAccessor(PropertyAccessorType.Set)
		})))
			.AppendNewLine();
	}

	public static AccessorDeclarationSyntax PropertyAccessor(PropertyAccessorType type)
	{
		AccessorDeclarationSyntax accessorDeclarationSyntax = SyntaxFactory.AccessorDeclaration((type == PropertyAccessorType.Get) ? SyntaxKind.GetAccessorDeclaration : SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)).AppendWhitespace();
		if (type == PropertyAccessorType.Get)
		{
			accessorDeclarationSyntax = accessorDeclarationSyntax.PrependWhitespace();
		}
		return accessorDeclarationSyntax;
	}

	public static TNode PrependWhitespace<TNode>(this TNode node) where TNode : SyntaxNode
	{
		return node.WithLeadingTrivia(node.GetLeadingTrivia().Add(SyntaxFactory.Whitespace(" ")));
	}

	public static TNode AppendWhitespace<TNode>(this TNode node) where TNode : SyntaxNode
	{
		return node.WithTrailingTrivia(node.GetTrailingTrivia().Add(SyntaxFactory.Whitespace(" ")));
	}

	public static SyntaxToken AppendWhitespace(this SyntaxToken token)
	{
		return token.WithTrailingTrivia(token.TrailingTrivia.Add(SyntaxFactory.Whitespace(" ")));
	}

	public static SyntaxToken AppendNewLine(this SyntaxToken token)
	{
		return token.WithTrailingTrivia(token.TrailingTrivia.Add(EndOfLineTrivia));
	}

	public static ExpressionSyntax WrapInConditional(this ExpressionSyntax expression, string propType)
	{
		List<BinaryExpressionSyntax> list = new List<BinaryExpressionSyntax>();
		MemberAccessExpressionSyntax memberAccessExpressionSyntax = expression as MemberAccessExpressionSyntax;
		while (memberAccessExpressionSyntax != null && memberAccessExpressionSyntax.Expression is MemberAccessExpressionSyntax)
		{
			BinaryExpressionSyntax item = SyntaxFactory.BinaryExpression(SyntaxKind.NotEqualsExpression, memberAccessExpressionSyntax.Expression, SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));
			list.Add(item);
			memberAccessExpressionSyntax = memberAccessExpressionSyntax.Expression as MemberAccessExpressionSyntax;
		}
		list.Reverse();
		if (list.Count == 0)
		{
			return expression;
		}
		ExpressionSyntax expressionSyntax = list.First();
		for (int i = 1; i < list.Count; i++)
		{
			expressionSyntax = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, expressionSyntax, list[i]);
		}
		ExpressionSyntax whenFalse = ((propType == null) ? ((ExpressionSyntax)SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)) : ((ExpressionSyntax)SyntaxFactory.DefaultExpression(SyntaxFactory.ParseTypeName(propType))));
		return SyntaxFactory.ConditionalExpression(expressionSyntax, expression, whenFalse).NormalizeWhitespace();
	}

	public static ExpressionSyntax AssignmentExpression(this string left, string right, string propType = null, bool verifyRightNotNull = false)
	{
		ExpressionSyntax expressionSyntax = right.ToMemberAccess();
		ExpressionSyntax node = (verifyRightNotNull ? expressionSyntax.WrapInConditional(propType) : expressionSyntax);
		return SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, left.ToMemberAccess().AppendWhitespace(), node.PrependWhitespace());
	}

	public static PropertyDeclarationSyntax GeneratePropertyDeclarationFromString(this string property)
	{
		return (PropertyDeclarationSyntax)((ClassDeclarationSyntax)((NamespaceDeclarationSyntax)((CompilationUnitSyntax)CSharpSyntaxTree.ParseText("\r\nnamespace MyNameSapce \r\n{\r\n    class MyDTO\r\n    {\r\n        " + property + "\r\n    }\r\n}").GetRoot()).Members[0]).Members[0]).Members[0];
	}

	public static ExpressionSyntax GenerateAssignmentExpressionFromString(this string expression)
	{
		return CSharpSyntaxTree.ParseText("\r\nnamespace MyNameSapce\r\n{\r\n    class MyMapper\r\n    {\r\n        public override Expression<Func<MyEntities, MyDTO>> SelectorExpression\r\n        {\r\n            get\r\n            {\r\n                return ((Expression<Func<MyEntities, MyDTO>>)(p => new MyDTO()\r\n                {\r\n                    " + expression + "\r\n                }));\r\n            }\r\n        }\r\n    }\r\n}").GetRoot().DescendantNodes()
			.OfType<AssignmentExpressionSyntax>()
			.FirstOrDefault();
	}

	public static ExpressionStatementSyntax AssignmentStatementFromString(this string expression)
	{
		return CSharpSyntaxTree.ParseText("\r\nnamespace MyNameSapce\r\n{\r\n    class MyMapper\r\n    {\r\n        public override void MapToModel(MyDTO dto, MyEntities model)\r\n        {\r\n            " + expression + "\r\n        }\r\n    }\r\n}").GetRoot().DescendantNodes()
			.OfType<ExpressionStatementSyntax>()
			.FirstOrDefault();
	}
}
