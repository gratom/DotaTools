#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;

namespace Tools
{

    [CustomPropertyDrawer(typeof(SDateTime))]
    public class SDateTimePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Rect yearRect = new Rect(position.x, position.y, 40, position.height);
            Rect monthRect = new Rect(position.x + 45, position.y, 30, position.height);
            Rect dayRect = new Rect(position.x + 80, position.y, 30, position.height);
            Rect hourRect = new Rect(position.x + 115, position.y, 30, position.height);
            Rect minuteRect = new Rect(position.x + 150, position.y, 30, position.height);
            Rect secondRect = new Rect(position.x + 185, position.y, 30, position.height);

            // Get the SDateTime object
            SerializedProperty sValueProperty = property.FindPropertyRelative("sValue");
            long sValue = sValueProperty.longValue;
            DateTime dateTime = new DateTime(sValue);

            // Draw fields - pass GUIContent.none to each so they don't have labels
            int year = EditorGUI.IntField(yearRect, dateTime.Year);
            int month = EditorGUI.IntField(monthRect, dateTime.Month);
            int day = EditorGUI.IntField(dayRect, dateTime.Day);
            int hour = EditorGUI.IntField(hourRect, dateTime.Hour);
            int minute = EditorGUI.IntField(minuteRect, dateTime.Minute);
            int second = EditorGUI.IntField(secondRect, dateTime.Second);

            // Update the sValue if changed
            try
            {
                DateTime newDateTime = new DateTime(year, month, day, hour, minute, second);
                sValueProperty.longValue = newDateTime.Ticks;
            }
            catch (ArgumentOutOfRangeException)
            {
                // Handle invalid date/time values
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
#endif