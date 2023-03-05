using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Лаб10
{
    class Block  // Клас для створення блоків
    {
        public List<Space> PointList = new List<Space>();
        public ConsoleColor BlockColor;
        public Block(ConsoleColor color, params Space[] points)
        {
            BlockColor = color;
            foreach (var point in points)
            {
                point.Color = color;
                point.NormalColor = color;
                point.IsBlock = true;
                PointList.Add(point);
            }
        }
        public void NormaliseColors()
        {
            for (int i = 0; i < PointList.Count; i++)
                PointList[i].Color = PointList[i].NormalColor;
        }
    }
    class Space // Клас для створення простору гри
    {
        public int X;
        public int Y;
        public ConsoleColor Color;
        public ConsoleColor NormalColor;
        public bool IsBlock;
        public Space(int x1, int y1)
        {
            X = x1;
            Y = y1;
            Color = ConsoleColor.Black;
        }
    }
    

    class Program
    {
        int w = 80;
        int h = 40;
        void Init() // Розміри консолі
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(w + 1, h + 3);
            Console.SetBufferSize(w + 1, h + 3);

        }
        void SplashScreen()
        {
            string[] ss = new string[8];
            ss[0] = "                                                  ";
            ss[1] = " ██████╗░██╗░░░░░░█████╗░░█████╗░██╗░░██╗░██████╗ ";
            ss[2] = " ██╔══██╗██║░░░░░██╔══██╗██╔══██╗██║░██╔╝██╔════╝ ";
            ss[3] = " ██████╦╝██║░░░░░██║░░██║██║░░╚═╝█████═╝░╚█████╗░ ";
            ss[4] = " ██╔══██╗██║░░░░░██║░░██║██║░░██╗██╔═██╗░░╚═══██╗ ";
            ss[5] = " ██████╦╝███████╗╚█████╔╝╚█████╔╝██║░╚██╗██████╔╝ ";
            ss[6] = " ╚═════╝░╚══════╝░╚════╝░░╚════╝░╚═╝░░╚═╝╚═════╝░ ";
            ss[7] = "                                                  ";




            for (int i = 0; i < ss.Length; i++)
            {
                for (int j = 0; j < ss[i].Length; j++)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(j + 15, i + 10);
                    Console.Write(ss[i][j]);
                    Thread.Sleep(15);
                }
                Console.SetCursorPosition(0, 0);
                Console.ResetColor();
            }
        }



        public static Space[,] Field = new Space[6, 6];
        public static Block[] blocks = new Block[0];                         // Створення типу, яким будуть подаватись основні об'єкти гри
        public static Block MainBlock = new Block(ConsoleColor.Green);



        public static void UpdateField(Space[,] Field)        // Метод для заповнення поля об'єктами
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(6, 2);
            Console.Write("Freedom");


            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < Field.GetLength(0); i++)
            {
                for (int j = 0; j < Field.GetLength(1); j++)
                {
                    Console.ForegroundColor = Field[i, j].Color;
                    if (Field[i, j].IsBlock)
                        Console.Write('█');
                    else
                        Console.Write(' ');

                }
                Console.WriteLine();
            }

        }

        public static bool Win(Block MainBlock)
        {
            if (MainBlock.PointList[1].X == 2 && MainBlock.PointList[1].Y == 5)
                return true;
            else
                return false;
        }

        public static void Move(Block MovedBlock, Block MainBlock)
        {
            ConsoleKeyInfo move;
            bool Horizontally = true;

            for (int i = 0; i < MovedBlock.PointList.Count - 1; i++)  // Перевірка чи блок довший по y чи x
            {
                if (MovedBlock.PointList[i].Y == MovedBlock.PointList[i + 1].Y)
                { Horizontally = false; }
            }

            for (int i = 0; i < MovedBlock.PointList.Count; i++)
                MovedBlock.PointList[i].Color = ConsoleColor.Gray;

            UpdateField(Field);

            if (Horizontally)
            {
                while (true)
                {
                    if (Win(MainBlock)) return;

                    move = Console.ReadKey(true);
                    switch (move.Key)
                    {
                        case ConsoleKey.Enter: return;

                        case ConsoleKey.RightArrow:
                            {
                                if (MovedBlock.PointList[MovedBlock.PointList.Count - 1].Y < Field.GetLength(1) - 1)  //Обмеження поля
                                {
                                    if (!Field[MovedBlock.PointList[MovedBlock.PointList.Count - 1].X, MovedBlock.PointList[MovedBlock.PointList.Count - 1].Y + 1].IsBlock)
                                    {
                                        Field[MovedBlock.PointList[MovedBlock.PointList.Count - 1].X, MovedBlock.PointList[MovedBlock.PointList.Count - 1].Y + 1].IsBlock = true;
                                        MovedBlock.PointList.Add(Field[MovedBlock.PointList[MovedBlock.PointList.Count - 1].X, MovedBlock.PointList[MovedBlock.PointList.Count - 1].Y + 1]);
                                        MovedBlock.PointList[MovedBlock.PointList.Count - 1].NormalColor = MovedBlock.BlockColor;
                                        MovedBlock.PointList[MovedBlock.PointList.Count - 1].Color = ConsoleColor.Gray;

                                        MovedBlock.PointList[0].IsBlock = false;
                                        MovedBlock.PointList[0].NormalColor = ConsoleColor.Black;
                                        MovedBlock.PointList.RemoveAt(0);
                                    }
                                }
                                break;

                            }

                        case ConsoleKey.LeftArrow:
                            {
                                if (MovedBlock.PointList[0].Y > 0)
                                {
                                    if (!Field[MovedBlock.PointList[0].X, MovedBlock.PointList[0].Y - 1].IsBlock)
                                    {
                                        Field[MovedBlock.PointList[0].X, MovedBlock.PointList[0].Y - 1].IsBlock = true;
                                        MovedBlock.PointList.Insert(0, Field[MovedBlock.PointList[0].X, MovedBlock.PointList[0].Y - 1]);
                                        MovedBlock.PointList[0].NormalColor = MovedBlock.BlockColor;
                                        MovedBlock.PointList[0].Color = ConsoleColor.Gray;

                                        Field[MovedBlock.PointList[MovedBlock.PointList.Count - 1].X, MovedBlock.PointList[MovedBlock.PointList.Count - 1].Y].IsBlock = false;
                                        MovedBlock.PointList[MovedBlock.PointList.Count - 1].NormalColor = ConsoleColor.Black;
                                        MovedBlock.PointList.RemoveAt(MovedBlock.PointList.Count - 1);
                                    }
                                }
                                break;

                            }

                    }
                    UpdateField(Field);

                }
            }

            if (!Horizontally)
            {
                while (true)
                {
                    if (Win(MainBlock)) return;

                    move = Console.ReadKey(true);
                    switch (move.Key)
                    {
                        case ConsoleKey.Enter: return;

                        case ConsoleKey.DownArrow:
                            {
                                if (MovedBlock.PointList[MovedBlock.PointList.Count - 1].X < Field.GetLength(0) - 1)
                                {
                                    if (!Field[MovedBlock.PointList[MovedBlock.PointList.Count - 1].X + 1, MovedBlock.PointList[MovedBlock.PointList.Count - 1].Y].IsBlock)
                                    {
                                        Field[MovedBlock.PointList[MovedBlock.PointList.Count - 1].X + 1, MovedBlock.PointList[MovedBlock.PointList.Count - 1].Y].IsBlock = true;
                                        MovedBlock.PointList.Add(Field[MovedBlock.PointList[MovedBlock.PointList.Count - 1].X + 1, MovedBlock.PointList[MovedBlock.PointList.Count - 1].Y]);
                                        MovedBlock.PointList[MovedBlock.PointList.Count - 1].NormalColor = MovedBlock.BlockColor;
                                        MovedBlock.PointList[MovedBlock.PointList.Count - 1].Color = ConsoleColor.Gray;

                                        Field[MovedBlock.PointList[0].X, MovedBlock.PointList[0].Y].IsBlock = false;
                                        MovedBlock.PointList[0].NormalColor = ConsoleColor.Black;
                                        MovedBlock.PointList.RemoveAt(0);
                                    }
                                }
                                break;

                            }

                        case ConsoleKey.UpArrow:
                            {
                                if (MovedBlock.PointList[0].X > 0)
                                {

                                    if (!Field[MovedBlock.PointList[0].X - 1, MovedBlock.PointList[0].Y].IsBlock)
                                    {
                                        Field[MovedBlock.PointList[0].X - 1, MovedBlock.PointList[0].Y].IsBlock = true;
                                        MovedBlock.PointList.Insert(0, Field[MovedBlock.PointList[0].X - 1, MovedBlock.PointList[0].Y]);
                                        MovedBlock.PointList[0].NormalColor = MovedBlock.BlockColor;
                                        MovedBlock.PointList[0].Color = ConsoleColor.Gray;

                                        Field[MovedBlock.PointList[MovedBlock.PointList.Count - 1].X, MovedBlock.PointList[MovedBlock.PointList.Count - 1].Y].IsBlock = false;
                                        MovedBlock.PointList[MovedBlock.PointList.Count - 1].NormalColor = ConsoleColor.Black;
                                        MovedBlock.PointList.RemoveAt(MovedBlock.PointList.Count - 1);

                                    }
                                }
                                break;

                            }

                    }
                    UpdateField(Field);
                }
            }

        }

        public static void Level(ConsoleKeyInfo lvl)
        {

            switch (lvl.Key)
            {
                case ConsoleKey.D1:
                    {
                        var blockLevel1 = new Block[]
                        {
                        new Block(ConsoleColor.DarkBlue, Field[0,0], Field[0,1]),
                        new Block(ConsoleColor.Magenta,Field[0,3],Field[1,3]),
                        new Block(ConsoleColor.DarkBlue,Field[2,5],Field[3,5],Field[4,5]),
                        new Block(ConsoleColor.DarkBlue,Field[3,1],Field[4,1]),
                        new Block(ConsoleColor.Blue,Field[4,3],Field[4,4]),
                        new Block(ConsoleColor.Magenta,Field[5,3],Field[5,4],Field[5,5])

                        };
                        Block MainBlocklevel1 = new Block(ConsoleColor.Green, Field[2, 0], Field[2, 1]);
                        Array.Resize<Block>(ref blocks, blockLevel1.Length);
                        blocks = blockLevel1;
                        MainBlock = MainBlocklevel1;
                        break;
                    }
                case ConsoleKey.D2:
                    {
                        var blockLevel2 = new Block[]
                        {
                            new Block(ConsoleColor.Blue, Field[0,2], Field[1,2]),
                            new Block(ConsoleColor.Red, Field[1,0],Field[1,1]),
                            new Block(ConsoleColor.DarkRed,Field[0,3],Field[0,4],Field[0,5]),
                            new Block(ConsoleColor.Red,Field[1,5],Field[2,5]),
                            new Block(ConsoleColor.Cyan,Field[2,1],Field[3,1], Field[4,1]),
                            new Block(ConsoleColor.DarkRed,Field[5,0],Field[5,1], Field[5,2]),
                            new Block(ConsoleColor.DarkBlue,Field[3,3],Field[4,3], Field[5,3]),
                            new Block(ConsoleColor.Magenta,Field[3,4],Field[3,5]),
                            new Block(ConsoleColor.Blue,Field[4,4],Field[5,4]),


                        };
                        Block MainBlocklevel2 = new Block(ConsoleColor.Green, Field[2, 2], Field[2, 3]);
                        Array.Resize<Block>(ref blocks, blockLevel2.Length);
                        blocks = blockLevel2;
                        MainBlock = MainBlocklevel2;
                        break;
                    }
                case ConsoleKey.D3:
                    {
                        var blockLevel3 = new Block[]
                        {
                            new Block(ConsoleColor.DarkBlue, Field[0,0], Field[0,1]),
                            new Block(ConsoleColor.Magenta,Field[1,0],Field[2,0],Field[3,0]),
                            new Block(ConsoleColor.Blue,Field[0,2],Field[1,2]),
                            new Block(ConsoleColor.DarkBlue,Field[0,4],Field[1,4], Field[2,4]),
                            new Block(ConsoleColor.Magenta,Field[3,4],Field[3,5]),
                            new Block(ConsoleColor.Yellow,Field[3,2],Field[4,2]),
                            new Block(ConsoleColor.Cyan,Field[0,3],Field[1,3]),
                            new Block(ConsoleColor.DarkMagenta,Field[3,3],Field[3,4], Field[3,5]),
                            new Block(ConsoleColor.DarkRed,Field[4,5],Field[5,5]),

                        };
                        Block MainBlocklevel3 = new Block(ConsoleColor.Green, Field[2, 2], Field[2, 3]);
                        Array.Resize<Block>(ref blocks, blockLevel3.Length);
                        blocks = blockLevel3;
                        MainBlock = MainBlocklevel3;
                        break;
                    }
                case ConsoleKey.D4:
                    {
                        var blockLevel4 = new Block[]
                        {
                            new Block(ConsoleColor.DarkBlue, Field[0,0], Field[0,1], Field[0,2]),
                            new Block(ConsoleColor.Magenta,Field[1,1],Field[1,2]),
                            new Block(ConsoleColor.Blue,Field[0,5],Field[1,5]),
                            new Block(ConsoleColor.DarkRed,Field[3,0],Field[3,1]),
                            new Block(ConsoleColor.Cyan,Field[2,2],Field[3,2], Field[4,2]),
                            new Block(ConsoleColor.Red,Field[2,5],Field[3,5]),
                            new Block(ConsoleColor.DarkBlue,Field[4,1],Field[5,1]),
                            new Block(ConsoleColor.DarkRed,Field[5,4],Field[5,5]),
                            new Block(ConsoleColor.DarkMagenta,Field[4,3],Field[4,4], Field[4,5]),
                            new Block(ConsoleColor.Yellow,Field[5,2],Field[5,3])

                        };
                        Block MainBlocklevel4 = new Block(ConsoleColor.Green, Field[2, 0], Field[2, 1]);
                        Array.Resize<Block>(ref blocks, blockLevel4.Length);
                        blocks = blockLevel4;
                        MainBlock = MainBlocklevel4;
                        break;
                    }
                case ConsoleKey.D5:
                    {
                        var blockLevel5 = new Block[]
                        {
                            new Block(ConsoleColor.DarkBlue, Field[0,0], Field[1,0]),
                            new Block(ConsoleColor.Yellow,Field[0,1],Field[0,2]),
                            new Block(ConsoleColor.Blue,Field[0,5],Field[1,5]),
                            new Block(ConsoleColor.DarkMagenta,Field[2,2],Field[3,2]),
                            new Block(ConsoleColor.Magenta,Field[1,3],Field[2,3], Field[3,3]),
                            new Block(ConsoleColor.DarkRed,Field[3,4],Field[3,5]),
                            new Block(ConsoleColor.Red,Field[3,0],Field[4,0]),
                            new Block(ConsoleColor.Cyan,Field[4,1],Field[4,2]),
                            new Block(ConsoleColor.DarkBlue,Field[4,5],Field[5,5])
                        };
                        Block MainBlocklevel5 = new Block(ConsoleColor.Green, Field[2, 0], Field[2, 1]);
                        Array.Resize<Block>(ref blocks, blockLevel5.Length);
                        blocks = blockLevel5;
                        MainBlock = MainBlocklevel5;
                        break;
                    }
                case ConsoleKey.Q:
                    {
                        Environment.Exit(0);
                        break;
                    }
            }
        }

        void History()
        {
            string[] hh = new string[8];
            hh[0] = "                                                  ";
            hh[1] = "A green block lives in the world, ";
            hh[2] = "In the world where the blocks do not strive for success";
            hh[3] = "and live in their own little rooms.";
            hh[4] = "The green block is not like the others,";
            hh[5] = "He wanted to move and get out of his usual room. ";
            hh[6] = "Help him!";
            hh[7] = "Help if you also want to leave the room of boring people... ";
            



            for (int i = 0; i < hh.Length; i++)
            {
                for (int j = 0; j < hh[i].Length; j++)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(j , i + 10);
                    Console.Write(hh[i][j]);
                    Thread.Sleep(80);
                    Console.Write(' ');
                }
               
                Console.ResetColor();
            }
        }
        public void Game()
        {
            Init();
            SplashScreen();
            Thread.Sleep(40);
            Console.Clear();
            History();


        }
        static void Main(string[] args)
        {
            try
            {
            
                Program program = new Program();
                program.Game();
                Console.CursorVisible = true;

          menu:
                for (int i = 0; i < Field.GetLength(0); i++)
                {
                    for (int j = 0; j < Field.GetLength(1); j++)
                    {
                        Field[i, j] = new Space(i, j);
                    }
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(30, 19);
                Console.Write("Select level: 1  2  3  4  5 ");
                var level = Console.ReadKey();

                Level(level);
                Console.Clear();


                ConsoleKeyInfo move;
                int CursorPositionX = 0;
                int CursorPositionY = 0;


                while (true)
                {
                    if (Win(MainBlock))
                    {
                        UpdateField(Field);
                        Console.SetCursorPosition(30, 25);
                        Console.ForegroundColor = ConsoleColor.Green;
                        string[] vv = new string[8];
                        vv[0] = "                                                       ";
                        vv[1] = " ██╗░░░██╗██╗░█████╗░████████╗░█████╗░██████╗░██╗░░░██╗";
                        vv[2] = " ██║░░░██║██║██╔══██╗╚══██╔══╝██╔══██╗██╔══██╗╚██╗░██╔╝";
                        vv[3] = " ╚██╗░██╔╝██║██║░░╚═╝░░░██║░░░██║░░██║██████╔╝░╚████╔╝░";
                        vv[4] = " ░╚████╔╝░██║██║░░██╗░░░██║░░░██║░░██║██╔══██╗░░╚██╔╝░░";
                        vv[5] = " ░░╚██╔╝░░██║╚█████╔╝░░░██║░░░╚█████╔╝██║░░██║░░░██║░░░";
                        vv[6] = " ░░░╚═╝░░░╚═╝░╚════╝░░░░╚═╝░░░░╚════╝░╚═╝░░╚═╝░░░╚═╝░░░";
                        vv[7] = "             You helped the green block!               ";

                        for (int i = 0; i < vv.Length; i++)
                        {
                            for (int j = 0; j < vv[i].Length; j++)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.SetCursorPosition(j + 15, i + 10);
                                Console.Write(vv[i][j]);
                            }
                            Console.ResetColor();
                        }

                        break;
                    }

                    UpdateField(Field);
                    for (int i = 0; i < blocks.Length; i++)
                        blocks[i].NormaliseColors();

                    MainBlock.NormaliseColors();

                    Console.SetCursorPosition(CursorPositionX, CursorPositionY);
                    move = Console.ReadKey(true);
                    switch (move.Key)
                    {
                        case ConsoleKey.Enter:
                            {
                                for (int i = 0; i < blocks.Length; i++)
                                {
                                    for (int j = 0; j < blocks[i].PointList.Count; j++)
                                        if (blocks[i].PointList[j].X == CursorPositionY && blocks[i].PointList[j].Y == CursorPositionX)  // Перевірка чи курсор знаходиться на блоці
                                            Move(blocks[i], MainBlock);
                                }

                                for (int i = 0; i < MainBlock.PointList.Count; i++)
                                {
                                    if (MainBlock.PointList[i].X == CursorPositionY && MainBlock.PointList[i].Y == CursorPositionX)
                                        Move(MainBlock, MainBlock);
                                }
                                break;
                            }

                        case ConsoleKey.UpArrow:
                            {
                                if (CursorPositionY > 0)
                                {
                                    CursorPositionY--;
                                    Console.SetCursorPosition(CursorPositionX, CursorPositionY);
                                    UpdateField(Field);

                                }
                                break;
                            }

                        case ConsoleKey.DownArrow:
                            {
                                if (CursorPositionY < Field.GetLength(1) - 1)
                                {
                                    CursorPositionY++;
                                    Console.SetCursorPosition(CursorPositionX, CursorPositionY);
                                    UpdateField(Field);

                                }
                                break;
                            }

                        case ConsoleKey.LeftArrow:
                            {
                                if (CursorPositionX > 0)
                                {
                                    CursorPositionX--;
                                    Console.SetCursorPosition(CursorPositionX, CursorPositionY);
                                    UpdateField(Field);

                                }
                                break;
                            }

                        case ConsoleKey.RightArrow:
                            {
                                if (CursorPositionX < Field.GetLength(0) - 1)
                                {
                                    CursorPositionX++;
                                    Console.SetCursorPosition(CursorPositionX, CursorPositionY);
                                    UpdateField(Field);

                                }
                                break;
                            }
                        case ConsoleKey.Q:
                            {
                                Console.Clear();
                                goto menu;
                                
                            }
                    }
                }



            }
            catch (Exception)
            {
               Console.WriteLine("Error!");
            }

        }
    }
}
        

