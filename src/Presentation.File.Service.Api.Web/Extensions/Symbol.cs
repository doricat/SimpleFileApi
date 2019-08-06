using System;

namespace Presentation.File.Service.Api.Web.Extensions
{
    public class Symbol
    {
        public Symbol(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = null;
        }

        public Symbol(string name, Type type) : this(name)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public string Name { get; }

        public Type Type { get; private set; }

        public object Value { get; set; }

        public bool TrySetValue(Symbol symbol, out string error)
        {
            Type = symbol.Type;

            if (Type == typeof(int) || Type == typeof(int?))
            {
                if (int.TryParse(Name, out var value))
                {
                    Value = value;
                }
            }

            if (Value == null)
            {
                var typeName = Type.IsGenericType ? Type.GenericTypeArguments[0].Name : Type.Name;
                error = $"{Name} ({{0}}) 不能转换到类型 {typeName}";
                return false;
            }

            error = null;
            return true;
        }
    }
}