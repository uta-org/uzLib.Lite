using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _System.Drawing;
using Unity.API;
using UnityEngine;
using UnityEngine.Global.IMGUI;
using uzLib.Lite.ExternalCode.Extensions;

namespace uzLib.Lite.ExternalCode.Unity.Extensions
{
    /// <summary>
    ///     The Form Helper class
    /// </summary>
    public static class FormHelper
    {
        /// <summary>
        ///     Gets the image list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">list</exception>
        /// <exception cref="NotSupportedException"></exception>
        public static ImageList GetImageList(this List<Sprite> list, string key)
        {
            if (list.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(list));

            var imageList = new ImageList();

            foreach (var sprite in list)
                if (!string.IsNullOrEmpty(sprite.name))
                    imageList.Images.Add(sprite.name, sprite.ToBitmap(true));
                else if (!string.IsNullOrEmpty(key))
                    imageList.Images.Add(key, sprite.ToBitmap(true));
                else
                    throw new NotSupportedException();

            return imageList;
        }

        /// <summary>
        ///     Gets the image list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static ImageList GetImageList(this List<Texture2D> list)
        {
            return GetImageList(list.AsEnumerable());
        }

        /// <summary>
        ///     Gets the image list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">list</exception>
        public static ImageList GetImageList(this IEnumerable<Texture2D> list)
        {
            if (list.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(list));

            var imageList = new ImageList();

            foreach (var texture in list)
                imageList.Images.Add(texture.ToBitmap());

            return imageList;
        }

        /// <summary>
        ///     Debugs the image list.
        /// </summary>
        /// <param name="imageList">The image list.</param>
        public static void DebugImageList(this ImageList imageList)
        {
            DebugImageList(imageList, null);
        }

