## What is *ToDoWebApi*?

*ToDoWebApi* is ASP.NET Core Web APi for simple evidence of tasks.

## Requirements
- .NET 6, .NET 7 or.NET 8 installed

## How to use it

### Install *FioBankApiClient* via NuGet package

    PM> Install-Package FioBankApiClient


### Set API secret key for token signature

You'll need store an API secret key in the environment variable.

Instructions for obtaining a token can be found in the "ZÍSKÁNÍ TOKENU" section of the FIO Bank API documentation: https://www.fio.cz/docs/cz/API_Bankovnictvi.pdf

### Initialization of the  Fio API Client
Add FioBankApiClient services to your project's IServiceCollection and provide your API token:

    // Your Fio API token
    string yourApiToken = "rdso4JIBS...";

    // Add Fio Bank Api client
    applicationBuilder.Services.AddFioBankApiClient(yourApiToken);

### Examples of using

**Important note:**
There's a 30-second limit between api calls.
If you try to call it too quickly, you'll get a 409 Conflict error."

#### Listing transactions in specific Date/Time range

    string[] formats = { "dd/MM/yyyy" };
    Result<DateTimeRange> rangeOrFailure = DateTimeRange.Create("01/06/2024", "01/07/2024", formats);

    if (rangeOrFailure.IsSuccess)
    {
        Result<AccountStatement> transactions = await apiClientCaller.GetTransactionsInDateRangeAsync(rangeOrFailure.Value);

        Result<string> xmlTransactions = await apiClientCaller.GetTransactionsInDateRangeAsync(rangeOrFailure.Value, FioDataFormat.Xml);
    }

#### Listing transactions since last download

    Result<AccountStatement> lastTransactions = await apiClientService.GetTransactionsSinceLastDownloadAsync();

    Result<string> csvLastTransactions = await apiClientService.GetTransactionsSinceLastDownloadAsync(FioDataFormat.Csv);

#### Listing transactions since a certain statement ID and year

    Result<AccountStatement> transactionsByIdYear = await apiClientService.GetTransactionsByStatementIdAndYearAsync(1, 2024);

    Result<string> xmlTransactionsByIdYear = await apiClientService.GetTransactionsByStatementIdAndYearAsync(1, 2024, FioDataFormat.Xml);

#### Request Id and Year of last created official account statement (optionally filtered by year)

    Result<string> lastStatement = await apiClientService.GetLastStatementAsync(2024);

#### Set the cursor to the date of the last unsuccessful download transaction

    DateTimeOffset unsuccessfulDateTime = new DateTimeOffset(2024, 5, 1, 8, 6, 32, new TimeSpan(1, 0, 0));

    await apiClientService.SetCursorToLastDateAsync(unsuccessfulDateTime);

#### Set the cursor to the ID of the last successfully downloaded transaction

    await apiClientService.SetCursorToLastIdAsync(1);

### Submitting feature requests and bugs
Bugs and feature request are tracked on [GitHub](https://github.com/dlouhy/FioBankApiClient/issues)