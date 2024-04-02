using UnityEngine;

public class ShowWhenAttribute : PropertyAttribute
{
    public string ConditionalPropertyName { get; }

    public ShowWhenAttribute(string conditionalPropertyName)
    {
        ConditionalPropertyName = conditionalPropertyName;
    }
}
