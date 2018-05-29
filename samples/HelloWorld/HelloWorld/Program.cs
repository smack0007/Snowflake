using System;
using Snowflake;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new ScriptEngine();
            engine.Execute("var x = 5;");
            engine.Execute("const y = \"Hello World!\";");

            Console.WriteLine(engine["x"]);
            Console.WriteLine(engine["y"]);
            Console.ReadKey();
        }
    }
}
