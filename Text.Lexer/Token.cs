// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Lexer Library
// :: Copyright 2011 mbg
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Created: 6/14/2011 9:50:24 PM
// ::      by: AD\mbg
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

#region References
using System;
#endregion

namespace Text.Lexer
{
    /// <summary>
    /// Represents a single token.
    /// </summary>
    public class Token<T> where T : struct, IConvertible
    {
        #region Instance Members
        /// <summary>
        /// The type of the token.
        /// </summary>
        private T type;
        /// <summary>
        /// The literal string data associated with this token.
        /// </summary>
        private String data;

        private Int32 line;
        private Int32 character;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the type of this token.
        /// </summary>
        public T Type
        {
            get
            {
                return this.type;
            }
        }
        /// <summary>
        /// Gets the literal string data associated with this token.
        /// </summary>
        public String Data
        {
            get
            {
                return this.data;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialises a new token with the specified type.
        /// </summary>
        /// <param name="type">The type of the token.</param>
        public Token(T type)
            : this(type, null)
        {
        }
        /// <summary>
        /// Initialises a new token with the specified type and literal
        /// string data.
        /// </summary>
        /// <param name="type">The type of the token.</param>
        /// <param name="data">The literal string data associated with the token.</param>
        public Token(T type, String data)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumeration.");

            this.type = type;
            this.data = data;
        }
        #endregion

        #region ToString
        /// <summary>
        /// Returns the type of the token as System.String if no data is
        /// associated with this token -or- the type of the token with
        /// its associated data in square brackets as System.String.
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            if (this.data == null)
                return this.type.ToString();
            else
                return String.Format("{0} [{1}]", this.type, this.data);
        }
        #endregion
    }
}

// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: End of File
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::