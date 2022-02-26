﻿using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BugTracker.Repositories.Db
{
    public class TicketDbRepository : ITicketRepository
    {
        private readonly BugTrackerDbContext context;        

        public TicketDbRepository(BugTrackerDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Ticket> GetAllTickets()
        {            
             return context.Tickets
                .Include(t => t.Project)
                .Include(t => t.Submitter)
                .Include(t => t.AssignedDeveloper)
                .OrderByDescending(t => t.CreatedAt);            
        }

        public List<Ticket> GetTicketsByProjectId(string id)
        {
            return context.Tickets
                .Where(t => t.ProjectId == id)
                .Include(t => t.Project)
                .Include(t => t.Submitter)
                .Include(t => t.AssignedDeveloper)                
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
        }

        public Ticket GetTicketById(string id)
        {
            return context.Tickets
                .Include(t => t.Project)
                .Include(t => t.Submitter)
                .Include(t => t.AssignedDeveloper)
                .Include(t => t.TicketHistoryRecords) 
                    .ThenInclude(t => t.Modifier)
                .Include(t => t.TicketAttachments)
                .Include(t => t.TicketComments)
                    .ThenInclude(c => c.Author)
                .First(t => t.Id == id);            
        }

        public Ticket Create(Ticket ticket)
        {
            context.Tickets.Add(ticket);
            context.SaveChanges();
            return ticket;
        }

        public Ticket Update(Ticket ticket)
        {
            EntityEntry<Ticket> attachedTicket = context.Tickets.Attach(ticket);
            attachedTicket.State = EntityState.Modified;
            context.SaveChanges();
            return ticket;
        }

        public Ticket Delete(string id)
        {
            Ticket? ticket = context.Tickets.Find(id);            
            context.Tickets.Remove(ticket);
            context.SaveChanges();
            return ticket;
        }                
    }
}