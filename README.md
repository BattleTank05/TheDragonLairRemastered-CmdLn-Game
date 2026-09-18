# <<--- Overview --->>
A long while ago, I made a text-based Solo RPG game. The player would choose their class and starting gear, then compete in an arena against a variety of enemies, earning potions and gear as they went.
The old game had turn-based combat, and a clunky, checkerboard layout on which the player and enemies would occupy spaces. On their turn, they could move one space, attack adjacent squares, or use an item/consumable. I also implemented some rather scuffed parry, block, and dodge actions.

This was my first coding project, using C#. While it did work, it was very messy. Since then, I've taken a couple of college courses on programming and have a much better design intuition.

Now, I'm going to remaster my old game. </br>
My main approach to the remaster is to go from a checkerboard arena to procedurally generated dungeons for the player to explore. Each dungeon would have chests and special elite/boss mobs which can be looted for treasure and items. Dungeons would be leveled, with different and more difficult mobs as the player progresses. There will be a final massive dungeon, with a dragon as the last boss.

---

## ----- Core Gameplay -----
Like Darkest Dungeon, this game will auto save constantly. Every action the player takes is permanent.

The goal of the game is to defeat the dragon's lair dungeon. </br>
Each run will be a series of ten procedurally generated dungeons, with the 3rd and 7th dungeons having bosses. The 11th dungeon will be the Dragon's Lair. Later on, bosses may have variants, as seen in Slay the Spire.

Each dungeon will be more difficult than the last, using the un-leveled scaling system from FTL: Faster Than Light.
The loot and enemies of each dungeon will scale with hidden dungeon levels, not the player level. This will make some dungeons more difficult but more rewarding than others.
For each dungeon, the player will be given descriptions of 2-4 options. These options will be procedurally generated based on the hidden dungeon level.
The player will have to choose the dungeon most likely to provide enough loot to take on the following dungeon.

Inside each dungeon, the player will explore, find treasure, defeat monsters, navigate traps and labyrinths, and find clues about the lore of the world.
Some dungeons may have NPCs which the player can interact with. A few of these NPCs may have quests.

After picking a dungeon the player will arrive in its starting room. From here, the player will be given the descriptions of the doors/passages adjacent to their current room. The player will pick which room to enter next. This will continue until the end of the dungeon, where the player will exit and this loop will repeat.
The player can retreat from a dungeon by returning to the entrance and leaving. There is no penalty for leaving early, except for missed loot.
However, a dungeon will still be considered complete if left early, meaning the player will have to face the next difficulty level without having what necessary loot the abandoned dungeon may have had.
</br>Boss Dungeons(3rd, 7th, 11th) cannot be abandoned.

Various actions will reward the player with experience points (XP). Upon aqcuiring enough XP, the player will level, increasing their stats and available skills.
Like DnD, the max level the player can reach is 20. The final dungeon will be designed for a level 17 player.
If the player takes the minimum possible risk, they should still reach levels 14 or 15 by the final dungeon.

After completing a dungeon, the player will go to town. Here, they can trade their loot for valuable gear and supplies. Some NPCs and Quests may involve town.

Upon defeating the Dragon's Lair, the player wins and the game will end.</br>
After winning, the next level of difficulty will be unlocked.</br>
Game difficulty will affect the RNG within dungeons.

---

## ----- Player -----
The player will have a class. This class determines what special abilities they have, the type of gear they will find, and how their stats are distributed.

Various actions will reward the player with Experience Points (XP).</br>
The player will have a starting level and can level up by acquiring enough XP. Leveling up will increase stats, upgrade class abilities, unlock new class abilities, and increase the chances of surviving more difficult dungeons. These dungeons will be more rewarding, but also more risky!

The player will have starting gear, and will find better equipment as they explore. Equipment is static, and provides either stat boosts or access to special abilities.

---

## ----- Dungeon Exploration -----
Dungeons will have a type. The type determines the theme of the dungeon, the types of inhabiting monsters, and the type of loot inside.
Dungeons will have a set of rooms, and each room will have links to the next. The types of the rooms, and the weight of each spawn will be determined by the dungeon's "generation" variables.

Some dungeons may have quests. These quests will follow a system inspired by FTL: Faster Than Light. Quests will involve traveling to specific rooms or specific dungeons, encountering specific NPCs, killing certain enemies, and obtaining specific quest items. Quests will be difficult, but will give the player permanent rewards, such as new classes to play.

Rooms will have types. The type determines the encounter inside the room.

I may also implement a town, which will act as a base and trade station which the player can return to in between dungeons.

---

## ----- Combat -----
Combat is going to be thoroughly overhauled. Instead of moving on the same board and whacking enemies repetitively until they die, new mechanics like stance, momentum, and reach will add a tactical layer to each fight. Winning will no longer be based on stats and rng, but will require strategy and planning.

---

## ----- Monsters -----
Monsters will have a race. Race determines the monster's stats, gear, and abilities.
Monsters will have a level. This is a static variable set on creation that scales the monster's stats
Monsters will have a fighting style. This is the AI behind the monster's tactical decisions.
Monsters will have an attitude. The attitudes, Hostile, Neutral, and Friendly determine how the monster interacts with the player.
Monsters will have a loot table, which will be used by the encounter manager to determine the reward for combat.
Elite enemies will be stronger variants of regular monsters. These may have special abilities and loot.
Bosses will be stronger variants of Elite enemies. These may have unique abilities and loot.

---

## ----- Loot -----
Loot will have a type. The type determines whether the item can be sold for gold, equipped for stats/abilities, consumed for special effects, or is a quest item that can be used in a dungeon.

---

## ----- NPCs -----
NPCs are characters with whom the player can interact through dialogue.
NPCs will have a race. This determines some possible interactions.
NPCs will have an attitude(Wary, Neutral, Friendly) which determines some possible interactions.
NPCs will have a type(Merchant, Quest) which determines some possible interactions.

---

## ----- Quests -----
 *To be expanded later in the project*

---

## ----- Town -----
 *To be expanded later in development*

---

## ----- Difficulty -----
Beating the game will unlock new difficulty and game options.
#### Difficulty modes:
 - Coward (easy) - This will be the starting difficulty, for the tutorial
 - Stalwart (normal) - unlocked after completing the tutorial.
 - Valor (hard) - unlocked after completing Stalwart
 - Honor (hard, +10 dungeons, steeper enemy scaling) - unlocked after completing Valor
 - Legacy (hard, +20 dungeons, steeper enemy scaling, reduced gold looted) - unlocked after completing honor

#### Potential later options:
 - Heroic (legacy, but with alternating boss variants)

#### Difficulty primarily affects the RNG within Dungeons:
 - Less loot, but better items
 - More monsters
 - More traps
 - Higher chance to encounter NPCs, Quests, and Lore clues

---