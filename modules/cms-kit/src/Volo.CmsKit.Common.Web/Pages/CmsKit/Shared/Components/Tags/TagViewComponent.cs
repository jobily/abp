﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Web.Pages.CmsKit.Shared.Components.Tags
{
    [ViewComponent(Name = "CmsTags")]
    public class TagViewComponent : AbpViewComponent
    {
        protected readonly ITagAppService TagAppService;

        public TagViewComponent(ITagAppService tagAppService)
        {
            TagAppService = tagAppService;
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(
            string entityType,
            string entityId,
            IEnumerable<string> tags = null)
        {
            var tagDtos = await TagAppService.GetAllRelatedTagsAsync(new GetRelatedTagsInput
            {
                EntityId = entityId,
                EntityType = entityType,
                Tags = tags?.ToList()
            });

            var viewModel = new TagViewModel
            {
                EntityId = entityId,
                EntityType = entityType,
                Tags = tagDtos
            };

            return View("~/Pages/CmsKit/Shared/Components/Tags/Default.cshtml", viewModel);
        }

        public class TagViewModel
        {
            public List<TagDto> Tags { get; set; }
            public string EntityId { get; set; }
            public string EntityType { get; set; }
        }
    }
}
