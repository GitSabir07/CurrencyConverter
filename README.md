# CurrencyConverter

Design and implement a robust, scalable, and maintainable currency conversion API using C# and ASP.NET Core, ensuring high performance, security, and resilience.

Setup Instructions

1. Clone the Repository
   git clone <your-repo-url>
   cd DotNetCrudWebApi2.

2. Configure Environment
   Update appsettings.json and environment-specific files (appsettings.Development.json, appsettings.UAT.json, etc.) as needed.
   Set the ASPNETCORE_ENVIRONMENT variable to select the environment (e.g., Development, UAT, Production).

3. Build and Run with Docker
   • Build the Docker image:
   docker build -t dotnetcrudwebapi .

Assumptions Made
• The project targets .NET 8 and requires the corresponding SDK/runtime.
• The API is stateless and suitable for horizontal scaling.
• Environment-specific configuration is managed via appsettings.{Environment}.json files.
• Docker is installed and available on the host machine.
• No external database or services are required by default; if needed, connection strings should be set in the appropriate configuration files.
• API versioning is implemented using URL segments (e.g., /api/v1.0/controller).
Possible Future Enhancements
• Add support for additional API providers (e.g., more currency data sources).
• Implement authentication and authorization basic changes added but add changes for role base validation (e.g., JWT, OAuth2).
• Integrate with a persistent database for storing user or transaction data.
• Add health checks and monitoring endpoints.
• Implement rate limiting and request throttling.
• Enhance API documentation (e.g., Swagger/OpenAPI improvements).
• Add CI/CD pipeline integration for automated builds, tests, and deployments.
• Support for gRPC or GraphQL endpoints.
• Improve error handling and logging with structured logging and distributed tracing.

---
