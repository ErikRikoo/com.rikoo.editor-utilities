using System;
using System.Reflection;

namespace EditorUtilities.Editor.Extensions.TypeSystemUtilities
{
    public abstract class InstanceField
    {
        public object Value => GetValue<object>();
        
        public abstract T GetValue<T>()
            where T : class;
        
        public abstract void SetValue<T>(T newValue);
    }

    public class InstanceWithNoField : InstanceField
    {
        public object Instance;

        public override T GetValue<T>()
        {
            return Instance as T;
        }

        public override void SetValue<T>(T newValue) {}
    }

    public class DirectInstanceField : InstanceField
    {
        public FieldInfo Info;
        public object Instance;

        public override T GetValue<T>()
        {
            return Info.GetValue(Instance) as T;
        }

        public override void SetValue<T>(T newValue)
        {
            Info.SetValue(Instance, newValue);
        }
    }

    public class ArrayInstanceField : InstanceField
    {
        public Array Instance;
        public int Index;
        
        public override T GetValue<T>()
        {
            return Instance.GetValue(Index) as T;
        }

        public override void SetValue<T>(T newValue)
        {
            Instance.SetValue(newValue, Index);
        }
    }
}