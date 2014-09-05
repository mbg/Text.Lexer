// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Lexer Library
// :: Copyright 2011 mbg
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Created: 6/14/2011 11:20:03 PM
// ::      by: AD\mbg
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

#region References
using System;
using System.Text;
#endregion

namespace Text.Lexer
{
    /// <summary>
    /// Represents a Lexer context.
    /// </summary>
    internal sealed class LexerContext<T> where T : struct, IConvertible
    {
        #region Instance Members
        /// <summary>
        /// The current stream of tokens.
        /// </summary>
        private TokenStream<T> tokens;
        private Token<T> lastDiscoveredToken = null;
        private String lastMatch = null;
        private Boolean ruleMatched = false;
        private Boolean foundToken = false;
        private Boolean endOfStream = false;
        private Int32 line = 1;
        private Int32 character = 0;
        private StringBuilder sb;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the TokenStream of the context.
        /// </summary>
        public TokenStream<T> Tokens
        {
            get
            {
                return this.tokens;
            }
        }
        /// <summary>
        /// Gets or sets the current input buffer.
        /// </summary>
        public StringBuilder InputBuffer
        {
            get
            {
                return this.sb;
            }
            set
            {
                this.sb = value;
            }
        }
        /// <summary>
        /// Gets or sets the last discovered token for the current input buffer or null
        /// if no token has been discovered for the current input buffer.
        /// </summary>
        public Token<T> LastDiscoveredToken
        {
            get
            {
                return this.lastDiscoveredToken;
            }
            set
            {
                this.lastDiscoveredToken = value;
            }
        }
        /// <summary>
        /// Gets or sets the string literal which generated the last discovered token.
        /// </summary>
        public String LastMatch
        {
            get
            {
                return this.lastMatch;
            }
            set
            {
                this.lastMatch = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicting whether a rule applied to the input
        /// buffer or not.
        /// </summary>
        public Boolean RuleMatched
        {
            get
            {
                return this.ruleMatched;
            }
            set
            {
                this.ruleMatched = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether a rule applied to the input
        /// buffer and the rule's function returned a token.
        /// </summary>
        public Boolean FoundToken
        {
            get
            {
                return this.foundToken;
            }
            set
            {
                this.foundToken = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the end of the input stream has
        /// been reached.
        /// </summary>
        public Boolean EndOfStream
        {
            get
            {
                return this.endOfStream;
            }
            set
            {
                this.endOfStream = value;
            }
        }
        /// <summary>
        /// Gets the current line number.
        /// </summary>
        public Int32 Line
        {
            get
            {
                return this.line;
            }
            set
            {
                this.line = value;
            }
        }
        /// <summary>
        /// Gets the current character number in the current line.
        /// </summary>
        public Int32 Character
        {
            get
            {
                return this.character;
            }
            set
            {
                this.character = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialises a new instance of this class.
        /// </summary>
        public LexerContext()
        {
            this.tokens = new TokenStream<T>();
            this.sb = new StringBuilder();
        }
        #endregion
    }
}

// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: End of File
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::