using Microsoft.Xna.Framework.Graphics;

namespace Battleship;
public class DrawHandler
{
    private Menu menu;
    private BattleshipGame battleshipGame;
    private ShipSelectionMenu shipSelection;

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
