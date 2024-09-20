using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class SettingsMenu
{
    private Dictionary<DifficultyState, Rectangle> buttonRects;

    private MouseState mouseState;

    private SpriteFont font;

    private Dictionary<DifficultyState, Color> buttonColors;

    public bool back { get; private set; }

    private Rectangle backButtonRect;

    private Color backButtonColor = Color.White;

    public DifficultyState SelectedDifficulty { get; set; }

    public SettingsMenu(SpriteFont font)
    {
        this.font = font;
        back = false; 

        // Initialize rectangles for each ship selection button (1-5 ships)
        buttonRects = new Dictionary<DifficultyState, Rectangle>
        {
            { DifficultyState.Easy, new Rectangle(580, 100, 300, 75) },
            { DifficultyState.Medium, new Rectangle(580, 200, 300, 75) },
            { DifficultyState.Hard, new Rectangle(580, 300, 300, 75) },
            { DifficultyState.Disabled, new Rectangle(580, 400, 300, 75) }
        };


        // Initialize button colors for ship selection
        buttonColors = new Dictionary<DifficultyState, Color>
        {
            { DifficultyState.Easy, Color.White },
            { DifficultyState.Medium, Color.White },
            { DifficultyState.Hard, Color.White },
            { DifficultyState.Disabled, Color.White }
        };
        // Initialize the "Start Game" and "Return to Main Menu" button rectangles
        backButtonRect = new  Rectangle(580, 500, 300, 75);
    }

    public void Update()
    {
        mouseState = Mouse.GetState();

        // Loop through each ship selection button and handle interaction
        foreach (var button in buttonRects)
        {
            if (button.Value.Contains(mouseState.Position))
            {
                buttonColors[button.Key] = Color.Gray; // Change color on hover

                // If the left mouse button is clicked, register the selection
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    SelectedDifficulty = button.Key;
                    buttonColors[button.Key] = Color.Red;
                }
            }
            else
            {
                buttonColors[button.Key] = Color.White; // Reset button color
            }
        }

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

    public void Draw(SpriteBatch spriteBatch)
    {
        Texture2D rectangleTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        rectangleTexture.SetData(new[] { Color.White });

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