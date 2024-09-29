# Group 1 Documentation

## TODO

- [X] Figure out MacOS installation
- [X] Add expected person hours and reasoning
- [X] Decide on a custom addition
  - [X] Create UML diagram
  - [ ] Implement idea
- [X] AI Opponent
  - [X] Create AI Opponent Selection interface
  - [X] Create AI Easy Difficulty Level
  - [X] Create AI Medium Difficulty Level
  - [X] Create AI Hard Difficulty Level
  - [X] Randomize AI Ship Placement
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
# BattleshipGame.cs
## `Initialize` 

The `Initialize` method sets up the game environment for Battleship by configuring graphics, initializing game objects, and establishing event handlers.

## Purpose

- Configure the game window size and title.
- Initialize player grids, ship manager, and turn manager.
- Set up event handling for ship placement and turn management.

## `LoadContent`

The `LoadContent` method is responsible for loading game assets and initializing game components for both the main menu and the gameplay phase of the Battleship game.

## Purpose

- Load fonts and content for the main menu and ship selection if the game has not started.
- Initialize game objects and event handlers when the game is in progress.

## `Update`

The `Update` method manages game state transitions, updates game objects, handles player input, and controls game logic in the Battleship game.

## Purpose

- To handle input for exiting the game and transitioning between menus and gameplay.
- To update game objects based on the current game state.
- To manage ship placements and shooting logic for players.

## Functionality

### Exit Conditions

- Exits the game if the back button or the escape key is pressed.

### Menu Updates (When Not in Game)

1. **Main Menu**:
   - Updates the main menu.
   - Transitions to ship selection or settings based on user selection.
   - Exits the game if "Exit" is selected.

2. **Ship Selection Menu**:
   - Updates the ship selection menu.
   - Transitions to gameplay if "Start Game" is clicked.
   - Returns to the main menu if the back button is clicked.

3. **Settings Menu**:
   - Updates settings and applies selected difficulty.
   - Returns to the main menu if the back button is clicked.

### Gameplay Updates (When in Game)

- Resets to the main menu if no ships are selected.
- Updates player grids and cursor based on current tile selections.

### Player Input Handling

- If AI is disabled:
  - Updates cursor for ship placement or shooting.
  - Checks for mouse button releases to acknowledge clicks.
  - Manages turn swapping and updates ship manager based on current actions.

- If AI is enabled:
  - Randomly selects grid positions for AI placements.
  - Handles cursor updates and clicking logic similarly to the player.

## `Draw`

The `Draw` method is responsible for rendering all visual elements of the Battleship game, updating the screen in a continuous loop based on the current game state.

## Purpose

- To draw the main menu, ship selection menu, settings, and game elements based on whether the game is in progress.

## `HandleShooting`

The `HandleShooting` method manages player and AI shooting logic in the Battleship game. It determines the actions to take based on the current game state and selected difficulty level.

## Purpose

- To handle shooting mechanics for both the player and the AI, updating game states and conditions accordingly.

## Functionality

### Player Shooting (When Difficulty is Disabled)

- **Perform Shooting**:
  - Depending on whose turn it is, calls the `Shoot()` method on the respective player's grid.
  - Decrements the hit limit for the opponent if the shot is successful.
- **Turn Management**: Switch turns and hides the ships of the player who is not currently taking their turn.
- **Game End Check**: If either player's hit limit reaches zero, the game ends, and the state is reset to the main menu.

### Easy Difficulty

- **AI Shooting Logic**: If it's the AI's turn, it randomly selects a tile to shoot at:
  - Continues to generate random coordinates until a valid tile is hit.
- **Turn Management**: Similar to the disabled difficulty, handles turn switching and ship visibility.

### Medium Difficulty

- **Priority Attacks**: If a ship is hit, adjacent tiles are added to a priority attack list.
- **Attack Logic**: The AI prioritizes attacking these tiles first, using random selection only when necessary.
- **Turn Management**: Manages turn switching and ship visibility as before.

### Hard Difficulty

- **Guaranteed Hits**: The AI continues to shoot until it hits a ship, ensuring a hit every turn.
- **Turn Management**: Consistent with previous difficulty levels, ensuring proper turn management and ship visibility.

# Constants.cs

The `Constants` class contains all the constant values used throughout the Battleship game. These constants define various parameters for the game grid and rendering.

# Cursor.cs

The `Cursor` class represents the cursor object in the Battleship game, handling its appearance and behavior during ship placement and gameplay.

## `LoadContent`

Loads the textures for the cursor.

## `UpdateWhilePlacing`

Updates the cursor while in ship placement mode. Rotates the cursor if the "R" key is pressed.

## `UpdateWhilePlaying`

Updates the cursor while in play mode. Ensures the cursor is always horizontal when shooting.


## `Draw`

Draws the cursor on the screen based on its current orientation.

## `UpdateRectangles`

Updates the start and end locations for the cursor's rectangle objects.

## `RemoveCursor`

Removes the cursor when the mouse is no longer over the board.

## `FlipOrientation`

Flips the cursor's orientation between horizontal and vertical.

## `OnTimeoutEvent`

Handles the event when the rotate timer times out, disposing of the timer.

# CursorOrientation.cs

The `CursorOrientation` enum defines the possible orientations of the cursor in the Battleship game.

## Enum Values
- **HORIZONTAL**
- **VERTICAL**

# DifficultyState.cs

The `DifficultyState` enum represents the different difficulty levels available in Battleship.

## Enum Values

