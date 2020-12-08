# Data Model

## User

User is the owner of library.
Diffrent users can have their own libraries on the same device.
API uses [Authentication](https://tomaszkumiega.github.io/PictureLibrary-API/endpoints/#authenticate).

**Properties**

| Name | Type | Notes |
|------|------|-------|
| Id | Guid | User Id | 
| Username | string | - |
| Email | string | - |

## Library

Library represents .xml file that contains albums names and its pictures.

**Xml file structure**

```xml
<?xml version="1.0" encoding="UTF-8"?>
<library name = "libraryName" owner = "userGuid">
    
    <album name = "albumName1">
      <imageFile name = "1642252321.jpg"/>
      <imageFile name = "1246531451.png"/>
    </album>
    
    <album name = "albumName2">
      <imageFile name = "1513515311.jpg"/>
    </album>
    
</library>
```

## Tag

Tags are used for image organization.
All tags existing in a library are stored in library xml file.
Every image is assigned at least one tag.

**Properties**

| Name | Type | Notes |
|------|------|-------|
| Name | string | - |
| Description | string | - |


## ImageFile

ImageFile describes an image and provides a way to access a file from the remote storage.
One image can be stored in multiple albums without multiplication of its file but every library has it's own copy of the image.

**Properties**

| Name | Type | Notes |
|------|------|-------|
| Name | string | Name of the image file without extension |
| Extension | string | File extension (jpg, png etc.) |
| Source | string | Path of the file |
| CreationTime | DateTime | The time of file creation |
| LastAccessTime | DateTime | The time file was last accessed |
| LastWriteTime | DateTime | The time file was last written to |
| Size | long | Size of the file in bytes |
