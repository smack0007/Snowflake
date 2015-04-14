# Snowflake

Snowflake is a scripting language which compiles to C#. The following is an example Snowflake script:

```
func buildMultiplier(x) {
	return func(y) {
		return x * y;
	};
}

var x5 = buildMultiplier(5);

for (var i = 0; i < 10; i += 1) {
	print("5 * " + i + " = " + x5(i));
}

var values = [ 1, 2, 3, 4, 5 ];
print(values.Count);
```

This script compiles to the following C# code:

```csharp
using System;
using System.Collections.Generic;

namespace Snowflake.Generated
{
	public class Script1 : Script
	{
		public override dynamic Execute(ScriptExecutionContext context)
		{
			try
			{
				context.DeclareVariable("buildMultiplier", isConst: true, value: new ScriptFunction(new Func<dynamic, dynamic>((v2) => { 
					context.PushStackFrame("buildMultiplier");
					context.DeclareVariable("x", v2);
					bool isError1 = false;
					try {
						return new ScriptFunction(new Func<dynamic, dynamic>((v3) => { 
							context.PushStackFrame("<anonymous>");
							context.DeclareVariable("y", v3);
							context.DeclareVariable("x", v2);
							bool isError2 = false;
							try {
								return (context["x"] * context["y"]);
							} catch(Exception) {
								isError2 = true;
								throw;
							} finally {
								if (!isError2) {
									context.PopStackFrame();
								}
							}
						}), null);
					} catch(Exception) {
						isError1 = true;
						throw;
					} finally {
						if (!isError1) {
							context.PopStackFrame();
						}
					}
				}), null));
				context.DeclareVariable("x5", value: Invoke(context, context["buildMultiplier"], 5));
				for (context.DeclareVariable("i", value: 0); (context["i"] < 10); context["i"] += 1) {
					Invoke(context, context["print"], ((("5 * " + context["i"]) + " = ") + Invoke(context, context["x5"], context["i"])));
				}
				context.DeclareVariable("values", value: new ScriptList { 1, 2, 3, 4, 5 });
				Invoke(context, context["print"], context["values"].Count);
				return null;
			}
			catch (Exception ex)
			{
				throw new ScriptExecutionException(ex.Message, context.GetStackFrames(), ex);
			}
		}
	}
}
```

## Interop

Snowflake scripts by default do not have access to any .NET BCL classes or methods besides those types which are baked into the language. Access can
be given to scripts if necessary by setting global variables inside the ScriptEngine:

```csharp
engine.SetGlobalFunction<object>("print", (x) => Console.WriteLine(x));
```

In order to allow objects to be created, you'll need to register the objects with the ScriptEngine:

```csharp
engine.RegisterType("MyApp.Person", typeof(Person));
```

Once the ScriptEngine has access to the constructor, your scripts can create objects:

```
var person = new MyApp.Person();
person.FirstName = "Bob";
person.LastName = "Freeman";
person.Age = 42;
```

Static objects can be made available via the SetGlobalStaticObject function:

```csharp
engine.SetGlobalStaticObject("Console", typeof(Console));
```

This makes the static methods of the Console object directly available in your script:

```
Console.WriteLine("Hello World!");
```


## Language Structure

The following is the structure of the language following the EBNF language as closely as possible:

```
Script = <Statement>* EOF ;

StatementBlock = "{" <Statement>* "}" ;

Statement = <ConstDeclaration> ";" |
            <VariableDeclaration> ";" |
            <FunctionDeclaration> |
			<If> |
			<While> |
			<For> |
			<ForEach> |
			<Return> |
			<Expression> ";" ;

ConstDeclaration = "const" <Identifier> "=" <Expression> ;

VariableDeclaration = "var" <Identifier> ( "=" <Expression> )? ;

FunctionDeclaration = "func" <Identifier> "(" <FunctionParameters>  ")" <StatementBlock> ;

FunctionParameters = <Identifier>? ( "," <Identifier> )* ;

If = "if" "(" <Expression> ")" <StatementBlock> { "else" <StatementBlock> } ;

While = "while" "(" <Expression> ")" <StatementBlock> ;

For = "for" "(" ( <VariableDeclaration> | <Assignment> ) ";" <Expression> ";" <Assignment>  ")" <StatementBlock> ;

ForEach = "foreach" "(" <VariableDeclaration> "in" <Expression>  ")" <StatementBlock> ;

Return = "return" <Expression> ";" ;

Expression = <AssignmentExpression> ;

AssignmentExpression = <ConditionalOrExpression> | <Assignment> ;

ConditionalOrExpression = <ConditionalAndExpression> ( "||" <ConditionalAndExpression> )* ;

ConditionalAndExpression = <EqualityExpression> ( "&&" <EqualityExpression> )* ;

EqualityExpression = <RelationalExpression> ( ( "==" | "!=" ) <RelationalExpression> )* ;

RelationalExpression = <AdditiveExpression> ( ( "<" | "<=" | ">" | ">=" ) <AdditiveExpression> )* ;

AdditiveExpression = <MultiplicativeExpression> ( ( "+" | "-" ) <MultiplicativeExpression> )* ;

MultiplicativeExpression = <UnaryExpression> ( ( "*" | "/" ) <UnaryExpression> )* ;

UnaryExpression = ( ( "-" | "!" ) <UnaryExpression> )* | <PrimaryExpression> ;

PrimaryExpression = "(" <Expression> ")" |
					<AnonymousFunction> |
					<ConstructorCall> |
					<FunctionCall> |
					<List> |
					<Array> |
					<Dictionary> |
					<MemberAccess> |
					<PostfixOperation> |
					<Identifier> |
					<Value> ;

Assignment = <AssignmentTarget> <AssignmentOperation> <Expression> ;

AssignmentTarget = <Identifier> | <MemberAccess> ;

AssignmentOperation = "=" | "+=" | "-=" | "*=" | "/=" ;

AnonymousFunction = "func" "(" <FunctionParameters> ")" <StatementBlock> ;

FunctionCall = <Expression> "(" ( <Expression> ("," <Expression>)* )? ")" ;

ConstructorCall = "new" <TypeName> "(" ( <Expression> ("," <Expression>)* )? ")" ;

TypeName = <Identifier> ( "." <Identifier> )* ( "<" <TypeName> ( "," <TypeName> )* ">" )? ;

List = "[" <Expression>? ( "," <Expression> )* "]" ;

Array = "[|" <Expression>? ( "," <Expression> )* "|]" ;

Dictionary = "{" <DictionaryPair>? ( "," <DictionaryPair> )* "}" ;

DictionaryPair = ( <Identifier> | <Expression> ) ":" <Expression> ;

MemberAccess = <Expression> "." <Identifier> ( "(" ( <Expression> ( "," <Expression> )* )? ")" )? ;

PostfixOperation = <Expression> ( "++" | "--" ) ;

ElementAccess = <Expression> "[" <Expression> "]" ;

Identifier = [A-Za-z_][A-Za-z0-9_]* ;

Value = <Null> | <Boolean> | <String> | <Char> | <Integer> | <Float> ;

Null = "null" ;

Boolean = "true" | "false" ;

String = "\"" <chars>* "\"" ;

Integer = [1-9]* ;

Float = [1-9]*[.][1-9]* ;

```