using BugTracker.Models;

namespace BugTracker.Services
{
    public interface ITicketRepository
    {
        public IEnumerable<Ticket> GetAllTickets();
        public Ticket GetTicket(string id);
        public Ticket Create(Ticket ticket);
        public Ticket Update(Ticket ticket);
        public Ticket Delete(string id);
    }
}
