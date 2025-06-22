using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class ScoreLoader
    {
        // Belirtilen dosyadan en yüksek skorları yükleyen metod
        public List<ScoreBoard> LoadTopScores(string filePath, int topCount = 5)
        {
            var scores = new List<ScoreBoard>();

            // Dosyadan her bir satırı okuma ve işleme
            foreach (var line in File.ReadAllLines(filePath))
            {
                // Her satırı "Oyuncu Adı :" ve " Skor :" bölümlerine ayırma
                var parts = line.Split(new[] { "Oyuncu Adı :", " Skor :" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    // Her parçayı ScoreBoard nesnesine dönüştürme ve listeye ekleme
                    scores.Add(new ScoreBoard { PlayerName = parts[0].Trim(), PlayerScore = int.Parse(parts[1].Trim()) });
                }
            }

            // Skorları azalan sırayla sıralayıp, en yüksek 'topCount' sayısını döndürme
            return scores.OrderByDescending(s => s.PlayerScore).Take(topCount).ToList();
        }
    }
}

