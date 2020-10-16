using System;
using System.Collections.Generic;
using System.Text;

namespace uzLib.Lite.Core.Input.Internal
{
    using Interfaces;

    /// <summary>
    /// The KeyHandler class
    /// </summary>
    internal class KeyHandler
    {
        /// <summary>
        /// The cursor position
        /// </summary>
        private int _cursorPos;

        /// <summary>
        /// The cursor limit
        /// </summary>
        private int _cursorLimit;

        /// <summary>
        /// The text
        /// </summary>
        private StringBuilder _text;

        /// <summary>
        /// The history
        /// </summary>
        private List<string> _history;

        /// <summary>
        /// The history index
        /// </summary>
        private int _historyIndex;

        /// <summary>
        /// The key information
        /// </summary>
        private ConsoleKeyInfo _keyInfo;

        /// <summary>
        /// The key actions
        /// </summary>
        private Dictionary<string, Action> _keyActions;

        /// <summary>
        /// The completions
        /// </summary>
        private string[] _completions;

        /// <summary>
        /// The completion start
        /// </summary>
        private int _completionStart;

        /// <summary>
        /// The completions index
        /// </summary>
        private int _completionsIndex;

        /// <summary>
        /// The console2
        /// </summary>
        private IConsole Console2;

        /// <summary>
        /// Determines whether [is start of line].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is start of line]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsStartOfLine() => _cursorPos == 0;

        /// <summary>
        /// Determines whether [is end of line].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is end of line]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsEndOfLine() => _cursorPos == _cursorLimit;

        /// <summary>
        /// Determines whether [is start of buffer].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is start of buffer]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsStartOfBuffer() => Console2.CursorLeft == 0;

        /// <summary>
        /// Determines whether [is end of buffer].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is end of buffer]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsEndOfBuffer() => Console2.CursorLeft == Console2.BufferWidth - 1;

        /// <summary>
        /// Determines whether [is in automatic complete mode].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is in automatic complete mode]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsInAutoCompleteMode() => _completions != null;

        /// <summary>
        /// Moves the cursor left.
        /// </summary>
        private void MoveCursorLeft()
        {
            if (IsStartOfLine())
                return;

            if (IsStartOfBuffer())
                Console2.SetCursorPosition(Console2.BufferWidth - 1, Console2.CursorTop - 1);
            else
                Console2.SetCursorPosition(Console2.CursorLeft - 1, Console2.CursorTop);

            _cursorPos--;
        }

        /// <summary>
        /// Moves the cursor home.
        /// </summary>
        private void MoveCursorHome()
        {
            while (!IsStartOfLine())
                MoveCursorLeft();
        }

        /// <summary>
        /// Builds the key input.
        /// </summary>
        /// <returns></returns>
        private string BuildKeyInput()
        {
            return _keyInfo.Modifiers != ConsoleModifiers.Control && _keyInfo.Modifiers != ConsoleModifiers.Shift ?
                _keyInfo.Key.ToString() : _keyInfo.Modifiers.ToString() + _keyInfo.Key.ToString();
        }

        /// <summary>
        /// Moves the cursor right.
        /// </summary>
        private void MoveCursorRight()
        {
            if (IsEndOfLine())
                return;

            if (IsEndOfBuffer())
                Console2.SetCursorPosition(0, Console2.CursorTop + 1);
            else
                Console2.SetCursorPosition(Console2.CursorLeft + 1, Console2.CursorTop);

            _cursorPos++;
        }

        /// <summary>
        /// Moves the cursor end.
        /// </summary>
        private void MoveCursorEnd()
        {
            while (!IsEndOfLine())
                MoveCursorRight();
        }

        /// <summary>
        /// Clears the line.
        /// </summary>
        private void ClearLine()
        {
            MoveCursorEnd();
            while (!IsStartOfLine())
                Backspace();
        }

        /// <summary>
        /// Writes the new string.
        /// </summary>
        /// <param name="str">The string.</param>
        private void WriteNewString(string str)
        {
            ClearLine();
            foreach (char character in str)
                WriteChar(character);
        }

