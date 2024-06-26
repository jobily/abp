﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Resources;

public class WebRequestResources : IWebRequestResources, IScopedDependency
{
    protected Dictionary<string, List<BundleFile>> Resources { get; }

    protected IHttpContextAccessor HttpContextAccessor { get; }

    public WebRequestResources(IHttpContextAccessor httpContextAccessor)
    {
        HttpContextAccessor = httpContextAccessor;
        Resources = new Dictionary<string, List<BundleFile>>();
    }

    public List<BundleFile> TryAdd(List<BundleFile> resources)
    {
        var path = HttpContextAccessor.HttpContext?.Request?.Path ?? "";

        if (Resources.TryGetValue(path, out var res))
        {
            var resourceToBeAdded = resources.Except(res).ToList();
            res.AddRange(resourceToBeAdded);
            return resourceToBeAdded;
        }

        Resources.Add(path, resources);
        return resources;
    }
}
