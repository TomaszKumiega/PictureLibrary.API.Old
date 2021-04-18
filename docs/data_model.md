# Data Model

## User

User is the owner of library.
Diffrent users can have their own libraries on the same device.
One library can have multiple owners.
API uses [Authentication](https://tomaszkumiega.github.io/PictureLibrary-API/endpoints/#authenticate).

**Properties**

| Name | Type | Notes |
|------|------|-------|
| Id | Guid | User Id | 
| Username | string | - |
| Password | string | - |
| Email | string | - |

## Library

Library represents .xml file that contains tags and images.

**Xml file structure**

```xml
<?xml version="1.0" encoding="UTF-8"?>
<library name = "libraryName" description = "Family picture library" owners = "userGuid,userGuid">
    
    <tags>
        <tag name = "Portraits" description = "" color = "#34ebe5"/>
        <tag name = "Vacation" description = "All pictures from vacation time" color = "#eb34b4"/>
    </tags>        
    
    <images>
        <imageFile name = "1642252321" extension = ".jpg" source = "C:/Pictures/Library1/images/1642252321.jpg" tags = "Portraits,Vacation"/>
        
        <imageFile name = "1246531451" extension = ".png" source = "C:/Pictures/Library1/images/1246531451.png" tags = ""/>
        
        <imageFile name = "1513515311" extension = ".jpg" source = "C:/Pictures/Library1/images/1513515311.jpg" tags = "Portraits"/>
    </images>    

</library>
```

**Properties**

| Name | Type | Notes |
|------|------|-------|
| Name | string | Name of the library | 
| FullName | string | Path to the library |
| Description | string | - |
| Tags | List of Tags | List of all tags used in the library |
| Images | List of ImageFiles | List of all files in the library |
| Owners | List of Users | Owners of the library |

## Tag

Tags are used for image organization.
All tags existing in a library are stored in library xml file.
Every image is assigned at least one tag.

**Properties**

| Name | Type | Notes |
|------|------|-------|
| Name | string | - |
| Description | string | - |
| Color | string | Color associated with the tag in HEX |


## ImageFile

ImageFile describes an image and provides a way to access a file from the remote storage.

**Properties**

| Name | Type | Notes |
|------|------|-------|
| Name | string | Name of the image file without extension |
| Extension | string | File extension (jpg, png etc.) |
| FullName | string | Path of the file |
| CreationTime | DateTime | The time of file creation |
| LastAccessTime | DateTime | The time file was last accessed |
| LastWriteTime | DateTime | The time file was last written to |
| Size | long | Size of the file in bytes |
| Tags | List of Tags | - | 
