# Endpoints

## Users

### Authenticate

**Endpoint**

```http
POST /users/authenticate
```

**Content**

```json
{
    "username": "user",
    "password": "password123"
}
```

**Response**

```http
HTTP/1.1 200 OK
```

```json
{
    "id": "33df9fba-1a02-45c7-afa4-886b6c751e15",
    "username": "user",
    "token":"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.
  eyJpc3MiOiJPbmxpbmUgSldUIEJ1aWxkZXIiLCJpYXQiOjE2MDcwOTM2NDUsImV4
  cCI6MTYzODYyOTY0NSwiYXVkIjoid3d3LmV4YW1wbGUuY29tIiwic3ViIjoianJvY2t
  ldEBleGFtcGxlLmNvbSIsIkdpdmVuTmFtZSI6IkpvaG5ueSIsIlN1cm5hbWUiOiJSb2Nr
  ZXQiLCJFbWFpbCI6Impyb2NrZXRAZXhhbXBsZS5jb20iLCJSb2xlIjpbIk1hbmFnZXIiLCJQ
  cm9qZWN0IEFkbWluaXN0cmF0b3IiXX0.KF1oNcLQ2rcovBWOapa2mh-oIGtmskT5NirenRckLjc",
    "refreshToken": "cPDQ46CnG8TCZAsfgCt3LkmTscxhOJlc0nlcCyyvYrM="
}
```

### Register

**Endpoint**

```http
POST /users/register
```

**Content**

```json
{
    "username": "user",
    "password": "password123"
    "email": "email@example.com"
}
```
**Response**

```http
HTTP/1.1 201 Created
```

```json
{
    "id": "33df9fba-1a02-45c7-afa4-886b6c751e15",
    "username": "user",
    "email": "email@example.com"
}
```

### Refresh

Refresh access token

```http
POST /users/refresh
```

**Content**

```json
{
    "token":"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.
  eyJpc3MiOiJPbmxpbmUgSldUIEJ1aWxkZXIiLCJpYXQiOjE2MDcwOTM2NDUsImV4
  cCI6MTYzODYyOTY0NSwiYXVkIjoid3d3LmV4YW1wbGUuY29tIiwic3ViIjoianJvY2t
  ldEBleGFtcGxlLmNvbSIsIkdpdmVuTmFtZSI6IkpvaG5ueSIsIlN1cm5hbWUiOiJSb2Nr
  ZXQiLCJFbWFpbCI6Impyb2NrZXRAZXhhbXBsZS5jb20iLCJSb2xlIjpbIk1hbmFnZXIiLCJQ
  cm9qZWN0IEFkbWluaXN0cmF0b3IiXX0.KF1oNcLQ2rcovBWOapa2mh-oIGtmskT5NirenRckLjc",
    "refreshToken": "cPDQ46CnG8TCZAsfgCt3LkmTscxhOJlc0nlcCyyvYrM="
}
```
**Response**

```http
HTTP/1.1 200 Ok
```

```json
{
    "token": "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.
  eyJpc3MiOiJPbmxpbmUgSldUIEJ1aWxkZXIiLCJpYXQiOjE2MDcwOTM2NDUsImV4
  cCI6MTYzODYyOTY0NSwiYXVkIjoid3d3LmV4YW1wbGUuY29tIiwic3ViIjoianJvY2t
  ldEBleGFtcGxlLmNvbSIsIkdpdmVuTmFtZSI6IkpvaG5ueSIsIlN1cm5hbWUiOiJSb2Nr
  ZXQiLCJFbWFpbCI6Impyb2NrZXRAZXhhbXBsZS5jb20iLCJSb2xlIjpbIk1hbmFnZXIiLCJQ
  cm9qZWN0IEFkbWluaXN0cmF0b3IiXX0.KF1oNcLQ2rcovBWOapa2mh-oIGtmskT5NirenRckLjc",
    "refreshToken": "EfwSrJSnKDdz0PiFfeBmwFFT1i/56/JnwOAGu7UaLFI="

}
```

### Update user

```http
PUT /users/{id}
```

**Content**

```json
{
    "username": "user",
    "password": "password123",
    "email": "email@example.com"
}
```
**Response**

```http
HTTP/1.1 200 Ok
```

## Libraries

### Get all Libraries

**Endpoint**

```http
GET /libraries
```

**Response**

```http
HTTP/1.1 200 OK
```

