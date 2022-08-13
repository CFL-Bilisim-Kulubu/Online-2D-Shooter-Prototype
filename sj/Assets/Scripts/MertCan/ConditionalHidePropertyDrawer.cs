#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionalShow))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalShow csAttribute = (ConditionalShow) attribute;
        bool enabled = GetConditionalHideAttributeResult(csAttribute, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        if (!csAttribute.HideInInspector || enabled)
            EditorGUI.PropertyField(position, property, label, true);

        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalShow csAttribute = (ConditionalShow) attribute;
        bool enabled = GetConditionalHideAttributeResult(csAttribute, property);

        if (!csAttribute.HideInInspector || enabled)
            return EditorGUI.GetPropertyHeight(property, label);
        else
            return -EditorGUIUtility.standardVerticalSpacing;
    }

    private bool GetConditionalHideAttributeResult(ConditionalShow condHAtt, SerializedProperty property)
    {
        bool enabled = false;
        string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
        string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField); //changes the path to the conditionalsource property path
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        // Eğer aldığımız şey enumsa
        if (sourcePropertyValue.propertyType == SerializedPropertyType.Enum)
        {
            if (condHAtt.EnumInt.Length > 0)
            {
                if (sourcePropertyValue != null)
                {
                    enabled = false;

                    for (int i = 0; i < condHAtt.EnumInt.Length; i++)
                    {
                        if (sourcePropertyValue.enumValueIndex == condHAtt.EnumInt[i])
                            enabled = true;
                    }
                }
                else
                    Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
            }
        }
        // Eğer aldığımız şey boolsa
        else if (sourcePropertyValue.propertyType == SerializedPropertyType.Boolean)
        {
            if (sourcePropertyValue != null)
                enabled = sourcePropertyValue.boolValue;
            else
                Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
        }


        return enabled;
    }
}
#endif