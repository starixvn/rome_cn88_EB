using EB.Features.Site.Main.Requests;
using Fanex.NAP.Core.Web.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EB.Features.Site.Shared
{
    public class QueryEntityModelBinder : DefaultModelBinder
    {
        private static readonly Type[] IUserType = { typeof(ICurrentSession) };
        private static readonly Type BaseQueryType = typeof(BaseQuery);

        protected override object CreateModel(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            Type modelType)
        {
            if (modelType != null
                && BaseQueryType.IsAssignableFrom(modelType)
                && modelType.GetConstructor(IUserType) != null)
            {
                return CreateInstanceOfAssignableBaseQuery(modelType);
            }

            return CallBaseCreateModel(controllerContext, bindingContext, modelType);
        }

        protected virtual object CreateInstanceOfAssignableBaseQuery(Type modelType)
            => Activator.CreateInstance(modelType, DependencyResolver.Current.GetService<ICurrentSession>());

        protected virtual object CallBaseCreateModel(
           ControllerContext controllerContext,
           ModelBindingContext bindingContext,
           Type modelType) => base.CreateModel(controllerContext, bindingContext, modelType);

        protected virtual object CallBaseBindModel(
           ControllerContext controllerContext,
           ModelBindingContext bindingContext)
            => base.BindModel(controllerContext, bindingContext);
    }
}