# Soc Ops - Design Specification

## Overview
Soc Ops is a Blazor WebAssembly bingo game built with .NET 10. The UI follows a **Gradient Glass UI** design system featuring frosted glass morphism, vibrant gradient backgrounds, and translucent components.

## Design System Implementation

### ✅ Completed Elements

#### CSS Design Tokens (`SocOps/wwwroot/css/app.css`)
- **Gradient Palette**: Primary, Warm, Cool, Success, Sunset gradients defined
- **Glass Effects**: `--glass-bg`, `--glass-bg-light`, `--glass-bg-dark`, `--glass-border`, `--glass-shadow`
- **Typography**: Poppins font imported from Google Fonts
- **Spacing Scale**: Soft spacing scale (xs: 0.25rem to 2xl: 3rem)
- **Border Radius**: Large radius for glass morphism (md: 12px, lg: 16px, xl: 24px)
- **Blur Effects**: `--blur-sm` (5px), `--blur-md` (10px), `--blur-lg` (20px)
- **Animation Easing**: `--ease-glass: cubic-bezier(0.4, 0, 0.2, 1)`

#### Component Classes Using Design System

| Component | File | Glass Classes Used | Status |
|-----------|------|-------------------|--------|
| StartScreen | `Components/StartScreen.razor` | `glass`, `rounded-xl`, `bg-gradient-primary`, `text-gradient`, `heading-xl`, `animate-fade-in`, `hover-scale` | ✅ |
| BingoBoard | `Components/BingoBoard.razor` | `grid-cols-5`, `gap-3`, `grid-fade`, `max-w-2xl` | ✅ |
| BingoSquare | `Components/BingoSquare.razor` | `glass`, `bg-gradient-warm`, `bg-gradient-success`, `rounded-lg`, `animate-glow`, `hover-scale` | ✅ |
| ScavengerHuntItem | `Components/ScavengerHuntItem.razor` | `glass`, `rounded-md`, `bg-gradient-success`, `hover-scale` | ✅ |
| CardDeckScreen | `Components/CardDeckScreen.razor` | `bg-gradient-sunset`, `glass`, `rounded-xl`, `text-gradient`, `heading-md`, `animate-fade-in`, `hover-scale` | ✅ |
| GameScreen | `Components/GameScreen.razor` | `bg-gradient-primary`, `glass`, `rounded-none`, `text-white` | ✅ |
| ScavengerHuntScreen | `Components/ScavengerHuntScreen.razor` | `bg-gradient-success`, `glass`, `rounded-none` | ✅ |

### 🎨 Visual Design Decisions

1. **Glass Morphism Priority**: All containers use `glass` class with backdrop blur
2. **Gradient Backgrounds**: Each screen has a distinct gradient:
   - Start: `bg-gradient-primary` (purple-blue)
   - Bingo: `bg-gradient-primary` (purple-blue)
   - Scavenger Hunt: `bg-gradient-success` (green-teal)
   - Card Deck: `bg-gradient-sunset` (pink-orange)
3. **Marked State**: Uses `bg-gradient-warm` (pink-red) for bingo squares, `bg-gradient-success` for scavenger items
4. **Typography**: Poppins with light weights (300-400) for body, semibold (600) for emphasis
5. **Animations**: `animate-fade-in` on page load, `animate-glow` on winning squares, `hover-scale` on interactive elements

### 📱 Screens Reviewed

#### 1. Start Screen
- **Layout**: Centered glass card on gradient background
- **Elements**: Title (SOC OPS with gradient text), subtitle, How to Play glass blocks, 3 mode buttons
- **Buttons**: Full-width with gradient backgrounds and hover effects
- **Status**: ✅ Complete and visually aligned

#### 2. Bingo Game Screen
- **Layout**: 5x5 grid (25 squares) with glass header
- **Square States**:
  - Default: `glass` class with blur effect
  - Marked: `bg-gradient-warm` with white text and ✕ indicator
  - Winning: `bg-gradient-success` with `animate-glow` pulse
  - Free Space: Special styling with semi-transparent circle icon
- **Status**: ✅ Complete with all states implemented

#### 3. Scavenger Hunt Screen
- **Layout**: Checklist with progress bar in glass header
- **Items**: Checkbox-style list with glass background
- **Progress**: "X/24 found" with percentage indicator
- **Marked State**: `bg-gradient-success` with white text
- **Status**: ✅ Complete

#### 4. Card Deck Screen
- **Layout**: Draw card interface with stats in glass container
- **States**:
  - Empty: "TAP TO DRAW" button with card emoji
  - Drawn: Card displayed with text and mark button
  - Complete: "DECK COMPLETE!" with celebration emoji
- **Stats**: "Cards Drawn: X / 27" with gradient text
- **Status**: ✅ Complete

## Build & Runtime Status

- ✅ `dotnet build SocOps/SocOps.csproj` - Builds successfully
- ✅ `dotnet run --project SocOps` - Runs on http://localhost:5166
- ✅ All navigation flows working (Back buttons functional)
- ✅ Interactive elements respond to clicks

## Next Steps / Potential Enhancements

1. **Win Detection**: Verify BINGO win detection works (5 in a row)
2. **Responsive Testing**: Test on various screen sizes
3. **Animation Polish**: Verify all animations trigger correctly
4. **Accessibility**: Review ARIA labels and keyboard navigation

## Design Rationale

The Gradient Glass UI was chosen to create a modern, visually appealing interface that feels premium and engaging. The glass morphism creates depth, while gradients add visual interest without being distracting. The large border radius and soft spacing create a friendly, approachable feel appropriate for a social game.

The consistent use of the design system tokens ensures maintainability and visual coherence across all screens.
