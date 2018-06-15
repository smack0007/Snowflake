namespace Snowflake.Lexing
{
    public enum LexemeType
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
        /// using
        /// </summary>
        Using,

		/// <summary>
		/// func
		/// </summary>
		Func,

		/// <summary>
		/// return
		/// </summary>
		Return,

        /// <summary>
		/// yield
		/// </summary>
		Yield,

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
		/// in
		/// </summary>
		In,
				
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
		/// Unidentified numeric value. This should not be output.
		/// </summary>
		Numeric,
		
		/// <summary>
		/// Integer value, i.e. "123".
		/// </summary>
		Integer,
		
		/// <summary>
		/// Float value, i.e. "123.456f".
		/// </summary>
		Float,

		/// <summary>
		/// Float value, i.e. "123.456".
		/// </summary>
		Double,

		/// <summary>
		/// ;
		/// </summary>
		EndStatement,
		
        /// <summary>
        /// const
        /// </summary>
        Const,

		/// <summary>
		/// var
		/// </summary>
		Var,

        /// <summary>
        /// new
        /// </summary>
        New,
		
		/// <summary>
		/// .
		/// </summary>
		Dot,
		
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
		/// :
		/// </summary>
		Colon,

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
		/// >
		/// </summary>
		GreaterThan,

		/// <summary>
		/// >=
		/// </summary>
		GreaterThanOrEqualTo,

		/// <summary>
		/// &lt;
		/// </summary>
		LessThan,

		/// <summary>
		/// &lt;=
		/// </summary>
		LessThanOrEqualTo,

		/// <summary>
		/// +
		/// </summary>
		Plus,

		/// <summary>
		/// ++
		/// </summary>
		Increment,
		
		/// <summary>
		/// +=
		/// </summary>
		PlusGets,
		
		/// <summary>
		/// -
		/// </summary>
		Minus,

		/// <summary>
		/// --
		/// </summary>
		Decrement,
		
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
		ConditionalAnd,
		
		/// <summary>
		/// ||
		/// </summary>
		ConditionalOr,
				
		/// <summary>
		/// Used to indicate the end of a list of Lexemes.
		/// </summary>
		EOF
	};
}
