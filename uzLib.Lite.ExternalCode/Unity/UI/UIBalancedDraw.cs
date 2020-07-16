using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using uzLib.Lite.ExternalCode.Extensions;

#if !(!UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5)
using uzLib.Lite.Extensions;
#endif

namespace UnityEngine.UI
{
    /// <summary>
    ///     The UI Balancer (used to call several blocks of the OnGUI method at different update rates (frequencies))
    /// </summary>
    [Obsolete]
    public class UIBalancedDraw
    {
        /// <summary>
        ///     The debug
        /// </summary>
        private const bool DEBUG = false;

        /// <summary>
        ///     The null compiled action message
        /// </summary>
        private const string NullCompiledActionMessage = "Compiled action can't be null!";

        /// <summary>
        ///     The main action name
        /// </summary>
        private const string MainActionName = "MAIN_ACTION";

        /// <summary>
        ///     The actions
        /// </summary>
        private Dictionary<string, Action<object[]>> m_actions;

        /// <summary>
        ///     The draw once
        /// </summary>
        private bool m_drawOnce;

        /// <summary>
        ///     The extenders
        /// </summary>
        private Dictionary<string, ExpressionExtender> m_extenders;

        /// <summary>
        ///     The repaint counter
        /// </summary>
        private int m_frameCounter;

        /// <summary>
        ///     Is ready?
        /// </summary>
        private bool m_isReady;

        /// <summary>
        ///     Is repaint triggered?
        /// </summary>
        private bool m_layoutTriggered;

        /// <summary>
        ///     The predicates
        /// </summary>
        private readonly Dictionary<string, Func<bool>> m_predicates;

        /// <summary>
        ///     The timer
        /// </summary>
        private float m_timer;

