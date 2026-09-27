using System;
namespace Utilities
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DefaultInitStaticField : Attribute
    {
    }
}