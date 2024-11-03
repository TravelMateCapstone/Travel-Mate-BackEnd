﻿using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class PostCommentDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public PostCommentDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PostComment>> GetAll(int postId)
        {
            return _dbContext.PostComments
                .Where(p => p.PostId == postId).ToList();
        }

        public async Task<PostComment> GetGroupPostCommentById(int commentId)
        {
            return await _dbContext.PostComments
                .FirstOrDefaultAsync(p => p.CommentId == commentId);
        }

        public async Task<PostComment> AddGroupPostCommentAsync(PostComment postComment)
        {
            await _dbContext.PostComments.AddAsync(postComment);
            await _dbContext.SaveChangesAsync();
            return postComment;
        }

        public async Task DeleteGroupPostAsync(int commentId)
        {
            var findComment = await _dbContext.PostComments.FindAsync(commentId);
            if (findComment != null)
            {
                _dbContext.PostComments.Remove(findComment);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateGroupPostDetailAsync(PostComment postComment)
        {
            _dbContext.PostComments.Update(postComment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsGroupMember(int groupId, int userId)
        {
            return await _dbContext.GroupParticipants
                .AnyAsync(p => p.GroupId == groupId && p.UserId == userId);
        }

        public async Task<bool> IsCommentCreator(int commentId, int userId)
        {
            return await _dbContext.PostComments
                .AnyAsync(p => p.CommentId == commentId && p.CommentedById == userId);
        }
    }
}