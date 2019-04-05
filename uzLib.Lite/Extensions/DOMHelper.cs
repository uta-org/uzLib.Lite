﻿using System;
using System.CodeDom;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The DOMHelper class
    /// </summary>
    public static class DOMHelper
    {
        /// <summary>
        /// Marks as static class with extension methods.
        /// </summary>
        /// <param name="class_">The class.</param>
        public static void MarkAsStaticClassWithExtensionMethods(this CodeTypeDeclaration class_)
        {
            class_.Attributes = MemberAttributes.Public;

            class_.StartDirectives.Add(new CodeRegionDirective(
                    CodeRegionMode.Start, Environment.NewLine + "\tstatic"));

            class_.EndDirectives.Add(new CodeRegionDirective(
                    CodeRegionMode.End, string.Empty));
        }
    }
}