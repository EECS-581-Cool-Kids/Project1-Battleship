# Battleship
This is a Battleship game implementation in C# using the Monogame engine. Only one team member had prior C# experience,
and no team member had experience with the Monogame engine. Setting up the environment and learning the engine should
not provide major hurtles and the code should be relatively easy to understand. Textures were produced in Asesprite, a 
common pixel art tool, and the game was developed in Visual Studio 2022 and Jetbrains Rider.

## Table of Contents
- [Required: Monogame engine setup](#required-monogame-engine-setup)
- [Optional: Aseprite installation](#optional-aseprite-installation)
- [Estimated Time Required](#estimated-time-required)
- [Actual Time Required](#actual-time-required)

### Required: Monogame engine setup
NOTE: these instructions are copied from the Monogame website at https://docs.monogame.net/articles/getting_started/aindex.html. 
If you have any questions or issues, please refer to the original documentation and their support forums. Our team had no 
major issues with the setup process using both the Visual Studio and Jetbrains Rider IDEs.
1. OS specific setup
    - Windows/OSX: install .NET 8 SDK from https://dotnet.microsoft.com/en-us/download/dotnet/8.0
2. IDE setup
    - Visual Studio 2022
      - ensure that the .Net desktop development workload is installed
      - install the MonoGame Framework C# project templates extension from the Visual Studio extension marketplace
      - restart Visual Studio and allow it to install the MonoGame project templates
    - Jetbrains Rider
      - from the terminal or Powershell run 
      
      ```dotnet new install MonoGame.Templates.CSharp```
    
      - install the Monogame plugin if it is not already installed
3. clone this repository and dive in!


### Notes on IDE setup and code structure:
- to add textures to the game, add them to the Content directory and make sure they are added to the Content.mgcb file.
    - in Visual Studio, right click on the Content.mgcb file and select "Open With" and choose "MonoGame Pipeline Tool"
    - in Rider, you may have to manually add entries to the Content.mgcb file but the format is simple and easy to 
    understand based on what is already there.
  

## Optional: Aseprite installation
#### **You only need Aseprite if you want to create or modify the pixel art textures used in the game.**
  - other free tools exist including online pixel art sites whic would also work but the Content directory does include the 
  original .aseprite files with the textures in addition to the .png files.
- Aseprite is available for purchase from the Steam store and other online sources. 
- Alternatively, you can compile it for free from the source code available at https://github.com/aseprite/aseprite
  - steps for compiling are listed below
  
NOTE: these instructions are adapted from the Aseprite github installation guide at https://github.com/aseprite/aseprite/blob/main/INSTALL.md.
If you have any questions or issues, please refer to the original documentation and their support forums.
1. install the dependencies
    - Install Ninja if it is not already installed
      - https://ninja-build.org/
    - install a precompiled Skia release such as Skia-m102
      - https://github.com/aseprite/skia/releases
2. clone the repository to your desired installation location

```git clone --recursive https://github.com/aseprite/aseprite.git```

3. build the project
    - in the terminal or Powershell, navigate to the aseprite directory and run the following commands
        - enter the directory and create a build directory
      
        ``` cd \INSTALL_PATH\aseprite && mkdir build ```
        - execute the cmake command
  
        ``` cd \INSTALL_PATH\aseprite\build && cmake -G Ninja -DLAF_BACKEND=skia ..```
        - compile the project

        ``` cd \INSTALL_PATH\\aseprite\build && ninja aseprite```
    - you should now have a compiled version of Aseprite.exe in the build/bin directory

### Notes on Aseprite and texture creation
- the color palette used was "endesga-64-32x" which is included in the Content directory
    - move `endesga-64-32x.png` to the Aseprite palettes directory `INSTALL_PATH\Aseprite.v1.3.7\palettes`
    - it should now be visible in the Aseprite palette preset list
      - if Aseprite was open when you moved the file, you will need to refresh the preset list


# Estimated Time Required
(copied from original team google sheet)

- 5 days x 1 hours/day + 2 days x 3  hours/day = **12 hours per team member per week**
- 12 hours per week x 2 weeks = **24 hours per team member total**
- 5 team members x 24 hours = **120 hours estimated total time required**

- this assumes team members will work 1 hour per day every weekday on average and 3 hours per day on weekends


# Actual Time Required
(copied from original team google sheet)

### Week 1
| Member    | 9/2   | 9/3     | 9/4     | 9/5   | 9/6     | 9/7   | 9/8    | Total    |
|-----------|-------|---------|---------|-------|---------|-------|--------|----------|
| Derek     | 0     | 0       | 0.5     | 1     | 1       | 1     | 0      | 3.5      |
| Ethan     | 0     | 0       | 0       | 0     | 1       | 1     | 0      | 2        |
| Mo        | 0     | 2       | 4       | 3     | 2       | 0     | 4      | 15       |
| Jacob     | 2     | 2       | 0.5     | 0     | 0.5     | 0     | 4      | 9        |
| Richard   | 0     | 1.5     | 0.5     | 0     | 0       | 2     | 4      | 8        |
| **Total** | **2** | **5.5** | **5.5** | **4** | **3.5** | **4** | **13** | **37.5** |


### Week 2
| Member     | 9/9 | 9/10 | 9/11 | 9/12 | 9/13 | 9/14 | 9/15 | Total |
|------------|-----|------|------|------|------|------|------|-------|
| Derek      | -   | -    | -    | 1    | -    | -    | -    | -     |
| Ethan      | 1.5 | 3    | 4    | 2    | 4    | -    | -    | -     |
| Mo         | 3   | 5    | -    | 4    | 3    | -    | -    | -     |
| Jacob      | 1.5 | 2    | 1    | 1    | 0    | 0    | 0    | 5.5   |
| Richard    | 4   | 4    | 0    | 1    | 2    | 4    | -    | -     | 
| **Total**  | **10**| **14** | **5** | **9** | **9** | **4** | **0** | **-** |