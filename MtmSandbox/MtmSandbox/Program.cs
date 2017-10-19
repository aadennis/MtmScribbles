using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.TestManagement.Client;

using System.Linq;

namespace ConsoleApp1
{
	class Program
	{

        const string TfsUrl = "https://jimbo.visualstudio.com";
        TfsTeamProjectCollection projectCollection;
		static void Main(String[] args)
        {
            //GetProjectsForServerV1(serverUrl);
            //GetProjectsForServerV2(serverUrl);

            Program p = new Program();
            Console.WriteLine("Done. Press any key");
            Console.ReadKey();
        }

        public Program()
        {
            projectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(TfsUrl));

            var x = GetTeamProjects();
            foreach (var project in x)
            {
                var testPlans = GetTestPlans(project);
                foreach (var plan in testPlans)
                {
                    Console.WriteLine(plan);
                    
                }
            }
        }

        public List<string> GetTeamProjects()
        {
            var service = projectCollection.GetService<VersionControlServer>();
            return service.GetAllTeamProjects(true).Select(i => i.Name).ToList();
        }

        public List<string> GetTestPlans(string teamProject)
        {
            var service = projectCollection.GetService<ITestManagementService>();
            var testProject = service.GetTeamProject(teamProject);
            return testProject.TestPlans.Query("SELECT * FROM TestPlan").Select(i => i.Name).ToList();
        }

        private static void GetProjectsForServerV2(Uri serverUrl)
        {
            //https://stackoverflow.com/questions/31031817/unable-to-load-dll-microsoft-witdatastore32-dll-teamfoundation-workitemtracki
            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(serverUrl);
            WorkItemStore workItemStore = tpc.GetService<WorkItemStore>();
            Project teamProject = workItemStore.Projects["ARM Basic Templates"];
            WorkItemType workItemType = teamProject.WorkItemTypes["Test Case"];

            var queryResults = workItemStore.Query(
                "Select [State], [Title] " +
   "From WorkItems " +
   "Where [Work Item Type] = 'Test Case' " +
   "Order By [State] Asc, [Changed Date] Desc");
        }

        private static void GetProjectsForServerV1(Uri serverUrl)
        {
            var configurationServer =
                            TfsConfigurationServerFactory.GetConfigurationServer(serverUrl);

            // Get the catalog of team project collections
            var collectionNodes = configurationServer.CatalogNode.QueryChildren(
                new[] { CatalogResourceTypes.ProjectCollection },
                false, CatalogQueryOptions.None);

            // List the team project collections
            foreach (var collectionNode in collectionNodes)
            {
                // Use the InstanceId property to get the team project collection
                var collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);
                var teamProjectCollection = configurationServer.GetTeamProjectCollection(collectionId);

                // Print the name of the team project collection
                Console.WriteLine("Collection: " + teamProjectCollection.Name);

                // Get a catalog of team projects for the collection
                var projectNodes = collectionNode.QueryChildren(
                    new[] { CatalogResourceTypes.TeamProject },
                    false, CatalogQueryOptions.None);

                // List the team projects in the collection
                foreach (var projectNode in projectNodes)
                {
                    Console.WriteLine(" Team Project: " + projectNode.Resource.DisplayName);

                }
            }
        }
    }
}



