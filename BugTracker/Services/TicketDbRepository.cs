using BugTracker.Data;
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
            IEnumerable<Ticket>? tickets = context.Tickets;            
            return tickets ?? new List<Ticket>();
        }

        public List<Ticket> GetTicketsByProjectId(string id)
        {
            List<Ticket> tickets = new();

            foreach (var ticket in context.Tickets)
            {
                if (ticket.ProjectId == id)
                {
                    tickets.Add(ticket);
                }
            }
            return tickets;
        }

        public Ticket GetTicketById(string id)
        {            
            return context.Tickets.Find(id) ?? new Ticket();
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
