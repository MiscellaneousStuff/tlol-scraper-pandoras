using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Api
{
    public struct Color : IEquatable<Color>
    {
        private float _r = 0;
        private float _g = 0;
        private float _b = 0;
        private float _a = 1;

        public float R
        {
            readonly get => _r; 
            set => _r = Clamp(value);
        }

        public float G
        {
            readonly get => _g;
            set => _g = Clamp(value);
        }

        public float B
        {
            readonly get => _b;
            set => _b = Clamp(value);
        }

        public float A
        { 
            readonly get => _a;
            set => _a = Clamp(value);
        }

        public float this[int key]
        {
            readonly get
            {
                return key switch
                {
                    0 => _r,
                    1 => _g,
                    2 => _b,
                    3 => _a,
                    _ => 0,
                };
            }
            set
            {
                switch(key)
                {
                    case 0:
                        R = value;
                        break;
                    case 1:
                        G = value;
                        break;
                    case 2:
                        B = value;
                        break;
                    case 3:
                        A = value;
                        break;
                }
            }
        }

        public static readonly Color White = new(1.0f, 1.0f, 1.0f, 1.0f);
        public static readonly Color Black = new(0.0f, 0.0f, 0.0f, 1.0f);
        public static readonly Color Red = new(1.0f, 0.0f, 0.0f, 1.0f);
        public static readonly Color Green = new(0.0f, 1.0f, 0.0f, 1.0f);
        public static readonly Color Blue = new(0.0f, 0.0f, 1.0f, 1.0f);
        public static readonly Color Yellow = new(1.0f, 1.0f, 0.0f, 1.0f);
        public static readonly Color Magenta = new(1.0f, 0.0f, 1.0f, 1.0f);
        public static readonly Color Cyan = new(0.0f, 1.0f, 1.0f, 1.0f);

        public Color() : this(0.0f, 0.0f, 0.0f, 1.0f) { }

        public Color(float r, float g, float b, float a) 
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(float r, float g, float b) : this(r, g, b, 1.0f) { }

        public Color(byte r, byte g, byte b, byte a) : this(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f) { }
        
        public Color(byte r, byte g, byte b) : this(r, g, b, 255) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float Clamp(float value)
        {
            return MathF.Max(MathF.Min(1, value), 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Lerp(Color value1, Color value2, float amount)
        {
            return (value1 * (1.0f - amount)) + (value2 * amount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator +(Color left, Color right)
        {
            return new Color(
                left._r + right._r,
                left._g + right._g,
                left._b + right._b,
                left._a + right._a
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator /(Color left, Color right)
        {
            return new Color(
                left._r / right._r,
                left._g / right._g,
                left._b / right._b,
                left._a / right._a
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator /(Color left, float right)
        {
            return new Color(
                left._r / right,
                left._g / right,
                left._b / right,
                left._a / right
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Color left, Color right)
        {
            return (left._r == right._r)
                && (left._g == right._g)
                && (left._b == right._b)
                && (left._a == right._a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Color left, Color right)
        {
            return (left._r != right._r)
                || (left._g != right._g)
                || (left._b != right._b)
                || (left._a != right._a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator *(Color left, Color right)
        {
            return new Color(
                left._r * right._r,
                left._g * right._g,
                left._b * right._b,
                left._a * right._a
            );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator *(Color left, float right)
        {
            return new Color(
                left._r * right,
                left._g * right,
                left._b * right,
                left._a * right
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator *(float left, Color right)
        {
            return new Color(
               right._r * left,
               right._g * left,
               right._b * left,
               right._a * left
           );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color operator -(Color left, Color right)
        {
            return new Color(
                left._r - right._r,
                left._g - right._g,
                left._b - right._b,
                left._a - right._a
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Color other)
        {
            return this == other;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals([NotNullWhen(true)] object? obj)
        {
            return (obj is Color other) && Equals(other);
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(_r, _g, _b, _a);
        }

        public override readonly string ToString()
        {
            return $"R: {_r:N3}, G: {_g:N3}, B: {_b:N3}, A: {_a:N3}";
        }
        
        public uint Rgba()
        {
            var r = (byte)(_r * 255);
            var g = (byte)(_g * 255);
            var b = (byte)(_b * 255);
            var a = (byte)(_a * 255);
            return (uint)((r) | (g << 8) | (b << 16) | (a << 24));
        }
    }
}
