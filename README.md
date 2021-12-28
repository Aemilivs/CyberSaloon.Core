# CyberSaloon Core

## Overview

CyberSaloon is a web application that allows it's users to introduce requests for arts creation to other users of the platform and also empovers them to express their appreciation towards requests that were posted by other users.
This allows artists to see what are the expectations from them as content creators and what are the preference of their audience.

CyberSaloon Core is a web API part of the platform serving as a back end.

## Design

CyberSaloon Core consitsts of 4 projects:
* API - Interface describing exposed endpoints
* BLL - Layer expressing the business logic
* DTO - Layer expressing the contract between the front end and back end
* Repo - Layer expressing entities stored in the database

Technical documentation for the API is expressed via swagger and accessible via the following uri:

> `{host}\swagger\ui\index`

Application is configured to have authentication via OAuth2 ran by Identity Service.

## How to build it

### Prerequisite

Host machine has to have [dotnet 5](https://dot.net/) SDK installed in order to compile the code.

### Command

The following command compiles the code into executable file with files required for launching the program and places it into `/out/` folder in the repository folder.

> `dotnet publish -c Release -o out`

Alternatively, code could be run using the following command:

> `dotnet run --project .\CyberSaloon.Core.API\`