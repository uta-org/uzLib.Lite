using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace UnityEngine.Extensions
{
    public static class FileHelper
    {
        private static readonly Lazy<string[]> InvalidFileNameChars = new Lazy<string[]>(() => Path
            .GetInvalidPathChars()
            .Union(Path.GetInvalidFileNameChars()
                .Union(new[] { '+', '#' })).Select(c => c.ToString(CultureInfo.InvariantCulture)).ToArray());

        private static readonly HashSet<string> ProhibitedNames = new HashSet<string>
        {
            @"aux",
            @"con",
            @"clock$",
            @"nul",
            @"prn",

            @"com1",
            @"com2",
            @"com3",
            @"com4",
            @"com5",
            @"com6",
            @"com7",
            @"com8",
            @"com9",

            @"lpt1",
            @"lpt2",
            @"lpt3",
            @"lpt4",
            @"lpt5",
            @"lpt6",
            @"lpt7",
            @"lpt8",
            @"lpt9"
        };

        public static bool IsValidFileName(string fileName)
        {
            return !string.IsNullOrWhiteSpace(fileName)
                   && fileName.All(o => !IsInvalidFileNameChar(o))
                   && !IsProhibitedName(fileName);
        }

        public static bool IsProhibitedName(string fileName)
        {
            return ProhibitedNames.Contains(fileName.ToLower(CultureInfo.InvariantCulture));
        }

        private static string ReplaceInvalidFileNameSymbols([CanBeNull] this string value, string replacementValue)
        {
            if (value == null) return null;

            return InvalidFileNameChars.Value.Aggregate(new StringBuilder(value),
                (sb, currentChar) => sb.Replace(currentChar, replacementValue)).ToString();
        }

        public static bool IsInvalidFileNameChar(char value)
        {
            return InvalidFileNameChars.Value.Contains(value.ToString(CultureInfo.InvariantCulture));
        }

#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

        /// <summary>
        ///     Checks if file name has invalid characters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool CheckIfFileNameHasInvalidCharacters([NotNull] this string value)
        {
            var newValue = GetValidFileName(value, @"_");
            return newValue != value;
        }

        public static string GetValidFileName([NotNull] this string value)
        {
            return GetValidFileName(value, @"_");
        }

        public static string GetValidFileName([NotNull] this string value, string replacementValue)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(@"value should be non empty", nameof(value));

            if (IsProhibitedName(value))
                return (string.IsNullOrWhiteSpace(replacementValue) ? @"_" : replacementValue) + value;

            return ReplaceInvalidFileNameSymbols(value, replacementValue);
        }

        public static bool IsValidTexture(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            var extension = Path.GetExtension(path).ToLowerInvariant();

            if (string.IsNullOrEmpty(extension))
                throw new ArgumentNullException(nameof(path), "Invalid path given.");

            // PSD, TIFF, JPG, TGA, PNG, GIF, BMP, IFF, PICT
            return IsValidTextureExtension(extension);
        }

        public static bool IsValidTextureExtension(string extension)
        {
            return extension == ".psd"
                   || extension == ".tiff"
                   || extension == ".jpg"
                   || extension == ".jpeg"
                   || extension == ".png"
                   || extension == ".tga"
                   || extension == ".bmp"
                   || extension == ".iff"
                   || extension == ".pict"
                   || extension == ".gif";
        }

#endif
    }
}