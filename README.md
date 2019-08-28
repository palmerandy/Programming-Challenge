# Programming-Challenge

A Web API for the management of products.  

## Decisions Made

- The back end is responsible for the creation of Product Ids.
- POST request method creates a new product and PUT request method updates existing products (will return a bad request if product does not exist).
- All Product endpoints to require an API Key - more on this under Authentication and Authorisation section below.
- Filtering implemented using query string parameters that can be combined (i.e. you can filter for a brand and a model and a description all at once).

## Usage

Clone repository, then either:

<details open>
  <summary>Run from Visual Studio 2019 </summary>

* open solution
* select the Products project and start without debugging (Ctrl + F5)

</details>

<details>
  <summary>Or run from command prompt</summary>

* open cmd from where the solution and run the following command
```
dotnet run --project Products\Products
```

**Use any other browser besides Chrome if you want to run from the command prompt** - otherwise you might see an ERR_CERT_AUTHORITY_INVALID that I haven't had time to look at.  

</details>

The API is available at <a href="https://localhost:44356/products" target="_">https://localhost:44356/products</a>.  If you see an invalid security certificate error, please click advanced add an exception and continue.  

**The API requires a header named "Authorization" with a value of "ApiKey sample-key"**

Swagger is available at <a href="https://localhost:44356" target="_">https://localhost:44356</a>.  When using Swagger Click the Authorize button (top right of screen), enter the value of **ApiKey sample-key** (this can be copied from the screen) and click Authorize.

## Tests

Unit tests and API integration tests have been supplied. 

### Running API Tests
* Within Visual Studio I find it easiest to start the Products Project without debugging
* Then Open Test Explorer (Tests > Windows > Test Explorer)
* Click the run all button or use the shortcut Ctrl + R, A

## Authentication and Authorisation

To keep the solution clean and simple I have implemented a basic API Key solution. **That is all Product requests require a header named "Authorization" with a value of "ApiKey sample-key".**  

I choose an API key solution as I wanted to keep this solution as simple as possible:
- Simple for the reviewers to consume.
- Simple to avoid implementing a complicated login process that might require a persistent data store, a UI front end, etc. 
- For simplicity there is only one API key setup. If you have access to the key you have access to all of the Products API.  This could be extended in the future to distinguish between requests.

I entertained implementing JwtBearerDefaults.AuthenticationScheme but decided against it as it was not specified whether this was a B2B API or an consumer facing API.  If this were a consumer facing API I would have implemented JWT authentication.

In a production scenario I would strongly recommend going down a different path, such as
- [Easy Auth](https://docs.microsoft.com/en-us/azure/app-service/overview-authentication-authorization) handled by the Azure App service 
- [Identity server](https://identityserver.io/)
- etc depending on requirements

## Technology used

- .net core 2.2.108
- Visual Studio 2019
- Resharper
- Swagger