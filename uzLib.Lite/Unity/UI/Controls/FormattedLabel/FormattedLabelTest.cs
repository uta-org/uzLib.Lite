using System;
using UnityEngine;

namespace UnityEngine.UI.Controls
{
    public class FormattedLabelTest : MonoBehaviour, IHyperlinkCallback
    {
        // Generate unique Window ID
        private const int WINDOW_ID = 0;

        private const int MOUSE_WINDOW_ID = 1;

        // The text to render in a series of formatted labels
        private FormattedLabel _formattedLabelText;

        // Used in the GUI to select the mouse cursor
        private int _selectedText;

        private readonly string[] _textLabels = Enum.GetNames(typeof(FormattedLabel.TestText));

        // The position and dimension of the window to draw the text
        private Rect _windowPosition = new Rect(100, 60, 300, 200);

        private void Start()
        {
        }

        private void OnGUI()
        {
            // Simple GUI to select the text to format
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, 50));
            GUILayout.BeginHorizontal();
            GUILayout.Label("Text:");
            var selectedText = GUILayout.SelectionGrid(
                _selectedText,
                _textLabels,
                _textLabels.Length);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            // Format the new text
            if (selectedText != _selectedText || _formattedLabelText == null)
            {
                _selectedText = selectedText;
                var testText = (FormattedLabel.TestText)
                    Enum.Parse(typeof(FormattedLabel.TestText),
                        _textLabels[_selectedText]);
                var textToFormat = FormattedLabel.GetTestText(testText);
                _formattedLabelText = new FormattedLabel(_windowPosition.width,
                    textToFormat);
                _formattedLabelText.setHyperlinkCallback(this);
            }

            // Draw the formatted text
            _windowPosition = GUILayout.Window(WINDOW_ID,
                _windowPosition,
                CreateFormattedLabelWindow,
                "Formatted Label");
        }

        /// <summary>
        ///     Create a window to display the formatted label
        /// </summary>
        /// <param name="windowID">The ID of the window</param>
        private void CreateFormattedLabelWindow(int windowID)
        {
            _formattedLabelText.draw();
        }

        /// <summary>
        ///     Create a window to draw the mouse cursor
        /// </summary>
        /// <param name="windowID">The ID of the window</param>
        private void CreateMouseCursorWindow(int windowID)
        {
            GUI.BringWindowToFront(windowID);
        }

        #region IHyperlinkCallback Members

        void IHyperlinkCallback.onHyperlinkEnter(string hyperlinkId)
        {
            // The mouse is over a hyperlink
            Debug.Log("onHyperlinkEnter: " + hyperlinkId);
            //_mouseCursorTexture = _mouseCursorTextureLink;
        }

        void IHyperlinkCallback.onHyperLinkActivated(string hyperlinkId)
        {
            // A hyperlink was activated/clicked
            Debug.Log("onHyperLinkActivated: " + hyperlinkId);
        }

        void IHyperlinkCallback.onHyperlinkLeave(string hyperlinkId)
        {
            // The mouse is no longer over a hyperlink
            Debug.Log("onHyperlinkLeave: " + hyperlinkId);
            //_mouseCursorTexture = _mouseCursorTextureArrow;
        }

        #endregion IHyperlinkCallback Members
    }
}