```json
{
    {"fullName":"\\lib\\lib\\lib.plib","name":"lib","description":"desccc","tags":[{"name":"tag1","description":"dgadgdag","color":"#eavadv55"}],"images":[{"name":"Image","extension":0,"fullName":"\\image\\cos\\well\\Image.jpg","libraryFullName":"\\lib\\lib\\lib.plib","creationTime":"2021-04-18T03:42:11.7968969+02:00","lastAccessTime":"2021-04-18T03:42:11.8108591+02:00","lastWriteTime":"2021-04-18T03:42:11.8109685+02:00","size":500,"tags":[{"name":"tag1","description":"dgadgdag","color":"#eavadv55"}]}],"owners":["690a722a-7f64-4501-b192-869cef69b2e8"]},
    {"fullName":"\\lib\\lib\\lib.plib","name":"lib","description":"desccc","tags":[{"name":"tag1","description":"dgadgdag","color":"#eavadv55"}],"images":[{"name":"Image","extension":0,"fullName":"\\image\\cos\\well\\Image.jpg","libraryFullName":"\\lib\\lib\\lib.plib","creationTime":"2021-04-18T03:42:11.7968969+02:00","lastAccessTime":"2021-04-18T03:42:11.8108591+02:00","lastWriteTime":"2021-04-18T03:42:11.8109685+02:00","size":500,"tags":[{"name":"tag1","description":"dgadgdag","color":"#eavadv55"}]}],"owners":["690a722a-7f64-4501-b192-869cef69b2e8"]},
    {"fullName":"\\lib\\lib\\lib.plib","name":"lib","description":"desccc","tags":[{"name":"tag1","description":"dgadgdag","color":"#eavadv55"}],"images":[{"name":"Image","extension":0,"fullName":"\\image\\cos\\well\\Image.jpg","libraryFullName":"\\lib\\lib\\lib.plib","creationTime":"2021-04-18T03:42:11.7968969+02:00","lastAccessTime":"2021-04-18T03:42:11.8108591+02:00","lastWriteTime":"2021-04-18T03:42:11.8109685+02:00","size":500,"tags":[{"name":"tag1","description":"dgadgdag","color":"#eavadv55"}]}],"owners":["690a722a-7f64-4501-b192-869cef69b2e8"]} 
}
```

### Get Library

**Endpoint**

```http
GET /libraries/{fullName}
```

**Response**

```http
HTTP/1.1 200 OK
```

```json
{"fullName":"\\lib\\lib\\lib.plib","name":"lib","description":"desccc","tags":[{"name":"tag1","description":"dgadgdag","color":"#eavadv55"}],"images":[{"name":"Image","extension":0,"fullName":"\\image\\cos\\well\\Image.jpg","libraryFullName":"\\lib\\lib\\lib.plib","creationTime":"2021-04-18T03:42:11.7968969+02:00","lastAccessTime":"2021-04-18T03:42:11.8108591+02:00","lastWriteTime":"2021-04-18T03:42:11.8109685+02:00","size":500,"tags":[{"name":"tag1","description":"dgadgdag","color":"#eavadv55"}]}],"owners":["690a722a-7f64-4501-b192-869cef69b2e8"]}  
```

### Add Library

**Endpoint**

```http
POST /libraries
```

**Content**

```json
{"fullName":"\\lib\\lib\\lib.plib","name":"lib","description":"desccc","tags":[{"name":"tag1","description":"dgadgdag","color":"#eavadv55"}],"images":[{"name":"Image","extension":0,"fullName":"\\image\\cos\\well\\Image.jpg","libraryFullName":"\\lib\\lib\\lib.plib","creationTime":"2021-04-18T03:42:11.7968969+02:00","lastAccessTime":"2021-04-18T03:42:11.8108591+02:00","lastWriteTime":"2021-04-18T03:42:11.8109685+02:00","size":500,"tags":[{"name":"tag1","description":"dgadgdag","color":"#eavadv55"}]}],"owners":["690a722a-7f64-4501-b192-869cef69b2e8"]}  
```

**Response**

```http
HTTP/1.1 201 Created
```

```json
{
    "fullName": "\\PictureLibraryFileSystem\\lib-49241c7d-9718-46ea-b1b3-1a20552857ce\\lib.plib",
    "name": "lib",
    "description": "desccc",
    "tags": [
        {
            "name": "tag1",
            "description": "dgadgdag",
            "color": "#eavadv55"
        }
    ],
    "images": [
        {
            "name": "Image",
            "extension": 0,
            "fullName": "\\image\\cos\\well\\Image.jpg",
            "libraryFullName": "\\lib\\lib\\lib.plib",
            "creationTime": "2021-04-18T03:42:11.7968969+02:00",
            "lastAccessTime": "2021-04-18T03:42:11.8108591+02:00",
            "lastWriteTime": "2021-04-18T03:42:11.8109685+02:00",
            "size": 500,
            "tags": [
                {
                    "name": "tag1",
                    "description": "dgadgdag",
                    "color": "#eavadv55"
                }
            ]
        }
    ],
    "owners": [
        "690a722a-7f64-4501-b192-869cef69b2e8"
    ]
}
```

### Update Library

**Endpoint**

```http
PUT /libraries/{fullName}
```

**Content**

```json
{
    TODO
}
```

**Response**

```http
HTTP/1.1 204 No Content
```

### Remove Library

**Endpoint**

```http
DELETE /libraries/{name}
```

**Response**

```http
HTTP/1.1 200 OK
```

```json
{
    TODO
}
```

## Images

### Get Images From Library

**Endpoint**

```http
GET /images/all/{libraryName}
```

**Response**

```http
HTTP/1.1 200 OK
```

```json
{
    TODO
}
```

### Get Specific Image

**Endpoint**

```http
GET /images/{imageSource}
```

**Response**

```http
HTTP/1.1 200 OK
```

```json
{
    TODO
}
```

### Add Image

**Endpoint**

```http
POST /images/{libraryName}
```

**Content**

```json
{
    TODO
}
```

**Response**

```http
HTTP/1.1 201 Created
```

```json
{
    TODO
}
```

### Update image

**Endpoint**

```http
PUT /images/{imageSource}
```

**Content**

```json
{
    TODO
}
```

**Response**

```http
HTTP/1.1 204 No Content
```

### Remove image

**Endpoint**

```http
DELETE /images/{imageSource}
```

**Response**

```http
HTTP/1.1 200 OK
```

```json
{
    TODO
}
```




