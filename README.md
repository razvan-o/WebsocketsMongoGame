# WebsocketsMongoGame

Steps:
1. Generate users ( GET https://localhost:44367/api/players/generate )
![image](https://user-images.githubusercontent.com/38734444/122840125-2ccd9d00-d302-11eb-986b-817273a03c41.png)

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

![image](https://user-images.githubusercontent.com/38734444/122839012-e0815d80-d2ff-11eb-87b1-eba2aa6261b9.png)

Improvements:
1. persist game status in db - in the event of downtime, restore game status once server is up.
2. schedule jobs to update game status according to desired timeframe:
  - nobody joins in the first 3 minutes since game start -> cancel game
  - first player joins -> 10 min to vote
  - other players join -> they need to vote within the previously initiallized 10 min timer
  - voting time ends -> declare winner + update skillPoints

To auto-test at scale:
Create endpoint that:
1. Generates 1k test users (each having all the others friends)
2. Call Auto-Pilot function 1k times, with different pairs of users as input

The Auto-Pilot function:
1. Sends WS event to service to create a room using player1
2. WS event to join game with player2
3. Ws events to vote for both players
