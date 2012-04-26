using System;

namespace Snowsoft.SnowflakeScript
{
	public enum ScriptLexemeType
	{
		/// <summary>
		/// Used internally to indicate we're currently looking for a lexeme.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// 'c'.
		/// </summary>
		Char,

		/// <summary>
		/// "String".
		/// </summary>
		String,
				
		/// <summary>
		/// An identifier lexeme.
		/// </summary>
		Identifier,

		/// <summary>
		/// func
		/// </summary>
		Func,

		/// <summary>
		/// return
		/// </summary>
		Return,

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

		/// <summary>
		/// list
		/// </summary>
		List,

		/// <summary>
		/// Unidentified numeric value. This should not be output.
		/// </summary>
		Numeric,
		
		/// <summary>
		/// Integer value, i.e. "123".
		/// </summary>
		Integer,
		
		/// <summary>
		/// Float value, i.e. "123.456".
		/// </summary>
		Float,

		/// <summary>
		/// ;
		/// </summary>
		EndStatement,
		
		/// <summary>
		/// $
		/// </summary>
		Variable,
		
		/// <summary>
		/// .
		/// </summary>
		Period,
		
		/// <summary>
		/// ,
		/// </summary>
		Comma,
		
		/// <summary>
		/// (
		/// </summary>
		OpenParen,
		
		/// <summary>
		/// )
		/// </summary>
		CloseParen,
		
		/// <summary>
		/// [
		/// </summary>
		OpenBracket,
		
		/// <summary>
		/// ]
		/// </summary>
		CloseBracket,
		
		/// <summary>
		/// {
		/// </summary>
		OpenBrace,
		
		/// <summary>
		/// }
		/// </summary>
		CloseBrace,

		/// <summary>
		/// =
		/// </summary>
		Gets,
		
		/// <summary>
		/// !
		/// </summary>
		Not,
		
		/// <summary>
		/// ==
		/// </summary>
		EqualTo,
		
		/// <summary>
		/// !=
		/// </summary>
		NotEqualTo,

		/// <summary>
		/// +
		/// </summary>
		Plus,
		
		/// <summary>
		/// +=
		/// </summary>
		PlusGets,
		
		/// <summary>
		/// -
		/// </summary>
		Minus,
		
		/// <summary>
		/// -=
		/// </summary>
		MinusGets,
		
		/// <summary>
		/// *
		/// </summary>
		Multiply,
		
		/// <summary>
		/// *=
		/// </summary>
		MultiplyGets,
		
		/// <summary>
		/// /
		/// </summary>
		Divide,
		
		/// <summary>
		/// /=
		/// </summary>
		DivideGets,

		/// <summary>
		/// &&
		/// </summary>
		LogicalAnd,
		
		/// <summary>
		/// ||
		/// </summary>
		LogicalOr,

		/// <summary>
		/// =>
		/// </summary>
		MapsTo,

		/// <summary>
		/// Used to indicate the end of a list of Lexemes.
		/// </summary>
		EOF
	};
}
