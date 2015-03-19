using System;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Snowflake.CodeGeneration
{
	public class CodeCompiler
	{        
		public Script Compile(string code, string className)
		{
			SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

			var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            var compilation = CSharpCompilation.Create(className)
				.WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "mscorlib.dll")))
                .AddReferences(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.dll")))
                .AddReferences(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll")))
                .AddReferences(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "Microsoft.CSharp.dll")))
                .AddReferences(MetadataReference.CreateFromFile(typeof(Script).Assembly.Location))
				.AddSyntaxTrees(syntaxTree);

			using (MemoryStream stream = new MemoryStream())
			{
				var result = compilation.Emit(stream);

				if (!result.Success)
				{
					string message = string.Join(Environment.NewLine, result.Diagnostics);
					throw new CodeCompilationException(string.Format("Failed to compile script:\n{0}", message), code);
				}

				Assembly assembly = Assembly.Load(stream.ToArray());
                return (Script)Activator.CreateInstance(assembly.GetType(CodeGenerator.GeneratedCodeNamespace + "." + className));
			}			
		}
	}
}
