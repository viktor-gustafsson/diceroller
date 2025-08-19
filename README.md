# DiceRoller

A small, flexible dice roller for tabletop games and simulations. DiceRoller evaluates standard dice notation (e.g. `2d6+3`) and returns both the individual die results and the computed total. This repository contains the implementation, examples, and tests.

Features
- Parse and evaluate dice notation (e.g. `3d8`, `1d20+5`, `2d6-1`).
- Returns both rolled values and computed total.
- Deterministic seeding option for reproducible rolls (if implemented).
- Small, easy-to-embed API for use in CLIs, bots, or game applications.

Dice expression syntax
- NdM: Roll N dice with M sides (e.g. `3d6` = roll three 6-sided dice).
- +K / -K: Add or subtract an integer modifier (e.g. `1d20+4`).
- d% or d100: Percentile dice (same as `d100`).
- N and M are positive integers. N defaults to 1 when omitted (e.g. `d6` = `1d6`).
- Whitespace is ignored.

Examples
- 1d20 -> rolls: [17], total: 17
- 2d6+3 -> rolls: [2,5], total: 10
