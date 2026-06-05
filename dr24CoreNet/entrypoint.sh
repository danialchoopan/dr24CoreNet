#!/bin/bash
set -e

# Run migrations if necessary or ensure DB is ready
# In this enterprise setup, we use EnsureCreated for demo speed + Seeding
# but in real prod we would use 'dotnet ef database update'

echo "Starting dr24CoreNet WebAPI..."
dotnet dr24CoreNet.WebAPI.dll
