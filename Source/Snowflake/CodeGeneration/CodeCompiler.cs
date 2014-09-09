using System;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Snowflake.CodeGeneration
{
	public class CodeCompiler
	{
		public Script Compile(string id, string code)
		{
			SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

			string scriptName = id + "Script";

			var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

			var compilation = CSharpCompilation.Create(scriptName)
				.WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
				.AddReferences(new MetadataFileReference(Path.Combine(assemblyPath, "mscorlib.dll")))
				.AddReferences(new MetadataFileReference(Path.Combine(assemblyPath, "System.dll")))
				.AddReferences(new MetadataFileReference(Path.Combine(assemblyPath, "System.Core.dll")))
				.AddReferences(new MetadataFileReference(Path.Combine(assemblyPath, "Microsoft.CSharp.dll")))
				.AddReferences(new MetadataFileReference(typeof(Script).Assembly.Location))
				.AddSyntaxTrees(syntaxTree);

			using (MemoryStream stream = new MemoryStream())
			{
				var result = compilation.Emit(stream);

				if (!result.Success)
				{
					string message = string.Join(Environment.NewLine, result.Diagnostics);
					throw new CodeCompilationException(string.Format("Failed to compile {0}:\n{1}", id, message), id, code);
				}

				Assembly assembly = Assembly.Load(stream.ToArray());
				return (Script)Activator.CreateInstance(assembly.GetType(CodeGenerator.GeneratedCodeNamespace + "." + scriptName));
			}			
		}
	}
}
