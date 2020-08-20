using Business.Interfaces;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Tests.Business")]
namespace Business.Services
{
    internal class EmphasizeService : IEmphasizeService
    {
        private readonly HashSet<char> _separators = new HashSet<char>() { ' ', '?', '.', '!', ',' };

        public string Emphasize(string text, HashSet<string> keywords)
        {
            if (keywords.IsNullOrEmpty() || string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var wordBuffer = new StringBuilder(); // buffer which captures words only
            var modifiedText = new StringBuilder(); // builder for whole text (including punctuation)

            foreach (var character in text)
            {
                // some kind of separator means end of the word
                // see if it needs highlighting and append modified version together 
                // with breaking point (separator)
                if (_separators.Contains(character))
                {
                    modifiedText.Append(
                        keywords.Contains(wordBuffer.ToString().ToLower())
                            ? $"<em>{wordBuffer}</em>"
                            : $"{wordBuffer}"
                    );

                    modifiedText.Append(character);
                    wordBuffer.Clear();
                }
                // if our word is not complete yet
                // move forward
                else
                {
                    wordBuffer.Append(character);
                }
            }

            return modifiedText.ToString();
        }

        public char[] GetSeparators()
        {
            return _separators.ToArray();
        }
    }
}
