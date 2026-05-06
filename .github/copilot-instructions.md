# Copilot Workspace Instructions

## Development Checklist

- [ ] `dotnet build SocOps/SocOps.csproj` passes with no errors
- [ ] `dotnet test` passes when tests exist
- [ ] `dotnet lint` or equivalent style/lint check passes
- [ ] `dotnet run --project SocOps` starts the app successfully

## Overview

Soc Ops is a Blazor WebAssembly game built with .NET 10. Players mark bingo squares by matching people to questions.

## Key Areas

- `SocOps/Components/` — reusable UI components
- `SocOps/Services/` — state and game logic
- `SocOps/Data/` — static quiz/questions
- `SocOps/wwwroot/css/app.css` — custom utility-style CSS

## Commands

```bash
cd SocOps
dotnet build SocOps.csproj
dotnet test
dotnet run --project SocOps
```

## Agent Guidance

- Follow `.github/instructions/frontend-design.instructions.md` and `.github/instructions/css-utilities.instructions.md`
- Keep UI changes aligned with existing utility classes and Blazor conventions
- Prefer minimal architecture changes unless a refactor is requested
- Verify the app builds and runs after changes