        /// <summary>
        /// Writes the string.
        /// </summary>
        /// <param name="str">The string.</param>
        private void WriteString(string str)
        {
            foreach (char character in str)
                WriteChar(character);
        }

        /// <summary>
        /// Writes the character.
        /// </summary>
        private void WriteChar() => WriteChar(_keyInfo.KeyChar);

        /// <summary>
        /// Writes the character.
        /// </summary>
        /// <param name="c">The c.</param>
        private void WriteChar(char c)
        {
            if (IsEndOfLine())
            {
                _text.Append(c);
                Console2.Write(c.ToString());
                _cursorPos++;
            }
            else
            {
                int left = Console2.CursorLeft;
                int top = Console2.CursorTop;
                string str = _text.ToString().Substring(_cursorPos);
                _text.Insert(_cursorPos, c);
                Console2.Write(c.ToString() + str);
                Console2.SetCursorPosition(left, top);
                MoveCursorRight();
            }

            _cursorLimit++;
        }

        /// <summary>
        /// Backspaces this instance.
        /// </summary>
        private void Backspace()
        {
            if (IsStartOfLine())
                return;

            MoveCursorLeft();
            int index = _cursorPos;
            _text.Remove(index, 1);
            string replacement = _text.ToString().Substring(index);
            int left = Console2.CursorLeft;
            int top = Console2.CursorTop;
            Console2.Write(string.Format("{0} ", replacement));
            Console2.SetCursorPosition(left, top);
            _cursorLimit--;
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        private void Delete()
        {
            if (IsEndOfLine())
                return;

            int index = _cursorPos;
            _text.Remove(index, 1);
            string replacement = _text.ToString().Substring(index);
            int left = Console2.CursorLeft;
            int top = Console2.CursorTop;
            Console2.Write(string.Format("{0} ", replacement));
            Console2.SetCursorPosition(left, top);
            _cursorLimit--;
        }

        /// <summary>
        /// Transposes the chars.
        /// </summary>
        private void TransposeChars()
        {
            // local helper functions
            bool almostEndOfLine() => _cursorLimit - _cursorPos == 1;
            int incrementIf(Func<bool> expression, int index) => expression() ? index + 1 : index;
            int decrementIf(Func<bool> expression, int index) => expression() ? index - 1 : index;

            if (IsStartOfLine()) { return; }

            var firstIdx = decrementIf(IsEndOfLine, _cursorPos - 1);
            var secondIdx = decrementIf(IsEndOfLine, _cursorPos);

            var secondChar = _text[secondIdx];
            _text[secondIdx] = _text[firstIdx];
            _text[firstIdx] = secondChar;

            var left = incrementIf(almostEndOfLine, Console2.CursorLeft);
            var cursorPosition = incrementIf(almostEndOfLine, _cursorPos);

            WriteNewString(_text.ToString());

            Console2.SetCursorPosition(left, Console2.CursorTop);
            _cursorPos = cursorPosition;

            MoveCursorRight();
        }

        /// <summary>
        /// Starts the automatic complete.
        /// </summary>
        private void StartAutoComplete()
        {
            while (_cursorPos > _completionStart)
                Backspace();

            _completionsIndex = 0;

            WriteString(_completions[_completionsIndex]);
        }

        /// <summary>
        /// Nexts the automatic complete.
        /// </summary>
        private void NextAutoComplete()
        {
            while (_cursorPos > _completionStart)
                Backspace();

            _completionsIndex++;

            if (_completionsIndex == _completions.Length)
                _completionsIndex = 0;

            WriteString(_completions[_completionsIndex]);
        }

        /// <summary>
        /// Previouses the automatic complete.
        /// </summary>
        private void PreviousAutoComplete()
        {
            while (_cursorPos > _completionStart)
                Backspace();

            _completionsIndex--;

            if (_completionsIndex == -1)
                _completionsIndex = _completions.Length - 1;

            WriteString(_completions[_completionsIndex]);
        }

        /// <summary>
        /// Previouses the history.
        /// </summary>
        private void PrevHistory()
        {
            if (_historyIndex > 0)
            {
                _historyIndex--;
                WriteNewString(_history[_historyIndex]);
            }
        }

        /// <summary>
        /// Nexts the history.
        /// </summary>
        private void NextHistory()
        {
            if (_historyIndex < _history.Count)
            {
                _historyIndex++;
                if (_historyIndex == _history.Count)
                    ClearLine();
                else
                    WriteNewString(_history[_historyIndex]);
            }
        }

        /// <summary>
        /// Resets the automatic complete.
        /// </summary>
        private void ResetAutoComplete()
        {
            _completions = null;
            _completionsIndex = 0;
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get
            {
                return _text.ToString();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyHandler"/> class.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="history">The history.</param>
        /// <param name="autoCompleteHandler">The automatic complete handler.</param>
        public KeyHandler(IConsole console, List<string> history, IAutoCompleteHandler autoCompleteHandler)
        {
            Console2 = console;

            _history = history ?? new List<string>();
            _historyIndex = _history.Count;
            _text = new StringBuilder();
            _keyActions = new Dictionary<string, Action>();

            _keyActions["LeftArrow"] = MoveCursorLeft;
            _keyActions["Home"] = MoveCursorHome;
            _keyActions["End"] = MoveCursorEnd;
            _keyActions["ControlA"] = MoveCursorHome;
            _keyActions["ControlB"] = MoveCursorLeft;
            _keyActions["RightArrow"] = MoveCursorRight;
            _keyActions["ControlF"] = MoveCursorRight;
            _keyActions["ControlE"] = MoveCursorEnd;
            _keyActions["Backspace"] = Backspace;
            _keyActions["Delete"] = Delete;
            _keyActions["ControlD"] = Delete;
            _keyActions["ControlH"] = Backspace;
            _keyActions["ControlL"] = ClearLine;
            _keyActions["Escape"] = ClearLine;
            _keyActions["UpArrow"] = PrevHistory;
            _keyActions["ControlP"] = PrevHistory;
            _keyActions["DownArrow"] = NextHistory;
            _keyActions["ControlN"] = NextHistory;
            _keyActions["ControlU"] = () =>
            {
                while (!IsStartOfLine())
                    Backspace();
            };
            _keyActions["ControlK"] = () =>
            {
                int pos = _cursorPos;
                MoveCursorEnd();
                while (_cursorPos > pos)
                    Backspace();
            };
            _keyActions["ControlW"] = () =>
            {
                while (!IsStartOfLine() && _text[_cursorPos - 1] != ' ')
                    Backspace();
            };
            _keyActions["ControlT"] = TransposeChars;

            _keyActions["Tab"] = () =>
            {
                if (IsInAutoCompleteMode())
                {
                    NextAutoComplete();
                }
                else
                {
                    if (autoCompleteHandler == null || !IsEndOfLine())
                        return;

                    string text = _text.ToString();

                    _completionStart = text.LastIndexOfAny(autoCompleteHandler.Separators);
                    _completionStart = _completionStart == -1 ? 0 : _completionStart + 1;

                    _completions = autoCompleteHandler.GetSuggestions(text, _completionStart);
                    _completions = _completions?.Length == 0 ? null : _completions;

                    if (_completions == null)
                        return;

                    StartAutoComplete();
                }
            };

            _keyActions["ShiftTab"] = () =>
            {
                if (IsInAutoCompleteMode())
                {
                    PreviousAutoComplete();
                }
            };
        }

        /// <summary>
        /// Handles the specified key information.
        /// </summary>
        /// <param name="keyInfo">The key information.</param>
        public void Handle(ConsoleKeyInfo keyInfo)
        {
            _keyInfo = keyInfo;

            // If in auto complete mode and Tab wasn't pressed
            if (IsInAutoCompleteMode() && _keyInfo.Key != ConsoleKey.Tab)
                ResetAutoComplete();

            Action action;
            _keyActions.TryGetValue(BuildKeyInput(), out action);
            action = action ?? WriteChar;
            action.Invoke();
        }
    }
}