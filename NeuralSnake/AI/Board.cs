﻿namespace NeuralSnake.AI
{
    public class Board
    {
        private FieldType[,] _board;

        public FieldType this[int x, int y]
        {
            get => _board[x, y];
            set => _board[x, y] = value;
        }

        public void Init(int size)
        {
            _board = new FieldType[size, size];

            for (var i = 0; i < size; i++)
            {
                _board[i, 0] = FieldType.Wall;
                _board[i, size - 1] = FieldType.Wall;

                _board[0, i] = FieldType.Wall;
                _board[size - 1, i] = FieldType.Wall;
            }
        }

        public FieldType[,] GetAll()
        {
            return _board;
        }
    }
}