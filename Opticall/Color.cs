namespace Opticall {

    public class Color
    {
            /// <summary>
            /// Red portion of the color
            /// </summary>
            public byte Red { get; private set; }

            /// <summary>
            /// Green portion of the color
            /// </summary>
            public byte Green { get; private set; }

            /// <summary>
            /// Blue portion of the color
            /// </summary>
            public byte Blue { get; private set; }

            /// <summary>
            /// Create a color object
            /// </summary>
            /// <param name="red">Red portion of the color</param>
            /// <param name="green">Green portion of the color</param>
            /// <param name="blue">Blue portion of the color</param>
            public Color(byte red, byte green, byte blue)
            {
                Red = red;
                Green = green;
                Blue = blue;
            }
    }

/*
    public abstract class Color
    {
        private short _color;

        public Color(short color)
        {
            _color = color;
        }

        public Color()

        public abstract byte Byte { get; }

        public override string ToString()
        {
            return 
        }
    }
    */
}