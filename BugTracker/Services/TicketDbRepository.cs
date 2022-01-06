﻿using BugTracker.Data;
using BugTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BugTracker.Services    
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
            IEnumerable<Ticket>? tickets = context.Tickets
                .Include(t => t.Project)
                .Include(t => t.Submitter)
                .Include(t => t.AssignedDeveloper);            
            return tickets ?? new List<Ticket>();
        }

        public List<Ticket> GetTicketsByProjectId(string id)
        {           
            return context.Tickets
                .Where(t => t.ProjectId == id)
                .Include(t => t.Project)
                .Include(t => t.Submitter)
                .Include(t => t.AssignedDeveloper).ToList();
        }

        public Ticket GetTicketById(string id)
        {
            return context.Tickets                  
                .Include(t => t.Project)
                .Include(t => t.Submitter)
                .Include(t => t.AssignedDeveloper)
                .Include(t => t.TicketHistoryRecords)
                .Include(t => t.TicketAttachments)
                .Include(t => t.TicketComments)
                .FirstOrDefault(t => t.Id == id) ?? new Ticket();
        }

        public Ticket Create(Ticket ticket)
        {
            context.Tickets.Add(ticket);
            context.SaveChanges();
            return ticket;
        }

        public Ticket Delete(string id)
        {
            Ticket? ticket = context.Tickets.Find(id);

            if (ticket == null)
            {
                return new Ticket();
            }

            context.Tickets.Remove(ticket);
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
    }
}
