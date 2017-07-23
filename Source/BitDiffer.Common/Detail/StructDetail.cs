using System;
using System.Reflection;

namespace BitDiffer.Common.Model
{
	[Serializable]
	public class StructDetail : EntityDetail
	{
		public StructDetail()
		{
		}

		public StructDetail(RootDetail parent, Type type)
			: base(parent, type, false)
		{
			BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			Type[] types = type.GetNestedTypes(flags);

			_category = "struct";

			foreach (Type nested in types)
			{
				if (nested.IsEnum)
				{
					_children.Add(new EnumDetail(this, nested));
				}
				else if (nested.IsInterface)
				{
					_children.Add(new InterfaceDetail(this, nested));
				}
				else if (nested.IsClass)
				{
					_children.Add(new ClassDetail(this, nested));
				}
				else if (nested.IsValueType && nested.IsPrimitive == false)
				{
					_children.Add(new StructDetail(this, nested));
				}
			}
		}

		protected override string SerializeGetElementName()
		{
			return "Struct";
		}
	}
}