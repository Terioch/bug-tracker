using BugTracker.Models;

namespace BugTracker.Repositories
{
    public interface ITicketRepository
    {
        public IEnumerable<Ticket> GetAllTickets();
        public List<Ticket> GetTicketsByProjectId(string id);
        public Ticket GetTicketById(string id);
        public Ticket Create(Ticket ticket);
        public Ticket Update(Ticket ticket);
        public Ticket Delete(string id);
    }
}
