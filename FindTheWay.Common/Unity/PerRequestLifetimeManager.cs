using Microsoft.Practices.Unity;
using System;
using System.Web;

namespace FindTheWay.Common.Unity
{
	public class PerRequestLifetimeManager : LifetimeManager, IDisposable
    {
        private const string ContainerName = "HttpContextContainer";

        public override object GetValue()
        {
            return HttpContext.Current.Items[ContainerName];
        }

        public override void RemoveValue()
        {
            HttpContext.Current.Items.Remove(ContainerName);
        }

        public override void SetValue(object newValue)
        {
            HttpContext.Current.Items[ContainerName] = newValue;
        }

        public void Dispose()
        {
            this.RemoveValue();
        }
    }
}
