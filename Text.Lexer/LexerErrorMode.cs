// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Lexer Library
// :: Copyright 2011 mbg
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Created: 6/22/2011 1:34:47 PM
// ::      by: AD\mbg
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

#region References
using System;
#endregion

namespace Text.Lexer
{
    /// <summary>
    /// Enumerates error detection modes for the Lexer.
    /// </summary>
    public enum LexerErrorMode
    {
        /// <summary>
        /// The Lexer will not attempt to detect errors.
        /// </summary>
        None,
        /// <summary>
        /// The Lexer will assume that every lexical rule either immediately
        /// applies to the input buffer or not all. Therefore an error occurs if
        /// no rule applies to the first character in the input buffer.
        /// 
        /// Rules like this will work:
        /// "\["
        /// "[a-zA-Z0-9]+"
        /// 
        /// Rules like this won't work because they rely on more complex 
        /// patterns. It is recommended to split complex patterns into simple
        /// patterns if this error mode is used:
        /// "[a-zA-Z][a-zA-Z0-9]+"
        /// "\"[a-zA-Z0-9 \t]*\""
        /// "class"
        /// </summary>
        Simple,
        /// <summary>
        /// The Lexer will 
        /// </summary>
        Backtracking
    }
}

// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: End of File
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::