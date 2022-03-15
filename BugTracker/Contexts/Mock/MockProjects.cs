using BugTracker.Models;

namespace BugTracker.Contexts.Mock
{
    public class MockProjects
    {
        public static List<Project> GetProjects()
        {
            return new List<Project>()
            {
                new Project
                {
                    Id = "p1",
                    Name = "Bug Tracker",
                    Description = "A Bug/Issue Tracker MVC project.",
                },
                new Project
                {
                    Id = "p2",
                    Name = "Technology Blog",
                    Description = "A programming tutorial blog built using .Net Web API and React.JS."
                },
                new Project
                {
                    Id = "p3",
                    Name = "Demo Project 1",
                    Description = "This is a demo project."
                },
                new Project
                {
                    Id = "p4",
                    Name = "Demo Project 2",
                    Description = "This is a demo project."
                }
            };
        }
    }
}
