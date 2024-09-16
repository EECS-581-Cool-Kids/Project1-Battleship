/*
 *   Module Name: DrawHandler.cs
 *   Purpose: This module is the DrawHandler class that is responsible for drawing the game based on the current game state.
 *            This module isn't currently implemented in the game.
 *   Inputs: None
 *   Output: None
 *   Additional code sources:
 *   Developers: Derek Norton, Ethan Berkley, Jacob Wilkus, Mo Morgan, and Richard Moser
 *   Date: 09/15/2024
 *   Last Modified: 09/15/2024
 */

using Microsoft.Xna.Framework.Graphics;

namespace Battleship;
/// <summary>
/// This class is responsible for drawing the game based on the current game state.
/// </summary>
public class DrawHandler
{
    /// <summary>
    /// 
    /// </summary>
    private Menu menu;

    /// <summary>
    ///
    /// </summary>
    private BattleshipGame battleshipGame;

    /// <summary>
    ///
    /// </summary>
    private ShipSelectionMenu shipSelection;

    /// <summary>
    /// Initializes a new instance of the DrawHandler class.
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="battleshipGame"></param>
    /// <param name="shipSelection"></param>
    public DrawHandler(Menu menu, BattleshipGame battleshipGame, ShipSelectionMenu shipSelection)
    {
        this.menu = menu;
        this.battleshipGame = battleshipGame;
        this.shipSelection = shipSelection;
    }

    public void DrawGame(SpriteBatch spriteBatch, GameState currentGameState)
    {
        switch (currentGameState)
        {
            case GameState.MainMenu:
                menu.Draw(spriteBatch);
                break;
            case GameState.Playing:
                menu.Draw(spriteBatch);
                break;
            case GameState.ShipSelection:
                shipSelection.Draw(spriteBatch);
                break;
            case GameState.Settings:
                // Optionally draw paused screen
                break;
            case GameState.Exit:
                // Optionally draw game over screen
                break;
        }
    }
}