using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Extensions
{
    /// <summary>
    ///     The Queue Helper
    /// </summary>
    public static class QueueHelper
    {
        /// <summary>
        ///     Removes the specified item to remove.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue">The queue.</param>
        /// <param name="itemToRemove">The item to remove.</param>
        public static void Remove<T>(this Queue<T> queue, T itemToRemove) where T : class
        {
            var list = queue.ToList(); //Needs to be copy, so we can clear the queue
            queue.Clear();
            foreach (var item in list)
            {
                if (item == itemToRemove) continue;

                queue.Enqueue(item);
            }
        }

        /// <summary>
        ///     Removes at.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue">The queue.</param>
        /// <param name="itemIndex">Index of the item.</param>
        public static void RemoveAt<T>(this Queue<T> queue, int itemIndex)
        {
            var cycleAmount = queue.Count;

            for (var i = 0; i < cycleAmount; i++)
            {
                var item = queue.Dequeue();
                if (i == itemIndex) continue;

                queue.Enqueue(item);
            }
        }

        /// <summary>
        ///     Removes the specified item to remove.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue">The queue.</param>
        /// <param name="itemToRemove">The item to remove.</param>
        public static void Remove<T>(this ConcurrentQueue<T> queue, T itemToRemove) where T : class
        {
            var list = queue.ToList(); //Needs to be copy, so we can clear the queue
            queue = new ConcurrentQueue<T>();
            foreach (var item in list)
            {
                if (item == itemToRemove) continue;

                queue.Enqueue(item);
            }
        }

        /// <summary>
        ///     Removes at.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue">The queue.</param>
        /// <param name="itemIndex">Index of the item.</param>
        /// <exception cref="Exception">Can't dequeue from ConcurrentQueue at this moment!</exception>
        public static void RemoveAt<T>(this ConcurrentQueue<T> queue, int itemIndex)
        {
            var cycleAmount = queue.Count;

            for (var i = 0; i < cycleAmount; i++)
            {
                var dequeued = queue.TryDequeue(out var item);

                if (!dequeued) throw new Exception("Can't dequeue from ConcurrentQueue at this moment!");

                if (i == itemIndex) continue;

                queue.Enqueue(item);
            }
        }
    }
}