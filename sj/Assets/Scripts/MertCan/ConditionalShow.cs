using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalShow : PropertyAttribute
{
    //The name of the bool field that will be in control
    public string ConditionalSourceField;
    //TRUE = Hide in inspector / FALSE = Disable in inspector 
    public bool HideInInspector = false;

    public int[] EnumInt;

    public ConditionalShow(string conditionalSourceField)
    {
        ConditionalSourceField = conditionalSourceField;
        HideInInspector = false;
        EnumInt = null;
    }

    public ConditionalShow(string conditionalSourceField, bool hideInInspector)
    {
        ConditionalSourceField = conditionalSourceField;
        HideInInspector = hideInInspector;
        EnumInt = null;
    }
    public ConditionalShow(string conditionalSourceField, params int[] _enum)
    {
        ConditionalSourceField = conditionalSourceField;
        HideInInspector = true;
        EnumInt = _enum;
    }
}