        /// <summary>
        ///     Debugs the image list.
        /// </summary>
        /// <param name="imageList">The image list.</param>
        /// <param name="args">The arguments.</param>
        /// <exception cref="System.ArgumentNullException">imageList</exception>
        public static void DebugImageList(this ImageList imageList, PaintArgs args)
        {
            if (imageList.Images == null || imageList.Images.Count == 0)
                throw new ArgumentNullException(nameof(imageList));

            var usingGlobalUI = args?.Event.Graphics == null;

            GUILayout.BeginHorizontal();
            {
                // Debug.Log("Before: " + GUI.color);

                foreach (var image in imageList.Images)
                {
                    var _image = (image as ImageList.ImageCollection.ImageInfo)?.Image;
                    var texture = (_image?.uTexture as UnityGdiTexture)?.texture;

                    if (usingGlobalUI)
                        GlobalGUILayout.DrawTexture(texture);
                    else
                    {
                        var r = GlobalGUILayout.GetRectForTexture(texture);
                        // Debug.Log($"Rect: {r} || Type: {Event.current.type}");

                        // GUI.skin.settings.selectionColor = UnityEngine.Color.red;
                        // ApiHolder.Graphics.Clear(Color.White);

                        args.Event.Graphics.uwfDrawImage(_image, r.x, r.y, r.width, r.height);
                    }
                }

                // Debug.Log("After: " + GUI.color);
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        ///     Displays the image list in layout.
        /// </summary>
        /// <param name="imageList">The image list.</param>
        /// <param name="rect">The rect.</param>
        /// <param name="form">The form.</param>
        public static void DisplayImageListInLayout(this ImageList imageList, Rect rect, PaintArgs args)
        {
            DisplayImageListInLayout(imageList, ImageListIteratingSettings.CreateDefaultSettings(rect, args));
        }

        /// <summary>
        ///     Displays the image list in layout.
        /// </summary>
        /// <param name="imageList">The image list.</param>
        /// <param name="settings">The settings.</param>
        /// <exception cref="ArgumentNullException">imageList</exception>
        private static void DisplayImageListInLayout(this ImageList imageList, ImageListIteratingSettings settings)
        {
            if (imageList.Images == null || imageList.Images.Count == 0)
                throw new ArgumentNullException(nameof(imageList));

            InternalIterateImageList(imageList, settings);
        }

        /// <summary>
        ///     Displays the image.
        /// </summary>
        /// <param name="imageList">The image list.</param>
        /// <param name="rect">The rect.</param>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        public static bool DisplayImage(this ImageList imageList, Rect rect, PaintArgs args)
        {
            return DisplayImageListInLayoutAsButton(imageList,
                ImageListIteratingSettings.CreateDefaultSettings(rect, args));
        }

        /// <summary>
        ///     Displays the image.
        /// </summary>
        /// <param name="imageList">The image list.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public static bool DisplayImage(this ImageList imageList, ImageListIteratingSettings settings)
        {
            return DisplayImageListInLayoutAsButton(imageList, settings);
        }

        /// <summary>
        ///     Displays the image.
        /// </summary>
        /// <param name="imageList">The image list.</param>
        /// <param name="rect">The rect.</param>
        /// <param name="form">The form.</param>
        /// <param name="imageInfo">The image information.</param>
        /// <returns></returns>
        public static bool DisplayImage(this ImageList imageList, Rect rect, PaintArgs e,
            out ImageList.ImageCollection.ImageInfo imageInfo)
        {
            return DisplayImageListInLayoutAsButton(imageList,
                ImageListIteratingSettings.CreateDefaultSettings(rect, e), out imageInfo);
        }

        /// <summary>
        ///     Displays the image.
        /// </summary>
        /// <param name="imageList">The image list.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="imageInfo">The image information.</param>
        /// <returns></returns>
        public static bool DisplayImage(this ImageList imageList, ImageListIteratingSettings settings,
            out ImageList.ImageCollection.ImageInfo imageInfo)
        {
            return DisplayImageListInLayoutAsButton(imageList, settings, out imageInfo);
        }

        /// <summary>
        ///     Displays the image list in layout as button.
        /// </summary>
        /// <param name="imageList">The image list.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        private static bool DisplayImageListInLayoutAsButton(this ImageList imageList,
            ImageListIteratingSettings settings)
        {
            return DisplayImageListInLayoutAsButton(imageList, settings, out var imageInfo);
        }

        /// <summary>
        ///     Displays the image list in layout as button.
        /// </summary>
        /// <param name="imageList">The image list.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="imageInfo">The image information.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">imageList</exception>
        /// <exception cref="Exception">Unexpected value!</exception>
        private static bool DisplayImageListInLayoutAsButton(this ImageList imageList,
            ImageListIteratingSettings settings, out ImageList.ImageCollection.ImageInfo imageInfo)
        {
            if (imageList.Images == null || imageList.Images.Count == 0)
                throw new ArgumentNullException(nameof(imageList));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            bool retValue;
            imageInfo = null;

            InternalIterateImageList(imageList, settings, ref imageInfo, out var value, true);

            if (value.HasValue)
                retValue = value.Value;
            else
                throw new Exception("Unexpected value!");

            return retValue;
        }

        /// <summary>
        ///     Internals the iterate image list.
        /// </summary>
        /// <param name="imageList">The image list.</param>
        /// <param name="settings">The settings.</param>
        private static void InternalIterateImageList(ImageList imageList, ImageListIteratingSettings settings)
        {
            ImageList.ImageCollection.ImageInfo imageInfo = null;
            InternalIterateImageList(imageList, settings, ref imageInfo, out var retValue, false);
        }

        /// <summary>
        ///     Internals the iterate image list.
        /// </summary>
        /// <param name="imageList">The image list.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="imageInfo">The image information.</param>
        /// <param name="retValue">The ret val</param>
        /// <param name="forceReturn">if set to <c>true</c> [force return].</param>
        /// <exception cref="ArgumentNullException">
        ///     imageList
        ///     or
        ///     settings
        /// </exception>
        private static void InternalIterateImageList(ImageList imageList, ImageListIteratingSettings settings,
            ref ImageList.ImageCollection.ImageInfo imageInfo, out bool? retValue, bool forceReturn)
        {
            if (imageList.Images == null || imageList.Images.Count == 0)
                throw new ArgumentNullException(nameof(imageList));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (forceReturn)
                retValue = false;
            else
                retValue = null;

            var margin = settings.Margin;

            var dims = settings.AreIcons
                ? new Vector2(imageList.Images[0].Width, imageList.Images[0].Height)
                : settings.ImageSize.HasValue
                    ? settings.ImageSize.Value
                    : new Vector2(128, 128);

            int tWidth = (int)dims.x,
                tHeight = (int)dims.y;

            float horPadding = margin?.horizontal ?? 0,
                verPadding = margin?.vertical ?? 0;

            int tWidthPad = settings.AreIcons ? tWidth + (int)horPadding : 138,
                tHeightPad = settings.AreIcons ? tHeight + (int)verPadding : 138;

            int columns = Mathf.FloorToInt(settings.Rectangle.width / tWidthPad),
                rows = Mathf.CeilToInt(imageList.Images.Count / (float)columns);

            // GUILayout.BeginHorizontal();
            {
                if (rows <= 0 || columns <= 0)
                    return;

                //// Vector use two determine the height of the ScrollView
                //var viewSize = new Vector2(
                //    -1,
                //    Mathf.Min(
                //        containingSize.y - (51 + (f_isEditor ? 0 : r.CurrentHeight + 10)),
                //        tHeightPad * rows - (f_isEditor ? 0 : r.CurrentHeight + 10)));

                //// Rect used to determine the inner rectangle of the ScrollView
                //var viewRectLayout = new Rect(new Vector2(0, r.CurrentHeight),
                //    new Vector2(columns * tWidthPad, rows * tHeightPad));

                //m_scroll = GlobalGUILayout.BeginScrollView(f_isEditor, m_scroll, viewSize);

                GUILayout.BeginVertical();

                for (var y = 0; y < rows; ++y)
                {
                    GUILayout.BeginHorizontal();

                    for (var x = 0; x < columns; ++x)
                    {
                        var i = x + y * columns;

                        if (i >= imageList.Images.Count)
                        {
                            GUILayout.FlexibleSpace();
                            break;
                        }

                        var image = imageList.Images[i];
                        var localImageInfo = imageList.Images.InfoItems[i];

                        var rect = DrawTextureWithMargin(settings, margin, image);

                        if (forceReturn && Button(rect))
                        {
                            imageInfo = localImageInfo;
                            retValue = true;
                        }
                    }

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
            }
            // GUILayout.EndHorizontal();
        }

        /// <summary>
        ///     Draws the texture with margin.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="margin">The margin.</param>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        private static Rect DrawTextureWithMargin(ImageListIteratingSettings settings, RectOffset margin, Image image)
        {
            if (margin != null)
                GlobalGUILayout.BeginMargin(margin);

            Rect rect;

            if (settings.ImageSize.HasValue)
            {
                rect = GlobalGUILayout.GetRectForTexture(settings.ImageSize.Value.x, settings.ImageSize.Value.y);

                settings.Args.Event.Graphics.uwfDrawImage(
                    image,
                    rect.x,
                    rect.y,
                    rect.width,
                    rect.height);
            }
            else
            {
                var texture = (image?.uTexture as UnityGdiTexture)?.texture;
                rect = GlobalGUILayout.GetRectForTexture(texture);

                settings.Args.Event.Graphics.uwfDrawImage(
                    image,
                    rect.x,
                    rect.y,
                    rect.width,
                    rect.height);
            }

            if (margin != null)
                GlobalGUILayout.EndMargin(margin);

            return rect;
        }

        /// <summary>
        ///     Draws the debug buttons.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="columns">The columns.</param>
        public static void DrawDebugButtons(RectOffset margin = null, Vector2 size = default, int rows = 10,
            int columns = 10)
        {
            if (size == default)
                size = new Vector2(16, 16);

            //if (margin == null)
            //    margin = new RectOffset(4, 4, 4, 4);

            GUILayout.BeginVertical();

            for (var y = 0; y < rows; ++y)
            {
                GUILayout.BeginHorizontal();

                for (var x = 0; x < columns; ++x)
                {
                    // var i = x + y * columns;

                    if (margin != null)
                        GlobalGUILayout.BeginMargin(margin);

                    var rect = GUILayoutUtility.GetRect(GUIContent.none, "button", GUILayout.Width(size.x),
                        GUILayout.Height(size.y));

                    GUI.Label(rect, string.Empty, "button");

                    if (margin != null)
                        GlobalGUILayout.EndMargin(margin);
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }

        /// <summary>
        ///     Buttons the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        private static bool Button(Rect rect)
        {
            var e = Event.current;
            return e.type == EventType.MouseDown && rect.Contains(e.mousePosition);
        }

        /// <summary>
        ///     The ImageListIteratingSettings class
        /// </summary>
        public class ImageListIteratingSettings
        {
            /// <summary>
            ///     Prevents a default instance of the <see cref="ImageListIteratingSettings" /> class from being created.
            /// </summary>
            private ImageListIteratingSettings()
            {
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="ImageListIteratingSettings" /> class.
            /// </summary>
            /// <param name="rect">The rect.</param>
            /// <param name="form">The form.</param>
            public ImageListIteratingSettings(Rect rect, PaintArgs args)
            {
                Rectangle = rect;
                Args = args;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="ImageListIteratingSettings" /> class.
            /// </summary>
            /// <param name="margin">The margin.</param>
            /// <param name="rect">The rect.</param>
            /// <param name="form">The form.</param>
            public ImageListIteratingSettings(float margin, Rect rect, PaintArgs args)
                : this(new Vector2(margin, margin), rect, args)
            {
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="ImageListIteratingSettings" /> class.
            /// </summary>
            /// <param name="margin">The margin.</param>
            /// <param name="rect">The rect.</param>
            /// <param name="form">The form.</param>
            public ImageListIteratingSettings(Vector2 margin, Rect rect, PaintArgs args)
                : this(new RectOffset(
                    (int)margin.x,
                    (int)margin.x,
                    (int)margin.y,
                    (int)margin.y), rect, args)
            {
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="ImageListIteratingSettings" /> class.
            /// </summary>
            /// <param name="margin">The margin.</param>
            /// <param name="rect">The rect.</param>
            /// <param name="form">The form.</param>
            public ImageListIteratingSettings(RectOffset margin, Rect rect, PaintArgs args)
                : this(rect, args)
            {
                Margin = margin;
            }

            /// <summary>
            ///     Gets the margin.
            /// </summary>
            /// <value>
            ///     The margin.
            /// </value>
            public RectOffset Margin { get; }

            /// <summary>
            ///     Gets the rectangle.
            /// </summary>
            /// <value>
            ///     The rectangle.
            /// </value>
            public Rect Rectangle { get; }

            /// <summary>
            ///     Gets or sets the size of the image.
            /// </summary>
            /// <value>
            ///     The size of the image.
            /// </value>
            public Vector2? ImageSize { get; set; }

            /// <summary>
            ///     Gets or sets a value indicating whether [are icons].
            /// </summary>
            /// <value>
            ///     <c>true</c> if [are icons]; otherwise, <c>false</c>.
            /// </value>
            public bool AreIcons { get; set; }

            /// <summary>
            ///     Gets the arguments.
            /// </summary>
            /// <value>
            ///     The arguments.
            /// </value>
            public PaintArgs Args { get; }

            /// <summary>
            ///     Creates the default settings.
            /// </summary>
            /// <param name="rect">The rect.</param>
            /// <param name="form">The form.</param>
            /// <returns></returns>
            public static ImageListIteratingSettings CreateDefaultSettings(Rect rect, PaintArgs args)
            {
                return new ImageListIteratingSettings(rect, args);
            }
        }

        /// <summary>
        /// Convert to TextAnchor.
        /// </summary>
        /// <param name="align">The align.</param>
        /// <returns></returns>
        public static TextAnchor ToTextAnchor(this ContentAlignment align)
        {
            var uAlign = TextAnchor.UpperLeft;

            switch (align)
            {
                case ContentAlignment.BottomCenter:
                    uAlign = TextAnchor.LowerCenter;
                    break;

                case ContentAlignment.BottomLeft:
                    uAlign = TextAnchor.LowerLeft;
                    break;

                case ContentAlignment.BottomRight:
                    uAlign = TextAnchor.LowerRight;
                    break;

                case ContentAlignment.MiddleCenter:
                    uAlign = TextAnchor.MiddleCenter;
                    break;

                case ContentAlignment.MiddleLeft:
                    uAlign = TextAnchor.MiddleLeft;
                    break;

                case ContentAlignment.MiddleRight:
                    uAlign = TextAnchor.MiddleRight;
                    break;

                case ContentAlignment.TopCenter:
                    uAlign = TextAnchor.UpperCenter;
                    break;

                case ContentAlignment.TopLeft:
                    uAlign = TextAnchor.UpperLeft;
                    break;

                case ContentAlignment.TopRight:
                    uAlign = TextAnchor.UpperRight;
                    break;
            }

            return uAlign;
        }
    }
}