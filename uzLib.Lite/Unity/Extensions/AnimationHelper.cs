using UnityEngine;

namespace uzLib.Lite.Unity.Extensions
{
    /// <summary>
    /// The AnimationHelper class
    /// </summary>
    public static class AnimationHelper
    {
        /// <summary>
        /// Gets the time perc.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public static float GetTimePerc(this AnimationState state)
        {
            return state.time / state.length;
        }

        /// <summary>
        /// Sets the time perc.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="perc">The perc.</param>
        public static void SetTimePerc(this AnimationState state, float perc)
        {
            state.time = state.length * perc;
        }
    }
}