[![The MIT License](https://img.shields.io/badge/license-MIT-orange.svg?style=flat-square)](http://opensource.org/licenses/MIT)
![Build Status](https://smack0007.visualstudio.com/_apis/public/build/definitions/3c2bd649-f280-4370-b4aa-e4a0a0b13fb8/6/badge)

# Snowflake

Snowflake is a scripting language implemented in C#. The following is an example Snowflake script:

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

## Interop

Snowflake scripts by default do not have access to any .NET BCL classes or methods besides those types which are baked into the language. Access can
be given to scripts if necessary by setting global variables inside the ScriptEngine:

```csharp
engine.SetGlobalFunction<object>("print", (x) => Console.WriteLine(x));
```

In order to allow objects to be available in your script, you'll need to register the objects with the ScriptEngine:

```csharp
engine.RegisterType("MyApp.Person", typeof(Person));
```

Once the ScriptEngine has access to the type, your scripts can create objects like so:

```
var person = new MyApp.Person();
person.FirstName = "Bob";
person.LastName = "Freeman";
person.Age = 42;
```

Static objects can also be accessed this way:

```csharp
engine.RegisterType("System.Console", typeof(Console));
```

In your script:

```
System.Console.WriteLine("Hello World!");
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