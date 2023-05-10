using System;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game = Engine.BaseSystems.Game;
#if DEBUG
using Engine.BaseSystems;
#endif

namespace Engine.BaseComponents;

public class TextMesh : Component
{
    public TextMesh() {}

    public TextMesh(TextMesh mesh) : base(mesh)
    {
        Text = mesh.Text;
        Font = mesh.Font;
        Color = mesh.Color;
        WordWrap = mesh.WordWrap;
        Width = mesh.Width;
        Offset = mesh.Offset;
    }

    public Vector2? CentralOffset
    {
        // ReSharper disable once ValueParameterNotUsed
        set => SetCentralOffset();
    }

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            RecalculateString();
#if DEBUG
            GenerateDebugTexture();
#endif
        }
    }

    public SpriteFont Font
    {
        get => _font;
        set
        {
            _font = value;
            RecalculateString();
#if DEBUG
            GenerateDebugTexture();
#endif
        }
    }

    public Color Color { get; set; }

    public bool WordWrap
    {
        get => _wordWrap;
        set
        {
            _wordWrap = value;
            RecalculateString();
#if DEBUG
            GenerateDebugTexture();
#endif
        }
    }

    public int Width
    {
        get => _width;
        set
        {
            _width = value;
            RecalculateString();
#if DEBUG
            GenerateDebugTexture();
#endif
        }
    }

    public Vector2 Offset { get; set; }

    public TextMesh SetText(string text)
    {
        Text = text;
        return this;
    }

    public TextMesh SetFont(SpriteFont font)
    {
        Font = font;
        return this;
    }

    public TextMesh SetColor(Color color)
    {
        Color = color;
        return this;
    }

    public TextMesh SetWordWrap(bool value)
    {
        WordWrap = value;
        return this;
    }

    public TextMesh SetWidth(int width)
    {
        Width = width;
        return this;
    }

    public TextMesh SetOffset(Vector2 offset)
    {
        Offset = offset;
        return this;
    }

    public TextMesh SetCentralOffset()
    {
        Offset = - _font.MeasureString(_editedText) / 2;
        return this;
    }

    public override void Draw()
    {
        Game.DrawSpace.Begin();
        Game.DrawSpace.DrawString(
            Font,
            _editedText,
            Transform.Position + Offset,
            Color,
            Transform.Rotation,
            new(),
            Transform.Scale,
            SpriteEffects.None,
            Transform.LayerDepth
        );
#if DEBUG
        if (_debugTexture != null && Game.DebugBoxesOn)
            Game.DrawSpace.Draw(
                _debugTexture,
                Transform.Position + Offset,
                null,
                Color.White,
                Transform.Rotation,
                new(),
                Transform.Scale,
                SpriteEffects.None,
                Transform.LayerDepth
            );
#endif
        Game.DrawSpace.End();
    }

    private string _text;
    private string _editedText;
    private SpriteFont _font;
    private bool _wordWrap;
    private int _width = 150;

    private void RecalculateString()
    {
        _editedText = _text;
        if (!_wordWrap || _font == null || _text == null) return;
        var lines = (int)MathF.Ceiling(_font.MeasureString(_editedText).X / _width);
        if (lines == 0) return;
        var charsPerLine = _editedText.Length / lines;
        for (var line = 1; line < lines; line++)
        for (var index = charsPerLine * line; index >= charsPerLine * (line - 1); index--)
        {
            if (_editedText[index] != ' ') continue;
            _editedText = _editedText.Remove(index, 1).Insert(index, "\n");
            break;
        }
    }

#if DEBUG
    private Texture2D _debugTexture;

    private void GenerateDebugTexture()
    {
        if (_font == null || _text == null) return;
        var height = (int)_font.MeasureString(_editedText).Y;
        _debugTexture = TextureGenerator.GenerateTexture(
            Width,
            height,
            (x, y) => y == 1 || y == height - 1 || x == 1 || x == Width - 1
                ? Color.LightGreen
                : Color.Transparent
        );
    }
#endif
}
