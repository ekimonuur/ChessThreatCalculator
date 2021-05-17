using System;
using System.Collections.Generic;
using System.IO;

namespace ChessThreatCalculator
{
    class Program
    {
        //Board üzerindeki elemanların pozisyonlarını tutan dizin
        static string[,] boardPieces = new string[8, 8];

        //Board üzerindeki elemanlarını değerlerini tutan dizin
        static float[,] boardValues = new float[8, 8];

        //Elemanların sayı değerlerini tutan liste
        static Dictionary<string, int> pieceValues = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            Console.WriteLine("Chess Threat Calculator\r\n");

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: ChessThreatCalculator board1.txt");
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("Error: File not found");
                return;
            }

            //Elemanların sayı değerlerini doldur
            pieceValues.Add("x", 0);
            pieceValues.Add("p", 1);
            pieceValues.Add("a", 3);
            pieceValues.Add("f", 3);
            pieceValues.Add("k", 5);
            pieceValues.Add("v", 9);
            pieceValues.Add("s", 100);

            //Verilen dosyadaki verileri satırlara böl
            var boardRows = File.ReadAllLines(args[0]);

            if(boardRows.Length!=8)
            {
                Console.WriteLine("Error: Invalid row count.");
                return;
            }

            Console.WriteLine("Board status:");

            for (int rowNo = 0; rowNo < 8; rowNo++)
            {
                //Her bir satırı sütunlara böl
                var boardColumns = boardRows[rowNo].Split(' ');

                if (boardColumns.Length != 8)
                {
                    Console.WriteLine("Error: Invalid column count.");
                    return;
                }

                var pieceLine = "";
                var valueLine = "";
                for (int colNo = 0; colNo < 8; colNo++)
                {
                    //Dizindeki ilgili pozisyona elemanı yerleştir
                    boardPieces[rowNo, colNo] = boardColumns[colNo];

                    //Dizindeki ilgili pozisyona değerini yerleştir
                    boardValues[rowNo, colNo] = pieceValues[boardColumns[colNo].Substring(0, 1)];

                    //Ekrana değerleri basmak için
                    pieceLine += boardPieces[rowNo,colNo] + ' ';
                    valueLine += boardValues[rowNo, colNo].ToString().PadLeft(4, ' ') + ' ';
                }
                Console.WriteLine(valueLine + pieceLine);
            }

            Console.WriteLine("");
            Console.WriteLine("Calculated piece values:");

