using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Battleship;
public class Menu
{
    private Dictionary<GameState, Rectangle> buttonRects;  // Rectangles for all buttons
    private MouseState mouseState;                         // Mouse state to detect input
    private SpriteFont font;                               // Font for the button text
    private Dictionary<GameState, Color> buttonColors;     // Button colors (changes on hover)
    private Dictionary<GameState, string> buttonTexts;     // Text displayed on each button

    public GameState SelectedState { get; private set; }   // Holds the selected game state

    public Menu(SpriteFont font)
    {
        this.font = font;

        // Initialize button rectangles for the main menu
        buttonRects = new Dictionary<GameState, Rectangle>
        {
            { GameState.ShipSelection, new Rectangle(580, 100, 300, 100) },   // "Play Game"
            { GameState.Settings, new Rectangle(580, 300, 300, 100) },   // "Settings"
            { GameState.Exit, new Rectangle(580, 500, 300, 100) }        // "Exit"
        };

        // Initialize button colors for each menu state
        buttonColors = new Dictionary<GameState, Color>
        {
            { GameState.ShipSelection, Color.White },
            { GameState.Settings, Color.White },
            { GameState.Exit, Color.White }
        };

        // Button texts for the main menu
        buttonTexts = new Dictionary<GameState, string>
        {
            { GameState.ShipSelection, "Play Game" },
            { GameState.Settings, "Settings" },
            { GameState.Exit, "Exit" }
        };

        SelectedState = GameState.MainMenu;
    }
    public void Update()
    {
        mouseState = Mouse.GetState();

        // Main Menu handling
        foreach (var button in buttonRects)
        {
            if (button.Value.Contains(mouseState.Position))
            {
                buttonColors[button.Key] = Color.Gray;

                // Handle button clicks
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (button.Key == GameState.ShipSelection)
                    {
                        SelectedState = GameState.ShipSelection;  // Transition to Ship Selection
                    }
                    else if (button.Key == GameState.Exit)
                    {
                        SelectedState = GameState.Exit;  // Exit the game
                    }
                    else if (button.Key == GameState.Settings)
                    {
                        SelectedState = GameState.Settings;  // Go to settings menu
                    }
                }
            }
            else
            {
                buttonColors[button.Key] = Color.White;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Texture2D rectangleTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        rectangleTexture.SetData(new[] { Color.White });

        foreach (var button in buttonRects)
        {
            spriteBatch.Draw(rectangleTexture, button.Value, buttonColors[button.Key]);

            Vector2 textSize = font.MeasureString(buttonTexts[button.Key]);
            Vector2 textPosition = new Vector2(
                button.Value.X + (button.Value.Width / 2) - (textSize.X / 2),
                button.Value.Y + (button.Value.Height / 2) - (textSize.Y / 2)
            );
            spriteBatch.DrawString(font, buttonTexts[button.Key], textPosition, Color.Black);
        }
    }
}
