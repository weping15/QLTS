using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyTraSua
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void mnuThoat_Click(object sender, EventArgs e)
        {
            Class.Database.Disconnect();
            Application.Exit();
        }

        private void mnuChatLieu_Click(object sender, EventArgs e)
        {
            frmDMChatLieu nguyenlieu = new frmDMChatLieu();
            nguyenlieu.MdiParent = this;
            nguyenlieu.Show();
        }

        private void mnuNhanVien_Click(object sender, EventArgs e)
        {
            frmDMNhanVien nhanvien = new frmDMNhanVien();
            nhanvien.MdiParent = this;
            nhanvien.Show();
        }

        private void mnuKhachHang_Click(object sender, EventArgs e)
        {
            frmDMKhachHang khachhang = new frmDMKhachHang();
            khachhang.MdiParent = this;
            khachhang.Show();
        }

        private void mnuHangHoa_Click(object sender, EventArgs e)
        {
            frmDMHang hanghoa = new frmDMHang();
            hanghoa.MdiParent = this;
            hanghoa.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Class.Database.Connect();
        }

        private void mnuHoaDonBan_Click(object sender, EventArgs e)
        {
            frmHoaDonBanHang hdbanhang = new frmHoaDonBanHang();
            hdbanhang.MdiParent = this;
            hdbanhang.Show();
        }

        private void mnuFindHoaDon_Click(object sender, EventArgs e)
        {
            frmTimKiemHoaDon frmTKHD = new frmTimKiemHoaDon();
            frmTKHD.MdiParent = this;
            frmTKHD.Show();
        }

        private void mnuFindHang_Click(object sender, EventArgs e)
        {
            FrmTimKiemHang frmTKH = new FrmTimKiemHang();
            frmTKH.MdiParent = this;
            frmTKH.Show();
        }
    }
}
