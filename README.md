# Project Documentation: Stage 1 - SWK5

### Source Code Management
- **Platform:** GitHub
- **Collaboration:** Pull requests undergo code reviews before merging.

### Directory Structure
```
ProjectName/
├── docs/                          
├── sql/                           # SQL scripts
│   ├── docker-compose.yml         
│   ├── init-db.sql                
├── src/                           # Source code
│   ├── DataAccess/                # Data access layer
│   ├── Models/                    # Data models
│   ├── Services/                  # Business logic
│   ├── Program.cs                 # Entry point
└── tests/                         # Unit tests
```

### Issue Tracking System
- **Platform:** GitHub Issues
- **Setup:** Issues are categorized into epics, features, and bugs.

### Build Pipeline
- **Tool:** GitHub Actions
- **Configuration:** Includes steps for:
  1. Pulling the latest code
  2. Running unit tests
  3. Building the project

### ER Diagram

![Diagram](SWK5-WEA5-VZ/docs/swk-er.svg)

### Implementation
As for stage 1, i have implemented basic SQL configuration, Github project and pipeline, CRUD Software, Tests.

## TO-FIX:
 - Pipeline sql container error.
 - Test runs
 - More implementation etc.