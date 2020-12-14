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

Library represents .xml file that contains tags and images.

**Xml file structure**

```xml
<?xml version="1.0" encoding="UTF-8"?>
<library name = "libraryName" description = "Family picture library" owner = "userGuid">
    
    <tags>
        <tag name = "Portraits" description = "" />
        <tag name = "Vacation" description = "All pictures from vacation time" />
    </tags>        
    
    <images>
        <imageFile name = "1642252321" extension = "jpg" source = "C:/Pictures/Library1/images/1642252321.jpg" CreationTime = "2008-10-01T17:04:32.0000000" 
                   LastAccessTime = "2008-10-01T17:04:32.0000000" LastWriteTime = "2008-10-01T17:04:32.0000000" size = "5130500"/>
        
        <imageFile name = "1246531451" extension = "png" source = "C:/Pictures/Library1/images/1246531451.png" CreationTime = "2008-10-01T17:04:32.0000000" 
                   LastAccessTime = "2008-10-01T17:04:32.0000000" LastWriteTime = "2008-10-01T17:04:32.0000000" size = "3003000"/>
        
        <imageFile name = "1513515311" extension = "jpg" source = "C:/Pictures/Library1/images/1513515311.jpg" CreationTime = "2008-10-01T17:04:32.0000000" 
                   LastAccessTime = "2008-10-01T17:04:32.0000000" LastWriteTime = "2008-10-01T17:04:32.0000000" size = "2050000" />
    </images>    

</library>
```

**Properties**

| Name | Type | Notes |
|------|------|-------|
| Name | string | Name of the library | 
| FullPath | string | Full path to the library |
| Description | string | - |
| Tags | List<Tag> | List of all tags used in the library |
| Images | List<ImageFIle> | List of all files in the library |
| Owner | User | Owner of the library |

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
