using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Tidy.Core;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The HTMLHelper class
    /// </summary>
    public static class HTMLHelper
    {
        /// <summary>
        /// Cleans the element.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static string CleanElement(this string html, string element)
        {
            string orEl = string.Format(@"\[{0}\]", element);

            return Regex.Replace(html, orEl, (m) => { return Callback(m, element, html); });
        }

        /// <summary>
        /// Callbacks the specified match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <param name="element">The element.</param>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        private static string Callback(Match match, string element, string html)
        {
            int oc = FindOccurrences(html.Substring(0, match.Index + 1), match.Index);
            string befChar = html.Substring(match.Index - (element.Length + 3), 1);
            string sep = new string(Convert.ToChar(9), oc);
            return string.Format("{3}<{0}>{1}{2}", element, Environment.NewLine, sep, befChar.Replace(Environment.NewLine, " ").IsNullOrWhiteSpace() ? sep : "");
        }

        /// <summary>
        /// Finds the occurrences.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="maxIndex">The maximum index.</param>
        /// <returns></returns>
        private static int FindOccurrences(string str, int maxIndex)
        {
            int lio = str.LastIndexOf("</");
            //Debug.LogFormat("LastIndexOf: {0}\nMatchIndex: {1}\nLength: {2}", lio, maxIndex, str.Length);
            return Regex.Matches(str.Substring(lio, maxIndex - lio), @"<\w+").Count;
        }

        /// <summary>
        ///     Gets the childs.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> GetChilds(this HtmlNode node)
        {
            return node?.ChildNodes.Where(n => n.NodeType != HtmlNodeType.Text);
            //return node?.ChildNodes.Where(n => n.OriginalName != "#text");
        }

        /// <summary>
        ///     Gets the first child.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static HtmlNode GetFirstChild(this HtmlNode node)
        {
            return GetChilds(node).FirstOrDefault();
        }

        /// <summary>
        ///     Gets the name of the nodes by.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="name">The name.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> GetNodesByName(this HtmlNode node, string name, bool toLower = false)
        {
            return node?.Descendants()
                ?.Where(n => (toLower ? n?.OriginalName.ToLowerInvariant() : n?.OriginalName) == name);
        }

        /// <summary>
        ///     Gets the name of the node by.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="name">The name.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns></returns>
        public static HtmlNode GetNodeByName(this HtmlNode node, string name, bool toLower = false)
        {
            return node?.GetNodesByName(name, toLower).FirstOrDefault();
        }

        /// <summary>
        ///     Gets the nodes by class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> GetNodesByClass(this HtmlNode node, string className, bool toLower = false)
        {
            return node?.Descendants()?.Where(n =>
                (toLower ? n?.Attributes?["class"]?.Value?.ToLowerInvariant() : n?.Attributes?["class"]?.Value) ==
                className);
        }

        /// <summary>
        ///     Gets the node by class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns></returns>
        public static HtmlNode GetNodeByClass(this HtmlNode node, string className, bool toLower = false)
        {
            return node?.GetNodesByClass(className, toLower).FirstOrDefault();
        }

        /// <summary>
        ///     Gets the nodes by containing class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> GetNodesByContainingClass(this HtmlNode node, string className,
            bool toLower = false)
        {
            return node?.Descendants()?.Where(n =>
                (toLower ? n?.Attributes?["class"]?.Value?.ToLowerInvariant() : n?.Attributes?["class"]?.Value)
                ?.Contains(className) == true);
        }

        /// <summary>
        ///     Gets the node by containing class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns></returns>
        public static HtmlNode GetNodeByContainingClass(this HtmlNode node, string className, bool toLower = false)
        {
            return node?.GetNodesByContainingClass(className, toLower).FirstOrDefault();
        }

        /// <summary>
        ///     Gets the nodes by attribute.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> GetNodesByAttr(this HtmlNode node, string attrName, bool toLower = false)
        {
            return node?.Descendants()?.Where(n =>
                (toLower ? n?.Attributes?[attrName.ToLowerInvariant()] : n?.Attributes?[attrName.ToLowerInvariant()]) !=
                null);
        }

        /// <summary>
        ///     Gets the node by attribute.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns></returns>
        public static HtmlNode GetNodeByAttr(this HtmlNode node, string attrName, bool toLower = false)
        {
            return node?.GetNodesByAttr(attrName, toLower).FirstOrDefault();
        }

        /// <summary>
        ///     Determines whether [has node name] [the specified name].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="name">The name.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns>
        ///     <c>true</c> if [has node name] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNodeName(this HtmlNode node, string name, bool toLower = false)
        {
            return node?.GetNodeByName(name, toLower) != null;
        }

        /// <summary>
        ///     Determines whether [has node class] [the specified class name].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns>
        ///     <c>true</c> if [has node class] [the specified class name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNodeClass(this HtmlNode node, string className, bool toLower = false)
        {
            return node?.GetNodeByClass(className, toLower) != null;
        }

        /// <summary>
        ///     Determines whether [has node containing class] [the specified class name].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns>
        ///     <c>true</c> if [has node containing class] [the specified class name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNodeContainingClass(this HtmlNode node, string className, bool toLower = false)
        {
            return node?.GetNodeByContainingClass(className, toLower) != null;
        }

        /// <summary>
        ///     Nodes the has attribute.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns></returns>
        public static bool NodeHasAttr(this HtmlNode node, string attrName, bool toLower = false)
        {
            return node?.GetNodeByAttr(attrName, toLower) != null;
        }

        /// <summary>
        ///     Determines whether [has node name] [the specified name].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="name">The name.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns>
        ///     <c>true</c> if [has node name] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNodeName(this HtmlNode node, string name, Func<HtmlNode, bool> predicate,
            bool toLower = false)
        {
            return predicate(node?.GetNodeByName(name, toLower));
        }

        /// <summary>
        ///     Determines whether [has node class] [the specified class name].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns>
        ///     <c>true</c> if [has node class] [the specified class name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNodeClass(this HtmlNode node, string className, Func<HtmlNode, bool> predicate,
            bool toLower = false)
        {
            return predicate(node?.GetNodeByClass(className, toLower));
        }

        /// <summary>
        ///     Determines whether [has node containing class] [the specified class name].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns>
        ///     <c>true</c> if [has node containing class] [the specified class name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNodeContainingClass(this HtmlNode node, string className, Func<HtmlNode, bool> predicate,
            bool toLower = false)
        {
            return predicate(node?.GetNodeByContainingClass(className, toLower));
        }

        /// <summary>
        ///     Nodes the has attribute.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns></returns>
        public static bool NodeHasAttr(this HtmlNode node, string attrName, Func<HtmlNode, bool> predicate,
            bool toLower = false)
        {
            return predicate(node?.GetNodeByAttr(attrName, toLower));
        }

        /// <summary>
        ///     Pretties the print.
        /// </summary>
        /// <param name="dirtyHtml">The dirty HTML.</param>
        /// <returns></returns>
        public static string PrettyPrint(string dirtyHtml)
        {
            return PrettyPrint(dirtyHtml, out var messages);
        }

        /// <summary>
        ///     Pretties the print.
        /// </summary>
        /// <param name="dirtyHtml">The dirty HTML.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static string PrettyPrint(string dirtyHtml, out TidyMessageCollection messages)
        {
            const int spaces = 8;

            var tidy = new Tidy.Core.Tidy();
            tidy.Options.SmartIndent = true;
            tidy.Options.IndentAttributes = false;
            tidy.Options.WrapLen = 0;
            tidy.Options.Spaces = spaces;

            messages = new TidyMessageCollection();

            using (var inStream = new MemoryStream(Encoding.Default.GetBytes(dirtyHtml)))
            using (var outStream = new MemoryStream())
            {
                tidy.Parse(inStream, outStream, messages);
                return Encoding.Default.GetString(outStream.ToArray())
                    .Replace(new string(' ', spaces), '\t'.ToString());
            }
        }

        /// <summary>
        ///     Converts HTML to plain text / strips tags.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static string ConvertToPlainText(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            using (var sw = new StringWriter())
            {
                ConvertTo(doc.DocumentNode, sw);
                sw.Flush();

                return sw.ToString();
            }
        }

        /// <summary>
        ///     Count the words.
        ///     The content has to be converted to plain text before (using ConvertToPlainText).
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns></returns>
        public static int CountWords(string plainText)
        {
            return !string.IsNullOrEmpty(plainText) ? plainText.Split(' ', '\n').Length : 0;
        }

        public static string Cut(string text, int length)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > length) text = text.Substring(0, length - 4) + " ...";
            return text;
        }

        /// <summary>
        ///     Converts the content to.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="outText">The out text.</param>
        private static void ConvertContentTo(HtmlNode node, TextWriter outText)
        {
            foreach (var subnode in node.ChildNodes) ConvertTo(subnode, outText);
        }

        /// <summary>
        ///     Converts to.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="outText">The out text.</param>
        private static void ConvertTo(HtmlNode node, TextWriter outText)
        {
            string html;
            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    // don't output comments
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                    // script and style must not be output
                    var parentName = node.ParentNode.Name;
                    if (parentName == "script" || parentName == "style") break;

                    // get text
                    html = ((HtmlTextNode)node).Text;

                    // is it in fact a special closing node output as text?
                    if (HtmlNode.IsOverlappedClosingElement(html)) break;

                    // check the text is meaningful and not a bunch of whitespaces
                    if (html.Trim().Length > 0) outText.Write(HtmlEntity.DeEntitize(html));

                    break;

                case HtmlNodeType.Element:
                    switch (node.Name)
                    {
                        case "p":
                            // treat paragraphs as crlf
                            outText.Write(Environment.NewLine);
                            break;

                        case "br":
                            outText.Write(Environment.NewLine);
                            break;
                    }

                    if (node.HasChildNodes) ConvertContentTo(node, outText);

                    break;
            }
        }
    }
}