- **Easy**: AI plays randomly.
- **Medium**: AI targets adjacent tiles after a hit.
- **Hard**: AI is guaranteed to hit every turn.
- **Disabled**: Allowing for a purely player vs. player.

# DrawHandler.cs

The `DrawHandler` class is responsible for managing the drawing of various game screens in the Battleship game.

## `DrawGame`

Draws the appropriate screen based on the current game state.

# ExtensionMethods.cs

The `ExtensionMethods` class provides extension methods for various types in the Battleship game.

## `Tuple<int, int> CoordinatesOf<T>(this T[,] matrix, T value)`

Finds the coordinates (x, y) of a specified value in a two-dimensional array (matrix).

# GameState.cs

The `GameState` enumeration defines the various states of the game.

## Enum Members

- **MainMenu**
- **Playing**
- **ShipSelection**
- **Settings**
- **Postgame**
- **Exit**
# Grid.cs

The `Grid` class represents the game grid in the Battleship game, storing the state of each tile and handling interactions such as shooting and ship placement.

## Properties

- **GridArray**: A 2D array of `GridTile` objects representing the grid.
- **SquareMissedTexture**: Texture displayed when a shot misses.
- **SquareHitTexture**: Texture displayed when a shot hits a ship.
- **CurrentTile**: The tile currently under the mouse cursor.
- **Size**: The width and height of the grid, always square.
- **_offset**: The horizontal offset for drawing the grid.

## `Grid`

Initializes a new instance of the `Grid` class.

## `LoadContent`

Loads the textures for the grid tiles and initializes their properties.

## `Update`

Updates the current tile based on mouse position.

## `DrawBackground`

Draws the base grid textures behind the ships.

## `DrawForeground`

Draws the textures of hit tiles in front of the ships.

## `ShipPlaced`

Updates the grid to indicate a ship has been placed on the selected tiles.

## `GetAdjustedCurrentTile`

Adjusts the current tile if the ship is too close to the grid's edge.

## `IsShipPlacementValid`

Checks if the ship can be placed on the selected tile without overlapping other ships.

## `Shoot`

Processes a shot at the current tile, marking it as hit or miss and returning the result.

# GridTile.cs

The `GridTile` class represents an individual tile on the Battleship game grid, handling its properties and behavior, such as its texture, ship occupancy, and mouse interactions.

### `GridTile(Point location, Point size)`

Initializes a new instance of the `GridTile` class.

## `GetCursorLeftHalfLocation`

Calculates the position for the left half of a horizontal cursor based on the tile's coordinates and the ship's size.

## `GetCursorRightHalfLocation`

Calculates the position for the right half of a horizontal cursor.

## `GetCursorTopHalfLocation`

Calculates the position for the top half of a vertical cursor.

## `GetCursorBottomHalfLocation`

Calculates the position for the bottom half of a vertical cursor.

## `GetCursorAdjustedHorizontalSize`

Returns the adjusted size for a horizontal cursor.

## `GetCursorAdjustedVerticalSize`

Returns the adjusted size for a vertical cursor.

## `GetLocation`

Returns the location of the grid tile.

# Menu.cs

The `Menu` class manages the main menu of the Battleship game, including button handling, updates based on user input, and rendering the menu on the screen.

## `Menu`

Initializes a new instance of the `Menu` class.

## `Update`

Updates the menu state based on user input. It checks for mouse hover over buttons and handles button clicks to change the selected game state.

## `Draw`

Draws the main menu on the screen.

# Program.cs

Runs the game!

# SettingsMenu.cs

The `SettingsMenu` class manages the settings menu for selecting the AI difficulty in the Battleship game. It handles user input for difficulty selection and provides a way to return to the main menu.

## `SettingsMenu`

Initializes a new instance of the `SettingsMenu` class.

## `Update`

Updates the settings menu based on user input. It detects mouse hover and click events for the difficulty buttons and the back button.

## `Draw`

Draws the settings menu on the screen.

# Ship.cs

The `Ship` class represents a ship in the Battleship game. It handles properties related to the ship's placement, length, and status (sunk or not).

## `Ship`

Initializes a new instance of the `Ship` class.

# ShipManager.cs

The `ShipManager` class is responsible for managing the ships of both players in the Battleship game. It handles ship textures, placements, and events related to ship management.


## `ShipManager`

Initializes a new instance of the `ShipManager` class.

## `LoadContent`

Loads textures for all ship sizes and orientations.

## `UpdateWhilePlacing`

Handles the logic for placing ships on the grid.


## `Draw`

Draws the ships for both players on the game screen.

## `OnTimeoutEvent`

Handles the timeout event for ship placement, ensuring players cannot place ships too quickly.

# ShipSelectionMenu.cs

## `Update`
Updates the ship selection menu based on user input. Detects mouse position and clicks to handle ship selection and button interactions.

## `Draw`
Draws the ship selection menu on the screen, including buttons for selecting ships, starting the game, and returning to the main menu.

# TurnManager.cs

## `LoadContent`
Loads the textures for the turn indicators (Player 1, Player 2, and swap indicator).

## `Draw`
Draws the turn indicator on the screen. Displays the current player's turn or the swap indicator if waiting.

## `NextTurn`
Toggles the turn between Player 1 and Player 2. Sets the `SwapWaiting` flag to true.
## Header Template


```
Name:
Description:
Inputs:
Outputs:
Collaborators/Sources: Michael Oliver, Peter Pham, Jack Youngquist, Andrew Uriell, Ian Wilson
Created: September 19, 2024
Last Modified: September 24, 2024
```