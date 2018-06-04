using System;
using Snowflake;

namespace HelloWorld
{
    public static class Program
    {
        public static void Main(string[] args)
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
            engine.Execute("sayHello(\"Bob\");");

            Console.WriteLine();
            Console.WriteLine($"x: {engine["x"]}");
            Console.WriteLine($"y: {engine["y"]}");
            Console.WriteLine($"z: {engine["z"]}");

            if (IsConsolePresent())
            {
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        private static bool IsConsolePresent()
        {
            try
            {
                int WindowHeight = Console.WindowHeight;
                return WindowHeight >= 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
