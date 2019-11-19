using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LinkedMink.Web.Infastructure.ViewModels
{
    public class RecordListViewModel<T>
    {
        static RecordListViewModel()
        {
            Type viewModelType = typeof(T);
            MethodInfo toViewModelMethod = viewModelType.GetMethod(ToViewModelMethodName);
            ToViewModelMethods[viewModelType] = toViewModelMethod;
        }

        public T[] Records { get; set; }

        public static T[] ToViewModel<TIn>(IEnumerable<TIn> data)
        {
            return data
                .Select(e => (T)ToViewModelMethods[typeof(T)].Invoke(null, new object[] { e }))
                .ToArray();
        }

        protected const string ToViewModelMethodName = "ToViewModel";

        protected static readonly Dictionary<Type, MethodInfo> ToViewModelMethods = new Dictionary<Type, MethodInfo>();
    }
}
