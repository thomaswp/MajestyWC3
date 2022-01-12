using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Blizzard;
using static War3Api.Common;

namespace Source.Interface
{


    public struct Color
    {
        public static readonly Color GOLD = new Color(255, 220, 0, 255);

        public int red, green, blue, alpha;

        public Color(int red, int green, int blue, int alpha = 255)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.alpha = alpha;
        }
    }

    public static class TextTag
    {

        public const float MEAN_CHAR_WIDTH = 5.5f;
        public const float MAX_TEXT_SHIFT = 32f;
        public const float FONT_SIZE = 0.026f;
        public const float Y_OFFSET = 125f;
        public const float DEFAULT_DURATION = 1.5f;

        public static void ShowTextTag(this unit unit, string message, Color color, float duration = DEFAULT_DURATION)
        {
            texttag tag = CreateTextTag();
            tag.SetText(message);
            tag.SetColor(color);
            tag.CenterAboveUnit(unit, message);
            tag.FloatUp(duration, duration / 2f, 0.02f);
            SetTextTagPermanent(tag, false);
            ShowTextTagForceBJ(true, tag, GetForceOfPlayer(unit.GetPlayer()));
            //SetTextTagVisibility(tag, true);
        }

        public static void FloatUp(this texttag tag, float duration, float fadepoint, float speed)
        {
            SetTextTagLifespan(tag, duration);
            SetTextTagFadepoint(tag, fadepoint);
            SetTextTagVelocity(tag, 0, speed);
        }

        public static texttag CreatePermenant(Color color)
        {
            texttag text = CreateTextTag();
            text.SetColor(color);
            SetTextTagPermanent(text, true);
            SetTextTagVisibility(text, true);
            return text;
        }

        public static void SetColor(this texttag text, Color color)
        {
            SetTextTagColor(text, 255, 220, 0, 255);
        }

        public static void CenterAboveUnit(this texttag text, unit unit, string message)
        {
            float shift = Math.Min(StringLength(message) * MEAN_CHAR_WIDTH / 2, MAX_TEXT_SHIFT) + 20;
            SetTextTagPos(text, GetUnitX(unit) - shift, GetUnitY(unit), Y_OFFSET);

        }

        public static void SetText(this texttag text, string message)
        {
            SetTextTagText(text, message, FONT_SIZE);
        }
    }
}
