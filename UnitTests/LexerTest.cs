using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Text.Lexer;

namespace UnitTests
{
    [TestClass]
    public class LexerTest
    {
        enum IniTokens
        {
            SquareBracketOpen,
            SquareBracketClosed,
            Equals,
            Identifier
        }

        private void LexerError(Int32 line, Int32 character, String buffer)
        {
            Console.WriteLine("Unexpected '{0}' in line {1}, character {2}", buffer, line, character);

            Assert.Fail();
        }

        [TestMethod]
        public void IniTest()
        {
            // :: Create a set of lexical rules.
            Lexer<IniTokens> lexer = new Lexer<IniTokens>();
            lexer.ErrorMode = LexerErrorMode.Simple;
            lexer.Error += new LexerErrorDelegate(LexerError);
            lexer.AddRule(Lexer<IniTokens>.WHITESPACE, l => null);
            lexer.AddRule(@"\[", l => new Token<IniTokens>(IniTokens.SquareBracketOpen));
            lexer.AddRule(@"\]", l => new Token<IniTokens>(IniTokens.SquareBracketClosed));
            lexer.AddRule(@"=", l => new Token<IniTokens>(IniTokens.Equals));
            lexer.AddRule(@"[a-zA-Z0-9]+", l => new Token<IniTokens>(IniTokens.Identifier, l));

            // :: Convert the input document into a stream of tokens based on the rules we
            // :: defined above.
            TokenStream<IniTokens> tokens = lexer.TokenizeString(@"[Section]
            Key = Value

            [AnotherSection]
            AnotherKey = AnotherValue
             ");

            
        }
    }
}
