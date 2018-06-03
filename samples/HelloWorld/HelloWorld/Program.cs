using System;
using Snowflake;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new ScriptEngine();
            engine["print"] = new Action<object>((x) => Console.WriteLine(x));

            engine.Execute(@"var sayHello = func(name) {
    print(""Hello "" + name);
};");

            engine.Execute("var x = 5;");
            engine.Execute("const y = 3;");
            engine.Execute("var z = x + y;");
            engine.Execute("z = z + 2;");
            engine.Execute("sayHello(\"Joe\");");

            Console.WriteLine();
            Console.WriteLine($"x: {engine["x"]}");
            Console.WriteLine($"y: {engine["y"]}");
            Console.WriteLine($"z: {engine["z"]}");
            Console.ReadKey();
        }
    }
}
