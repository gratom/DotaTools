using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static class ComponentExtensions
    {
        public static bool TryGetComponentInChildren<T>(this Component component, out T result, bool includeInactive = false) where T : Component
        {
            result = component.GetComponentInChildren<T>(includeInactive);
            return result != null;
        }

        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T result, bool includeInactive = false) where T : Component
        {
            result = gameObject.GetComponentInChildren<T>(includeInactive);
            return result != null;
        }

        public static bool TryGetComponentInParent<T>(this Component component, out T result, bool includeInactive = false) where T : Component
        {
            result = component.GetComponentInParent<T>(includeInactive);
            return result != null;
        }

        public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T result, bool includeInactive = false) where T : Component
        {
            result = gameObject.GetComponentInParent<T>(includeInactive);
            return result != null;
        }

        public static List<GameObject> GetListOfChildren(this GameObject obj)
        {
            return obj.transform.GetListOfChildren();
        }

        public static List<GameObject> GetListOfChildren(this Component component)
        {
            return component.transform.GetListOfChildren();
        }

        public static List<GameObject> GetListOfChildren(this Transform parent)
        {
            List<GameObject> children = new List<GameObject>();
            for (int i = 0; i < parent.childCount; i++)
            {
                children.Add(parent.GetChild(i).gameObject);
            }
            return children;
        }

        public static void DestroyAllChildGameObjectsImmediately(this GameObject parent)
        {
            if (parent == null)
            {
                return;
            }

            for (int i = parent.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = parent.transform.GetChild(i);
                Object.DestroyImmediate(child.gameObject);
            }
        }

        public static bool TryDestroyFirstChild(this GameObject gameObject)
        {
            if (gameObject.transform.childCount > 0)
            {
                Transform firstChild = gameObject.transform.GetChild(0);
                if (Application.isPlaying)
                {
                    Object.Destroy(firstChild.gameObject);
                }
                else
                {
                    Object.DestroyImmediate(firstChild.gameObject);
                }
                return true;
            }
            return false;
        }

    }

}