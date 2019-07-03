﻿using System;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Models.Http;

namespace Bog.Api.Domain.Coordinators
{
    public class UpdateArticleCoordinator : IUpdateArticleCoordinator
    {
        private readonly IBlogApiDbContext _context;

        public UpdateArticleCoordinator(IBlogApiDbContext context)
        {
            _context = context;
        }

        public async Task<bool> TryUpdateArticle(Guid articleId, ArticleRequest updatedArticle)
        {
            if (updatedArticle == null) throw new ArgumentNullException(nameof(updatedArticle));

            var existingArticle = await _context.Find<Article>(articleId);

            if (existingArticle == null)
            {
                return false;
            }

            UpdateValues(existingArticle, updatedArticle);

            await _context.SaveChanges();

            return true;
        }

        private void UpdateValues(Article existingArticle, ArticleRequest updatedArticle)
        {
            existingArticle.Author = updatedArticle.Author;
        }
    }
}