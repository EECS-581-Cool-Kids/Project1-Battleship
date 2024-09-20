/*
 *   Module Name: Menu.cs
 *   Purpose: This module is the main menu class that is responsible for displaying the main menu and handling user input.
 *   Inputs: None
 *   Output: None
 *   Additional code sources: None
 *   Developers: Derek Norton, Ethan Berkley, Jacob Wilkus, Mo Morgan, and Richard Moser
 *   Date: 09/15/2024
 *   Last Modified: 09/15/2024
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Battleship;

/// <summary>
/// This class is responsible for displaying the main menu and handling user input.
/// </summary>
public class Menu
{
    /// <summary>
    /// Rectangle for all buttons
    /// </summary>
    private Dictionary<GameState, Rectangle> buttonRects;

    /// <summary>
    /// Mouse state to detect input
    /// </summary>
    private MouseState mouseState;

    /// <summary>
    /// Font for the button text
    /// </summary>
    private SpriteFont font;

    /// <summary>
    /// Button colors (changes on hover)
    /// </summary>
    private Dictionary<GameState, Color> buttonColors;

    /// <summary>
    /// Holds the text for each button
    /// </summary>
    private Dictionary<GameState, string> buttonTexts;

    /// <summary>
    /// Holds the selected game state
    /// </summary>
    public GameState SelectedState { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Menu class.
    /// </summary>
    /// <param name="font"></param>
    public Menu(SpriteFont font)
    {
        this.font = font;  // Set the font

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
            { GameState.Settings, "AI Difficulty" },
            { GameState.Exit, "Exit" }
        };

        SelectedState = GameState.MainMenu; // Default to main menu
    }

    /// <summary>
    /// Updates the main menu based on user input.
    /// </summary>
    public void Update()
    {
        mouseState = Mouse.GetState(); // Get the current mouse state

        // Main Menu handling
        foreach (var button in buttonRects)
        {
            // Change button color on hover
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
                buttonColors[button.Key] = Color.White; // Reset button color
            }
        }
    }

    /// <summary>
    /// Draws the main menu.
    /// </summary>
    /// <param name="spriteBatch"></param>
    public void Draw(SpriteBatch spriteBatch)
    {
        /// Draw the main menu buttons
        Texture2D rectangleTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1); // Create a 1x1 texture
        rectangleTexture.SetData(new[] { Color.White }); // Set the color of the rectangle to white

        foreach (var button in buttonRects)
        {
            // Draw the button rectangle
            spriteBatch.Draw(rectangleTexture, button.Value, buttonColors[button.Key]);

            // Center the button text on the button rectangle
            Vector2 textSize = font.MeasureString(buttonTexts[button.Key]);
            Vector2 textPosition = new Vector2(
                button.Value.X + (button.Value.Width / 2) - (textSize.X / 2),
                button.Value.Y + (button.Value.Height / 2) - (textSize.Y / 2)
            );

            spriteBatch.DrawString(font, buttonTexts[button.Key], textPosition, Color.Black); // Draw the button text
        }
    }
}