# Running Picture Library API

## Manually 

### 1. [Install Docker](https://www.docker.com/get-started)

### 2. Mount storage

Mount drives that will be used for storing data using this command:

```sh
sudo mergerfs DRIVE_1:DRIVE_2:DRIVE_3 MOUNTDIRECTORY
```

Change DRIVE_1, DRIVE_2, etc... with paths to the drives you would like to mount, and MOUNTDIRECTORY, with a directory they will be mounted to. 

If you want to specify RAID configuration, use this command:

```sh
sudo mdadm --create --verbose /dev/md0 --level=1 /dev/sda1 /dev/sdb2
```

Change /dev/sda1, /dev/sdb2, with drives you would like to mount and level, with RAID level you would like to use.

After creating RAID array mount it to the directory:

```sh
sudo mount /dev/md0 MOUNTDIRECTORY
```

Change MOUNTDIRECTORY, with a directory array will be mounted to.

### 3. Download and setup SQLite

Download SQLite:

```sh
sudo apt-get update
sudo apt-get install sqlite3
```

Create database instance in mounted directory:

```sh
sudo mkdir MOUNTEDDIRECTORY/Database
sudo sqlite3 MOUNTEDDIRECTORY/Database/PictureLibraryAPI.db
```

### 4. Create a self-signed certificate


### 5. Run the container

Run the container by using this command and modifying CONTAINER_NAME with a suitable name and DIRECTORY with a path to a directory, drives are mounted to:

```sh
docker run -d -it -p 5000:5000 -p 5001:5001 --name CONTAINER_NAME --mount type=bind,source=DIRECTORY,target=PictureLibraryFileSystem/ docker.pkg.github.com/tomaszkumiega/picturelibrary-api/picturelibraryapi:master
```

Command for ARM32 architecture machine (RaspberryPi):

```sh
docker run -d -it -p 5000:5000 -p 5001:5001 --name CONTAINER_NAME --mount type=bind,source=DIRECTORY,target=PictureLibraryFileSystem/ docker.pkg.github.com/tomaszkumiega/picturelibrary-api/picturelibraryapi:master
```

## Using configuration app

**SOON**

