= Snowflake

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

```
using System;
using System.Collections.Generic;

namespace Snowflake.Generated
{
	public class CompiledScript : Script
	{
		protected override object Execute()
		{
			var stack = new Stack<ScriptStackFrame>();
			try
			{
				dynamic v1 = new ScriptFunction(new Func<dynamic, dynamic>((v2) => { 
					stack.Push(new ScriptStackFrame("buildMultiplier"));
					try {
						return new ScriptFunction(new Func<dynamic, dynamic>((v3) => { 
							stack.Push(new ScriptStackFrame("<anonymous>"));
							try {
								return (v2 * v3);
							} finally {
								stack.Pop();
							}
						 }), null);
					} finally {
						stack.Pop();
					}
				 }), null);
				dynamic v4 = Invoke(v1, 5);
				for (dynamic v5 = 0; (v5 < 10); v5 += 1) {
					Invoke(this.GetGlobalVariable("print"), ((("5 * " + v5) + " = ") + Invoke(v4, v5)));
				}
				dynamic v6 = new List<dynamic> { 1, 2, 3, 4, 5 };
				Invoke(this.GetGlobalVariable("print"), v6.Count);
				return null;
			}
			catch (Exception ex)
			{
				throw new ScriptExecutionException(ex.Message, ex, this.Id, stack);
			}
		}
	}
}
```

== Language Structure

The following is the structure of the language following the EBNF language as closely as possible:

Script = <Statement>* EOF ;

StatementBlock = "{" <Statement>* "}" ;

Statement = <VariableDeclaration> ";" |
            <FunctionDeclaration> |
			<If> |
			<While> |
			<For> |
			<ForEach> |
			<Return> |
			<Expression> ";" ;

VariableDeclaration = "var" <Identifier> { "=" <Expression> } ;

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

FunctionCall = <Expression> "(" { <Expression> ("," <Expression>)* } ")" ;

List = "[" <Expression>? ( "," <Expression> )* "]" ;

Array = "[|" <Expression>? ( "," <Expression> )* "|]" ;

Dictionary = "{" <DictionaryPair>? ( "," <DictionaryPair> )* "}" ;

DictionaryPair = ( <Identifier> | <Expression> ) ":" <Expression> ;

MemberAccess = <Expression> "." <Identifier> ;

PostfixOperation = <Expression> ( "++" | "--" ) ;

ElementAccess = <Expression> "[" <Expression> "]" ;

Identifier = [A-Za-z_][A-Za-z0-9_]* ;

Value = <Undefined> | <Null> | <Boolean> | <String> | <Char> | <Integer> | <Float> ;

Undefined = "undef" ;

Null = "null" ;

Boolean = "true" | "false" ;

String = "\"" <chars>* "\"" ;

Integer = [1-9]* ;

Float = [1-9]*[.][1-9]* ;
