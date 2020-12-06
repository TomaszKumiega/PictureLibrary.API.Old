# Endpoints

## Authenticate

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

**Result**

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
  cm9qZWN0IEFkbWluaXN0cmF0b3IiXX0.KF1oNcLQ2rcovBWOapa2mh-oIGtmskT5NirenRckLjc"  
}
```

## Register

**Endpoint**

```http
POST /users/register
```

**Content**

```json
{
    "username": "user",
    "firstName": "name",
    "lastName": "surname",
    "email": "email@example.com",
    "password": "password123"
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
    "firstName": "name",
    "lastName": "surname",
    "email": "email@example.com"
}
```

## Get all Libraries

**Endpoint**

```http
GET /libraries
```

**Response**

```http
HTTP/1.1 200 OK
```

```json
[
    TODO
]
```

## Get Library

**Endpoint**

```http
GET /libraries/{name}
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

## Add Library

**Endpoint**

```http
POST /libraries
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

## Update Library

**Endpoint**

```http
PUT /libraries/{name}
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

## Remove Library

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

## Get Images From Library

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

## Get Specific Image

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

## Add Image

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

## Update image

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

## Remove image

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




