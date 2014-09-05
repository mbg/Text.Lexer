// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Lexer Library
// :: Copyright 2011 mbg
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Created: 6/15/2011 5:28:15 AM
// ::      by: AD\mbg
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

#region References
using System;
#endregion

namespace Text.Lexer
{
    /// <summary>
    /// Represents a base error type for exceptions thrown in the Lexer
    /// library.
    /// </summary>
    public class LexerException : Exception
    {
        #region Instance Members
        #endregion

        #region Properties
        #endregion

        #region Constructor
        /// <summary>
        /// Initialises a new instance of this class.
        /// </summary>
        public LexerException()
            : this("The Lexer encountered an error.")
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public LexerException(String message)
            : base(message)
        {
        }
        #endregion
    }
}

// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: End of File
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::