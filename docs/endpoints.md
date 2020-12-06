# Endpoints

## Authenticate

### Endpoint

```HTTP
POST /users/authenticate
```

### Content

```javascript
{
    username: "user",
    password: "password123"
}
```

### Result

```javascript
HTTP/1.1 200 OK
{
    id: "33df9fba-1a02-45c7-afa4-886b6c751e15",
    username: "user
    token:"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.
  eyJpc3MiOiJPbmxpbmUgSldUIEJ1aWxkZXIiLCJpYXQiOjE2MDcwOTM2NDUsImV4
  cCI6MTYzODYyOTY0NSwiYXVkIjoid3d3LmV4YW1wbGUuY29tIiwic3ViIjoianJvY2t
  ldEBleGFtcGxlLmNvbSIsIkdpdmVuTmFtZSI6IkpvaG5ueSIsIlN1cm5hbWUiOiJSb2Nr
  ZXQiLCJFbWFpbCI6Impyb2NrZXRAZXhhbXBsZS5jb20iLCJSb2xlIjpbIk1hbmFnZXIiLCJQ
  cm9qZWN0IEFkbWluaXN0cmF0b3IiXX0.KF1oNcLQ2rcovBWOapa2mh-oIGtmskT5NirenRckLjc"  
}
```

## Register

### Endpoint

```HTTP
POST /users/register
```

### Content

```javascript
{
    username: "user",
    firstName: "name",
    lastName: "surname",
    email: "email@example.com",
    password: "password123"
}
```
### Response

```javascript
HTTP/1.1 201 Created
{
    id: "33df9fba-1a02-45c7-afa4-886b6c751e15",
    username: "user",
    firstName: "name",
    lastName: "surname",
    email: "email@example.com"
}
```

## Get all Libraries

### Endpoint

```HTTP
GET /libraries
```

### Response

```javascript
HTTP/1.1 200 OK
[
    TODO
]
```

## Get Library

### Endpoint

```HTTP
GET /libraries/{name}
```

### Response

```javascript
HTTP/1.1 200 OK
{
    TODO
}
```

## Add Library

### Endpoint

```HTTP
POST /libraries
```

### Content

```javascript
{
    TODO
}
```

### Response

```javascript
HTTP/1.1 201 Created
{
    TODO
}
```

## Update Library

### Endpoint

```HTTP
PUT /libraries/{name}
```

### Content

```javascript
{
    TODO
}
```

### Response

```HTTP
HTTP/1.1 204 No Content
```

## Remove Library

### Endpoint

```HTTP
DELETE /libraries/{name}
```

### Response

```HTTP
HTTP/1.1 200 OK
{
    TODO
}
```




