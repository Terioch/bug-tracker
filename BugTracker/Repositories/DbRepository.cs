using BugTracker.Data;

namespace BugTracker.Repositories
{
    public class DbRepository
    {
        private readonly BugTrackerDbContext context;

        public DbRepository(BugTrackerDbContext context)
        {
            this.context = context;
        }

        public virtual IProjectRepository? ProjectRepo { get; set; }

        public IUserProjectRepository? UserProjectRepo { get; set; }

        public ITicketRepository? TicketRepo { get; set; }

        public int age = 58;
    }

    class Child : DbRepository
    {
        public new int age;

        public Child(BugTrackerDbContext context) : base(context)
        {
            age = base.age - 26;
            ProjectRepo.Create(new Models.Project { Id = "-1" });
        }        
    }

    class GrandChild : Child
    {
        public new int age;       

        public GrandChild(BugTrackerDbContext context) : base(context)
        {            
            age = base.age - 27;
            base.ProjectRepo.Create(new Models.Project { Id = Guid.NewGuid().ToString() });
        }     
        
        public void Hi()
        {
            
        }
    }
}
