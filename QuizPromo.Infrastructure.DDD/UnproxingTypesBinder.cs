using System;
using System.Linq;

namespace QuizPromo.Infrastructure.DDD
{
    //public class UnproxingTypesBinder : DefaultSerializationBinder
    //{
    //    private const string proxyClassMarker = "System.Data.Entity.DynamicProxies";
    //    private static ConcurrentDictionary<string, Type> types = new ConcurrentDictionary<string, Type>();

    //    public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
    //    {
    //        if (proxyClassMarker.Equals(serializedType.Namespace) && serializedType.BaseType != null)
    //        {
    //            assemblyName = serializedType.BaseType.Assembly.GetName().Name;
    //            typeName = serializedType.BaseType.FullName;
    //            return;
    //        }
    //        base.BindToName(serializedType, out assemblyName, out typeName);
    //    }

    //    public override Type BindToType(string assemblyName, string typeName)
    //    {
    //        if (typeName.StartsWith(proxyClassMarker))
    //        {
    //            string assemblyNameClean = assemblyName.Split("-".ToCharArray())[1];
    //            //Предполагаем, что нижнее подчеркивание не используется в именах типов сущностей!
    //            string typeNameClean = typeName.Split(new char[] { '_' })[0];
    //            typeNameClean = typeNameClean.Split(new char[] { '.' }).LastOrDefault() ?? String.Empty;

    //            if (!types.ContainsKey(typeNameClean))
    //            {
    //                var currentAssembly = AppDomain.CurrentDomain.GetAssemblies()
    //                    .First(a => a.FullName.Contains(assemblyNameClean));
    //                //Single - тут не должно возникать неоднозначности.
    //                var currentType = currentAssembly.ExportedTypes.SingleOrDefault(t => t.Name.Equals(typeNameClean));
    //                //Если не нашли точное соответствие, то ищем вхождение.
    //                currentType = currentType ?? currentAssembly.ExportedTypes.Single(t => t.Name.Contains(typeNameClean));
    //                types[typeNameClean] = currentType;
    //            }
                
    //            return base.BindToType(assemblyName: assemblyNameClean, typeName: types[typeNameClean].FullName);
                
    //        }
            
    //        return base.BindToType(assemblyName, typeName);
    //    }
    //}
}
