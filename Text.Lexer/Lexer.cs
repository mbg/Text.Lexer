// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Lexer Library
// :: Copyright 2011 mbg
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: Created: 6/14/2011 9:55:50 PM
// ::      by: AD\mbg
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

#region References
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
#endregion

namespace Text.Lexer
{
    public delegate void LexerErrorDelegate(Int32 line, Int32 character, String buffer);

    /// <summary>
    /// Represents a simple, generic Lexer which uses regex-based rules to
    /// analyse input and structure it into tokens.
    /// </summary>
    public class Lexer<T> where T : struct, IConvertible
    {
        #region Instance Members
        /// <summary>
        /// The encoding used by this Lexer.
        /// </summary>
        private Encoding encoding;
        /// <summary>
        /// This list stores all rules for the Lexer.
        /// </summary>
        private List<LexerRule<T>> rules;
        /// <summary>
        /// The error detection method used by this Lexer.
        /// </summary>
        private LexerErrorMode errorMode = LexerErrorMode.Simple;
        #endregion

        #region Constants
        /// <summary>
        /// A generic regular expression for whitespace characters (includes
        /// tabs, newlines, etc.).
        /// </summary>
        public const String WHITESPACE = "[ \t\r\n]";
        /// <summary>
        /// A generic regular expression for a sequence (at least one) of
        /// whitespace characters.
        /// </summary>
        public const String WHITESPACES = WHITESPACE + "+";
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the error detection method used by this Lexer.
        /// </summary>
        public LexerErrorMode ErrorMode
        {
            get
            {
                return this.errorMode;
            }
            set
            {
                this.errorMode = value;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// 
        /// </summary>
        public event LexerErrorDelegate Error;
        #endregion

        #region Constructor
        /// <summary>
        /// Initialises a new instance of this class.
        /// </summary>
        public Lexer()
            : this(Encoding.ASCII)
        {
        }
        /// <summary>
        /// Initialises a new instance of this class.
        /// </summary>
        /// <param name="encoding">The encoding that should be used by this Lexer.</param>
        public Lexer(Encoding encoding)
        {
            this.encoding = encoding;
            this.rules = new List<LexerRule<T>>();
        }
        #endregion

        #region AddRule
        /// <summary>
        /// Adds a rule to the Lexer.
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="function"></param>
        public void AddRule(String regex, LexerRuleDelegate<T> function)
        {
            this.AddRule(new LexerRule<T>(regex, function));
        }
        /// <summary>
        /// Adds a rule to the Lexer.
        /// </summary>
        /// <param name="rule">The rule to add.</param>
        public void AddRule(LexerRule<T> rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");

            this.rules.Add(rule);
        }
        #endregion

        #region TokenizeString
        /// <summary>
        /// Translates a System.String into a stream of tokens.
        /// </summary>
        /// <param name="input">The string to analyse.</param>
        /// <returns>Returns a stream of tokens which are representive of the input string.</returns>
        public TokenStream<T> TokenizeString(String input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            Byte[] stringBytes = this.encoding.GetBytes(input);

            using (MemoryStream stream = new MemoryStream(stringBytes))
            {
                return this.Tokenize(stream);
            }
        }
        #endregion

        #region Tokenize
        /// <summary>
        /// Analyses the file with the specified name and returns a stream of
        /// tokens which represent the file.
        /// </summary>
        /// <param name="filename"></param>
        public TokenStream<T> Tokenize(String filename)
        {
            if (String.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");
            if (!File.Exists(filename))
                throw new FileNotFoundException(null, filename);

            return this.Tokenize(File.Open(
                filename,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read));
        }
        /// <summary>
        /// Analyses the input stream and returns a stream of tokens which
        /// represent the character data in the input stream.
        /// </summary>
        /// <param name="stream"></param>
        public TokenStream<T> Tokenize(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new IOException("Cannot read from the input stream.");

            // :: Initialise the context as well as some other helper variables.
            LexerContext<T> context = new LexerContext<T>();
            StreamReader reader = new StreamReader(stream);
            String literal = null;
            Int32 read = -1;
            Char[] buffer = new Char[1];

            // :: Read until the end of the stream is reached.
            while (!reader.EndOfStream)
            {
                read = reader.Read(buffer, 0, buffer.Length);

                if (read > 0)
                {
                    // :: Increase the character and line numbers.
                    context.Character++;

                    if (buffer[0] == '\n')
                    {
                        context.Character = 0;
                        context.Line++;
                    }

                    // :: Append the character to the StringBuilder.
                    context.InputBuffer.Append(buffer[0]);

                    literal = context.InputBuffer.ToString();
                    context.FoundToken = false;

                    this.Analyse(context, literal);
                }
            }

            context.EndOfStream = true;
            this.Analyse(context, context.InputBuffer.ToString());

            // :: Verify that the StringBuilder is empty.
            if (context.InputBuffer.Length != 0)
                throw new LexerException("Input buffer is not empty, but the end of the stream has been reached.");

            return context.Tokens;
        }
        #endregion

        #region Analyse
        /// <summary>
        /// Analyses a literal and modifies the current context accordingly.
        /// </summary>
        /// <param name="context">The current context of the Lexer.</param>
        /// <param name="literal">The literal to analyse.</param>
        private void Analyse(LexerContext<T> context, String literal)
        {
            if (String.IsNullOrEmpty(literal))
                return;

            context.RuleMatched = false;

            // :: Attempt to find a rule which applies to the current literal.
            foreach (LexerRule<T> rule in this.rules)
            {
                if (rule.IsMatch(literal))
                {
                    context.RuleMatched = true;

                    // :: A rule was found, run it.
                    context.LastDiscoveredToken = rule.Run(literal);

                    // :: Buffer the result of the rule if there is one,
                    // :: otherwise (the Run method returned null) discard
                    // :: of it and remove the current literal from the
                    // :: input buffer. Do not modify the input buffer if
                    // :: the rule returned a token.
                    if (context.LastDiscoveredToken != null)
                    {
                        context.LastMatch = literal;
                        context.FoundToken = true;
                    }
                    else
                    {
                        context.InputBuffer = context.InputBuffer.Remove(
                            0, literal.Length);
                    }

                    // :: We only need to find one rule which matches the input.
                    break;
                }
            }

            // :: If we didn't find a rule which matched the input token, but a
            // :: rule matched the input buffer previously, use the token that
            // :: was generated previously.
            if ((!context.FoundToken || context.EndOfStream) && context.LastDiscoveredToken != null)
            {
                context.Tokens.Write(context.LastDiscoveredToken);

                // :: Only remove the part of the input buffer which matched
                // :: the rule. Reset the context afterwards.
                context.InputBuffer = context.InputBuffer.Remove(
                    0, context.LastMatch.Length);

                context.LastDiscoveredToken = null;
                context.LastMatch = null;

                // :: If the input buffer still contains text, then it might
                // :: be worth calling this function recursively in case it
                // :: contains a single character which matches a rule.
                if(context.InputBuffer.Length > 0)
                    this.Analyse(context, context.InputBuffer.ToString());
            }
            else if (!context.RuleMatched && context.LastDiscoveredToken == null 
                && this.errorMode == LexerErrorMode.Simple)
            {
                // :: Call the error handler if one is set.
                if (this.Error != null)
                    this.Error(context.Line, context.Character, literal);

                // :: Clear the input buffer and try to proceed.
                context.InputBuffer.Clear();
            }
        }
        #endregion
    }
}

// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// :: End of File
// :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::