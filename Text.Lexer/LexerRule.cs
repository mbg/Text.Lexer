// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Lexer Library
// :: Copyright 2011 mbg
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Created: 6/14/2011 10:04:39 PM
// ::      by: AD\mbg
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

#region References
using System;
using System.Text.RegularExpressions;
#endregion

namespace Text.Lexer
{
    public delegate Token<T> LexerRuleDelegate<T>(String literal) where T : struct, IConvertible;

    /// <summary>
    /// Represents a rule for the Lexer class.
    /// </summary>
    public sealed class LexerRule<T> where T : struct, IConvertible
    {
        #region Instance Members
        /// <summary>
        /// The regular expression which represents this rule.
        /// </summary>
        private String regex;
        /// <summary>
        /// The delegate which should be executed if this rule applies to
        /// a string literal.
        /// </summary>
        private LexerRuleDelegate<T> function;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        /// <summary>
        /// Initialises a new instance of this class.
        /// </summary>
        public LexerRule(String regex, LexerRuleDelegate<T> function)
        {
            this.regex = String.Format("^{0}$", regex);
            this.function = function;
        }
        #endregion

        #region IsMatch
        /// <summary>
        /// Gets a value indicating whether the specified input string is
        /// an exact match of the rule.
        /// </summary>
        /// <param name="literal"></param>
        /// <returns></returns>
        public Boolean IsMatch(String literal)
        {
            return Regex.IsMatch(literal, this.regex);
        }
        #endregion

        #region Run
        /// <summary>
        /// Runs the rule's function on the specified literal.
        /// </summary>
        /// <param name="literal"></param>
        /// <returns>Returns a token or null.</returns>
        public Token<T> Run(String literal)
        {
            return this.function(literal);
        }
        #endregion
    }
}

// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: End of File
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::