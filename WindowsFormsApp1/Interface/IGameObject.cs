using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public interface IGameObject
    {
        PictureBox ObjectPictureBox { get; set; }
        bool IsActive { get; set; }
        void InitializeObject(); // Nesneyi başlatmak için kullanılır
        void UpdatePosition(); // Nesnenin pozisyonunu günceller
        void ClearObject(); // Nesneyi temizler
    }

}
