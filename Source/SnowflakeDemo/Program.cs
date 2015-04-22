using System;
using System.Linq;
using Snowflake;
using System.IO;
using System.Collections.Generic;

namespace SnowflakeDemo
{
    public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<Person> Friends { get; private set; }

        public Person()
        {
            this.Friends = new List<Person>();
        }

        public override string ToString()
        {
            return this.FirstName + " " + this.LastName;
        }
    }

	class Program
	{
		static void Main(string[] args)
		{			
			Random random = new Random();
                        
            ScriptEngine engine = new ScriptEngine();

            File.WriteAllText(@"..\..\..\SnowflakeDemoOutput\Script1.cs", engine.GenerateCode(File.ReadAllText("SnowflakeDemo.sfs"), "Script1"));

            engine.SetGlobalFunction<object>("print", (x) => Console.WriteLine(x));
            engine.RegisterType("Namespace.Person", typeof(Person));
            engine.RegisterType("System.Console", typeof(Console));
            engine.RegisterType("System.TimeSpan", typeof(TimeSpan));
            engine.RegisterType("System.Tuple", typeof(Tuple<>));
            engine.RegisterType("System.Tuple", typeof(Tuple<,>));
            //engine.RegisterAllTypesInNamespace("System", "System");
            //engine.SetGlobalFunction("GetTupleType", () => { return new ScriptType(typeof(Tuple), new ScriptType("int"), new ScriptType("string")); });
            engine.SetGlobalFunction("import", (Func<string, ScriptType>)((name) => { return ScriptUtilityFunctions.Import(engine, name); }));
            engine.SetGlobalFunction("export", (Action<string, dynamic>)((name, value) => { ScriptUtilityFunctions.Export(engine, name, value); }));
                           
            var result = engine.ExecuteFile("SnowflakeDemo.sfs");
                        
            //Console.WriteLine("Result is: {0} ({1})", result, result.GetType());

            var export = engine.GetGlobalVariable("Script.Export");

            Console.WriteLine("Script.Export:");
            foreach (var pair in export)
            {
                Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
            }

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
