using System;
using System.Linq;
using System.Reflection;

namespace ToDoList.EntityFramework
{
	internal static class ReflectionExtensions
	{
		public static bool IsGenericType(this TypeInfo typeInfo, Type genericType)
			=> typeInfo != null && typeInfo.IsGenericType && !typeInfo.IsGenericTypeDefinition && typeInfo.GetGenericTypeDefinition() == genericType;

		public static bool IsGenericType(this Type type, Type genericType)
		{
			if (type == null)
			{
				return false;
			}
			return IsGenericType(type.GetTypeInfo(), genericType);
		}

		public static IOrderedEnumerable<PropertyInfo> GetPropertyInfos(this TypeInfo typeInfo)
		{
			var propertyInfos =
				from propertyInfo in typeInfo.GetRuntimeProperties()
				//where !propertyInfo.IsDefined(typeof(IgnoreDataMemberAttribute))
				orderby propertyInfo.MetadataToken
				select propertyInfo;
			return propertyInfos;
		}

		public static IOrderedEnumerable<PropertyInfo> GetPropertyInfos(this Type type) => type.GetTypeInfo().GetPropertyInfos();

		public static IOrderedEnumerable<PropertyInfo> GetPropertyInfos<T>(this T item) => typeof(T).GetPropertyInfos();

		public static IOrderedEnumerable<PropertyInfo> GetPropertyInfos<T>() => typeof(T).GetPropertyInfos();
	}
}