---
page_type: sample
languages:
- csharp
products:
- azure
- cosmos-db
- dotnet
description: "This sample shows you how to optimize throughput when importing data to Azure Cosmos DB through the SQL API using bulk support on .NET SDK"
---

# Optimize throughput when bulk importing data to Azure Cosmos DB SQL API account

This sample shows you how to optimize throughput when importing data to Azure Cosmos DB through the SQL API using bulk support on .NET SDK.

For a complete end-to-end walkthrough, please refer to the [full tutorial on the Azure Cosmos DB documentation page](https://aka.ms/CosmosDotnetBulkSupport).

## Running this sample

1. Before you can run this sample, you must have the following prerequisites:
	- An active Azure Cosmos DB account - If you don't have an account, refer to the [Create a database account](https://docs.microsoft.com/azure/cosmos-db/create-sql-api-dotnet#create-a-database-account) article. Optionally, you can use the [Azure Cosmos DB Emulator](https://docs.microsoft.com/azure/cosmos-db/local-emulator).
	- [NET Core SDK 3+](https://dotnet.microsoft.com/download).

1. Clone this repository, or download the zip file.

1. Browse to the `src` folder.

1. Run `dotnet build` to restore all packages.

1. Retrieve the URI and PRIMARY KEY (or SECONDARY KEY) values from the Keys blade of your Azure Cosmos DB account in the Azure portal. For more information on obtaining endpoint & keys for your Azure Cosmos DB account refer to [View, copy, and regenerate access keys and passwords](https://docs.microsoft.com/en-us/azure/cosmos-db/manage-account#keys). If you are using the Emulator, you can also use [its credentials](https://docs.microsoft.com/azure/cosmos-db/local-emulator#authenticating-requests).

If you don't have an account, see [Create a database account](https://docs.microsoft.com/azure/cosmos-db/create-sql-api-dotnet#create-a-database-account) to set one up.

1. In the [Program.cs](.\src\Program.cs) file, located in the `src` directory, find **EndPointUri** and **AuthorizationKey** and replace the placeholder values with the values obtained for your account.

    private const string EndpointUrl = "https://<your-account>.documents.azure.com:443/";
    private const string AuthorizationKey = "<your-account-key>";

1. You can now run the application with `dotnet run`.

## About the code
The code included in this sample is intended to show you how to leverage the bulk support to optimize throughput when you import data to Azure Cosmos DB.

## More information

- [Azure Cosmos DB Documentation](https://docs.microsoft.com/azure/cosmos-db/index)
- [Azure Cosmos DB .NET SDK for SQL API](https://docs.microsoft.com/azure/cosmos-db/sql-api-sdk-dotnet)
- [Azure Cosmos DB .NET SDK Reference Documentation](https://docs.microsoft.com/dotnet/api/overview/azure/cosmosdb?view=azure-dotnet)

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
