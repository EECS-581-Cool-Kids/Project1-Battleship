# Group 1 Documentation

## TODO

- [X] Figure out MacOS installation
- [ ] Add expected person hours and reasoning
- [ ] Decide on a custom addition
  - [ ] Create UML diagram
  - [ ] Implement idea
- [ ] AI Opponent
  - [X] Create AI Opponent Selection interface
  - [ ] Create AI Easy Difficulty Level
  - [ ] Create AI Medium Difficulty Level
  - [ ] Create AI Hard Difficulty Level
  - [ ] Randomize AI Ship Placement
- **Continuous Documentation**
- **Person Hours Record-Keeping**

## Getting Monogame to work on OSX

1) Install Microsoft .NET for Mac using the x64 (and ARM installers for Apple Sillicon)(https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
2) Setup VSCode using this installation guide. Make sure to install the optional MonoGame extension (https://docs.monogame.net/articles/getting_started/2_choosing_your_ide_vscode.html?tabs=macos)
3) Install Homebrew (MacOS package manager) using this website: (https://brew.sh/)
4) Install the necessary libraries using the following command:

    ```bash
    brew install freeimage freetype libpng
    ```

5) Fix a dependency by linking libfreetype's install directory to the one checked by Monogame using the command

    ```bash
    sudo ln -s /usr/local/lib/libfreetype.6.dylib /usr/local/lib/freetype6
    ```

## Header Template

```
Name:
Description:
Inputs:
Outputs:
Collaborators/Sources: Michael Oliver, Peter Pham, Jack Youngquist, Andrew Uriell, Ian Wilson
Created: September 19, 2024
Last Modified: September 19, 2024
```