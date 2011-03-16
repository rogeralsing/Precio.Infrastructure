using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Precio.Serialization
{
    /// <summary>
    /// This class offers the ability to save the fields
    /// of types that don't have the <c ref="System.SerializableAttribute">SerializableAttribute</c>.
    /// </summary>
    public class NonSerialiazableTypeSurrogateSelector : ISerializationSurrogate, ISurrogateSelector
    {
        private ISurrogateSelector _nextSelector;

        #region ISerializationSurrogate Members

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            FieldInfo[] fieldInfos =
                obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo fi in fieldInfos)
            {
                if (IsKnownType(fi.FieldType))
                {
                    info.AddValue(fi.Name, fi.GetValue(obj));
                }
                else
                {
                    if (fi.FieldType.IsClass)
                    {
                        info.AddValue(fi.Name, fi.GetValue(obj));
                    }
                }
            }
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context,
                                    ISurrogateSelector selector)
        {
            FieldInfo[] fieldInfos =
                obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo fi in fieldInfos)
            {
                if (IsKnownType(fi.FieldType))
                {
                    //var value = info.GetValue(fi.Name, fi.FieldType);

                    if (IsNullableType(fi.FieldType))
                    {
                        // Nullable<argumentValue>
                        Type argumentValueForTheNullableType = GetFirstArgumentOfGenericType(fi.FieldType);
                        //fi.FieldType.GetGenericArguments()[0];
                        fi.SetValue(obj, info.GetValue(fi.Name, argumentValueForTheNullableType));
                    }
                    else
                    {
                        fi.SetValue(obj, info.GetValue(fi.Name, fi.FieldType));
                    }
                }
                else if (fi.FieldType.IsClass)
                {
                    fi.SetValue(obj, info.GetValue(fi.Name, fi.FieldType));
                }
            }

            return obj;
        }

        #endregion

        #region ISurrogateSelector Members

        public void ChainSelector(ISurrogateSelector selector)
        {
            _nextSelector = selector;
        }

        public ISurrogateSelector GetNextSelector()
        {
            return _nextSelector;
        }

        public ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector)
        {
            if (IsKnownType(type))
            {
                selector = null;
                return null;
            }
            else if (type.IsClass || type.IsValueType)
            {
                selector = this;
                return this;
            }
            else
            {
                selector = null;
                return null;
            }
        }

        #endregion

        private bool IsKnownType(Type type)
        {
            return
                type == typeof (string)
                || type.IsPrimitive
                || type.IsSerializable
                ;
        }

        private Type GetFirstArgumentOfGenericType(Type type)
        {
            return type.GetGenericArguments()[0];
        }

        private bool IsNullableType(Type type)
        {
            if (type.IsGenericType)
                return type.GetGenericTypeDefinition() == typeof (Nullable<>);
            return false;
        }
    }
}