#region Using Directives

using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#endregion Using Directives

namespace ScintillaNET_Components.SearchTypes
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
            SearchRange = new CharacterRange();
            SearchUp = false;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="CharacterRange"/> to be searched.
        /// </summary>
        public CharacterRange SearchRange { get; set; }

        /// <summary>
        /// Gets or sets the search direction. If true, the search is performed from the bottom up.
        /// </summary>
        public bool SearchUp { get; set; }

        #endregion Properties

        /// <summary>
        /// Search for the first match in the <see cref="Scintilla"/> text using the properties of this query object.
        /// </summary>
        /// <param name="editor"><see cref="Scintilla"/> control to search.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public virtual CharacterRange Find(Scintilla editor) {
            // Search capability can be implemented by children
            return new CharacterRange();
        }

        /// <summary>
        /// Search for the first match in the <see cref="Scintilla"/> text using the properties of this query object.
        /// If it is already selected, replace it with the given string.
        /// </summary>
        /// <param name="editor"><see cref="Scintilla"/> control to search.</param>
        /// <param name="replaceString">String to replace any matches.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public virtual CharacterRange Replace(Scintilla editor, string replaceString, bool wrap) {
            CharacterRange searchRange = SearchRange;
            CharacterRange selRange = new CharacterRange(editor.Selections[0].Start, editor.Selections[0].End);
            SearchRange = selRange;
            if ((selRange.cpMax - selRange.cpMin) > 0) {
                if (selRange.Equals(Find(editor))) {
                    ReplaceText(editor, replaceString);
                    if (SearchUp) {
                        editor.GotoPosition(selRange.cpMin);
                    }
                }
            }
            SearchRange = searchRange;
            return FindNext(editor, wrap);
        }

        /// <summary>
        /// Replace the selection in the <see cref="Scintilla"/> text with the given string.
        /// </summary>
        /// <param name="editor"><see cref="Scintilla"/> control to edit.</param>
        /// <param name="replaceString">String to replace the selection.</param>
        protected virtual void ReplaceText(Scintilla editor, string replaceString) {; }

        /// <summary>
        /// Replace all matches in the <see cref="Scintilla"/> text with the given string, using the properties of this query object to search.
        /// </summary>
        /// <param name="editor"><see cref="Scintilla"/> control to search.</param>
        /// <param name="replaceString">String to replace any matches.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public virtual List<CharacterRange> ReplaceAll(Scintilla editor, string replaceString) {
            // Replace all capability can be implemented by children
            return new List<CharacterRange>();
        }

        /// <summary>
        /// Search for all matches in the <see cref="Scintilla"/> text using the properties of this query object.
        /// </summary>
        /// <param name="editor"><see cref="Scintilla"/> control to search.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public List<CharacterRange> FindAll(Scintilla editor) {
            List<CharacterRange> results = new List<CharacterRange>();
            CharacterRange searchRange = SearchRange;
            int findCount = 0;
            while (true) {
                // Keep searching until no more matches are found
                CharacterRange findRange = Find(editor);
                if (findRange.cpMin == findRange.cpMax) {
                    break;
                }
                else {
                    results.Add(findRange);
                    findCount++;
                    SearchRange = new CharacterRange(findRange.cpMax, SearchRange.cpMax);
                }
            }
            SearchRange = searchRange;
            return results;
        }

        /// <summary>
        /// Search for the next match in the <see cref="Scintilla"/> text using the properties of this query object.
        /// </summary>
        /// <param name="editor"><see cref="Scintilla"/> control to search.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the beginning of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange FindNext(Scintilla editor, bool wrap) {
            CharacterRange findRange = new CharacterRange();

            int caret = editor.CurrentPosition;
            // If the caret is outside the search range, simply return the first match in the range
            if (!(caret >= SearchRange.cpMin && caret <= SearchRange.cpMax)) {
                findRange = Find(editor);
            }
            else {
                // Otherwise, find the next match after the caret
                CharacterRange originalSearchRange = SearchRange;
                SearchRange = new CharacterRange(caret, originalSearchRange.cpMax);
                findRange = Find(editor);

                // If there were no results, try wrapping back to the top if enabled
                if ((findRange.cpMin == findRange.cpMax) && wrap) {
                    SearchRange = new CharacterRange(originalSearchRange.cpMin, caret);
                    findRange = Find(editor);
                }
            }
            return findRange;
        }

        /// <summary>
        /// Search for the previous match in the <see cref="Scintilla"/> text using the properties of this query object.
        /// </summary>
        /// <param name="editor"><see cref="Scintilla"/> control to search.</param>
        /// <param name="wrap">Set to true to allow the search to wrap back to the end of the text.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public CharacterRange FindPrevious(Scintilla editor, bool wrap) {
            SearchUp = true;
            CharacterRange findRange = new CharacterRange();

            int caret = editor.CurrentPosition;
            // If the caret is outside the search range, simply return the last match in the range
            if (!(caret >= SearchRange.cpMin && caret <= SearchRange.cpMax)) {
                findRange = Find(editor);
            }
            else {
                int anchor = editor.AnchorPosition;
                // If the anchor is otuside the search range, set the anchor to the caret
                if (!(anchor >= SearchRange.cpMin && anchor <= SearchRange.cpMax)) {
                    anchor = caret;
                }

                // Otherwise, find the previous match before the anchor
                CharacterRange originalSearchRange = SearchRange;
                SearchRange = new CharacterRange(originalSearchRange.cpMin, anchor);
                findRange = Find(editor);

                // If there were no results, try wrapping back to the end if enabled
                if ((findRange.cpMin == findRange.cpMax) && wrap) {
                    SearchRange = new CharacterRange(anchor, originalSearchRange.cpMax);
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
            else {
                //return SearchRange.Equals(((Search)obj).SearchRange);
                return true;
            }
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
            Flags = SearchFlags.None;
        }

        /// <summary>
        /// Constructs a <see cref="StringSearch"/> instance with the given parameters.
        /// </summary>
        /// <param name="searchRange">The <see cref="CharacterRange"/> to be searched.</param>
        /// <param name="searchString">The string for which to search.</param>
        /// <param name="matchCase">If true, a match will only occur with text that matches the case of the search string.</param>
        /// <param name="wholeWord">If true, a match will only occur if the characters before and after are not word characters.</param>
        public StringSearch(CharacterRange searchRange, string searchString, bool matchCase, bool wholeWord) {
            SearchRange = searchRange;
            SearchString = searchString;

            Flags = SearchFlags.None;
            MatchCase = matchCase;
            WholeWord = wholeWord;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the search string used when attempting to find or replace.
        /// </summary>
        public string SearchString { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SearchFlags"/> enumeration that determines how strings will be matched.
        /// </summary>
        public SearchFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the 'MatchCase' flag for case-sensitive searching
        /// </summary>
        public bool MatchCase {
            get {
                return (Flags & SearchFlags.MatchCase) != 0;
            }
            set {
                Flags = value ? Flags |= SearchFlags.MatchCase : Flags &= ~SearchFlags.MatchCase;
            }
        }

        /// <summary>
        /// Gets or sets the 'WholeWord' flag for exact searching
        /// </summary>
        public bool WholeWord {
            get {
                return (Flags & SearchFlags.WholeWord) != 0;
            }
            set {
                Flags = value ? Flags |= SearchFlags.WholeWord : Flags &= ~SearchFlags.WholeWord;
            }
        }

        #endregion Properties

        /// <summary>
        /// Search for the first match in the <see cref="Scintilla"/> text using the search string of this query object.
        /// </summary>
        /// <param name="editor"><see cref="Scintilla"/> control to search.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public override CharacterRange Find(Scintilla editor) {
            CharacterRange findRange = base.Find(editor);
            if (string.IsNullOrEmpty(SearchString)) {
                return findRange;
            }
            else {
                if (SearchUp) {
                    editor.TargetStart = SearchRange.cpMax;
                    editor.TargetEnd = SearchRange.cpMin;
                }
                else {
                    editor.TargetStart = SearchRange.cpMin;
                    editor.TargetEnd = SearchRange.cpMax;
                }
                editor.SearchFlags = Flags;
                if (editor.SearchInTarget(SearchString) == -1) {
                    return findRange;
                }
                else {
                    findRange = new CharacterRange(editor.TargetStart, editor.TargetEnd);
                }
                return findRange;
            }
        }

        /// <summary>
        /// Replace the selection in the <see cref="Scintilla"/> text with the given string.
        /// </summary>
        /// <param name="editor"><see cref="Scintilla"/> control to edit.</param>
        /// <param name="replaceString">String to replace the selection.</param>
        protected override void ReplaceText(Scintilla editor, string replaceString) {
            editor.ReplaceSelection(replaceString);
        }

        /// <summary>
        /// Replace all matches in the <see cref="Scintilla"/> text with the given string, using the properties of this query object to search.
        /// </summary>
        /// <param name="editor"><see cref="Scintilla"/> control to search.</param>
        /// <param name="replaceString">String to replace any matches.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public override List<CharacterRange> ReplaceAll(Scintilla editor, string replaceString) {
            List<CharacterRange> results = base.ReplaceAll(editor, replaceString);
            int findCount = 0;

            editor.BeginUndoAction();
            int diff = replaceString.Length - SearchString.Length;
            while (true) {
                CharacterRange findRange = Find(editor);
                if (findRange.cpMin == findRange.cpMax) {
                    break;
                }
                else {
                    editor.SelectionStart = findRange.cpMin;
                    editor.SelectionEnd = findRange.cpMax;
                    editor.ReplaceSelection(replaceString);
                    findRange.cpMax = findRange.cpMin + replaceString.Length;
                    SearchRange = new CharacterRange(findRange.cpMax, SearchRange.cpMax + diff);

                    results.Add(findRange);
                    findCount++;
                }
            }

            editor.EndUndoAction();
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
            if (base.Equals(obj)) {
                StringSearch q = (StringSearch)obj;
                return SearchString.Equals(q.SearchString) && Flags.Equals(q.Flags);
            }
            else {
                return false;
            }
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
        /// <param name="searchRange">The <see cref="CharacterRange"/> to be searched.</param>
        /// <param name="pattern">The pattern for which to search.</param>
        /// <param name="options"><see cref="RegexOptions"/> enumeration that specifies pattern matching options.</param>
        public RegexSearch(CharacterRange searchRange, string pattern, RegexOptions options) {
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
        /// Search for the first match in the <see cref="Scintilla"/> text using the Regex of this query object.
        /// </summary>
        /// <param name="editor"><see cref="Scintilla"/> control to search.</param>
        /// <returns><see cref="CharacterRange"/> where the result was found. 
        /// <see cref="CharacterRange.cpMin"/> will be the same as <see cref="CharacterRange.cpMax"/> if no match was found.</returns>
        public override CharacterRange Find(Scintilla editor) {
            CharacterRange findRange = base.Find(editor);
            string text = editor.GetTextRange(SearchRange.cpMin, SearchRange.cpMax - SearchRange.cpMin + 1);
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
        /// Replace the selection in the <see cref="Scintilla"/> text with the given string.
        /// </summary>
        /// <param name="editor"><see cref="Scintilla"/> control to edit.</param>
        /// <param name="replaceString">String to replace the selection. Can be a regular expression pattern.</param>
        protected override void ReplaceText(Scintilla editor, string replaceString) {
            string searchRangeText = editor.GetTextRange(SearchRange.cpMin, SearchRange.cpMax - SearchRange.cpMin);
            editor.ReplaceSelection(SearchExpression.Replace(searchRangeText, replaceString));
        }

        /// <summary>
        /// Replace all matches in the <see cref="Scintilla"/> text with the given string, using the properties of this query object to search.
        /// </summary>
        /// <param name="editor"><see cref="Scintilla"/> control to search.</param>
        /// <param name="replaceString">String to replace any matches. Can be a regular expression pattern.</param>
        /// <returns><see cref="List{CharacterRange}"/> containing the locations of every match. Empty if none were found.</returns>
        public override List<CharacterRange> ReplaceAll(Scintilla editor, string replaceString) {
            List<CharacterRange> results = base.ReplaceAll(editor, replaceString);
            editor.BeginUndoAction();

            var replaceOffset = 0;
            var replaceCount = 0;

            string text = editor.GetTextRange(SearchRange.cpMin, SearchRange.cpMax - SearchRange.cpMin + 1);
            SearchExpression.Replace(text,
                new MatchEvaluator(
                    delegate (Match m) {
                        string replacement = m.Result(replaceString);
                        int start = SearchRange.cpMin + m.Index + replaceOffset;
                        int end = start + m.Length;

                        replaceCount++;
                        editor.SelectionStart = start;
                        editor.SelectionEnd = end;
                        editor.ReplaceSelection(replacement);
                        results.Add(new CharacterRange(start, end));

                        // The replacement has shifted the original match offsets
                        replaceOffset += replacement.Length - m.Value.Length;

                        return replacement;
                    }
                    )
                    );

            editor.EndUndoAction();
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
            if (base.Equals(obj)) {
                return SearchExpression.ToString().Equals(((RegexSearch)obj).SearchExpression.ToString());
            }
            else {
                return false;
            }
        }

        // Returns the CharacterRange that represents the current match found in the search range in the given text
        private static CharacterRange GetMatchRange(CharacterRange searchRange, string text, Match m) {
            int start = searchRange.cpMin + text.Substring(0, m.Index).Length;
            int end = text.Substring(m.Index, m.Length).Length;
            CharacterRange range = new CharacterRange(start, start + end);
            return range;
        }
    }
}
