using System;
using System.Diagnostics;
using Client.classes.Uno;
using Client.interfaces;

namespace Client.components.Uno
{
    [Serializable()]
    public class Card : IDrawable, ICloneable
    {
        
        private readonly bool _isDeck;
        public string Id = Guid.NewGuid().ToString();
        public ConsoleColor Color { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public static int Width => CardManager.LongestCard + 4;
        
        public Card()
        {
            Value = "****";
            Color = ConsoleColor.White;
            IsSelected = false;
            _isDeck = true;
        }
        
        public Card(string value, ConsoleColor color = ConsoleColor.White)
        {
            Value = value;
            Color = color;
            IsSelected = false;
        }

        private Card(Card card)
        {
            Value = card.Value;
            Color = card.Color;
            IsSelected = card.IsSelected;
            _isDeck = card._isDeck;
            X = card.X;
            Y = card.Y;
        }

        public void MoveLeft(int min, int max)
        {
            X += Width;
            if (X > max)
            {
                X = min;
            }else if (X < min)
            {
                X = max;
            }
        }
        public void MoveRight(int min, int max)
        {
            X -= Width;
            if (X > max)
            {
                X = min;
            }else if (X < min)
            {
                X = max;
            }
        }

        public bool IsDrawn()
        {
            return !(X < 0 || X + Width >= Console.WindowWidth-1 || Y < 0 || Y + 5 >= Console.WindowHeight-1);
        }
                                                                                                                                                                                                                                            
        public void Draw()
        {
            if (!IsDrawn()) return;
            var x = X;
            var y = Y;
            Console.SetCursorPosition(x,y);
            Console.BackgroundColor = IsSelected ? ConsoleColor.White : ConsoleColor.Black;
            Console.ForegroundColor = Console.BackgroundColor == ConsoleColor.White && Color == ConsoleColor.White ? ConsoleColor.Black : Color;
            if (_isDeck)
            {
                const string deck = " Deck ";
                for (var i = 0; i < Width/2-deck.Length/2; i++)
                {
                    Console.Write("*");
                }   
                Console.Write(deck);
                for (var i = 0; i < Width/2-deck.Length/2; i++)
                {
                    Console.Write("*");
                }
            }
            else
            {
                for (var i = 0; i < Width; i++)
                {
                    Console.Write("*");
                }  
            }
            y++;
            Console.SetCursorPosition(x,y);
            Console.Write("*");
            Console.SetCursorPosition(x+Width-1,y);
            Console.Write("*");
            y++;
            Console.SetCursorPosition(x,y);
            Console.Write("*");
            Console.SetCursorPosition(x+Width/2-(Value.Length/2),y);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = Color;
            Console.Write(Value);
            Console.BackgroundColor = IsSelected ? ConsoleColor.White : ConsoleColor.Black;
            Console.ForegroundColor = Console.BackgroundColor == ConsoleColor.White && Color == ConsoleColor.White ? ConsoleColor.Black : Color;
            Console.SetCursorPosition(x+Width-1,y);
            Console.Write("*");
            y++;
            Console.SetCursorPosition(x,y);
            Console.Write("*");
            Console.SetCursorPosition(x+Width-1,y);
            Console.Write("*");
            y++;
            Console.SetCursorPosition(x,y);
            for (var i = 0; i < Width; i++)
            {
                Console.Write("*");
            }
        }

        public void Clear(bool full = false)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            if (!IsDrawn()) return;
            var windowWidth = Console.WindowWidth;
            var windowHeight = Console.WindowHeight;
            if (full)
            {
                for (var i = X; i < X+Width && X+Width < windowWidth; i++)
                {
                    for (var j = Y; j < Y+5 && Y+5 < windowHeight; j++)
                    {
                        Console.SetCursorPosition(i,j);
                        Console.Write(" ");
                    }
                }
            }
            else
            {
                for (var i = X+1; i < X+Width-1 && X+Width-1 < windowWidth; i++)
                {
                    for (var j = Y+1; j < Y+4 && Y+4 < windowHeight; j++)
                    {
                        Console.SetCursorPosition(i,j);
                        Console.Write(" ");
                    }
                }
            }
        }

        public object Clone()
        {
            return new Card(this);
        }
    }
}