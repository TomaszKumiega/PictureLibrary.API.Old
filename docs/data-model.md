# Data Model

## User

API has user authentication.
Diffrent users can have their own libraries on the same device.

## Library

Library represents .xml file that contains albums names and its pictures.

### Xml file structure

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

## Album

Element in library.xml file. Organizes images. 


## ImageFile

ImageFile is representation of image on the storage.
One image can be stored in multiple albums without multiplication of its file but every library has it's own copy of the image.
