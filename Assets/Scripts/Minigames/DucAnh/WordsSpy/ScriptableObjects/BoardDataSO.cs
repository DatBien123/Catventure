using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu()]
public class BoardDataSO : ScriptableObject
{
    [System.Serializable]
    public class SearchingWord {
        public string Word;
    }

    [System.Serializable]
    public class BoardRow {
        public int Size;
        public string[] Row;

        public BoardRow() { }

        public BoardRow(int size) {
            CreateRow(size);
        }

        private void CreateRow(int size) {
            Size = size;
            Row = new string[Size];
            ClearRow();
        }

        public void ClearRow() {
            for (int i = 0; i < Size; i++) {
                Row[i] = " ";
            }
        }
    }

    public int timeInSeconds;
    public int Columns = 0;
    public int Rows = 0;

    public BoardRow[] Board;
    public List<SearchingWord> SearchWords = new List<SearchingWord>();

    public void ClearBoard() {
        for (int i = 0; i < Columns; i++) {
            Board[i].ClearRow();
        }
    }

    public void CreateBoard() {
        Board = new BoardRow[Columns];
        for (int i = 0;i < Columns; i++) {
            Board[i] = new BoardRow(Rows);
        }
    }
}
