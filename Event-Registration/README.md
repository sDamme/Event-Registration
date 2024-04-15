# Event Registration App

## Overview
This Event Registration App is a web application designed to manage and track event registrations. Users can create events, add attendees (individuals or businesses), and manage event attendees.

## Features
- **Create Events**: Users can add upcoming events with details such as name, time, location, and additional information.
- **Manage Attendees**: For each event, users can add and manage attendees, who may be individuals or representatives from businesses.
- **View Events**: Users can view past and upcoming events, along with details and a list of attendees.
- **Edit Attendees**: Edit functionality allows modification of attendee details.
- **Delete Events and Attendees**: Users can remove events that are no longer relevant, along with their associated attendees.

## Technology Stack
- **ASP.NET Core**: For building the web framework.
- **Entity Framework Core**: For ORM-based data access.
- **SQL Server/PostgreSQL**: Database systems supported (configurable).
- **Docker**: For containerization and deployment.
- **XUnit**: For unit testing.
- **Microsoft.EntityFrameworkCore.InMemory**: For in-memory database testing.


## Getting Started

### Prerequisites
- .NET 5.0 SDK or later
- Visual Studio 2019 or later (or a similar IDE with C# support)
- Docker (This app lives inside docker containers and speaks between them)

### Installation and usage
1. Clone the repository

2. Navigate to the solution and project directory 
 - `cd Event-Registration/Event-Registration`

3. Run the following command to build the webapp and database
 - `docker-compose build`

4. Run the following command to start the webapp and database
 - `docker-compose up`

5. Open the webapp from docker through the provided port url or start a debug session through Visual Studio

6. Tests can be run from the test explorer in Visual Studio

### Database Configuration
- Database tables are automatically migrated on startup with the provided connection string in the `appsettings.json` file.


## Troubleshooting

### Web Application Starts Before Database is Ready

**Problem Description:**
Sometimes, the web application container may fail to start correctly because it attempts to connect to the database before the database server is fully ready. This can occur even though the `docker-compose.yml` file configures the web application to depend on the database.

**Root Cause:**
The `depends_on` option in Docker Compose only ensures that the database container starts before the web application container. However, it does not guarantee that the database service within the container is fully initialized and ready to accept connections when the web application starts.

**Solutions:**
Either manually start the webapp container again or run the debug session through Visual Studio.

## Final thoughts

Probably has a lot of room for improvement. I feel I didn't really adhere to DDD principles and the architecture could be better.
However I am happy with the result and it was a great learning experience for me.