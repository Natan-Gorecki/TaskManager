# TaskManager

### Ports
Deafult pattern is 35 + ServiceID (00-99) + HTTPS enabled (0-1).

### Services
| Service name        | HTTP/S  | Port  |
|---------------------|---------|-------|
| TaskManager.Service | http    | 35010 |
| TaskManager.Service | https   | 35011 |

### Migrations

- Add new migration
```
cd TaskManager.Service
dotnet ef migrations add [NAME] --context SqliteTaskManagerContext --output-dir Database/Sqlite/Migrations
```
