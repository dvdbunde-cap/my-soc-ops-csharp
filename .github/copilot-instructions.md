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

## Design System

### Bold Constructivist Style

The UI follows a **Bold Constructivist** design inspired by Russian Constructivism (1919-1930s). Key characteristics:
- Heavy geometric aesthetic with thick borders (2px-8px)
- Primary color palette: red (`#CC0000`), black (`#000000`), white (`#FFFFFF`), yellow (`#FFCC00`)
- Sharp corners (no rounded corners or minimal 2px radius)
- Uppercase, condensed typography with wide letter spacing
- Diagonal stripe patterns for emphasis
- "Stamp" effects and bold, geometric animations

### Color Palette

```css
--color-red: #CC0000;
--color-black: #000000;
--color-white: #FFFFFF;
--color-yellow: #FFCC00;
--color-blue: #0066CC;
```

**Semantic usage:**
- Red: Accent, buttons, winning states, alerts
- Black: Headers, borders, text
- White: Backgrounds, text on dark
- Yellow: Highlights, warnings, winning squares

### Typography

- **Headings**: Oswald (Google Fonts) — bold, condensed, uppercase
- **Body**: PT Sans (Google Fonts) — clean, readable
- Always use `text-constructivist` class for uppercase elements with letter spacing
- Use `tracking-wide` (0.05em) or `tracking-wider` (0.1em) for constructivist text

### Component Styling Guidelines

1. **Borders**: Use thick borders (`--border-standard: 3px`, `--border-thick: 6px`, `--border-heavy: 8px`)
2. **Corners**: Use `rounded-none` or `rounded-sm` (2px max) — avoid rounded corners
3. **Colors**: Stick to the primary palette — avoid gray tones
4. **Text**: Uppercase for buttons, headings, labels; use `uppercase` and `tracking-wide` classes
5. **Spacing**: Use larger spacing scale (`--space-md: 1rem`, `--space-lg: 1.5rem`, `--space-xl: 2rem`)

### Component States

- **Default squares**: White background, black border, black text
- **Marked squares**: Red background (`#CC0000`), white text, white border
- **Winning squares**: Yellow background (`#FFCC00`), black text, black border, stamp animation
- **Free space**: White background with geometric diamond pattern, disabled state

### Animations

- **Stamp effect** (`animate-stamp`): Used for square clicks — scale + rotate animation (0.2s)
- **Sweep** (`animate-sweep`): Used for BINGO win — horizontal slide animation (0.3s)
- **Staggered reveal** (`.grid-reveal > *`): Used for grid load — sequential fade-in with delays
- **Loader pulse** (`loader-pulse`): Used for loading animation — scale + opacity pulse
- Always use `--ease-constructivist` timing function (`cubic-bezier(0.2, 0.8, 0.2, 1)`)

### Diagonal Stripes Pattern

Use these utility classes for constructivist patterns:
- `.diagonal-stripes-red`: Red/white 45° stripes
- `.diagonal-stripes-yellow`: Yellow/black 45° stripes
- `.diagonal-stripes-black`: Black/white 45° stripes

### Geometric Decorations

- `.geo-circle`: Circular decoration with thick border
- `.geo-square`: Square decoration with standard border
- `.geo-line-horizontal`: Horizontal heavy line
- `.geo-line-vertical`: Vertical heavy line

### Key Files

| File | Purpose |
|------|---------|
| `SocOps/wwwroot/css/app.css` | Full design system with tokens, utilities, and animations |
| `SocOps/wwwroot/index.html` | Font imports (Oswald + PT Sans), theme-color, loading animation |
| `SocOps/Components/StartScreen.razor` | Poster-style landing page with geometric blocks |
| `SocOps/Components/GameScreen.razor` | Industrial game interface with black header |
| `SocOps/Components/BingoBoard.razor` | Geometric grid with thick black borders |
| `SocOps/Components/BingoSquare.razor` | Bold square states with stamp animation |

### Design Principles

1. **Bold over subtle**: Use high contrast, thick borders, large typography
2. **Geometric over organic**: Prefer squares, circles, straight lines, diagonal patterns
3. **Uppercase over lowercase**: Use uppercase for UI elements, headings, buttons
4. **Sharp over rounded**: Avoid rounded corners, use sharp 90° angles
5. **Limited palette**: Stick to red/black/white/yellow — avoid gradients or multiple colors
6. **Motion with purpose**: Animations should feel like a "stamp" or "sweep" — bold and decisive

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
