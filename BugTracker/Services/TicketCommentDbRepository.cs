﻿using BugTracker.Data;
using BugTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BugTracker.Services
{
    public class TicketCommentDbRepository : ITicketCommentRepository
    {
        private readonly BugTrackerDbContext context;

        public TicketCommentDbRepository(BugTrackerDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<TicketComment> GetAllComments()
        {
            IEnumerable<TicketComment>? comments = context.TicketComments;
            return comments ?? new List<TicketComment>();
        }

        public TicketComment GetComment(string id)
        {          
            return context.TicketComments.Find(id) ?? new TicketComment();
        }

        public TicketComment Create(TicketComment comment)
        {
            context.TicketComments.Add(comment);
            context.SaveChanges();
            return comment;
        }

        public TicketComment Delete(string id)
        {
            TicketComment? comment = context.TicketComments.Find(id);

            if (comment == null)
            {
                return new TicketComment();
            }

            context.TicketComments.Remove(comment);
            context.SaveChanges();
            return comment;
        }        

        public TicketComment Update(TicketComment comment)
        {
            EntityEntry<TicketComment> attachedComment = context.TicketComments.Attach(comment);
            attachedComment.State = EntityState.Modified;
            context.SaveChanges();
            return comment;
        }
    }
}