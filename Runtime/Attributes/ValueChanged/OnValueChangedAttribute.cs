using UnityEngine;

namespace Attributes.ValueChanged
{
    public class OnValueChangedAttribute : PropertyAttribute
    {
        public readonly string Method;

        public readonly bool CallOnlyWhenModificationOver;
        
        public OnValueChangedAttribute(string _method, bool callOnlyWhenModificationOver = true)
        {
            Method = _method;
            CallOnlyWhenModificationOver = callOnlyWhenModificationOver;
        }
    }
}