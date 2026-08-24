namespace AngleSharp.Core.Tests.Library
{
    using AngleSharp.Common;
    using AngleSharp.Text;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class TextParsingTests
    {
        [Test]
        public void TextSourceJustIteratesOverCharacters()
        {
            var str = "img\r\n{";
            var source = new TextSource(str);

            Assert.AreEqual(0, source.Index);

            var i = source.ReadCharacter();
            var m = source.ReadCharacter();
            var g = source.ReadCharacter();
            var carriageReturn = source.ReadCharacter();
            var newline = source.ReadCharacter();
            var openBracket = source.ReadCharacter();
            var eof = source.ReadCharacter();
            var stillEof = source.ReadCharacter();

            Assert.AreEqual('i', i);
            Assert.AreEqual('m', m);
            Assert.AreEqual('g', g);
            Assert.AreEqual('\r', carriageReturn);
            Assert.AreEqual('\n', newline);
            Assert.AreEqual('{', openBracket);
            Assert.AreEqual(Symbols.EndOfFile, eof);
            Assert.AreEqual(Symbols.EndOfFile, stillEof);
        }

        [Test]
        public void TokenizerTreatsCarriageReturnNewlineAsNewline()
        {
            var str = "img\r\n{";
            var source = new TextSource(str);
            var tokenizer = new ExampleTokenizer(source);

            var i = tokenizer.Next();
            var m = tokenizer.Next();
            var g = tokenizer.Next();
            var newline = tokenizer.Next();
            var previousG = tokenizer.Previous();
            var newlineAgain = tokenizer.Next();
            var openBracket = tokenizer.Next();
            var eof = tokenizer.Next();
            var stillEof = tokenizer.Next();

            Assert.AreEqual('i', i);
            Assert.AreEqual('m', m);
            Assert.AreEqual('g', g);
            Assert.AreEqual('\n', newline);
            Assert.AreEqual('g', previousG);
            Assert.AreEqual('\n', newlineAgain);
            Assert.AreEqual('{', openBracket);
            Assert.AreEqual(Symbols.EndOfFile, eof);
            Assert.AreEqual(Symbols.EndOfFile, stillEof);
        }

        class ExampleTokenizer(TextSource source) : BaseTokenizer(source)
        {
            public Char Next() => GetNext();

            public Char Previous() => GetPrevious();
        }
    }
}
