using UnityEngine;

public class SubclassSelectorAttribute : PropertyAttribute
{
    private bool includeMono;

    public SubclassSelectorAttribute(bool includeMono = false)
    {
        this.includeMono = includeMono;
    }

    public bool IsIncludeMono() => includeMono;
}
