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
    public partial class FrmTimKiemHang : Form
    {
        public FrmTimKiemHang()
        {
            InitializeComponent();
        }
        DataTable tkH;
        private void FrmTimKiemHang_Load(object sender, EventArgs e)
        {
            ResetValuesTKH();
            dgvTimKiemHang.DataSource = null;
        }

        private void ResetValuesTKH()
        {
            foreach (Control Ctr in this.Controls)
            {
                if (Ctr is TextBox)
                {
                    Ctr.Text = "";
                }
                txtMaHang.Focus();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtMaHang.Text == "" && txtTenHang.Text == "" && cboNguyenLieu.Text == "" &&txtSoLuong.Text == "" && txtDonGiaNhap.Text=="" && txtDonGiaBan.Text=="" && txtGhiChu.Text=="")
            {
                MessageBox.Show("Hãy nhập một điều kiện tìm kiếm!!!", "Yêu cầu ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "SELECT * FROM tblHang WHERE 1=1";
            if (txtMaHang.Text != "")
            {
                sql = sql + " AND MaHang Like N'%" + txtMaHang.Text + "%'";
            }
            if (txtTenHang.Text != "")
            {
                sql = sql + " AND TenHang Like N'%" + txtTenHang.Text + "%'";
            }
            if (cboNguyenLieu.Text != "")
            {
                sql = sql + " AND MaChatLieu Like N'%" + cboNguyenLieu.Text + "%'";
            }
            if (txtSoLuong.Text != "")
            {
                sql = sql + " AND SoLuong Like N'%" + txtSoLuong.Text + "%'";
            }
            if (txtDonGiaNhap.Text != "")
            {
                sql = sql + " AND DonGiaNhap Like N'%" + txtDonGiaNhap.Text + "%'";
            }
            if (txtDonGiaBan.Text != "")
            {
                sql = sql + " AND DonGiaBan Like N'%" + txtDonGiaBan.Text + "%'";
            }
            if (txtGhiChu.Text != "")
            {
                sql = sql + " AND GhiChu Like N'%" + txtGhiChu.Text + "%'";
            }
            tkH = Database.GetDataToTable(sql);
            if (tkH.Rows.Count == 0)
            {
                MessageBox.Show("Không có bản ghi thỏa mãn điều kiện!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Có " + tkH.Rows.Count + " bản ghi thỏa mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dgvTimKiemHang.DataSource = tkH;
            LoadDataTKH();
        }

        private void LoadDataTKH()
        {
            dgvTimKiemHang.Columns[0].HeaderText = "Mã hàng";
            dgvTimKiemHang.Columns[1].HeaderText = "Tên hàng";
            dgvTimKiemHang.Columns[2].HeaderText = "Nguyên liệu";
            dgvTimKiemHang.Columns[3].HeaderText = "Số lượng";
            dgvTimKiemHang.Columns[4].HeaderText = "Đơn giá nhập";
            dgvTimKiemHang.Columns[5].HeaderText = "Đơn giá bán";
            dgvTimKiemHang.Columns[6].HeaderText = "Ghi chú";
            dgvTimKiemHang.Columns[0].Width = 80;
            dgvTimKiemHang.Columns[1].Width = 140;
            dgvTimKiemHang.Columns[2].Width = 80;
            dgvTimKiemHang.Columns[3].Width = 80;
            dgvTimKiemHang.Columns[4].Width = 100;
            dgvTimKiemHang.Columns[5].Width = 100;
            dgvTimKiemHang.Columns[6].Width = 200;
            dgvTimKiemHang.AllowUserToAddRows = false;
            dgvTimKiemHang.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void btnTimLai_Click(object sender, EventArgs e)
        {
            ResetValuesTKH();
            dgvTimKiemHang.DataSource = null;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvTimKiemHang_DoubleClick(object sender, EventArgs e)
        {
            string mahang;
            if (MessageBox.Show("Bạn có muốn hiển thị thông tin chi tiết?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                mahang = dgvTimKiemHang.CurrentRow.Cells["colMaHang"].Value.ToString();
                frmDMHang frmH = new frmDMHang();
                frmH.txtMaHang.Text = mahang;
                frmH.StartPosition = FormStartPosition.CenterParent;
                frmH.ShowDialog();
            }
        }
    }
}
