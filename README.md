# TicTacToe.WebApi

# Using SQL Server database
You can create migration to postgers or e.t.c for example:
 1) Change Program.cs configuration to use Npgsql or e.t.c.
 2) You need to Remove-Migration, then Add-Migration
 3) Migration will automatically applied to database at the start of application.
# Authorization with jwt tokens.
1) You need to register user .../api/Auth/register
2) You need to login .../api/Auth/login. After successful login it will responce with JWT-token. To use this u need to add header "Authorization" with values "Bearer {token}" (In token u paste ur token, without {} brackets).
# Playing:
1) You can access to already created games using .../api/Games/games
2) You can create ur own game using .../api/Games/create
3) If u want to join some game u need to use .../api/Games/join?gameId={GameId}.
4) After this u can use .../api/Games/move?gameId={GameId} with move body.
