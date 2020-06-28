using QuanLyTraSua.Class;
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
    public partial class frmTimKiemHoaDon : Form
    {
        public frmTimKiemHoaDon()
        {
            InitializeComponent();
        }

        DataTable tblHDB;
        private void FrmTimKiemHoaDon_Load(object sender, EventArgs e)
        {
            ResetValuesTK();
            dgvHienThiDSHoaDon.DataSource = null;
        }

        private void ResetValuesTK()
        {
            foreach (Control Ctr in this.Controls)
            {
                if (Ctr is TextBox)
                {
                    Ctr.Text = "";
                }
                txtMaHoaDon.Focus();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string sql;
            if ((txtMaHoaDon.Text == "") && (txtThang.Text == "") && (txtNam.Text == "") &&
               (txtMaNhanVien.Text == "") && (txtMakhachhang.Text == "") &&
               (txtTongTien.Text == ""))
            {
                MessageBox.Show("Hãy nhập một điều kiện tìm kiếm!!!", "Yêu cầu ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "SELECT * FROM tblHoaDon WHERE 1=1";
            if (txtMaHoaDon.Text != "")
                sql = sql + " AND MaHoaDon Like N'%" + txtMaHoaDon.Text + "%'";
            if (txtThang.Text != "")
                sql = sql + " AND MONTH(NgayBan) =" + txtThang.Text;
            if (txtNam.Text != "")
                sql = sql + " AND YEAR(NgayBan) =" + txtNam.Text;
            if (txtMaNhanVien.Text != "")
                sql = sql + " AND MaNhanVien Like N'%" + txtMaNhanVien.Text + "%'";
            if (txtMakhachhang.Text != "")
                sql = sql + " AND MaKhachHang Like N'%" + txtMakhachhang.Text + "%'";
            if (txtTongTien.Text != "")
                sql = sql + " AND TongTien <=" + txtTongTien.Text;
            tblHDB = Database.GetDataToTable(sql);
            if (tblHDB.Rows.Count == 0)
            {
                MessageBox.Show("Không có bản ghi thỏa mãn điều kiện!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Có " + tblHDB.Rows.Count + " bản ghi thỏa mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgvHienThiDSHoaDon.DataSource = tblHDB;
            LoadDataTKHD();
        }

        private void LoadDataTKHD()
        {
            dgvHienThiDSHoaDon.Columns[0].HeaderText = "Mã Hoá Đơn";
            dgvHienThiDSHoaDon.Columns[1].HeaderText = "Mã nhân viên";
            dgvHienThiDSHoaDon.Columns[2].HeaderText = "Ngày bán";
            dgvHienThiDSHoaDon.Columns[3].HeaderText = "Mã khách";
            dgvHienThiDSHoaDon.Columns[4].HeaderText = "Tổng tiền";
            dgvHienThiDSHoaDon.Columns[0].Width = 150;
            dgvHienThiDSHoaDon.Columns[1].Width = 100;
            dgvHienThiDSHoaDon.Columns[2].Width = 80;
            dgvHienThiDSHoaDon.Columns[3].Width = 80;
            dgvHienThiDSHoaDon.Columns[4].Width = 80;
            dgvHienThiDSHoaDon.AllowUserToAddRows = false;
            dgvHienThiDSHoaDon.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void btnTimLai_Click(object sender, EventArgs e)
        {
            ResetValuesTK();
            dgvHienThiDSHoaDon.DataSource = null;
        }

        private void txtTongTien_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void dgvHienThiDSHoaDon_DoubleClick(object sender, EventArgs e)
        {
            string mahd;
            if (MessageBox.Show("Bạn có muốn hiển thị thông tin chi tiết?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                mahd = dgvHienThiDSHoaDon.CurrentRow.Cells["colMaHoaDon"].Value.ToString();
                frmHoaDonBanHang frm = new frmHoaDonBanHang();
                frm.txtMaHoaDon.Text = mahd;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
