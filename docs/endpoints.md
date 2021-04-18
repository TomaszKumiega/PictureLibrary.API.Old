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
GET /libraries/all
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
GET /libraries/library/?fullName={fullName}
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
PUT /libraries/{name}
```

**Content**

```json
{
    "fullName": "F:\\PictureLibraryFileSystem\\lib11-86e5daee-4542-4457-aa9c-769023be9d20\\lib11.plib",
    "name": "lib11",
    "description": "desccc",
    "tags": [],
    "images": [],
    "owners": [
        "d645a27e-b9ae-489a-b51c-71b0ca4faa40"
    ]
}
```

**Response**

```http
HTTP/1.1 204 No Content
```

### Remove Library

**Endpoint**

```http
DELETE /libraries/?fullName={fullName}
```

**Response**

```http
HTTP/1.1 200 OK
```

```json
{
    "fullName": "F:\\PictureLibraryFileSystem\\lib11-86e5daee-4542-4457-aa9c-769023be9d20\\lib11.plib",
    "name": "lib11",
    "description": "desccc",
    "tags": [],
    "images": [],
    "owners": [
        "d645a27e-b9ae-489a-b51c-71b0ca4faa40"
    ]
}
```

## Images

### Get All Images From Library

**Endpoint**

```http
GET /images/all/?libraryFullName={libraryFullName}
```

**Response**

```http
HTTP/1.1 200 OK
```

```json
{
    [
        {[72,161,184,127,50,31,38,54,98,14,112,188,100,156,103,25,61,135,39,214,189,136,70,21,169,181,174,135,230,25,127,16,230,56,220,66,117,33,203,125,78,90,41,214,107,143,37,167,   59,136,203,44,135,24,247,31,165,116,154,135,135,87,196,118,242,219,181,164,50,196,80,199,229,196,138,165,136,30,188,127,147,245,175,159,199,84,167,66,14,117,29,145,247,216,105,85,168,226,233,187,159,26,124,97,248,27,22,168,147,75,109,167,92,67,125,110,26,40,161,129,148,40,193,200,224,96,119,245,175,136,238,188,53,227,239,6,106,171,101,107,107,168,155,113,106,226,234,221,84,133,9,184,54,85,199,221,113,215,35,177,199,67,92,56,108,109,41,97,157,57,84,186,119,243,47,63,203,243,124,54,26,57,134,5,105,43,38,122,238,131,111,47,136,96,179,185,213,85,162,142,230,117,138,225,100,203,60,106,59,241,201,233,142,5,112,30,46,248,65,253,179,111,127,61,188,108,161,119,72,50,163,247,171,208,149,30,160,119,246,226,190,99,50,171,39,38,233,45,182,60,124,46,58,84,36,253,190,214,63,255,217]},
        {[72,161,184,127,50,31,38,54,98,14,112,188,100,156,103,25,61,135,39,214,189,136,70,21,169,181,174,135,230,25,127,16,230,56,220,66,117,33,203,125,78,90,41,214,107,143,37,167,   59,136,203,44,135,24,247,31,165,116,154,135,135,87,196,118,242,219,181,164,50,196,80,199,229,196,138,165,136,30,188,127,147,245,175,159,199,84,167,66,14,117,29,145,247,216,105,85,168,226,233,187,159,26,124,97,248,27,22,168,147,75,109,167,92,67,125,110,26,40,161,129,148,40,193,200,224,96,119,245,175,136,238,188,53,227,239,6,106,171,101,107,107,168,155,113,106,226,234,221,84,133,9,184,54,85,199,221,113,215,35,177,199,67,92,56,108,109,41,97,157,57,84,186,119,243,47,63,203,243,124,54,26,57,134,5,105,43,38,122,238,131,111,47,136,96,179,185,213,85,162,142,230,117,138,225,100,203,60,106,59,241,201,233,142,5,112,30,46,248,65,253,179,111,127,61,188,108,161,119,72,50,163,247,171,208,149,30,160,119,246,226,190,99,50,171,39,38,233,45,182,60,124,46,58,84,36,253,190,214,63,255,217],
        {[72,161,184,127,50,31,38,54,98,14,112,188,100,156,103,25,61,135,39,214,189,136,70,21,169,181,174,135,230,25,127,16,230,56,220,66,117,33,203,125,78,90,41,214,107,143,37,167,   59,136,203,44,135,24,247,31,165,116,154,135,135,87,196,118,242,219,181,164,50,196,80,199,229,196,138,165,136,30,188,127,147,245,175,159,199,84,167,66,14,117,29,145,247,216,105,85,168,226,233,187,159,26,124,97,248,27,22,168,147,75,109,167,92,67,125,110,26,40,161,129,148,40,193,200,224,96,119,245,175,136,238,188,53,227,239,6,106,171,101,107,107,168,155,113,106,226,234,221,84,133,9,184,54,85,199,221,113,215,35,177,199,67,92,56,108,109,41,97,157,57,84,186,119,243,47,63,203,243,124,54,26,57,134,5,105,43,38,122,238,131,111,47,136,96,179,185,213,85,162,142,230,117,138,225,100,203,60,106,59,241,201,233,142,5,112,30,46,248,65,253,179,111,127,61,188,108,161,119,72,50,163,247,171,208,149,30,160,119,246,226,190,99,50,171,39,38,233,45,182,60,124,46,58,84,36,253,190,214,63,255,217]}
   
    ]
}
```

### Get Specific Image

**Endpoint**

```http
GET /images
```

**Content**

```json
{"name":"Name","extension":0,"fullName":"gda\\gadgad\\name.jpg","libraryFullName":"gdaga\\gadda\\lib.plib","creationTime":"2021-04-18T19:48:54.7515726+02:00","lastAccessTime":"2021-04-18T19:48:54.7674382+02:00","lastWriteTime":"2021-04-18T19:48:54.7675470+02:00","size":509,"tags":[]}
```

**Response**

```http
HTTP/1.1 200 OK
```

```json
 {[72,161,184,127,50,31,38,54,98,14,112,188,100,156,103,25,61,135,39,214,189,136,70,21,169,181,174,135,230,25,127,16,230,56,220,66,117,33,203,125,78,90,41,214,107,143,37,167,   59,136,203,44,135,24,247,31,165,116,154,135,135,87,196,118,242,219,181,164,50,196,80,199,229,196,138,165,136,30,188,127,147,245,175,159,199,84,167,66,14,117,29,145,247,216,105,85,168,226,233,187,159,26,124,97,248,27,22,168,147,75,109,167,92,67,125,110,26,40,161,129,148,40,193,200,224,96,119,245,175,136,238,188,53,227,239,6,106,171,101,107,107,168,155,113,106,226,234,221,84,133,9,184,54,85,199,221,113,215,35,177,199,67,92,56,108,109,41,97,157,57,84,186,119,243,47,63,203,243,124,54,26,57,134,5,105,43,38,122,238,131,111,47,136,96,179,185,213,85,162,142,230,117,138,225,100,203,60,106,59,241,201,233,142,5,112,30,46,248,65,253,179,111,127,61,188,108,161,119,72,50,163,247,171,208,149,30,160,119,246,226,190,99,50,171,39,38,233,45,182,60,124,46,58,84,36,253,190,214,63,255,217]}
```

### Add Image

**Endpoint**

```http
POST /images
```

**Content**

```json
{
 "imageContent": [0,94,227,39,41,159,85,95,56,192,225,98,156,37,205,253,124,206,219,194,191,18,31,196,113,95,111,145,110,53,25,88,181,132,112,128,132,12,131,200,35,158,50,56,238,125,176,125,187,194,145,235,186,236,150,114,106,9,114,237,118,12,88,135,31,187,93,216,195,122,255,0,159,90,234,159,179,133,87,73,45,79,63,54,227,58,152,108,154,120,218,20,249,211,209,35,234,173,11,224,197,245,205,181,180,247,211,155,27,91,128,178,180,158,65,45,36,75,156,34,158,64,201,24,39,131,215,21,165,174,120,22,223,66,183,204,230,72,161,184,127,50,31,38,54,98,14,112,188,100,156,103,25,61,135,39,214,189,136,70,21,169,181,174,135,230,25,127,16,230,56,220,66,117,33,203,125,78,90,41,214,107,143,37,167,0,59,136,203,44,135,24,247,31,165,116,154,135,135,87,196,118,242,219,181,164,50,196,80,199,229,196,138,165,136,30,188,127,147,245,175,159,199,84,167,66,14,117,29,145,247,216,105,85,168,226,233,187,159,26,124,97,248,27,22,168,147,75,109,167,92,67,125,110,26,40,161,129,148,40,193,200,224,96,119,245,175,136,238,188,53,227,239,6,106,171,101,107,107,168,155,113,106,226,234,221,84,133,9,184,54,85,199,221,113,215,35,177,199,67,92,56,108,109,41,97,157,57,84,186,119,243,47,63,203,243,124,54,26,57,134,5,105,43,38,122,238,131,111,47,136,96,179,185,213,85,162,142,230,117,138,225,100,203,60,106,59,241,201,233,142,5,112,30,46,248,65,253,179,111,127,61,188,108,161,119,72,50,163,247,171,208,149,30,160,119,246,226,190,99,50,171,39,38,233,45,182,60,124,46,58,84,36,253,190,214,63,255,217],"imageFile":{"name":"Name","extension":0,"fullName":"gda\\gadgad\\name.jpg","libraryFullName":"gdaga\\gadda\\lib.plib","creationTime":"2021-04-18T19:52:25.4168151+02:00","lastAccessTime":"2021-04-18T19:52:25.4324204+02:00","lastWriteTime":"2021-04-18T19:52:25.4325436+02:00","size":509,"tags":[]}}
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

### Update image file

**Endpoint**

```http
PUT /images/imageFile
```

**Content**

```json
{"name":"Name","extension":0,"fullName":"gda\\gadgad\\name.jpg","libraryFullName":"gdaga\\gadda\\lib.plib","creationTime":"2021-04-18T19:52:25.4168151+02:00","lastAccessTime":"2021-04-18T19:52:25.4324204+02:00","lastWriteTime":"2021-04-18T19:52:25.4325436+02:00","size":509,"tags":[]}}
```

**Response**

```http
HTTP/1.1 204 No Content
```

### Update image content

**Endpoint**

```http
PUT /images/image
```

**Content**

```json
{
 "imageContent": [0,94,227,39,41,159,85,95,56,192,225,98,156,37,205,253,124,206,219,194,191,18,31,196,113,95,111,145,110,53,25,88,181,132,112,128,132,12,131,200,35,158,50,56,238,125,176,125,187,194,145,235,186,236,150,114,106,9,114,237,118,12,88,135,31,187,93,216,195,122,255,0,159,90,234,159,179,133,87,73,45,79,63,54,227,58,152,108,154,120,218,20,249,211,209,35,234,173,11,224,197,245,205,181,180,247,211,155,27,91,128,178,180,158,65,45,36,75,156,34,158,64,201,24,39,131,215,21,165,174,120,22,223,66,183,204,230,72,161,184,127,50,31,38,54,98,14,112,188,100,156,103,25,61,135,39,214,189,136,70,21,169,181,174,135,230,25,127,16,230,56,220,66,117,33,203,125,78,90,41,214,107,143,37,167,0,59,136,203,44,135,24,247,31,165,116,154,135,135,87,196,118,242,219,181,164,50,196,80,199,229,196,138,165,136,30,188,127,147,245,175,159,199,84,167,66,14,117,29,145,247,216,105,85,168,226,233,187,159,26,124,97,248,27,22,168,147,75,109,167,92,67,125,110,26,40,161,129,148,40,193,200,224,96,119,245,175,136,238,188,53,227,239,6,106,171,101,107,107,168,155,113,106,226,234,221,84,133,9,184,54,85,199,221,113,215,35,177,199,67,92,56,108,109,41,97,157,57,84,186,119,243,47,63,203,243,124,54,26,57,134,5,105,43,38,122,238,131,111,47,136,96,179,185,213,85,162,142,230,117,138,225,100,203,60,106,59,241,201,233,142,5,112,30,46,248,65,253,179,111,127,61,188,108,161,119,72,50,163,247,171,208,149,30,160,119,246,226,190,99,50,171,39,38,233,45,182,60,124,46,58,84,36,253,190,214,63,255,217],"imageFile":{"name":"Name","extension":0,"fullName":"gda\\gadgad\\name.jpg","libraryFullName":"gdaga\\gadda\\lib.plib","creationTime":"2021-04-18T19:52:25.4168151+02:00","lastAccessTime":"2021-04-18T19:52:25.4324204+02:00","lastWriteTime":"2021-04-18T19:52:25.4325436+02:00","size":509,"tags":[]}}
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
{"name":"Name","extension":0,"fullName":"gda\\gadgad\\name.jpg","libraryFullName":"gdaga\\gadda\\lib.plib","creationTime":"2021-04-18T19:52:25.4168151+02:00","lastAccessTime":"2021-04-18T19:52:25.4324204+02:00","lastWriteTime":"2021-04-18T19:52:25.4325436+02:00","size":509,"tags":[]}
```

### Get all icons of specified files from library

**Endpoint**

```http
GET /images/icons/?libraryFullName={libraryFullName}
```

**Content**

```json
["gdadgagadg\\gdagda\\gdagadg\\gadgadg\\image1.jpg","gdagadg\\gada\\gdad\\gdaimage2.jpg","gadgda\\gadgad\\gdaah\\had\\image3.jpg"]
```

**Response**

```http
HTTP/1.1 200 OK
```

```json
    TODO
```

### Get icon of an image from specified library

**Endpoint**

```http
GET /images/icon/?libraryFullName={libraryFullName}&imageFullName={imageFullName}
```

**Response**

```http
HTTP/1.1 200 OK
```

```json
    TODO
```


