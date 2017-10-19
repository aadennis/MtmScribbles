using System;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace ConsoleApp1
{
	class Program
	{
		const string TfsUrl = "https://dennis.visualstudio.com";
		static void Main(String[] args)
        {
            var serverUrl = new Uri(TfsUrl);

            GetProjectsForServerV1(serverUrl);
            Console.WriteLine("Done. Press any key");
            Console.ReadKey();
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



