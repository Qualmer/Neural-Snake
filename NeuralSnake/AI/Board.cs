﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NeuralSnake.AI
{
    public class Board
    {
        public int Turn { get; private set; }
        public int Score { get; private set; }
        public Direction Direction { get; set; }
        public bool Dead { get; private set; }
        public Point SnakeHead => _snake[0];

        private FieldType[,] _board;
        private int _width;
        private int _height;
        private int _foodInterval;
        private int _foodDensity;
        private List<Point> _snake;

        private Random _random;

        public Board(int width, int height, int foodInterval, int foodDensity)
        {
            _board = new FieldType[width, height];
            _width = width;
            _height = height;
            _foodInterval = foodInterval;
            _foodDensity = foodDensity;
            _snake = new List<Point>();
            _random = new Random();

            Dead = false;
            Turn = 1;
            Direction = Direction.None;

            for (var i = 0; i < _width; i++)
            {
                _board[i, 0] = FieldType.Wall;
                _board[i, _height - 1] = FieldType.Wall;
            }

            for (var i = 0; i < _height; i++)
            {
                _board[0, i] = FieldType.Wall;
                _board[_width - 1, i] = FieldType.Wall;
            }

            _snake.Add(new Point(_width / 2, _height / 2));
            _board[_snake[0].X, _snake[0].Y] = FieldType.Head;
        }

        public void NextTurn()
        {
            UpdateSnake();
            UpdateFood();

            Turn++;
            Score++;
        }

        public FieldType[,] GetAll()
        {
            return _board;
        }

        public FieldType this[int x, int y]
        {
            get => _board[x, y];
            set => _board[x, y] = value;
        }

        private void UpdateFood()
        {
            if (Turn % _foodInterval != 0)
            {
                return;
            }

            for (var i = 0; i < _foodDensity; i++)
            {
                var failedCount = 0;
                while (failedCount < 10)
                {
                    var x = _random.Next(_width - 1);
                    var y = _random.Next(_height - 1);

                    var field = _board[x, y];
                    if (field != FieldType.Empty)
                    {
                        failedCount++;
                        continue;
                    }

                    _board[x, y] = FieldType.Food;
                    break;
                }
            }
        }

        private void UpdateSnake()
        {
            if (Direction == Direction.None)
            {
                return;
            }

            var updatedHeadPos = _snake[0];
            switch (Direction)
            {
                case Direction.Up:
                {
                    updatedHeadPos = new Point(updatedHeadPos.X, updatedHeadPos.Y - 1);
                    break;
                }

                case Direction.Down:
                {
                    updatedHeadPos = new Point(updatedHeadPos.X, updatedHeadPos.Y + 1);
                    break;
                }

                case Direction.Right:
                {
                    updatedHeadPos = new Point(updatedHeadPos.X + 1, updatedHeadPos.Y);
                    break;
                }

                case Direction.Left:
                {
                    updatedHeadPos = new Point(updatedHeadPos.X - 1, updatedHeadPos.Y);
                    break;
                }
            }

            var updatedHeadField = _board[updatedHeadPos.X, updatedHeadPos.Y];
            if (updatedHeadField == FieldType.Body || updatedHeadField == FieldType.Wall)
            {
                Dead = true;
                return;
            }

            _board[_snake[0].X, _snake[0].Y] = FieldType.Body;
            _snake.Insert(0, updatedHeadPos);
            _board[_snake[0].X, _snake[0].Y] = FieldType.Head;

            if (updatedHeadField != FieldType.Food)
            {
                _board[_snake.Last().X, _snake.Last().Y] = FieldType.Empty;
                _snake.Remove(_snake.Last());
            }
        }
    }
}
