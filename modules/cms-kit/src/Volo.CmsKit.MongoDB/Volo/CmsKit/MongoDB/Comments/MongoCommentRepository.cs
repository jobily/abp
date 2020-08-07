﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Comments;

namespace Volo.CmsKit.MongoDB.Comments
{
    public class MongoCommentRepository : MongoDbRepository<ICmsKitMongoDbContext, Comment, Guid>, ICommentRepository
    {
        public MongoCommentRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<CommentWithAuthor>> GetListWithAuthorsAsync(
            string entityType,
            string entityId)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

            var query = from comment in GetMongoQueryable()
                join user in DbContext.CmsUsers on comment.CreatorId equals user.Id
                where entityType == comment.EntityType && entityId == comment.EntityId
                orderby comment.CreationTime
                select new CommentWithAuthor
                {
                    Comment = comment,
                    Author = user
                };

            return await query.ToListAsync();
        }

        public override async Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var replies = await GetMongoQueryable()
                .Where(x => x.RepliedCommentId == id)
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var reply in replies)
            {
                //TODO: Discuss if it is better to mark it as deleted and show in the ui as "This is deleted" instead of deleting it and replies completely
                await base.DeleteAsync(reply.Id, autoSave, GetCancellationToken(cancellationToken));
            }

            await base.DeleteAsync(id, autoSave, GetCancellationToken(cancellationToken));
        }
    }
}
