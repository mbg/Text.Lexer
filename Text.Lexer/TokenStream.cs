// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Lexer Library
// :: Copyright 2011 mbg
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Created: 6/14/2011 10:20:45 PM
// ::      by: AD\mbg
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

#region References
using System;
using System.Collections.Generic;
#endregion

namespace Text.Lexer
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenStream<T> where T : struct, IConvertible
    {
        #region Instance Members
        /// <summary>
        /// The tokens in this stream.
        /// </summary>
        private Queue<Token<T>> tokens;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the remaining number of tokens in the stream.
        /// </summary>
        public Int32 Length
        {
            get
            {
                return this.tokens.Count;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the stream contains any tokens.
        /// </summary>
        public Boolean HasTokens
        {
            get
            {
                return this.Length > 0;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialises a new instance of this class.
        /// </summary>
        public TokenStream()
        {
            this.tokens = new Queue<Token<T>>();
        }
        #endregion

        public Token<T> Read()
        {
            return this.tokens.Dequeue();
        }

        public void Write(Token<T> token)
        {
            if (token == null)
                return;

            this.tokens.Enqueue(token);
        }

        #region Peek
        /// <summary>
        /// Returns the next token in the stream without consuming it.
        /// </summary>
        /// <returns>The next token in the stream.</returns>
        public Token<T> Peek()
        {
            return this.tokens.Peek();
        }
        #endregion
    }
}

// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: End of File
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::