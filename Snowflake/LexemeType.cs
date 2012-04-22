using System;

namespace Snowsoft.SnowflakeScript
{
	public enum LexemeType
	{
		Unknown = 0,

		/// <summary>
		/// String using '.
		/// </summary>
		String,
		
		/// <summary>
		/// String using ".
		/// </summary>
		MagicString, // '' and "" strings

		/// <summary>
		/// An identifier lexeme.
		/// </summary>
		Identifier,

		/// <summary>
		/// if
		/// </summary>
		If,
		
		/// <summary>
		/// else
		/// </summary>
		Else,
		
		/// <summary>
		/// while
		/// </summary>
		While,
	
		/// <summary>
		/// for
		/// </summary>
		For,
		
		/// <summary>
		/// foreach
		/// </summary>
		ForEach,
		
		/// <summary>
		/// as
		/// </summary>
		As,
		
		/// <summary>
		/// echo
		/// </summary>
		Echo,
		
		/// <summary>
		/// null
		/// </summary>
		Null,
		
		/// <summary>
		/// true
		/// </summary>
		True,
		
		/// <summary>
		/// false
		/// </summary>
		False,
		
		/// <summary>
		/// array
		/// </summary>
		Array,


		Numeric,
		
		/// <summary>
		/// Integer value, i.e. "123".
		/// </summary>
		Integer,
		
		/// <summary>
		/// Float value, i.e. "123.456".
		/// </summary>
		Float,

		EndStatement, Variable, Period, Comma, OpenParen, CloseParen, OpenBracket, CloseBracket, OpenBrace, CloseBrace, // Symbols

		Gets, Not, EqualTo, NotEqualTo, // Symbols

		Plus, PlusGets, Minus, MinusGets, Multiply, MultiplyGets, Divide, DivideGets, // Symbols

		And, Or,

		MapsTo, // Symbols

		EOF
	};
}