        /// <summary>
        ///     Prevents a default instance of the <see cref="UIBalancedDraw" /> class from being created.
        /// </summary>
        private UIBalancedDraw()
        {
            m_actions = new Dictionary<string, Action<object[]>>();
            m_extenders = new Dictionary<string, ExpressionExtender>();
            m_predicates = new Dictionary<string, Func<bool>>();

            m_timer = Time.realtimeSinceStartup;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UIBalancedDraw" /> class.
        /// </summary>
        /// <param name="eachXFrames">The each x frames.</param>
        public UIBalancedDraw(int eachXFrames)
            : this()
        {
            EachXFrames = eachXFrames;
        }

        /// <summary>
        ///     Gets the each repaints.
        /// </summary>
        /// <value>
        ///     The each repaints.
        /// </value>
        public int EachXFrames { get; }

        /// <summary>
        ///     Gets or sets the awaiting signal.
        /// </summary>
        /// <value>
        ///     The awaiting signal.
        /// </value>
        public Action AwaitingSignal { get; set; }

        public void Setup(Action<Action> drawAction)
        {
            Setup(drawAction, 0);
        }

        public void Setup(Action<Action> drawAction, int depth)
        {
            if (drawAction == null) throw new ArgumentNullException(nameof(drawAction));

            // Add the draw action
            m_actions.Add(MainActionName, objs => drawAction(objs.GetValue<Action>(0)));
            m_extenders.Add(MainActionName, new ExpressionExtender(MainActionName, depth));
        }

        /// <summary>
        ///     Draws the specified draw action.
        /// </summary>
        /// <param name="drawAction">The draw action.</param>
        /// <param name="update">if set to <c>true</c> [update].</param>
        /// <exception cref="ArgumentNullException">drawAction</exception>
        public void Draw()
        {
            var e = Event.current;
            EventType? typeAwaiter = EventType.Layout;

            // Review: In play mode this will have an unexpected behaviour (try to adapt)
            // In any case, we need to call directly AddInList callbacks when isEditor flag is false (I think this is already implemented)

            if (!m_drawOnce)
            {
                if (typeAwaiter.HasValue && e.rawType == typeAwaiter || !typeAwaiter.HasValue)
                {
                    // We need to call the drawAction once to add the rest of the actions
                    // Then we can continue by sorting them
                    m_actions[MainActionName](new[] { new Action(ExecuteWhenLoaded) });

                    // Exit draw once block
                    m_drawOnce = true;
                }

                if (!m_drawOnce)
                {
                    if (DEBUG) Debug.Log($"Awaiting for {typeAwaiter.ToString()} event.");

                    return;
                }
            }

            AwaitingSignal?.Invoke();

            if (!m_isReady) return;

            // Note: Can't use GUI.depth approach because this is used between different MonoBehaviour's OnGUI methods
            try
            {
                foreach (var kv in m_actions)
                    try
                    {
                        var storedPredicate = m_predicates.GetValue(kv.Key);

                        if (storedPredicate == null || storedPredicate != null && storedPredicate())
                        {
                            var extender = m_extenders.GetValue(kv.Key);

                            // If we are on the balanced method then...
                            if (kv.Key == MainActionName)
                            {
                                if (DEBUG) Debug.Log($"Iteration: {m_frameCounter} || Type: {e.rawType}");

                                // Declare the continue flag
                                var skipThisFrame = true;

                                // We will execute this when Layout or Repaint is executed
                                if (e.rawType == EventType.Layout || e.rawType == EventType.Repaint)
                                {
                                    // Sum the counter only if the condition isn't met
                                    if (m_frameCounter < EachXFrames) ++m_frameCounter;

                                    // So, if repaint counter is above EachXFrames
                                    // For example, if EachXFrames is 20, we will execute this block on iteration 20 (21 with the increment) and 21 (22 with the increment)
                                    if (m_frameCounter >= EachXFrames)
                                    {
                                        // If the layout was already executed, then execute this block also...
                                        if (m_layoutTriggered)
                                        {
                                            if (DEBUG)
                                                Debug.Log(
                                                    $"Has passed {(Time.realtimeSinceStartup - m_timer) * 1000f:F0} ms since the last {EachXFrames} repaints.");

                                            // Reset values
                                            m_frameCounter = 0;
                                            m_timer = Time.realtimeSinceStartup;
                                            m_layoutTriggered = false;

                                            // We are now on Repaint so we won't skip this frame neither
                                            skipThisFrame = false;
                                        }

                                        // We need to check that Layout is triggered and ensure that the repaint counter wasn't already reset (to avoid infinite calls)
                                        if (e.rawType == EventType.Layout && m_frameCounter != 0)
                                        {
                                            // We won't skip this frame
                                            skipThisFrame = false;

                                            // We set the flag to true to allow repaint being called also
                                            m_layoutTriggered = true;
                                        }
                                    }
                                }

                                if (skipThisFrame) continue;
                            }

                            // Test 3
                            kv.Value(extender?.Objects);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                    }
            }
            catch
            {
                // Due to recompile the collection will modified, so we need to avoid the exception
            }
        }

        /// <summary>
        ///     Executes this method when the (anything) is loaded.
        /// </summary>
        private void ExecuteWhenLoaded()
        {
            if (DEBUG)
                Debug.Log(
                    "Before sorting:" +
                    Environment.NewLine +
                    string.Join(Environment.NewLine, m_actions.Keys.Select((k, i) => $"{Substring(k, 15)} -- {i}")));

            // Order everything depending on its depth (lower values first, because we need that higher ones (-1 or 0) draw last)
            var orderedExtenders = m_extenders.OrderByDescending(ext => ext.Value.Depth)
                .ToDictionary(t => t.Key, t => t.Value);

            if (DEBUG) Debug.Log($"Are equal?: {DictionaryHelper.AreEqual(orderedExtenders, m_extenders)}");

            m_extenders = orderedExtenders;

            // Sort actions based on ordered extenders
            var tempActions = new Dictionary<string, Action<object[]>>();
            var debugList = new List<int>();

            m_extenders.ForEach(ext =>
            {
                tempActions.Add(ext.Key, m_actions[ext.Key]);

                if (DEBUG) debugList.Add(m_actions.IndexOfKey(ext.Key));
            });
            m_actions = tempActions; // Will this cause problems?

            if (DEBUG)
                Debug.Log(
                    "After sorting:" +
                    Environment.NewLine +
                    string.Join(Environment.NewLine,
                        m_actions.Keys.Select((k, i) => $"{Substring(k, 15)} -- {debugList[i]}")));

            m_isReady = true;
        }

        private string Substring(string str, int length)
        {
            return str.Length > length ? str.Substring(0, length) : str;
        }

        /// <summary>
        ///     Adds the in queue.
        /// </summary>
        /// <param name="balancedDraw">The balanced draw.</param>
        /// <param name="action">The action.</param>
        /// <param name="isEditor">if set to <c>true</c> [is editor].</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="extender">The extender.</param>
        /// <param name="storedPredicate">The stored predicate.</param>
        /// <exception cref="ArgumentNullException">predicate</exception>
        public static void AddInQueue(UIBalancedDraw balancedDraw, Expression<Action<object[]>> action, bool isEditor,
            Func<bool> predicate, ExpressionExtender extender = null, Func<bool> storedPredicate = null)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            if (predicate())
                AddInQueue(balancedDraw, action, isEditor, extender, storedPredicate);
            else
                ExecuteCompiledAction(action, extender);
        }

        /// <summary>
        ///     Adds the in queue.
        /// </summary>
        /// <param name="balancedDraw">The balanced draw.</param>
        /// <param name="action">The action.</param>
        /// <param name="isEditor">if set to <c>true</c> [is editor].</param>
        /// <param name="extender">The extender.</param>
        /// <param name="storedPredicate">The stored predicate.</param>
        /// <exception cref="ArgumentNullException">action</exception>
        public static void AddInQueue(UIBalancedDraw balancedDraw, Expression<Action<object[]>> action, bool isEditor,
            ExpressionExtender extender = null, Func<bool> storedPredicate = null)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            // If Editor, add once the actions to list
            // Else, call them (draw it)

            if (storedPredicate != null) AddStoredPredicate(balancedDraw, action, extender, storedPredicate);

            if (isEditor)
                Add(balancedDraw, action, extender);
            else
                ExecuteCompiledAction(action, extender);
        }

