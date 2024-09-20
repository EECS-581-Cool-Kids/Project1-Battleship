/*
 *   Module Name: SettingsMenu.cs
 *   Purpose: This module contains the code for drawing the settings menu and selecting an AI difficulty
 *   Inputs: None
 *   Output: None
 *   Additional code sources: None
 *   Developers: Peter Pham
 *   Date: 09/19/2024
 *   Last Modified: 09/20/2024
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

/// <summary>
/// This class is responsible for displaying the settings menu and getting/storing the user's chosen AI difficulty.
/// </summary>
public class SettingsMenu
{
    // Object for storing the button rectangles
    private Dictionary<DifficultyState, Rectangle> buttonRects;

    // Object for storing the mouse state to detect input
    private MouseState mouseState;

    // Object for storing the button text
    private SpriteFont font;

    // Object for storing the button colors
    private Dictionary<DifficultyState, Color> buttonColors;

    // Temp object for checking whether the game should return back to the main menu
    public bool back { get; private set; }

    // Object for storing the back button rectangle
    private Rectangle backButtonRect;

    // Object for storing the back button color and setting the default color to white.
    private Color backButtonColor = Color.White;

    // Object for storing the selected AI difficulty
    public DifficultyState SelectedDifficulty { get; set; }

    // Initializes the SettingsMenu class
    public SettingsMenu(SpriteFont font)
    {
        this.font = font;
        back = false; 

        // Populates the button rectangles with hardcoded values for each button
        buttonRects = new Dictionary<DifficultyState, Rectangle>
        {
            { DifficultyState.Easy, new Rectangle(580, 100, 300, 75) },
            { DifficultyState.Medium, new Rectangle(580, 200, 300, 75) },
            { DifficultyState.Hard, new Rectangle(580, 300, 300, 75) },
            { DifficultyState.Disabled, new Rectangle(580, 400, 300, 75) }
        };

        // Initialize button colors for each of the previously declared buttons
        buttonColors = new Dictionary<DifficultyState, Color>
        {
            { DifficultyState.Easy, Color.White },
            { DifficultyState.Medium, Color.White },
            { DifficultyState.Hard, Color.White },
            { DifficultyState.Disabled, Color.White }
        };

        // Also initializes the back button rectangle
        backButtonRect = new  Rectangle(580, 500, 300, 75);
    }

    // Updates the menu based upon the user input
    public void Update()
    {
        // Gets the current mouse state
        mouseState = Mouse.GetState();

        foreach (var button in buttonRects)
        {
            // Checks whether the mouse is currently positioned on any button and changes the color
            if (button.Value.Contains(mouseState.Position))
            {
                buttonColors[button.Key] = Color.Gray;

                // If the button is clicked, store the selected difficulty value and update the button color
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    SelectedDifficulty = button.Key;
                    buttonColors[button.Key] = Color.Red;
                }
            }
            // If mouse is not selecting any button, return all button colors to their default state
            else
            {
                buttonColors[button.Key] = Color.White;
            }
        }

        // Check for persisting the button colors based upon what the user has previously chosen
        switch (SelectedDifficulty)
        {
            case DifficultyState.Easy:
                buttonColors[DifficultyState.Easy] = Color.Red;
                break;
            case DifficultyState.Medium:
                buttonColors[DifficultyState.Medium] = Color.Red;
                break;
            case DifficultyState.Hard:
                buttonColors[DifficultyState.Hard] = Color.Red;
                break;
            case DifficultyState.Disabled:
                buttonColors[DifficultyState.Disabled] = Color.Red;
                break;
        }

        // Checks whether the back button is hovered and selected and updates the back variable accordingly
        if (backButtonRect.Contains(mouseState.Position))
        {
            backButtonColor = Color.Gray; 

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                back = true; 
            }
        }
        else
        {
            backButtonColor = Color.White; 
        }
    }

    // Draws the UI
    public void Draw(SpriteBatch spriteBatch)
    {
        Texture2D rectangleTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        rectangleTexture.SetData(new[] { Color.White });

        // Draws each of the button and their text
        foreach (var button in buttonRects)
        {
            spriteBatch.Draw(rectangleTexture, button.Value, buttonColors[button.Key]);

            string buttonText = $"{button.Key}";
            Vector2 textSize = font.MeasureString(buttonText);
            Vector2 textPosition = new Vector2(
                button.Value.X + (button.Value.Width / 2) - (textSize.X / 2),
                button.Value.Y + (button.Value.Height / 2) - (textSize.Y / 2)
            );

            spriteBatch.DrawString(font, buttonText, textPosition, Color.Black);
        }

        // Draws the back button and its text
        spriteBatch.Draw(rectangleTexture, backButtonRect, backButtonColor);

        string backText = "Return to Main Menu";
        Vector2 backTextSize = font.MeasureString(backText);
        Vector2 backTextPosition = new Vector2(
            backButtonRect.X + (backButtonRect.Width / 2) - (backTextSize.X / 2),
            backButtonRect.Y + (backButtonRect.Height / 2) - (backTextSize.Y / 2)
        );

        spriteBatch.DrawString(font, backText, backTextPosition, Color.Black);
    }
}