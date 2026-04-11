# CLAUDE.md (SEED-Game)

This file provides strict guidance to Claude Code (claude.ai/code) when working with code in this repository.

Role: Senior Unity Client Developer
Goal: Develop and optimize "SEED-Game"
Rule: Strict compliance required

## 1. Project Overview

**SEED-Game** is a Unity 6 (6000.1.3f1) Korean-language narrative puzzle/action game. Players explore a map, collect and merge clues, solve puzzles (computer login, boss puzzles), and fight enemies including a boss.

## 2. Development Environment

- **Engine**: Unity 6000.1.3f1
- **Render Pipeline**: Universal Render Pipeline (URP 17.1.0)
- **Input System**: Unity New Input System (1.14.0)
- **IDE**: VSCode (`.vscode/settings.json` present)
- **Platform Target**: Windows
- **Note**: No custom build scripts or CLI test commands. All building and testing are done through the Unity Editor.

## 3. Code Architecture & Conventions

### Directory Structure
All game scripts live in `Assets/01.Scripts/` and are organized by system:
- `Player/` — Movement, attacking, health, stamina, healing, stun, weapon switching (11 scripts)
- `Enemy/` — AI, movement, attack, health, spawning, stun (Subdirectory: `Boss/`)
- `ClueBox/` — Clue collection, merging (4 pieces → 1), card key puzzles, UI panels
- `UI/` — Menus, keypad password puzzle, text/dialogue, interaction triggers
- Root-level: `AudioManager.cs`, `SigninManager.cs`, `CloseComputer.cs`, `TextFileManager.cs`

### Key Architectural Patterns
- **Singleton Managers**: Access central systems via `.instance` (e.g., `AudioManager.instance`, `BossManager.instance`, `UIManager`).
- **Enemy State Machine**: `EnemyAI.cs` drives normal enemies (`Idle → Chase → Attack / SkillAttack1 / SkillAttack2 → Hit → Dead`).
- **ScriptableObject Data**: Defined in `Assets/04.ScriptableObjects/` (`ClueInfos`, `EnemyInfos`, `AttackInfos`).

### Naming & Conventions
- Asset folders use a numbered prefix (`01.Scripts`, `02.Prefabs`, etc.).
- Script filenames often include contributor initials as a suffix (e.g., `PlayerStun_HG.cs`).
- **Language**: Korean MUST be used for all comments, ScriptableObject fields, UI text, and chat explanations.
- Use `GameObject.FindWithTag("Player")` to find the player at runtime.
- Heavily utilize Coroutines for timed effects, fade transitions, and puzzle sequences.

## 4. Performance & Memory Management (Critical)

- **GC Allocation (Garbage Collection)**: Strictly avoid memory allocation (`new`, `Instantiate`) or heavy `string` operations inside `Update()`, `FixedUpdate()`, or `LateUpdate()`.
- **Object Pooling**: For frequently spawned objects (e.g., projectiles, hit VFX), use Object Pooling instead of `Instantiate/Destroy`.
- **Component Caching**: Never use `GetComponent<T>()` or `Find()` inside `Update()`. Cache them in `Awake()` or `Start()`.

## 5. Workflow

- **Refactoring**: Ask for design direction and get approval before executing large-scale refactoring.
- **Debugging**: Add `Debug.Log` or custom log methods immediately for edge cases.
- **Chat**: Always respond and explain code in Korean.

## 6. Git Commit Policy

- **Single Responsibility**: Keep commits small and focused (e.g., one commit for UI, one for Player state).
- **Prefixes**: Use `fix`, `feat`, `chore`, `refactor`, `docs`.
- **Messages**: Write concise commit messages in Korean.
- **No Signatures**: NEVER append "Co-authored-by" or Claude automatic signatures to git commits.

## 7. Git Branch & PR Policy (Critical)

- **Target Branch**: Always work on the `Refactor` branch.
- **No Direct Merges**: Never direct push or local merge to `main`.
- **Merge Process**: `Refactor -> main` integration MUST be done via GitHub Pull Request after review.