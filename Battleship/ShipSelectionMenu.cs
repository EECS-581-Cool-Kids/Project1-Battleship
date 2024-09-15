using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class ShipSelectionMenu
{
    private Dictionary<int, Rectangle> shipButtonRects;  // Rectangles for the ship selection buttons
    private MouseState mouseState;                        // Mouse state for detecting input
    private SpriteFont font;                              // Font for drawing text
    private Dictionary<int, Color> buttonColors;          // Button colors for each ship selection
    public int SelectedShipCount { get; set; }    // Stores the selected number of ships
    public bool IsSelectionMade { get; private set; }     // Indicates if a selection has been made
    public bool back { get; private set; }

    private Rectangle startGameButtonRect;
    private Color startGameButtonColor = Color.White;
    private Rectangle backButtonRect;
    private Color backButtonColor = Color.White;


    public ShipSelectionMenu(SpriteFont font)
    {
        this.font = font;
        back = false;
        IsSelectionMade = false;

        // Initialize rectangles for each ship selection button (1-5 ships)
        shipButtonRects = new Dictionary<int, Rectangle>
        {
            { 1, new Rectangle(600, 40, 200, 50) },
            { 2, new Rectangle(600, 120, 200, 50) },
            { 3, new Rectangle(600, 200, 200, 50) },
            { 4, new Rectangle(600, 280, 200, 50) },
            { 5, new Rectangle(600, 360, 200, 50) }
        };

        // Initialize button colors for ship selection
        buttonColors = new Dictionary<int, Color>
        {
            { 1, Color.White },
            { 2, Color.White },
            { 3, Color.White },
            { 4, Color.White },
            { 5, Color.White }
        };
        startGameButtonRect = new Rectangle(600, 440, 200, 50);
        backButtonRect = new Rectangle(600, 520, 200, 50);

    }

    public void Update()
    {
        mouseState = Mouse.GetState();

        // Loop through each ship selection button and handle interaction
        foreach (var button in shipButtonRects)
        {
            if (button.Value.Contains(mouseState.Position))
            {
                buttonColors[button.Key] = Color.Gray;

                // If the left mouse button is clicked, register the selection
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    SelectedShipCount = button.Key;  // Store the number of selected ships
                    buttonColors[button.Key] = Color.Red;
                }
            }
            else
            {
                buttonColors[button.Key] = Color.White;
            }
        }
        switch (SelectedShipCount)
        {
            case 1:
                buttonColors[1] = Color.Red;
                SelectedShipCount = 1;
                break;
            case 2:
                buttonColors[2] = Color.Red;
                SelectedShipCount = 2;
                break;
            case 3:
                buttonColors[3] = Color.Red;
                SelectedShipCount = 3;
                break;
            case 4:
                buttonColors[4] = Color.Red;
                SelectedShipCount = 4;
                break;
            case 5:
                buttonColors[5] = Color.Red;
                SelectedShipCount = 5;
                break;
        }
        if (startGameButtonRect.Contains(mouseState.Position))
        {
            startGameButtonColor = Color.Gray;  // Change color on hover

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                IsSelectionMade = true;  // Ready to start the game
            }
        }
        else
        {
            startGameButtonColor = Color.White;  // Reset button color
        }
        if (backButtonRect.Contains(mouseState.Position))
        {
            backButtonColor = Color.Gray;  // Change color on hover

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                back = true;  // Return to main menu
            }
        }
        else
        {
            backButtonColor = Color.White;  // Reset button color
        }

    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Create a 1x1 pixel texture that we can scale to draw as a rectangle
        Texture2D rectangleTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        rectangleTexture.SetData(new[] { Color.White });

        // Draw buttons for selecting the number of ships (1-5)
        foreach (var button in shipButtonRects)
        {
            // Draw the rectangle as the button
            spriteBatch.Draw(rectangleTexture, button.Value, buttonColors[button.Key]);

            // Center the text in each button
            string buttonText = $"{button.Key} Ships";
            Vector2 textSize = font.MeasureString(buttonText);
            Vector2 textPosition = new Vector2(
                button.Value.X + (button.Value.Width / 2) - (textSize.X / 2),
                button.Value.Y + (button.Value.Height / 2) - (textSize.Y / 2)
            );

            // Draw the button label (number of ships) in black
            spriteBatch.DrawString(font, buttonText, textPosition, Color.Black);
        }
        // Draw "Start Game" button
        spriteBatch.Draw(rectangleTexture, startGameButtonRect, startGameButtonColor);

        string startGameText = "Start Game";
        Vector2 startGameTextSize = font.MeasureString(startGameText);
        Vector2 startGameTextPosition = new Vector2(
            startGameButtonRect.X + (startGameButtonRect.Width / 2) - (startGameTextSize.X / 2),
            startGameButtonRect.Y + (startGameButtonRect.Height / 2) - (startGameTextSize.Y / 2)
        );
        spriteBatch.DrawString(font, startGameText, startGameTextPosition, Color.Black);

        // Draw "Retrun to Main Menu" button
        spriteBatch.Draw(rectangleTexture, backButtonRect, backButtonColor);

        string backText = "Return to Main Menu";
        Vector2 backTextSize = font.MeasureString(backText);
        Vector2 backTextPosition = new Vector2(
            backButtonRect.X + (backButtonRect.Width / 2) - (backTextSize.X / 2),
            backButtonRect.Y + (backButtonRect.Height / 2) - (backTextSize.Y / 2)
        );
        spriteBatch.DrawString(font, backText, backTextPosition, Color.Black);
        // Clean up the texture after drawing (dispose it)
        //rectangleTexture.Dispose();
    }
}