            //Tüm pozisyonları gezerek tehdit kontrolü yap
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    ThreatOtherPieces(row, col);
                }
            }

            //Sonuçta oluşan sayı değeri haritasından siyah ve beyaz için
            //değerleri topla
            var totalBlack = 0f;
            var totalWhite = 0f;

            for (int row = 0; row < 8; row++)
            {
                var valueLine = "";
                for (int col = 0; col < 8; col++)
                {
                    //Siyah için sayıyı siyah toplamına ekle
                    if (boardPieces[row, col].Substring(1, 1) == "s")
                    {
                        totalBlack += boardValues[row, col];
                    }
                    //Beyaz için sayıyı siyah toplamına ekle
                    else
                    {
                        totalWhite += boardValues[row, col];
                    }

                    //Ekrana basmak için
                    valueLine += boardValues[row, col].ToString().PadLeft(4, ' ') + ' ';
                }
                Console.WriteLine(valueLine);
            }
            Console.WriteLine("");
            Console.WriteLine("Total piece values:");

            //Sonucu yaz uygulamayı bitir
            Console.WriteLine($"Black: {totalBlack} White: {totalWhite}");
        }

        //Belirtilen noktada eleman varsa o elemanın tehdit ettiği elemanları bul ve
        //sayılarını yarıya indir
        static void ThreatOtherPieces(int pieceRow, int pieceCol)
        {
            var pieceColor = boardPieces[pieceRow, pieceCol].Substring(1,1);
            var pieceType = boardPieces[pieceRow, pieceCol].Substring(0, 1);
            var targetColor = pieceColor == "s" ? "b" : "s";

            //Elemanın tipine göre ilgili metoda gönder
            switch (pieceType)
            {
                case "a":
                    ThreatWithKnight(pieceRow, pieceCol, targetColor);
                    break;
                case "f":
                    ThreatWithBishop(pieceRow, pieceCol, targetColor);
                    break;
            }
        }

        //At için tehdit kontrolü yapar
        static void ThreatWithKnight(int pieceRow, int pieceCol, string targetColor)
        {

            //Up
            Threat(pieceRow - 2, pieceCol - 1, targetColor);
            Threat(pieceRow - 2, pieceCol + 1, targetColor);

            //Right
            Threat(pieceRow - 1, pieceCol + 2, targetColor);
            Threat(pieceRow + 1, pieceCol + 2, targetColor);

            //Down
            Threat(pieceRow + 2, pieceCol - 1, targetColor);
            Threat(pieceRow + 2, pieceCol + 1, targetColor);

            //Left
            Threat(pieceRow - 1, pieceCol - 2, targetColor);
            Threat(pieceRow + 1, pieceCol - 2, targetColor);
        }

        //Fil için tehdit kontrolü yapar
        static void ThreatWithBishop(int pieceRow, int pieceCol, string targetColor)
        {
            bool upLeftPieceFound = false;
            bool upRightPieceFound = false;
            bool downLeftPieceFound = false;
            bool downRightPieceFound = false;


            //Dört yöne çapraz ilerleyerek tehdit arar. Eğer tehdit ararken tehdidi yada
            //kendi elemanımızdan birini bulursa o yöndeki arama durur
            for (int i = 1; i < 8; i++)
            {
                //Up-Left
                if (!upLeftPieceFound && Threat(pieceRow - i, pieceCol - i, targetColor))
                    upLeftPieceFound=true;

                //Up-Right
                if (!upRightPieceFound && Threat(pieceRow - i, pieceCol + i, targetColor))
                    upRightPieceFound=true;

                //Down-Right
                if (!downRightPieceFound && Threat(pieceRow + i, pieceCol + i, targetColor))
                    downRightPieceFound = true;

                //Down-Left
                if (!downLeftPieceFound && Threat(pieceRow + i, pieceCol - i, targetColor))
                    downLeftPieceFound = true;
            }
        }


        //Belirtilen noktada tehdit edebileceğimiz bir eleman var mı kontrol eder
        //Varsa değer dizinindeki ilgili noktanın pozisyonuna ilgili elemanın yarı değerini yazar
        //Aynı zamanda geriye belirtilen noktada herhangi bir eleman varsa true döndürür
        //Bu filin çapraz yönde hareket ederek tek tek kontrol yapan algoritmasını durdurur
        //Çünkü fil çapraz olarak tehdit ederken bir elemana denk gelirse onun arkasındakileri
        //tehdit edemez
        static bool Threat(int targetRow, int targetCol, string targetColor)
        {
            //Belirtilen nokta oyun alanı dışında ise atla
            if (targetRow >= 0 && targetRow < 8 && targetCol >= 0 && targetCol < 8)
            {
                //Belirtilen nokta boş ise atla
                if (boardPieces[targetRow, targetCol] != "xx")
                {
                    //Belirtilen noktadaki eleman hedef renkte değilse atla
                    if (boardPieces[targetRow, targetCol].Substring(1, 1) == targetColor)
                    {
                        boardValues[targetRow, targetCol] = pieceValues[boardPieces[targetRow, targetCol].Substring(0, 1)] / 2f;
                        return true;
                    }

                    //Kendi elemanlarımızdan biri var filin çapraz kontrolünü
                    //durdurmak için true gönderiyoruz
                    return true;
                }
            }

            return false;
        }

    }
}
