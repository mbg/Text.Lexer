Text.Lexer
==========

A simple .NET library for lexical analysis, written in 2011. This may or may not work correctly and is most likely very inefficient!

I have updated the Visual Studio solution to VS 2012 and the .NET Framework version to 4.5 since then, but it should work correctly with previous .NET versions down to at least 2.0.

## Usage

Firstly, we need a type which enumerates different types of lexical tokens:

```C#
enum IniTokens
{
    SquareBracketOpen,
    SquareBracketClosed,
    Equals,
    Identifier
}
```

Next we initialise the lexer:

```C#
Lexer<IniTokens> lexer = new Lexer<IniTokens>();
lexer.Error += new LexerErrorDelegate(LexerError);
```

`LexerError` is a reference to a method which should be invoked when an error occurs and could be defined as e.g.:

```C#
private void LexerError(Int32 line, Int32 character, String buffer)
{
    Console.WriteLine("Unexpected '{0}' in line {1}, character {2}", buffer, line, character);
}
```

Once the lexer object has been created, we can add rules to it. Each rule consists of a regular expression and a function which turns the result of a successful match into a token:

```C#
lexer.AddRule(Lexer<IniTokens>.WHITESPACE, l => null);
lexer.AddRule(@"\[", l => new Token<IniTokens>(IniTokens.SquareBracketOpen));
lexer.AddRule(@"\]", l => new Token<IniTokens>(IniTokens.SquareBracketClosed));
lexer.AddRule(@"=", l => new Token<IniTokens>(IniTokens.Equals));
lexer.AddRule(@"[a-zA-Z0-9]+", l => new Token<IniTokens>(IniTokens.Identifier, l));
```

Lastly, we can use the lexer to scan e.g. a String value. The lexer will read characters from the input and add them to an internal buffer while rules match. Once no more rules match, a rule which matched the string previously will be used to convert the buffer into a token. The rules are tested in the order in which they were added to the lexer.

```C#
TokenStream<IniTokens> tokens = lexer.TokenizeString(@"[Section]
Key = Value

[AnotherSection]
AnotherKey = AnotherValue
 ");
```

`tokens` will be a list of tokens obtained from scanning the String value.

Note: despite the naming convention used here, TokenStream<T> is not actually a stream.