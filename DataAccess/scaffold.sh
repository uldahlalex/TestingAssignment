#!/bin/bash

dotnet ef dbcontext scaffold "Server=localhost;Database=testdb;User Id=dbuser;Password=dbpass;" Npgsql.EntityFrameworkCore.PostgreSQL --output-dir Models --context-dir . --context LibraryContext --force --no-onconfiguring