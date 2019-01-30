using DiffPlex;
using DiffPlex.DiffBuilder.Model;
using DiffPlex.Model;
using System;
using System.Collections.Generic;

namespace WordDiff
{
    public static class DiffEngine
    {
        static string TagInsertOpen = "<ins>";
        static string TagInsertClose = "</ins>";
        static string TagDeletetOpen = "<del>";
        static string TagDeleteClose = "</del>";

        private static string DoTheDiff(string oldText, string newText)
        {
            var diff = BuildDiffModel(oldText, newText);

            var prevType = diff.Lines[0].Type;
            var result = GetTag(diff.Lines[0].Type, true);
            var needToClose = true;
            var pieceCount = 0;

            foreach (var piece in diff.Lines)
            {
                if (prevType != piece.Type)
                {
                    result += GetTag(prevType, false) + GetTag(piece.Type, true) + piece.Text;
                    needToClose = true;
                }
                else
                {
                    result += piece.Text;
                }

                pieceCount++;

                if (pieceCount == diff.Lines.Count && needToClose)
                {
                    result += GetTag(piece.Type, false);
                }

                prevType = piece.Type;
            }

            return result;
        }

        private static string GetTag(ChangeType changeType, bool isOpenningTag)
        {
            string tag;

            switch (changeType)
            {
                case ChangeType.Deleted:
                    tag = (isOpenningTag == true) ? TagDeletetOpen : TagDeleteClose;
                    break;
                case ChangeType.Inserted:
                    tag = (isOpenningTag == true) ? TagInsertOpen : TagInsertClose;
                    break;
                default:
                    tag = string.Empty;
                    break;
            }
            return tag;
        }

        private static DiffPaneModel BuildDiffModel(string oldText, string newText)
        {
            char[] WordSeparaters = {
                ' ',
                '\t',
                '.',
                ',',
                ';',
                ':',
                '(',
                ')',
                '{',
                '}',
                '[',
                ']',
                '!',
                '/',
                '\\'};

            var model = new DiffPaneModel();
            var differ = new Differ();
            var diffResult = differ.CreateWordDiffs(oldText, newText, false, WordSeparaters);
            BuildDiffPieces(diffResult, model.Lines);
            return model;
        }

        private static void BuildDiffPieces(DiffResult diffResult, List<DiffPiece> pieces)
        {
            int bPos = 0;

            foreach (var diffBlock in diffResult.DiffBlocks)
            {
                for (; bPos < diffBlock.InsertStartB; bPos++)
                    pieces.Add(new DiffPiece(diffResult.PiecesNew[bPos], ChangeType.Unchanged, bPos + 1));

                int i = 0;
                for (; i < Math.Min(diffBlock.DeleteCountA, diffBlock.InsertCountB); i++)
                    pieces.Add(new DiffPiece(diffResult.PiecesOld[i + diffBlock.DeleteStartA], ChangeType.Deleted));

                i = 0;
                for (; i < Math.Min(diffBlock.DeleteCountA, diffBlock.InsertCountB); i++)
                {
                    pieces.Add(new DiffPiece(diffResult.PiecesNew[i + diffBlock.InsertStartB], ChangeType.Inserted, bPos + 1));
                    bPos++;
                }

                if (diffBlock.DeleteCountA > diffBlock.InsertCountB)
                {
                    for (; i < diffBlock.DeleteCountA; i++)
                        pieces.Add(new DiffPiece(diffResult.PiecesOld[i + diffBlock.DeleteStartA], ChangeType.Deleted));
                }
                else
                {
                    for (; i < diffBlock.InsertCountB; i++)
                    {
                        pieces.Add(new DiffPiece(diffResult.PiecesNew[i + diffBlock.InsertStartB], ChangeType.Inserted, bPos + 1));
                        bPos++;
                    }
                }
            }

            for (; bPos < diffResult.PiecesNew.Length; bPos++)
                pieces.Add(new DiffPiece(diffResult.PiecesNew[bPos], ChangeType.Unchanged, bPos + 1));
        }
    }
}
