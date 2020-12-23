# Running Picture Library API

### 1. [Install Docker](https://www.docker.com/get-started)

### 2. Run the container using terminal on linux or powershell on windows

When running the container, you need to specify which directories will be used to store data by using --mount syntax.

To specify a directory on host device, where data will be stored, change the source parameter value:

```sh
--mount type=bind,source=MY_DIRECTORY/,target=Directory1
```

When you need to use multiple directories, for example when you want to use multiple drives, use --mount syntax multiple times.
With every new directory, change the number of the target directory as follows:

- Directory1
- Directory2
- Directory3
- ...

```sh
--mount type=bind,source=MY_DIRECTORY/,target=Directory1 --mount type=bind,source=MY_OTHER_DIRECTORY/,target=Directory2
```

**WARNING** 
It's important to name target directories by following this convention, because application uses this naming scheme to recognize where it's supposed to store data.

Run the container by using this command and modifying CONTAINER_NAME and --mount syntax as shown above:

```sh
docker run -d -it -p 5000:5000 -p 5001:5001 --name CONTAINER_NAME --mount type=bind,source=DIRECTORY,target=Directory1/ docker.pkg.github.com/tomaszkumiega/picturelibrary-api/picturelibrary-api:master
```

