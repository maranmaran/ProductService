using System.Collections.Generic;

namespace Business.Interfaces
{
    /// <summary>
    /// Service for emphasizing certain keywords in text
    /// </summary>
    public interface IEmphasizeService
    {
        /// <summary>
        /// Emphasizes given keywords in text and wraps them with em html tag
        /// </summary>
        string Emphasize(string text, HashSet<string> keywords);

        /// <summary>
        /// Returns list of relevant separators when splitting text into words for emphasizing
        /// </summary>
        /// 
        /// <improvements>
        /// Depending on business needs these separators should maybe be
        /// outsourced to config in case of change or as an extension point
        /// </improvements>
        char[] GetSeparators();
    }
}
