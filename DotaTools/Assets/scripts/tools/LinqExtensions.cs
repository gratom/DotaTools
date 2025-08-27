using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace System
{
    public static class LinqExtensions
    {
        public static T ElementWithMin<T>(this IEnumerable<T> source, Func<T, float> selector)
        {
            if (source == null || !source.Any())
            {
                throw new ArgumentException("The source collection is null or empty.");
            }

            float min = float.MaxValue;
            T element = default;

            foreach (T item in source)
            {
                float value = selector(item);
                if (value < min)
                {
                    min = value;
                    element = item;
                }
            }

            return element;
        }

        public static T ElementWithMax<T>(this IEnumerable<T> source, Func<T, float> selector)
        {
            if (source == null || !source.Any())
            {
                throw new ArgumentException("The source collection is null or empty.");
            }

            float max = float.MinValue;
            T element = default;

            foreach (T item in source)
            {
                float value = selector(item);
                if (value > max)
                {
                    max = value;
                    element = item;
                }
            }

            return element;
        }

        public static T RandomWeightedElement<T>(this IList<T> elements, Func<T, float> weightFunc, out int index, T def = default)
        {
            if (elements.Count == 0)
            {
                index = -1;
                return def;
            }
            if (elements.Count == 1)
            {
                index = 0;
                return elements[0];
            }

            float sum = elements.Sum(t => Math.Max(weightFunc(t), 0));

            if (sum == 0)
            {
                index = -1;
                return def;
            }

            float rand = UnityEngine.Random.Range(0f, sum);
            int selectedInd = -1;
            for (int i = 0; i < elements.Count; i++)
            {
                float weight = Math.Max(weightFunc(elements[i]), 0);

                if (rand < weight)
                {
                    selectedInd = i;
                    break;
                }
                rand -= weight;
            }
            if (selectedInd == -1)
            {
                throw new Exception("wtf");
            }
            index = selectedInd;
            return elements[selectedInd];
        }

        public static string Debug<T>(this IEnumerable<T> source, Func<T, string> selector = null, string prefix = "", bool autolog = true)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (selector == null)
            {
                selector = element => element.ToString();
            }
            string result = prefix + "\n" + string.Join("\n", source.Select(selector));
            if (autolog)
            {
                UnityEngine.Debug.Log(result);
            }
            return result;
        }

#if UNITY_EDITOR
        public static List<string> GetClipboardLines()
        {
            string clipboardText = EditorGUIUtility.systemCopyBuffer;

            if (string.IsNullOrEmpty(clipboardText))
            {
                return new List<string>();
            }

            string[] lines = clipboardText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return new List<string>(lines);
        }
#endif

    }
}