extern alias HTMLLib;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HTMLLib::HtmlAgilityPack;
using Tidy.Core;
using HtmlDocument = HTMLLib::HtmlAgilityPack.HtmlDocument;

namespace uzLib.Lite.Extensions
{
    extern alias HTMLLib;

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
            string orEl = $@"\[{element}\]";

            return Regex.Replace(html, orEl, m => Callback(m, element, html));
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
            return string.Format("{3}<{0}>{1}{2}", element, Environment.NewLine, sep, string.IsNullOrWhiteSpace(befChar.Replace(Environment.NewLine, " ")) ? sep : "");
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
        /// <returns></returns>
        public static IEnumerable<HtmlNode> GetNodesByName(this HtmlNode node, string name, string rgxPattern = "")
        {
            return node?.Descendants()?
                .Where(n => n.NodeType == HtmlNodeType.Element)
                .Where(n => string.IsNullOrEmpty(rgxPattern) ? n.OriginalName == name : Regex.IsMatch(n.OriginalName, rgxPattern));
        }

        /// <summary>
        ///     Gets the name of the node by.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static HtmlNode GetNodeByName(this HtmlNode node, string name)
        {
            return node?.GetNodesByName(name).FirstOrDefault();
        }

        /// <summary>
        ///     Gets the nodes by class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> GetNodesByClass(this HtmlNode node, string className)
        {
            Regex regex = new Regex("\\b" + Regex.Escape(className) + "\\b", RegexOptions.Compiled);

            return node?.Descendants()?
                .Where(n => n.NodeType == HtmlNodeType.Element)
                .Where(n => n.Attributes?["class"] != null && regex.IsMatch(n.GetAttributeValue("class", string.Empty)));

            // (toLower ? n?.Attributes?["class"]?.Value?.ToLowerInvariant() : n?.Attributes?["class"]?.Value) == className
        }

        /// <summary>
        ///     Gets the node by class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        public static HtmlNode GetNodeByClass(this HtmlNode node, string className)
        {
            return node?.GetNodesByClass(className).FirstOrDefault();
        }

        /// <summary>
        ///     Gets the nodes by containing class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> GetNodesByContainingClass(this HtmlNode node, string className,
            bool toLower = false)
        {
            return node?.Descendants()?
                .Where(n => n.NodeType == HtmlNodeType.Element)
                .Where(n => (toLower ? n?.Attributes?["class"]?.Value?.ToLowerInvariant() : n?.Attributes?["class"]?.Value)
                ?.Contains(className) == true);
        }

        /// <summary>
        ///     Gets the node by containing class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        public static HtmlNode GetNodeByContainingClass(this HtmlNode node, string className)
        {
            return node?.GetNodesByContainingClass(className).FirstOrDefault();
        }

        /// <summary>
        ///     Gets the nodes by attribute.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> GetNodesByAttr(this HtmlNode node, string attrName)
        {
            return node?.Descendants()?
                .Where(n => n.NodeType == HtmlNodeType.Element)
                .Where(n => n.Attributes?[attrName.ToLowerInvariant()] != null);
        }

        /// <summary>
        ///     Gets the node by attribute.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <returns></returns>
        public static HtmlNode GetNodeByAttr(this HtmlNode node, string attrName)
        {
            return node?.GetNodesByAttr(attrName).FirstOrDefault();
        }

        /// <summary>
        ///     Determines whether [has node name] [the specified name].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        ///     <c>true</c> if [has node name] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNodeName(this HtmlNode node, string name)
        {
            return node?.GetNodeByName(name) != null;
        }

        /// <summary>
        ///     Determines whether [has node class] [the specified class name].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns>
        ///     <c>true</c> if [has node class] [the specified class name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNodeClass(this HtmlNode node, string className)
        {
            return node?.GetNodeByClass(className) != null;
        }

        /// <summary>
        ///     Determines whether [has node containing class] [the specified class name].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns>
        ///     <c>true</c> if [has node containing class] [the specified class name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNodeContainingClass(this HtmlNode node, string className)
        {
            return node?.GetNodeByContainingClass(className) != null;
        }

        /// <summary>
        ///     Nodes the has attribute.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <returns></returns>
        public static bool NodeHasAttr(this HtmlNode node, string attrName)
        {
            return node?.GetNodeByAttr(attrName) != null;
        }

        /// <summary>
        ///     Determines whether [has node name] [the specified name].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="name">The name.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     <c>true</c> if [has node name] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNodeName(this HtmlNode node, string name, Func<HtmlNode, bool> predicate,
            bool toLower = false)
        {
            return predicate(node?.GetNodeByName(name));
        }

        /// <summary>
        ///     Determines whether [has node class] [the specified class name].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     <c>true</c> if [has node class] [the specified class name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNodeClass(this HtmlNode node, string className, Func<HtmlNode, bool> predicate,
            bool toLower = false)
        {
            return predicate(node?.GetNodeByClass(className));
        }

        /// <summary>
        ///     Determines whether [has node containing class] [the specified class name].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///     <c>true</c> if [has node containing class] [the specified class name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNodeContainingClass(this HtmlNode node, string className, Func<HtmlNode, bool> predicate,
            bool toLower = false)
        {
            return predicate(node?.GetNodeByContainingClass(className));
        }

        /// <summary>
        ///     Nodes the has attribute.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static bool NodeHasAttr(this HtmlNode node, string attrName, Func<HtmlNode, bool> predicate,
            bool toLower = false)
        {
            return predicate(node?.GetNodeByAttr(attrName));
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
        /// Gets the element child nodes.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> GetElementChildNodes(this HtmlNode node)
        {
            return GetChildNodesByType(node, HtmlNodeType.Element);
        }

        /// <summary>
        /// Gets the element child node by index.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static HtmlNode GetElementChildNode(this HtmlNode node, int index)
        {
            return InternalGetElementNodes(node)[index];
        }

        /// <summary>
        /// Returns the first child.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static HtmlNode FirstChild(this HtmlNode node)
        {
            return InternalGetElementNodes(node)?.FirstOrDefault();
        }

        /// <summary>
        /// Returns the last child.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static HtmlNode LastChild(this HtmlNode node)
        {
            return InternalGetElementNodes(node)?.LastOrDefault();
        }

        /// <summary>
        /// Returns the odd childs.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> OddChilds(this HtmlNode node)
        {
            return InternalGetElementNodes(node)?.Where((n, index) => index % 2 != 0);
        }

        /// <summary>
        /// Returns the even childs.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> EvenChilds(this HtmlNode node)
        {
            return InternalGetElementNodes(node)?.Where((n, index) => index % 2 == 0);
        }

        /// <summary>
        /// Filters the child by index.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">predicate</exception>
        public static IEnumerable<HtmlNode> FilterChildByIndex(this HtmlNode node, Predicate<int> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return InternalGetElementNodes(node)?.Where((n, index) => predicate(index));
        }

        /// <summary>
        /// Filters the child by node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">predicate</exception>
        public static IEnumerable<HtmlNode> FilterChildByNode(this HtmlNode node, Predicate<HtmlNode> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return InternalGetElementNodes(node)?.Where(n => predicate(n));
        }

        private static HtmlNode[] InternalGetElementNodes(HtmlNode node)
        {
            return GetChildNodesByType(node, HtmlNodeType.Element).ToArray();
        }

        /// <summary>
        /// Gets the child nodes by type.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">node</exception>
        public static IEnumerable<HtmlNode> GetChildNodesByType(this HtmlNode node, HtmlNodeType type)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return node.ChildNodes.Where(n => n.NodeType == type);
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