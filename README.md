# WepApiConsumingSpotifyAPI
A WebApi Consuming SpotifyAPI and made a internal manipulation of data using EntityFramework InMemory Database, with a docker cfg for Linux, but if you will run on docker change the Environment Variables for other kind of External repository of configuration data.


Additional Information:

IDE: Visual Studio Code

.NetCore 2.2

Albums:

https://localhost:5001/api/Albuns/170

https://localhost:5001/api/Albuns?genre=CLASSIC

https://localhost:5001/api/Albuns?page=2&perpage=5

https://localhost:5001/api/Albuns?page=2&perpage=5&genre=CLASSIC

Sales:

https://localhost:5001/api/sales

https://localhost:5001/api/sales/1

https://localhost:5001/api/sales?dateini=26/05/2019&datefin=29/05/2019&page=2&perpage=2

POST https://localhost:5001/api/sales 
BODY: [32,101,92,30](sample Ids of Albums) RAW APPLICATION/JSON (Im using POSTMAN)

_________________________________________________________________________________________

DataBase => InMemory

"Environment Variables" (Containing ClientId and Secret of your register in Spotify)
MySpotifyClientID
MySpotifyClientSecret
