using UnityEngine;
using System;

namespace HeathenEngineering.UIX
{
    [Serializable]
    public struct KeyGlyphData
    {
        public string normal;
        public string shifted;
        public string altGr;
        public string altGrShifted;
        public KeyCode code;

        public KeyGlyphData(KeyboardKeyGlyph glyph)
        {
            normal = glyph.normalDisplay.text;
            shifted = glyph.shiftedDisplay.text;
            altGr = glyph.altGrDisplay.text;
            altGrShifted = glyph.shiftedAltGrDisplay.text;
            code = glyph.code;
        }
    }
}
