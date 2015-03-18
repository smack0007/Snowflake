using System;
using System.Linq;
using Snowflake;
using System.IO;

namespace SnowflakeDemo
{
	class Program
	{
        class Person
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }
        }

		static void Main(string[] args)
		{			
			Random random = new Random();
                        
            ScriptDictionary export = new ScriptDictionary();
            ScriptEngine engine = new ScriptEngine();

            File.WriteAllText("Output.cs", engine.GenerateCode(File.ReadAllText("SnowflakeDemo.sfs"), "Script1"));
							
			engine.SetGlobalFunction<object>("print", (x) => Console.WriteLine(x));
			engine.SetGlobalVariable("export", export);
            engine.RegisterType("Person", typeof(Person));
                        
            var result = engine.ExecuteFile("SnowflakeDemo.sfs");

			//Console.WriteLine("Result is: {0} ({1})", result, result.GetType());
                        
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
