using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeEditor_Components.SearchTypes
{
    /// <summary>
    /// Class to encapsulate search/replace operations.
    /// </summary>
    public class Search
    {
        /// <summary>
        /// Constructs a new default <see cref="Search"/> instance.
        /// </summary>
        public Search() {
            SearchRange = new TextRange();
            SearchUp = false;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="TextRange"/> to be searched.
        /// </summary>
        public TextRange SearchRange { get; set; }

        /// <summary>
        /// Gets or sets the search direction. If true, the search is performed from the bottom up.
        /// </summary>
        public bool SearchUp { get; set; }

        #endregion Properties

        /// <summary>
        /// Search for the first match in the <see cref="IEditor"/> text using the properties of this query object.
        /// </summary>
        /// <param name="editor"><see cref="IEditor"/> control to search.</param>
        /// <returns><see cref="TextRange"/> where the result was found. 
        /// <see cref="TextRange.Start"/> will be the same as <see cref="TextRange.End"/> if no match was found.</returns>
        public virtual TextRange Find(IEditor editor) {
            // Search capability can be implemented by children
            return new TextRange();
        }

        /// <summary>
        /// Search for the first match in the <see cref="IEditor"/> text using the properties of this query object.
        /// If it is already selected, replace it with the given string.
        /// </summary>
        /// <param name="editor"><see cref="IEditor"/> control to search.</param>
        /// <param name="replaceString">String to replace any matches.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="TextRange"/> where the result was found. 
        /// <see cref="TextRange.Start"/> will be the same as <see cref="TextRange.End"/> if no match was found.</returns>
        public virtual TextRange Replace(IEditor editor, string replaceString, bool wrap) {
            TextRange searchRange = SearchRange;
            TextRange selRange = new TextRange(editor.SelectionStart, editor.SelectionEnd);
            SearchRange = selRange;
            if ((selRange.End - selRange.Start) > 0) {
                if (selRange.Equals(Find(editor))) {
                    ReplaceText(editor, replaceString);
                    if (SearchUp) {
                        editor.GoToPosition(selRange.Start);
                    }
                }
            }
            SearchRange = searchRange;
            return FindNext(editor, wrap);
        }

        /// <summary>
        /// Replace the selection in the <see cref="IEditor"/> text with the given string.
        /// </summary>
        /// <param name="editor"><see cref="IEditor"/> control to edit.</param>
        /// <param name="replaceString">String to replace the selection.</param>
        protected virtual void ReplaceText(IEditor editor, string replaceString) {; }

        /// <summary>
        /// Replace all matches in the <see cref="IEditor"/> text with the given string, using the properties of this query object to search.
        /// </summary>
        /// <param name="editor"><see cref="IEditor"/> control to search.</param>
        /// <param name="replaceString">String to replace any matches.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public virtual List<TextRange> ReplaceAll(IEditor editor, string replaceString) {
            // Replace all capability can be implemented by children
            return new List<TextRange>();
        }

        /// <summary>
        /// Search for all matches in the <see cref="IEditor"/> text using the properties of this query object.
        /// </summary>
        /// <param name="editor"><see cref="IEditor"/> control to search.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public List<TextRange> FindAll(IEditor editor) {
            List<TextRange> results = new List<TextRange>();
            TextRange searchRange = SearchRange;
            int findCount = 0;
            while (true) {
                // Keep searching until no more matches are found
                TextRange findRange = Find(editor);
                if (findRange.Start == findRange.End) {
                    break;
                }
                else {
                    results.Add(findRange);
                    findCount++;
                    SearchRange = new TextRange(findRange.End, SearchRange.End);
                }
            }
            SearchRange = searchRange;
            return results;
        }

        /// <summary>
        /// Search for the next match in the <see cref="IEditor"/> text using the properties of this query object.
        /// </summary>
        /// <param name="editor"><see cref="IEditor"/> control to search.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="TextRange"/> where the result was found. 
        /// <see cref="TextRange.Start"/> will be the same as <see cref="TextRange.End"/> if no match was found.</returns>
        public TextRange FindNext(IEditor editor, bool wrap) {
            TextRange findRange;

            int caret = editor.CurrentPosition;
            // If the caret is outside the search range, simply return the first match in the range
            if (!(caret >= SearchRange.Start && caret <= SearchRange.End)) {
                findRange = Find(editor);
            }
            else {
                // Otherwise, find the next match after the caret
                TextRange originalSearchRange = SearchRange;
                SearchRange = new TextRange(caret, originalSearchRange.End);
                findRange = Find(editor);

                // If there were no results, try wrapping back to the top if enabled
                if ((findRange.Start == findRange.End) && wrap) {
                    SearchRange = new TextRange(originalSearchRange.Start, caret);
                    findRange = Find(editor);
                }
            }
            return findRange;
        }

        /// <summary>
        /// Search for the previous match in the <see cref="IEditor"/> text using the properties of this query object.
        /// </summary>
        /// <param name="editor"><see cref="IEditor"/> control to search.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the end of the text.</param>
        /// <returns><see cref="TextRange"/> where the result was found. 
        /// <see cref="TextRange.Start"/> will be the same as <see cref="TextRange.End"/> if no match was found.</returns>
        public TextRange FindPrevious(IEditor editor, bool wrap) {
            SearchUp = true;
            TextRange findRange;

            int caret = editor.CurrentPosition;
            // If the caret is outside the search range, simply return the last match in the range
            if (!(caret >= SearchRange.Start && caret <= SearchRange.End)) {
                findRange = Find(editor);
            }
            else {
                int anchor = editor.AnchorPosition;
                // If the anchor is otuside the search range, set the anchor to the caret
                if (!(anchor >= SearchRange.Start && anchor <= SearchRange.End)) {
                    anchor = caret;
                }

                // Otherwise, find the previous match before the anchor
                TextRange originalSearchRange = SearchRange;
                SearchRange = new TextRange(originalSearchRange.Start, anchor);
                findRange = Find(editor);

                // If there were no results, try wrapping back to the end if enabled
                if ((findRange.Start == findRange.End) && wrap) {
                    SearchRange = new TextRange(anchor, originalSearchRange.End);
                    findRange = Find(editor);
                }
            }
            return findRange;
        }

        /// <summary>
        /// Determines if the given object is equal to this <see cref="Search"/>.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if the objects are equal.</returns>
        public new virtual bool Equals(object obj) {
            //Check for null and compare run-time types.
            if ((obj == null) || !GetType().Equals(obj.GetType())) {
                return false;
            }
            return SearchRange.Equals(((Search)obj).SearchRange);
            //return true;
        }
    }

    /// <summary>
    /// Class to encapsulate search/replace operations using a string query.
    /// </summary>
    public class StringSearch : Search
    {
        /// <summary>
        /// Constructs a default <see cref="StringSearch"/> instance;
        /// </summary>
        public StringSearch() : base() {
            SearchString = string.Empty;
        }

        /// <summary>
        /// Constructs a <see cref="StringSearch"/> instance with the given parameters.
        /// </summary>
        /// <param name="searchRange">The <see cref="TextRange"/> to be searched.</param>
        /// <param name="searchString">The string for which to search.</param>
        /// <param name="matchCase">If true, a match will only occur with text that matches the case of the search string.</param>
        /// <param name="wholeWord">If true, a match will only occur if the characters before and after are not word characters.</param>
        public StringSearch(TextRange searchRange, string searchString, bool matchCase, bool wholeWord) {
            SearchRange = searchRange;
            SearchString = searchString;

            MatchCase = matchCase;
            WholeWord = wholeWord;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the search string used when attempting to find or replace.
        /// </summary>
        public string SearchString { get; set; }


        /// <summary>
        /// Gets or sets the 'MatchCase' flag for case-sensitive searching
        /// </summary>
        public bool MatchCase { get; set; }

        /// <summary>
        /// Gets or sets the 'WholeWord' flag for exact searching
        /// </summary>
        public bool WholeWord { get; set; }

        #endregion Properties

        /// <summary>
        /// Search for the first match in the <see cref="IEditor"/> text using the search string of this query object.
        /// </summary>
        /// <param name="editor"><see cref="IEditor"/> control to search.</param>
        /// <returns><see cref="TextRange"/> where the result was found. 
        /// <see cref="TextRange.Start"/> will be the same as <see cref="TextRange.End"/> if no match was found.</returns>
        public override TextRange Find(IEditor editor) {
            if (string.IsNullOrEmpty(SearchString)) {
                return new TextRange();
            }
            if (SearchUp) {
                editor.SearchRange = new TextRange(SearchRange.End, SearchRange.Start);
            }
            else {
                editor.SearchRange = SearchRange;
            }
            return editor.Search(SearchString, MatchCase, WholeWord);
        }

        /// <summary>
        /// Replace the selection in the <see cref="IEditor"/> text with the given string.
        /// </summary>
        /// <param name="editor"><see cref="IEditor"/> control to edit.</param>
        /// <param name="replaceString">String to replace the selection.</param>
        protected override void ReplaceText(IEditor editor, string replaceString) {
            editor.ReplaceSelection(replaceString);
        }

        /// <summary>
        /// Replace all matches in the <see cref="IEditor"/> text with the given string, using the properties of this query object to search.
        /// </summary>
        /// <param name="editor"><see cref="IEditor"/> control to search.</param>
        /// <param name="replaceString">String to replace any matches.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public override List<TextRange> ReplaceAll(IEditor editor, string replaceString) {
            List<TextRange> results = base.ReplaceAll(editor, replaceString);
            int findCount = 0;

            try { editor.BeginUndoAction(); }
            catch (NotImplementedException) { }

            int diff = replaceString.Length - SearchString.Length;
            while (true) {
                TextRange findRange = Find(editor);
                if (findRange.Start == findRange.End) {
                    break;
                }
                else {
                    editor.SelectionStart = findRange.Start;
                    editor.SelectionEnd = findRange.End;
                    editor.ReplaceSelection(replaceString);
                    findRange = new TextRange(findRange.Start, findRange.Start + replaceString.Length);
                    SearchRange = new TextRange(findRange.End, SearchRange.End + diff);

                    results.Add(findRange);
                    findCount++;
                }
            }
            try { editor.EndUndoAction(); }
            catch (NotImplementedException) { }


            return results;
        }

        /// <summary>
        /// Returns the string representation of the <see cref="StringSearch"/> object.
        /// </summary>
        /// <returns>The search string.</returns>
        public override string ToString() {
            return SearchString;
        }

        /// <summary>
        /// Determines if the given object is equal to this <see cref="StringSearch"/>.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if the objects are equal.</returns>
        public override bool Equals(object obj) {
            if (!base.Equals(obj)) {
                return false;
            }
            StringSearch q = (StringSearch)obj;
            return SearchString.Equals(q.SearchString) && MatchCase.Equals(q.MatchCase) && WholeWord.Equals(q.WholeWord);
        }
    }

    /// <summary>
    /// Class to encapsualte search/replace operations using a regular expression query.
    /// </summary>
    public class RegexSearch : Search
    {
        /// <summary>
        /// Constructs a new default <see cref="RegexSearch"/> instance.
        /// </summary>
        public RegexSearch() : base() { }

        /// <summary>
        /// Constructs a new <see cref="RegexSearch"/> instance with the given parameters.
        /// </summary>
        /// <param name="searchRange">The <see cref="TextRange"/> to be searched.</param>
        /// <param name="pattern">The pattern for which to search.</param>
        /// <param name="options"><see cref="RegexOptions"/> enumeration that specifies pattern matching options.</param>
        public RegexSearch(TextRange searchRange, string pattern, RegexOptions options) {
            SearchRange = searchRange;
            SearchExpression = new Regex(pattern, options);
        }

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="Regex"/> used when attempting to find or replace.
        /// </summary>
        public Regex SearchExpression { get; set; }

        #endregion Properties

        /// <summary>
        /// Search for the first match in the <see cref="IEditor"/> text using the Regex of this query object.
        /// </summary>
        /// <param name="editor"><see cref="IEditor"/> control to search.</param>
        /// <returns><see cref="TextRange"/> where the result was found. 
        /// <see cref="TextRange.Start"/> will be the same as <see cref="TextRange.End"/> if no match was found.</returns>
        public override TextRange Find(IEditor editor) {
            TextRange findRange = base.Find(editor);
            string text = editor.GetTextRange(SearchRange);
            Match m = SearchExpression.Match(text);

            if (!m.Success) {
                return findRange;
            }
            else {
                findRange = GetMatchRange(SearchRange, text, m);
                if (SearchUp) {
                    while (m.Success) {
                        findRange = GetMatchRange(SearchRange, text, m);
                        m = m.NextMatch();
                    }
                }
                return findRange;
            }
        }

        /// <summary>
        /// Replace the selection in the <see cref="IEditor"/> text with the given string.
        /// </summary>
        /// <param name="editor"><see cref="IEditor"/> control to edit.</param>
        /// <param name="replaceString">String to replace the selection. Can be a regular expression pattern.</param>
        protected override void ReplaceText(IEditor editor, string replaceString) {
            string searchRangeText = editor.GetTextRange(SearchRange);
            editor.ReplaceSelection(SearchExpression.Replace(searchRangeText, replaceString));
        }

        /// <summary>
        /// Replace all matches in the <see cref="IEditor"/> text with the given string, using the properties of this query object to search.
        /// </summary>
        /// <param name="editor"><see cref="IEditor"/> control to search.</param>
        /// <param name="replaceString">String to replace any matches. Can be a regular expression pattern.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public override List<TextRange> ReplaceAll(IEditor editor, string replaceString) {
            List<TextRange> results = base.ReplaceAll(editor, replaceString);
            var replaceOffset = 0;
            var replaceCount = 0;

            try { editor.BeginUndoAction(); }
            catch (NotImplementedException) { }

            string text = editor.GetTextRange(SearchRange);
            SearchExpression.Replace(text,
                new MatchEvaluator(
                    delegate (Match m) {
                        string replacement = m.Result(replaceString);
                        int start = SearchRange.Start + m.Index + replaceOffset;
                        int end = start + m.Length;

                        replaceCount++;
                        editor.SelectionStart = start;
                        editor.SelectionEnd = end;
                        editor.ReplaceSelection(replacement);
                        results.Add(new TextRange(start, end));

                        // The replacement has shifted the original match offsets
                        replaceOffset += replacement.Length - m.Value.Length;

                        return replacement;
                    }
                    )
                    );
            try { editor.EndUndoAction(); }
            catch (NotImplementedException) { }

            return results;
        }

        /// <summary>
        /// Returns the string representation of the <see cref="RegexSearch"/> object.
        /// </summary>
        /// <returns>The string used to construct the <see cref="Regex"/>.</returns>
        public override string ToString() {
            return SearchExpression.ToString();
        }

        /// <summary>
        /// Determines if the given object is equal to this <see cref="RegexSearch"/>.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if the objects are equal.</returns>
        public override bool Equals(object obj) {
            if (!base.Equals(obj)) {
                return false;
            }
            return SearchExpression.ToString().Equals(((RegexSearch)obj).SearchExpression.ToString());
        }

        // Returns the CharacterRange that represents the current match found in the search range in the given text
        private static TextRange GetMatchRange(TextRange searchRange, string text, Match m) {
            int start = searchRange.Start + text.Substring(0, m.Index).Length;
            int end = text.Substring(m.Index, m.Length).Length;
            TextRange range = new TextRange(start, start + end);
            return range;
        }
    }
}
