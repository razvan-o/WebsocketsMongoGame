# WebsocketsMongoGame

Steps:
1. Generate users ( GET https://localhost:44367/api/players/generate )

2. Open 2 (or more) tabs on https://localhost:44367

3. Create a game in tab 1:
  - Player: a playerId generated in step 1 
  - Friends: 1 ore more playerIds separated by comma, no white spaces (skillPoints difference must be 100 max) 
  - GameId: a unique name for your game
  - Click "Start Game"

4. Join a game in tab 2:
  - Player: a playerId among the "Friends" from tab 1
  - Friends: leave empty
  - GameId: same as in tab 1
  - Click "Join Game"

5. Vote in both tabs ( integer 0-100 )

6. Click "End Game" in tab 1 (playerId must be the one who started the game)
    
![image](https://user-images.githubusercontent.com/38734444/122838908-ad3ece80-d2ff-11eb-91b2-65df82e60bb9.png)