        /// <summary>
        ///     Executes the compiled action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="extender">The extender.</param>
        /// <exception cref="ArgumentNullException"></exception>
        private static void ExecuteCompiledAction(Expression<Action<object[]>> action, ExpressionExtender extender)
        {
            var compiledAction = action.Compile();

            if (compiledAction == null) throw new ArgumentNullException(NullCompiledActionMessage);

            compiledAction(extender?.Objects);
        }

        /// <summary>
        ///     Adds the specified balanced draw.
        /// </summary>
        /// <param name="balancedDraw">The balanced draw.</param>
        /// <param name="action">The action.</param>
        /// <param name="extender">The extender.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        private static bool Add(UIBalancedDraw balancedDraw, Expression<Action<object[]>> action,
            ExpressionExtender extender)
        {
            var key = GetKey(action, extender);
            if (balancedDraw.m_actions.ContainsKey(key)) return false;

            var compiledAction = action.Compile();

            if (compiledAction == null) throw new NullReferenceException(NullCompiledActionMessage);

            balancedDraw.m_actions.Add(key, compiledAction);
            balancedDraw.m_extenders.Add(key, extender);

            return true;
        }

        /// <summary>
        ///     Adds the stored predicate.
        /// </summary>
        /// <param name="balancedDraw">The balanced draw.</param>
        /// <param name="action">The action.</param>
        /// <param name="extender">The extender.</param>
        /// <param name="storedPredicate">The stored predicate.</param>
        /// <returns></returns>
        private static bool AddStoredPredicate(UIBalancedDraw balancedDraw, Expression<Action<object[]>> action,
            ExpressionExtender extender, Func<bool> storedPredicate)
        {
            var key = GetKey(action, extender);
            if (balancedDraw.m_predicates.ContainsKey(key)) return false;

            balancedDraw.m_predicates.Add(key, storedPredicate);

            return true;
        }

        /// <summary>
        ///     Gets the key.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="extender">The extender.</param>
        /// <returns></returns>
        private static string GetKey(Expression<Action<object[]>> action, ExpressionExtender extender)
        {
            return action + extender?.Salt;
        }

        /// <summary>
        ///     The Expression Extender (used to specify more aguments between calls)
        /// </summary>
        public class ExpressionExtender
        {
            /// <summary>
            ///     Prevents a default instance of the <see cref="ExpressionExtender" /> class from being created.
            /// </summary>
            private ExpressionExtender()
            {
                Depth = 1;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="ExpressionExtender" /> class.
            /// </summary>
            /// <param name="salt">The salt.</param>
            /// <param name="objs">The objs.</param>
            public ExpressionExtender(string salt, params object[] objs)
                : this()
            {
                Salt = salt;
                Objects = objs;

                DebugMethod();
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="ExpressionExtender" /> class.
            /// </summary>
            /// <param name="salt">The salt.</param>
            /// <param name="depth">The depth.</param>
            /// <param name="objs">The objs.</param>
            public ExpressionExtender(string salt, int depth, params object[] objs)
                : this()
            {
                Salt = salt;
                Depth = depth;
                Objects = objs;

                DebugMethod();
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="ExpressionExtender" /> class.
            /// </summary>
            /// <param name="salt">The salt.</param>
            /// <param name="depth">The depth.</param>
            public ExpressionExtender(string salt, int depth)
                : this()
            {
                Salt = salt;
                Depth = depth;

                DebugMethod();
            }

            /// <summary>
            ///     Gets the salt.
            /// </summary>
            /// <value>
            ///     The salt.
            /// </value>
            public string Salt { get; }

            /// <summary>
            ///     Gets the depth.
            /// </summary>
            /// <value>
            ///     The depth.
            /// </value>
            public int Depth { get; }

            /// <summary>
            ///     Gets the objects.
            /// </summary>
            /// <value>
            ///     The objects.
            /// </value>
            public object[] Objects { get; }

            private void DebugMethod()
            {
                if (!DEBUG) return;

                Debug.Log($"Added extender '{Salt}' with depth {Depth}");
            }
        }
    }
}