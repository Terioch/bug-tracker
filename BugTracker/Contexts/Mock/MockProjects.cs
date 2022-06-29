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
                    Name = "Bugtrace",
                    Description = "A Bug/Issue Tracking system built with .NET MVC that stores and maintains issues in the form of tickets for a collection of projects.",
                },
                new Project
                {
                    Id = "p2",
                    Name = "Techtrace",
                    Description = "A technical blog site built with .NET Web API and React.JS."
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
