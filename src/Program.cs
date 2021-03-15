// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// ------------------------------------------------------------

namespace Microsoft.Azure.Cosmos.Samples.Bulk
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    // ----------------------------------------------------------------------------------------------------------
    // Prerequisites -
    //
    // 1. An Azure Cosmos account - 
    //    https://azure.microsoft.com/en-us/itemation/articles/itemdb-create-account/
    //
    // 2. Microsoft.Azure.Cosmos NuGet package - 
    //    http://www.nuget.org/packages/Microsoft.Azure.Cosmos/ 
    // ----------------------------------------------------------------------------------------------------------
    public class Program
    {
        private const string EndpointUrl = "https://<your-account>.documents.azure.com:443/";
        private const string AuthorizationKey = "<your-account-key>";
        private const string DatabaseName = "bulk-tutorial";
        private const string ContainerName = "items";
        private const int AmountToInsert = 300000;

        static async Task Main(string[] args)
        {
            // <CreateClient>
            CosmosClient cosmosClient = new CosmosClient(EndpointUrl, AuthorizationKey, new CosmosClientOptions() { AllowBulkExecution = true });
            // </CreateClient>

            // Create with a throughput of 50000 RU/s
            // Indexing Policy to exclude all attributes to maximize RU/s usage
            Console.WriteLine("This tutorial will create a 50000 RU/s container, press any key to continue.");
            Console.ReadKey();

            // <Initialize>
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(Program.DatabaseName);

            await database.DefineContainer(Program.ContainerName, "/pk")
                    .WithIndexingPolicy()
                        .WithIndexingMode(IndexingMode.Consistent)
                        .WithIncludedPaths()
                            .Attach()
                        .WithExcludedPaths()
                            .Path("/*")
                            .Attach()
                    .Attach()
                .CreateAsync(50000);
            
            // </Initialize>

            try
            {
                // Prepare items for insertion
                Console.WriteLine($"Preparing {AmountToInsert} items to insert...");
                // <Operations>
                IReadOnlyCollection<Item> itemsToInsert = Program.GetItemsToInsert();
                // </Operations>

                // Create the list of Tasks
                Console.WriteLine($"Starting...");
                Stopwatch stopwatch = Stopwatch.StartNew();
                // <ConcurrentTasks>
                Container container = database.GetContainer(ContainerName);
                List<Task> tasks = new List<Task>(AmountToInsert);
                foreach (Item item in itemsToInsert)
                {
                    tasks.Add(container.CreateItemAsync(item, new PartitionKey(item.pk))
                        .ContinueWith(itemResponse =>
                        {
                            if (!itemResponse.IsCompletedSuccessfully)
                            {
                                AggregateException innerExceptions = itemResponse.Exception.Flatten();
                                if (innerExceptions.InnerExceptions.FirstOrDefault(innerEx => innerEx is CosmosException) is CosmosException cosmosException)
                                {
                                    Console.WriteLine($"Received {cosmosException.StatusCode} ({cosmosException.Message}).");
                                }
                            }
                        }));
                }

                // Wait until all are done
                await Task.WhenAll(tasks);
                // </ConcurrentTasks>
                stopwatch.Stop();

                Console.WriteLine($"Finished in writing {AmountToInsert} items in {stopwatch.Elapsed}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("Cleaning up resources...");
                await database.DeleteAsync();
            }
        }

        // <Bogus>
        private static IReadOnlyCollection<Item> GetItemsToInsert()
        {
            return new Bogus.Faker<Item>()
            .StrictMode(true)
            //Generate item
            .RuleFor(o => o.id, f => Guid.NewGuid().ToString()) //id
            .RuleFor(o => o.username, f => f.Internet.UserName())
            .RuleFor(o => o.pk, (f, o) => o.id) //partitionkey
            .Generate(AmountToInsert);
        }
        // </Bogus>

        // <Model>
        public class Item
        {
            public string id {get;set;}
            public string pk {get;set;}

            public string username{get;set;}
        }
        // </Model>
    }
}
