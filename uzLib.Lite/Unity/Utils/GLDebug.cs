using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace uzLib.Lite.Unity.Utils
{
    /// <summary>
    /// The GLDebug class
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class GLDebug : MonoBehaviour
    {
        /// <summary>
        /// The Line class
        /// </summary>
        private struct Line
        {
            /// <summary>
            /// The start
            /// </summary>
            public Vector3 start;

            /// <summary>
            /// The end
            /// </summary>
            public Vector3 end;

            /// <summary>
            /// The color
            /// </summary>
            public Color color;

            /// <summary>
            /// The start time
            /// </summary>
            public float startTime;

            /// <summary>
            /// The duration
            /// </summary>
            public float duration;

            /// <summary>
            /// Initializes a new instance of the <see cref="Line"/> struct.
            /// </summary>
            /// <param name="start">The start.</param>
            /// <param name="end">The end.</param>
            /// <param name="color">The color.</param>
            /// <param name="startTime">The start time.</param>
            /// <param name="duration">The duration.</param>
            public Line(Vector3 start, Vector3 end, Color color, float startTime, float duration)
            {
                this.start = start;
                this.end = end;
                this.color = color;
                this.startTime = startTime;
                this.duration = duration;
            }

            /// <summary>
            /// Durations the elapsed.
            /// </summary>
            /// <param name="drawLine">if set to <c>true</c> [draw line].</param>
            /// <returns></returns>
            public bool DurationElapsed(bool drawLine)
            {
                if (drawLine)
                {
                    GL.Color(color);
                    GL.Vertex(start);
                    GL.Vertex(end);
                }

                return Time.time - startTime >= duration;
            }
        }

        /// <summary>
        /// The instance
        /// </summary>
        private static GLDebug instance;

        /// <summary>
        /// The mat z on
        /// </summary>
        private static Material matZOn;

        /// <summary>
        /// The mat z off
        /// </summary>
        private static Material matZOff;

        /// <summary>
        /// The toggle key
        /// </summary>
        public KeyCode toggleKey;

        /// <summary>
        /// The display lines
        /// </summary>
        public bool displayLines = true;

#if UNITY_EDITOR
        public bool displayGizmos = true;
#endif
        //public ScreenRect rect = new ScreenRect (0, 0, 150, 20);

        /// <summary>
        /// The lines z on
        /// </summary>
        private List<Line> linesZOn = new List<Line>();

        /// <summary>
        /// The lines z off
        /// </summary>
        private List<Line> linesZOff = new List<Line>();

        //        private float milliseconds;

        /// <summary>
        /// The z on shader
        /// </summary>
        public Shader zOnShader;

        /// <summary>
        /// The z off shader
        /// </summary>
        public Shader zOffShader;

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        private void Awake()
        {
            if (instance)
            {
                DestroyImmediate(this);
                return;
            }
            instance = this;
            SetMaterial();

            if (null == this.GetComponent<Camera>())
            {
                Debug.LogError("There should be camera attached to the same game object");
            }
        }

        /// <summary>
        /// Sets the material.
        /// </summary>
        private void SetMaterial()
        {
            matZOn = new Material(this.zOnShader);
            matZOn.hideFlags = HideFlags.HideAndDontSave;
            //matZOn.shader.hideFlags = HideFlags.HideAndDontSave;

            matZOff = new Material(this.zOffShader);
            matZOff.hideFlags = HideFlags.HideAndDontSave;
            //matZOff.shader.hideFlags = HideFlags.HideAndDontSave;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
                displayLines = !displayLines;

            if (!displayLines)
            {
                //                        Stopwatch timer = Stopwatch.StartNew ();

                linesZOn = linesZOn.Where(l => !l.DurationElapsed(false)).ToList();
                linesZOff = linesZOff.Where(l => !l.DurationElapsed(false)).ToList();

                //                        timer.Stop ();
                //                        milliseconds = timer.Elapsed.Ticks / 10000f;
            }
        }

        /*void OnGUI ()
        {
                GUI.Label (rect, "GLDebug : " + milliseconds.ToString ("f") + " ms");
        }*/

#if UNITY_EDITOR
        void OnDrawGizmos ()
        {
                if (!displayGizmos || !Application.isPlaying)
                        return;
                for (int i = 0; i < linesZOn.Count; i++)
                {
                        Gizmos.color = linesZOn[i].color;
                        Gizmos.DrawLine (linesZOn[i].start, linesZOn[i].end);
                }
                for (int i = 0; i < linesZOff.Count; i++)
                {
                        Gizmos.color = linesZOff[i].color;
                        Gizmos.DrawLine (linesZOff[i].start, linesZOff[i].end);
                }
        }
#endif

        /// <summary>
        /// Called when [post render].
        /// </summary>
        private void OnPostRender()
        {
            if (!displayLines) return;

            //            Stopwatch timer = Stopwatch.StartNew ();

            matZOn.SetPass(0);
            GL.Begin(GL.LINES);
            linesZOn = linesZOn.Where(l => !l.DurationElapsed(true)).ToList();
            GL.End();

            matZOff.SetPass(0);
            GL.Begin(GL.LINES);
            linesZOff = linesZOff.Where(l => !l.DurationElapsed(true)).ToList();
            GL.End();

            //              timer.Stop ();
            //                milliseconds = timer.Elapsed.Ticks / 10000f;
        }

        /// <summary>
        /// Draws the line.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="depthTest">if set to <c>true</c> [depth test].</param>
        private static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0, bool depthTest = false)
        {
            if (duration == 0 && !instance.displayLines)
                return;
            if (start == end)
                return;
            if (depthTest)
                instance.linesZOn.Add(new Line(start, end, color, Time.time, duration));
            else
                instance.linesZOff.Add(new Line(start, end, color, Time.time, duration));
        }

        /// <summary>
        /// Draw a line from start to end with color for a duration of time and with or without depth testing.
        /// If duration is 0 then the line is rendered 1 frame.
        /// </summary>
        /// <param name="start">Point in world space where the line should start.</param>
        /// <param name="end">Point in world space where the line should end.</param>
        /// <param name="color">Color of the line.</param>
        /// <param name="duration">How long the line should be visible for.</param>
        /// <param name="depthTest">Should the line be obscured by objects closer to the camera ?</param>
        public static void DrawLine(Vector3 start, Vector3 end, Color? color = null, float duration = 0, bool depthTest = false)
        {
            DrawLine(start, end, color ?? Color.white, duration, depthTest);
        }

        /// <summary>
        /// Draw a line from start to start + dir with color for a duration of time and with or without depth testing.
        /// If duration is 0 then the ray is rendered 1 frame.
        /// </summary>
        /// <param name="start">Point in world space where the ray should start.</param>
        /// <param name="dir">Direction and length of the ray.</param>
        /// <param name="color">Color of the ray.</param>
        /// <param name="duration">How long the ray should be visible for.</param>
        /// <param name="depthTest">Should the ray be obscured by objects closer to the camera ?</param>
        public static void DrawRay(Vector3 start, Vector3 dir, Color? color = null, float duration = 0, bool depthTest = false)
        {
            if (dir == Vector3.zero)
                return;
            DrawLine(start, start + dir, color, duration, depthTest);
        }

        /// <summary>
        /// Draw an arrow from start to end with color for a duration of time and with or without depth testing.
        /// If duration is 0 then the arrow is rendered 1 frame.
        /// </summary>
        /// <param name="start">Point in world space where the arrow should start.</param>
        /// <param name="end">Point in world space where the arrow should end.</param>
        /// <param name="arrowHeadLength">Length of the 2 lines of the head.</param>
        /// <param name="arrowHeadAngle">Angle between the main line and each of the 2 smaller lines of the head.</param>
        /// <param name="color">Color of the arrow.</param>
        /// <param name="duration">How long the arrow should be visible for.</param>
        /// <param name="depthTest">Should the arrow be obscured by objects closer to the camera ?</param>
        public static void DrawLineArrow(Vector3 start, Vector3 end, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20, Color? color = null, float duration = 0, bool depthTest = false)
        {
            DrawArrow(start, end - start, arrowHeadLength, arrowHeadAngle, color, duration, depthTest);
        }

        /// <summary>
        /// Draw an arrow from start to start + dir with color for a duration of time and with or without depth testing.
        /// If duration is 0 then the arrow is rendered 1 frame.
        /// </summary>
        /// <param name="start">Point in world space where the arrow should start.</param>
        /// <param name="dir">Direction and length of the arrow.</param>
        /// <param name="arrowHeadLength">Length of the 2 lines of the head.</param>
        /// <param name="arrowHeadAngle">Angle between the main line and each of the 2 smaller lines of the head.</param>
        /// <param name="color">Color of the arrow.</param>
        /// <param name="duration">How long the arrow should be visible for.</param>
        /// <param name="depthTest">Should the arrow be obscured by objects closer to the camera ?</param>
        public static void DrawArrow(Vector3 start, Vector3 dir, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20, Color? color = null, float duration = 0, bool depthTest = false)
        {
            if (dir == Vector3.zero)
                return;
            DrawRay(start, dir, color, duration, depthTest);
            Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
            Vector3 left = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;
            DrawRay(start + dir, right * arrowHeadLength, color, duration, depthTest);
            DrawRay(start + dir, left * arrowHeadLength, color, duration, depthTest);
        }

        /// <summary>
        /// Draw a square with color for a duration of time and with or without depth testing.
        /// If duration is 0 then the square is renderer 1 frame.
        /// </summary>
        /// <param name="pos">Center of the square in world space.</param>
        /// <param name="rot">Rotation of the square in euler angles in world space.</param>
        /// <param name="scale">Size of the square.</param>
        /// <param name="color">Color of the square.</param>
        /// <param name="duration">How long the square should be visible for.</param>
        /// <param name="depthTest">Should the square be obscured by objects closer to the camera ?</param>
        public static void DrawSquare(Vector3 pos, Vector3? rot = null, Vector3? scale = null, Color? color = null, float duration = 0, bool depthTest = false)
        {
            DrawSquare(Matrix4x4.TRS(pos, Quaternion.Euler(rot ?? Vector3.zero), scale ?? Vector3.one), color, duration, depthTest);
        }

        /// <summary>
        /// Draw a square with color for a duration of time and with or without depth testing.
        /// If duration is 0 then the square is renderer 1 frame.
        /// </summary>
        /// <param name="pos">Center of the square in world space.</param>
        /// <param name="rot">Rotation of the square in world space.</param>
        /// <param name="scale">Size of the square.</param>
        /// <param name="color">Color of the square.</param>
        /// <param name="duration">How long the square should be visible for.</param>
        /// <param name="depthTest">Should the square be obscured by objects closer to the camera ?</param>
        public static void DrawSquare(Vector3 pos, Quaternion? rot = null, Vector3? scale = null, Color? color = null, float duration = 0, bool depthTest = false)
        {
            DrawSquare(Matrix4x4.TRS(pos, rot ?? Quaternion.identity, scale ?? Vector3.one), color, duration, depthTest);
        }

        /// <summary>
        /// Draw a square with color for a duration of time and with or without depth testing.
        /// If duration is 0 then the square is renderer 1 frame.
        /// </summary>
        /// <param name="matrix">Transformation matrix which represent the square transform.</param>
        /// <param name="color">Color of the square.</param>
        /// <param name="duration">How long the square should be visible for.</param>
        /// <param name="depthTest">Should the square be obscured by objects closer to the camera ?</param>
        public static void DrawSquare(Matrix4x4 matrix, Color? color = null, float duration = 0, bool depthTest = false)
        {
            Vector3
                    p_1 = matrix.MultiplyPoint3x4(new Vector3(.5f, 0, .5f)),
                    p_2 = matrix.MultiplyPoint3x4(new Vector3(.5f, 0, -.5f)),
                    p_3 = matrix.MultiplyPoint3x4(new Vector3(-.5f, 0, -.5f)),
                    p_4 = matrix.MultiplyPoint3x4(new Vector3(-.5f, 0, .5f));

            DrawLine(p_1, p_2, color, duration, depthTest);
            DrawLine(p_2, p_3, color, duration, depthTest);
            DrawLine(p_3, p_4, color, duration, depthTest);
            DrawLine(p_4, p_1, color, duration, depthTest);
        }

        /// <summary>
        /// Draw a cube with color for a duration of time and with or without depth testing.
        /// If duration is 0 then the square is renderer 1 frame.
        /// </summary>
        /// <param name="pos">Center of the cube in world space.</param>
        /// <param name="rot">Rotation of the cube in euler angles in world space.</param>
        /// <param name="scale">Size of the cube.</param>
        /// <param name="color">Color of the cube.</param>
        /// <param name="duration">How long the cube should be visible for.</param>
        /// <param name="depthTest">Should the cube be obscured by objects closer to the camera ?</param>
        public static void DrawCube(Vector3 pos, Vector3? rot = null, Vector3? scale = null, Color? color = null, float duration = 0, bool depthTest = false)
        {
            DrawCube(Matrix4x4.TRS(pos, Quaternion.Euler(rot ?? Vector3.zero), scale ?? Vector3.one), color, duration, depthTest);
        }

        /// <summary>
        /// Draw a cube with color for a duration of time and with or without depth testing.
        /// If duration is 0 then the square is renderer 1 frame.
        /// </summary>
        /// <param name="pos">Center of the cube in world space.</param>
        /// <param name="rot">Rotation of the cube in world space.</param>
        /// <param name="scale">Size of the cube.</param>
        /// <param name="color">Color of the cube.</param>
        /// <param name="duration">How long the cube should be visible for.</param>
        /// <param name="depthTest">Should the cube be obscured by objects closer to the camera ?</param>
        public static void DrawCube(Vector3 pos, Quaternion? rot = null, Vector3? scale = null, Color? color = null, float duration = 0, bool depthTest = false)
        {
            DrawCube(Matrix4x4.TRS(pos, rot ?? Quaternion.identity, scale ?? Vector3.one), color, duration, depthTest);
        }

        /// <summary>
        /// Draw a cube with color for a duration of time and with or without depth testing.
        /// If duration is 0 then the square is renderer 1 frame.
        /// </summary>
        /// <param name="matrix">Transformation matrix which represent the cube transform.</param>
        /// <param name="color">Color of the cube.</param>
        /// <param name="duration">How long the cube should be visible for.</param>
        /// <param name="depthTest">Should the cube be obscured by objects closer to the camera ?</param>
        public static void DrawCube(Matrix4x4 matrix, Color? color = null, float duration = 0, bool depthTest = false)
        {
            Vector3
                    down_1 = matrix.MultiplyPoint3x4(new Vector3(.5f, -.5f, .5f)),
                    down_2 = matrix.MultiplyPoint3x4(new Vector3(.5f, -.5f, -.5f)),
                    down_3 = matrix.MultiplyPoint3x4(new Vector3(-.5f, -.5f, -.5f)),
                    down_4 = matrix.MultiplyPoint3x4(new Vector3(-.5f, -.5f, .5f)),
                    up_1 = matrix.MultiplyPoint3x4(new Vector3(.5f, .5f, .5f)),
                    up_2 = matrix.MultiplyPoint3x4(new Vector3(.5f, .5f, -.5f)),
                    up_3 = matrix.MultiplyPoint3x4(new Vector3(-.5f, .5f, -.5f)),
                    up_4 = matrix.MultiplyPoint3x4(new Vector3(-.5f, .5f, .5f));

            DrawLine(down_1, down_2, color, duration, depthTest);
            DrawLine(down_2, down_3, color, duration, depthTest);
            DrawLine(down_3, down_4, color, duration, depthTest);
            DrawLine(down_4, down_1, color, duration, depthTest);

            DrawLine(down_1, up_1, color, duration, depthTest);
            DrawLine(down_2, up_2, color, duration, depthTest);
            DrawLine(down_3, up_3, color, duration, depthTest);
            DrawLine(down_4, up_4, color, duration, depthTest);

            DrawLine(up_1, up_2, color, duration, depthTest);
            DrawLine(up_2, up_3, color, duration, depthTest);
            DrawLine(up_3, up_4, color, duration, depthTest);
            DrawLine(up_4, up_1, color, duration, depthTest);
        }

        // EXTRAS
        public static void DrawCircle(Vector3 center, float radius, Color? color = null, float duration = 0, bool depthTest = false)
        {
            //            float degRad = Mathf.PI / 180;
            for (float theta = 0.0f; theta < (2 * Mathf.PI); theta += 0.2f)
            {
                Vector3 ci = (new Vector3(Mathf.Cos(theta) * radius + center.x, Mathf.Sin(theta) * radius + center.y, center.z));
                DrawLine(ci, ci + new Vector3(0, 0.02f, 0), color, duration, depthTest);
            }
        }
    }
}