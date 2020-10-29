using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ControllerTest
{
    public static class Helper
    {
        //Methode gekopieerd van https://wrightfully.com/mocking-objects-with-restricted-access-members
        private static FieldInfo GetFieldReference(Type targetType, string fieldName)
        {
            FieldInfo field = targetType.GetField(fieldName,
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance);

            if (field == null && targetType.BaseType != null)
            {
                //if the field isn't actually on the type we're working on, rather it's
                //defined in a base class as private, it won't be returned in the above call,
                //so we have to walk the type hierarchy until we find it.
                // See: http://agsmith.wordpress.com/2007/12/13/where-are-my-fields/

                return GetFieldReference(targetType.BaseType, fieldName);

            }
            return field;
        }

        public static T GetPrivate<T>(object ob, string fieldName)
        {
            FieldInfo field = GetFieldReference(ob.GetType(), fieldName);
            return (T)field.GetValue(ob);
        }
    }
}
