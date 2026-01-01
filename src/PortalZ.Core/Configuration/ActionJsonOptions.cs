using PortalZ.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace PortalZ.Configuration
{
    public class ActionJsonOptions : IConfigureOptions<MvcNewtonsoftJsonOptions>
    {
        private readonly IServiceProvider provider;

        public ActionJsonOptions(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public void Configure(MvcNewtonsoftJsonOptions options)
        {
            options.SerializerSettings.Converters.Add(new ActionJsonConverter(provider));
        }
    }
}




