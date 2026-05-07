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

### Gradient Glass UI Style

The UI follows a **Gradient Glass UI** design featuring frosted glass morphism, vibrant gradient backgrounds, and translucent components. Key characteristics:
- Glass morphism with `backdrop-filter: blur(10px)` and translucent backgrounds
- Vibrant gradient backgrounds (purple-blue, pink-orange, teal-green)
- Large border radius (12px-24px) for glass morphism feel
- Modern, rounded typography (Poppins) with light weights
- Soft shadows and glow effects for depth
- Smooth, glowing transitions (scale, opacity, blur)

### Gradient Palette

```css
--gradient-primary: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
--gradient-warm: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
--gradient-cool: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
--gradient-success: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
--gradient-sunset: linear-gradient(135deg, #fa709a 0%, #fee140 100%);
```

**Glass Effect Colors:**
```css
--glass-bg: rgba(255, 255, 255, 0.25);
--glass-bg-light: rgba(255, 255, 255, 0.15);
--glass-bg-dark: rgba(0, 0, 0, 0.2);
--glass-border: rgba(255, 255, 255, 0.18);
--glass-shadow: 0 8px 32px 0 rgba(31, 38, 135, 0.37);
```

### Typography

- **Primary**: Poppins (Google Fonts) — modern, rounded, glass-friendly
- Use `text-gradient` class for gradient text effects
- Use `text-light` (font-weight: 300) for subtle text
- Use `font-medium` (500) or `font-semibold` (600) for emphasis.

### Component Styling Guidelines

1. **Glass Effect**: Use `glass`, `glass-light`, or `glass-dark` classes for frosted glass morphism
2. **Corners**: Use `rounded-md` (12px), `rounded-lg` (16px), or `rounded-xl` (24px)
3. **Colors**: Use gradient backgrounds (`bg-gradient-primary`, etc.) for vibrant visuals
4. **Text**: Modern, rounded typography — avoid uppercase for body text
5. **Spacing**: Use softer spacing scale (`p-3`, `p-4`, `p-6`, `p-8`)

### Component States

- **Default squares**: Frosted glass (`glass` class), blur effect, 12px radius
- **Marked squares**: Gradient fill (warm gradient), white text, glow effect
- **Winning squares**: Gradient fill (success gradient), pulse glow animation
- **Free space**: Special glass effect with semi-transparent icon.

### Animations

- **Glow pulse** (`animate-glow`): Used for BINGO win — box-shadow glow animation (2s)
- **Ripple effect** (`animate-ripple`): Used for square clicks — expanding circle with opacity fade
- **Fade in** (`animate-fade-in`): Used for page load — scale + opacity animation (0.4s)
- **Staggered fade** (`.grid-fade > *`): Used for grid load — sequential fade-in with delays
- **Glass pulse** (`glass-pulse`): Used for loading animation — scale + glow pulse
- Always use `--ease-glass` timing function (`cubic-bezier(0.4, 0, 0.2, 1)`)

### Glass Morphism Utilities

Use these utility classes for glass effects:
- `.glass`: Standard glass morphism (white translucent, blur, border)
- `.glass-light`: Lighter glass effect (less opacity)
- `.glass-dark`: Dark glass effect for dark backgrounds
- `.bg-gradient-primary`: Purple-to-blue gradient background
- `.bg-gradient-warm`: Pink-to-orange gradient background
- `.bg-gradient-cool`: Blue-to-teal gradient background
- `.bg-gradient-success`: Green-to-teal gradient background.

### Key Files

| File | Purpose |
|------|---------|
| `SocOps/wwwroot/css/app.css` | Full glass morphism system with tokens, utilities, and animations |
| `SocOps/wwwroot/index.html` | Font imports (Poppins), theme-color, glass loading animation |
| `SocOps/Components/StartScreen.razor` | Glass card on gradient background |
| `SocOps/Components/GameScreen.razor` | Glass header with blur, gradient background |
| `SocOps/Components/BingoBoard.razor` | Glass container with gap, rounded corners |
| `SocOps/Components/BingoSquare.razor` | Glass squares with gradients and blur |

### Design Principles

1. **Glass over solid**: Use frosted glass morphism with backdrop blur
2. **Gradients over flat**: Use vibrant gradient backgrounds for visual interest
3. **Soft over bold**: Use smooth, glowing transitions (scale, opacity, blur)
4. **Rounded over sharp**: Use large border radius (12px-24px) for glass morphism
5. **Modern over condensed**: Use Poppins typography with light weights
6. **Depth over flat**: Use layered shadows and glow effects for 3D depth.

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
