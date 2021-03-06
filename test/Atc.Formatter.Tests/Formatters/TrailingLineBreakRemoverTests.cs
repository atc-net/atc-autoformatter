﻿using System.Linq;
using Atc.AutoFormatter.Formatters;
using Atc.Formatter.Tests.TestInfrastructure;
using NSubstitute;
using Xunit;
using static Atc.Formatter.Tests.Formatters.TextViewFactory;

namespace Atc.Formatter.Tests.Formatters
{
    public class TrailingLineBreakRemoverTests
    {
        [Theory, AutoNSubstituteData]
        public void Execute_Calls_CreateEdit_On_TextBuffer(
            TrailingLineBreakRemover sut,
            string filePath,
            string[] lines)
        {
            var emptyLines = Enumerable.Repeat(string.Empty, 2);
            var textView = CreateTextView(lines.Union(emptyLines));

            sut.Execute(filePath, textView);

            textView.TextSnapshot.TextBuffer
                .Received(1)
                .CreateEdit();
        }

        [Theory, AutoNSubstituteData]
        public void Execute_Calls_Delete_On_TextEdit(
            TrailingLineBreakRemover sut,
            string filePath,
            string[] lines)
        {
            var emptyLines = Enumerable.Repeat(string.Empty, 2);
            var allLines = lines.Union(emptyLines);
            var allText = string.Join(LineBreak, allLines);
            var textView = CreateTextView(allLines);

            sut.Execute(filePath, textView);

            var edit = textView.TextSnapshot.TextBuffer.CreateEdit();
            edit
                .Received(1)
                .Delete(
                    allText.TrimEnd().Length,
                    emptyLines.Count() * LineBreak.Length);
        }

        [Theory, AutoNSubstituteData]
        public void Execute_Calls_Apply_On_TextEdit(
            TrailingLineBreakRemover sut,
            string filePath,
            string[] lines)
        {
            var emptyLines = Enumerable.Repeat(string.Empty, 2);
            var textView = CreateTextView(lines.Union(emptyLines));

            sut.Execute(filePath, textView);

            var edit = textView.TextSnapshot.TextBuffer.CreateEdit();
            edit
                .Received(1)
                .Apply();
        }

        [Theory, AutoNSubstituteData]
        public void Execute_Does_Not_Call_CreateEdit_If_No_Trailing_LineBreaks(
            TrailingLineBreakRemover sut,
            string filePath,
            string[] lines)
        {
            var textView = CreateTextView(lines);

            sut.Execute(filePath, textView);

            textView.TextSnapshot.TextBuffer
                .DidNotReceive()
                .CreateEdit();
        }
    }
}