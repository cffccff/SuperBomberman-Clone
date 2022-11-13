# SuperBomberman-Clone 
- This is cloned game from Super Bomberman 1 of Nintendo made by Unity.
## Rule 
- Rule is the same as bomberman game: Destroy all the enemy in the level, find the gate to go to next level.
## Map 
- Every 2 level will change the map that have difference tile of soft blocks and hard blocks.
- Every 3 level will have a boss.
- Every level will have difference postion of soft blocks, some of hard blocks and enemy. It is random by computer.
- Each level has power up items. It drops when kill enemy or destroy soft blocks.
- There are total 12 level. (you can skip level if you want by press ";").
## Characteristics of enemy
- There are 5 characteristics of enemy in this game.
- Normal enemy: only change its direction when collide with other enemy, bomb or block.
- Aimless enemy: Can random change its direction, although it still changes direction when collide with other enemy or block.
- Follow-player enemy: When detect player in horizontal/ vertical line, it will start chasing player. If it loses the sight of player, it behaves like normal enemy.
- Special enemy: It performs its special skill after random second or have some impact when collide with something, after done it returns normal stage and move.
- Softblock Pass enemy: Like the name of itself, it moves through soft block.
## Enemy
- Each enemy has atleast one characteristic and having 2 or 3 characteristic is the most.
- Each enemy has diffrerence stats like move speed, health.
- There are total 16 type of enemies and 3 bosses.
## When creating this game i use
- Singleton.
- Object Pooling.
- OOP.
- ScriptableObject for store some information.
- Sprite Library to reuse object with same animator.
## Link WebGL
- https://cffccff.itch.io/super-bomberman-clone
