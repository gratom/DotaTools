#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class DebugSelection
{
    [MenuItem("Assets/Debug Selected Object Names")]
    private static void DebugSelectedNames()
    {
        // Get all selected objects in the editor
        Object[] selectedObjects = Selection.objects;

        // Create a string to store names
        string names = "Selected Object Names: \n";

        // Loop through each selected object and append its name to the string
        foreach (Object obj in selectedObjects)
        {
            names += obj.name + ",\n";
        }

        // Output the concatenated names to the console
        Debug.Log(names);
    }
}

#endif