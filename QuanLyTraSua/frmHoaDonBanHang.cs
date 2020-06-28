﻿using QuanLyTraSua.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using COMExcel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;

namespace QuanLyTraSua
{
    public partial class frmHoaDonBanHang : Form
    {
        public frmHoaDonBanHang()
        {
            InitializeComponent();
        }
        DataTable tblCTHDB;

        private void frmHoaDonBanHang_Load(object sender, EventArgs e)
        {
            btnThemHoaDon.Enabled = true;
            btnLuuHoaDon.Enabled = false;
            btnHuyHoaDon.Enabled = false;
            btnInHoaDon.Enabled = false;
            txtMaHoaDon.ReadOnly = true;
            txtTenNhanVien.ReadOnly = true;
            txtTenKhachHang.ReadOnly = true;
            txtDiaChi.ReadOnly = true;
            mtbDienThoai.ReadOnly = true;
            txtTenHang.ReadOnly = true;
            txtDonGiaBan.ReadOnly = true;
            txtThanhTien.ReadOnly = true;
            txtTongTien.ReadOnly = true;
            txtGiamGia.Text = "0";
            txtTongTien.Text = "0";
            Database.FillCombo("SELECT MaKhachHang, TenKhachHang FROM tblKhachHang", cboMaKhachHang, "MaKhachHang", "MaKhachHang");
            cboMaKhachHang.SelectedIndex = -1;
            Database.FillCombo("SELECT MaNhanVien, TenNhanVien FROM tblNhanVien", cboMaNhanVien, "MaNhanVien", "MaNhanVien");
            cboMaNhanVien.SelectedIndex = -1;
            Database.FillCombo("SELECT MaHang, TenHang FROM tblHang", cboMaHang, "MaHang", "MaHang");
            cboMaHang.SelectedIndex = -1;
            //Hiển thị thông tin của một hóa đơn được gọi từ form tìm kiếm
            if (txtMaHoaDon.Text != "")
            {
                LoadInfoHoaDon();
                btnHuyHoaDon.Enabled = true;
                btnInHoaDon.Enabled = true;
            }
            LoadDataGridViewHoaDon();
        }

        private void LoadDataGridViewHoaDon()
        {
            string sql;
            sql = "SELECT a.MaHang, b.TenHang, a.SoLuong, b.DonGiaBan, a.GiamGia,a.ThanhTien FROM tblChiTietHoaDon a, tblHang b WHERE a.MaHoaDon = N'" + txtMaHoaDon.Text + "' AND b.MaHang=a.MaHang";
            tblCTHDB = Database.GetDataToTable(sql);
            dgvHoaDon.DataSource = tblCTHDB;
            dgvHoaDon.Columns[0].HeaderText = "Mã Hàng";
            dgvHoaDon.Columns[1].HeaderText = "Tên Hàng";
            dgvHoaDon.Columns[2].HeaderText = "Số Lượng";
            dgvHoaDon.Columns[3].HeaderText = "Đơn Giá";
            dgvHoaDon.Columns[4].HeaderText = "Giảm Giá %";
            dgvHoaDon.Columns[5].HeaderText = "Thành Tiền";
            dgvHoaDon.Columns[0].Width = 100;
            dgvHoaDon.Columns[1].Width = 100;
            dgvHoaDon.Columns[2].Width = 100;
            dgvHoaDon.Columns[3].Width = 100;
            dgvHoaDon.Columns[4].Width = 100;
            dgvHoaDon.Columns[5].Width = 138;
            dgvHoaDon.AllowUserToAddRows = false;
            dgvHoaDon.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void LoadInfoHoaDon()
        {
            string str;
            str = "SELECT NgayBan FROM tblHoaDon WHERE MaHoaDon = N'" + txtMaHoaDon.Text + "'";
            txtNgayBan.Text = Database.ConvertDateTime(Database.GetFieldValuesHoaDon(str));
            str = "SELECT MaNhanVien FROM tblHoaDon WHERE MaHoaDon = N'" + txtMaHoaDon.Text + "'";
            cboMaNhanVien.Text = Database.GetFieldValuesHoaDon(str);
            str = "SELECT MaKhachHang FROM tblHoaDon WHERE MaHoaDon = N'" + txtMaHoaDon.Text + "'";
            cboMaKhachHang.Text = Database.GetFieldValuesHoaDon(str);
            str = "SELECT TongTien FROM tblHoaDon WHERE MaHoaDon = N'" + txtMaHoaDon.Text + "'";
            txtTongTien.Text = Database.GetFieldValuesHoaDon(str);
            //lblBangChu.Text = "Bằng chữ: " + Database.ChuyenSoSangChu(txtTongTien.Text);
        }

        private void btnThemHoaDon_Click(object sender, EventArgs e)
        {
            btnHuyHoaDon.Enabled = false;
            btnLuuHoaDon.Enabled = true;
            btnInHoaDon.Enabled = false;
            btnThemHoaDon.Enabled = false;
            ResetValuesHoaDon();
            txtMaHoaDon.Text = Database.CreateKey("HoaDon");
            LoadDataGridViewHoaDon();
        }

        private void ResetValuesHoaDon()
        {
            txtMaHoaDon.Text = "";
            txtNgayBan.Text = DateTime.Now.ToShortDateString();
            cboMaNhanVien.Text = "";
            cboMaKhachHang.Text = "";
            txtTongTien.Text = "0";
            lblBangChu.Text = "Bằng chữ: ";
            cboMaHang.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void btnLuuHoaDon_Click(object sender, EventArgs e)
        {
            string sql;
            double sl, SLcon, tong, Tongmoi;
            sql = "SELECT MaHoaDon FROM tblHoaDon WHERE MaHoaDon=N'" + txtMaHoaDon.Text + "'";
            if (!Database.CheckKey(sql))
            {
                // Mã hóa đơn chưa có, tiến hành lưu các thông tin chung
                // Mã HDBan được sinh tự động do đó không có trường hợp trùng khóa
                if (txtNgayBan.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập ngày bán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNgayBan.Focus();
                    return;
                }
                if (cboMaNhanVien.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaNhanVien.Focus();
                    return;
                }
                if (cboMaKhachHang.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaKhachHang.Focus();
                    return;
                }
                sql = "INSERT INTO tblHoaDon(MaHoaDon, NgayBan, MaNhanVien, MaKhachHang, TongTien) VALUES (N'" + txtMaHoaDon.Text.Trim() + "','" +
                        Database.ConvertDateTime(txtNgayBan.Text.Trim()) + "','" + cboMaNhanVien.SelectedValue + "','" +
                        cboMaKhachHang.SelectedValue + "'," + txtTongTien.Text + ")";
                Database.RunSQL(sql);
            }
            // Lưu thông tin của các mặt hàng
            if (cboMaHang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaHang.Focus();
                return;
            }
            if ((txtSoLuong.Text.Trim().Length == 0) || (txtSoLuong.Text == "0"))
            {
                MessageBox.Show("Bạn phải nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }
            if (txtGiamGia.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập giảm giá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGiamGia.Focus();
                return;
            }
            sql = "SELECT MaHang FROM tblChiTietHoaDon WHERE MaHang=N'" + cboMaHang.SelectedValue + "' AND MaHoaDon = N'" + txtMaHoaDon.Text.Trim() + "'";
            if (Database.CheckKey(sql))
            {
                MessageBox.Show("Mã hàng này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetValuesHang();
                cboMaHang.Focus();
                return;
            }
            // Kiểm tra xem số lượng hàng trong kho còn đủ để cung cấp không?
            sl = Convert.ToDouble(Database.GetFieldValuesHoaDon("SELECT SoLuong FROM tblHang WHERE MaHang = N'" + cboMaHang.SelectedValue + "'"));
            if (Convert.ToDouble(txtSoLuong.Text) > sl)
            {
                MessageBox.Show("Số lượng mặt hàng này chỉ còn " + sl, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }
            sql = "INSERT INTO tblChiTietHoaDon(MaHoaDon,MaHang,SoLuong,DonGia, GiamGia,ThanhTien) VALUES(N'" + txtMaHoaDon.Text.Trim() + "',N'" + cboMaHang.SelectedValue + "'," + txtSoLuong.Text + "," + txtDonGiaBan.Text + "," + txtGiamGia.Text + "," + txtThanhTien.Text + ")";
            Database.RunSQL(sql);
            LoadDataGridViewHoaDon();
            // Cập nhật lại số lượng của mặt hàng vào bảng tblHang
            SLcon = sl - Convert.ToDouble(txtSoLuong.Text);
            sql = "UPDATE tblHang SET SoLuong =" + SLcon + " WHERE MaHang= N'" + cboMaHang.SelectedValue + "'";
            Database.RunSQL(sql);
            // Cập nhật lại tổng tiền cho hóa đơn bán
            //MessageBox.Show(Database.GetFieldValuesHoaDon(string.Format("SELECT TongTien FROM tblHoaDon where MaHoaDon = N'{0}'", txtMaHoaDon.Text)));
            tong = Convert.ToDouble(Database.GetFieldValuesHoaDon(string.Format("SELECT TongTien FROM tblHoaDon where MaHoaDon = N'{0}'", txtMaHoaDon.Text)));
            Tongmoi = tong + Convert.ToDouble(txtThanhTien.Text);
            sql = "UPDATE tblHoaDon SET TongTien =" + Tongmoi + " WHERE MaHoaDon = N'" + txtMaHoaDon.Text + "'";
            Database.RunSQL(sql);
            txtTongTien.Text = Tongmoi.ToString();
           // lblBangChu.Text = "Bằng chữ: " + Database.ChuyenSoSangChu(Tongmoi.ToString());
            ResetValuesHang();
            btnHuyHoaDon.Enabled = true;
            btnThemHoaDon.Enabled = true;
            btnInHoaDon.Enabled = true;
        }

        private void ResetValuesHang()
        {
            cboMaHang.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void dgvHoaDon_DoubleClick(object sender, EventArgs e)
        {
            string MaHangxoa, sql;
            Double ThanhTienxoa, SoLuongxoa, sl, slcon, tong, tongmoi;
            if (tblCTHDB.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if ((MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                //Xóa hàng và cập nhật lại số lượng hàng 
                MaHangxoa = dgvHoaDon.CurrentRow.Cells["colMaHang"].Value.ToString();
                SoLuongxoa = Convert.ToDouble(dgvHoaDon.CurrentRow.Cells["colSoLuong"].Value.ToString());
                ThanhTienxoa = Convert.ToDouble(dgvHoaDon.CurrentRow.Cells["colThanhTien"].Value.ToString());
                sql = "DELETE tblChiTietHoaDon WHERE MaHoaDon=N'" + txtMaHoaDon.Text + "' AND MaHang = N'" + MaHangxoa + "'";
                Database.RunSQL(sql);
                // Cập nhật lại số lượng cho các mặt hàng
                sl = Convert.ToDouble(Database.GetFieldValuesHoaDon("SELECT SoLuong FROM tblHang WHERE MaHang = N'" + MaHangxoa + "'"));
                slcon = sl + SoLuongxoa;
                sql = "UPDATE tblHang SET SoLuong =" + slcon + " WHERE MaHang= N'" + MaHangxoa + "'";
                Database.RunSQL(sql);
                // Cập nhật lại tổng tiền cho hóa đơn bán
                tong = Convert.ToDouble(Database.GetFieldValuesHoaDon("SELECT TongTien FROM tblHoaDon WHERE MaHoaDon = N'" + txtMaHoaDon.Text + "'"));
                tongmoi = tong - ThanhTienxoa;
                sql = "UPDATE tblHoaDon SET TongTien =" + tongmoi + " WHERE MaHoaDon = N'" + txtMaHoaDon.Text + "'";
                Database.RunSQL(sql);
                txtTongTien.Text = tongmoi.ToString();
                //lblBangChu.Text = "Bằng chữ: " + Database.ChuyenSoSangChu(tongmoi.ToString());
                LoadDataGridViewHoaDon();
            }
        }

        private void btnHuyHoaDon_Click(object sender, EventArgs e)
        {
            double sl, slcon, slxoa;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "SELECT MaHang,SoLuong FROM tblChiTietHoaDon WHERE MaHoaDon = N'" + txtMaHoaDon.Text + "'";
                DataTable tblHang = Database.GetDataToTable(sql);
                for (int hang = 0; hang <= tblHang.Rows.Count - 1; hang++)
                {
                    // Cập nhật lại số lượng cho các mặt hàng
                    sl = Convert.ToDouble(Database.GetFieldValuesHang("SELECT SoLuong FROM tblHang WHERE MaHang = N'" + tblHang.Rows[hang][0].ToString() + "'"));
                    slxoa = Convert.ToDouble(tblHang.Rows[hang][1].ToString());
                    slcon = sl + slxoa;
                    sql = "UPDATE tblHang SET SoLuong =" + slcon + " WHERE MaHang= N'" + tblHang.Rows[hang][0].ToString() + "'";
                    Database.RunSQL(sql);
                }

                //Xóa chi tiết hóa đơn
                sql = "DELETE tblChiTietHoaDon WHERE MaHoaDon=N'" + txtMaHoaDon.Text + "'";
                Database.RunSqlDel(sql);

                //Xóa hóa đơn
                sql = "DELETE tblHoaDon WHERE MaHoaDon=N'" + txtMaHoaDon.Text + "'";
                Database.RunSqlDel(sql);
                ResetValuesHoaDon();
                LoadDataGridViewHoaDon();
                btnHuyHoaDon.Enabled = false;
                btnInHoaDon.Enabled = false;
            }
        }

        private void cboMaNhanVien_TextChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaNhanVien.Text == "")
                txtTenNhanVien.Text = "";
            // Khi chọn Mã nhân viên thì tên nhân viên tự động hiện ra
            str = "Select TenNhanVien from tblNhanVien where MaNhanVien =N'" + cboMaNhanVien.SelectedValue + "'";
            txtTenNhanVien.Text = Database.GetFieldValuesHoaDon(str);
        }

        private void cboMaKhachHang_TextChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaKhachHang.Text == "")
            {
                txtTenKhachHang.Text = "";
                txtDiaChi.Text = "";
                mtbDienThoai.Text = "";
            }
            //Khi chọn Mã khách hàng thì các thông tin của khách hàng sẽ hiện ra
            str = "Select TenKhachHang from tblKhachHang where MaKhachHang = N'" + cboMaKhachHang.SelectedValue + "'";
            txtTenKhachHang.Text = Database.GetFieldValuesHoaDon(str);
            str = "Select DiaChi from tblKhachHang where MaKhachHang = N'" + cboMaKhachHang.SelectedValue + "'";
            txtDiaChi.Text = Database.GetFieldValuesHoaDon(str);
            str = "Select SDT from tblKhachHang where MaKhachHang= N'" + cboMaKhachHang.SelectedValue + "'";
            mtbDienThoai.Text = Database.GetFieldValuesHoaDon(str);
        }

        private void cboMaHang_TextChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaHang.Text == "")
            {
                txtTenHang.Text = "";
                txtDonGiaBan.Text = "";
            }
            // Khi chọn mã hàng thì các thông tin về hàng hiện ra
            str = "SELECT TenHang FROM tblHang WHERE MaHang =N'" + cboMaHang.SelectedValue + "'";
            txtTenHang.Text = Database.GetFieldValuesHoaDon(str);
            str = "SELECT DonGiaBan FROM tblHang WHERE MaHang =N'" + cboMaHang.SelectedValue + "'";
            txtDonGiaBan.Text = Database.GetFieldValuesHoaDon(str);
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtGiamGia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamGia.Text);
            if (txtDonGiaBan.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGiaBan.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhTien.Text = tt.ToString();
        }

        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtGiamGia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamGia.Text);
            if (txtDonGiaBan.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGiaBan.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhTien.Text = tt.ToString();
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            COMExcel.Application exApp = new COMExcel.Application();
            COMExcel.Workbook exBook; //Trong 1 chương trình Excel có nhiều Workbook
            COMExcel.Worksheet exSheet; //Trong 1 Workbook có nhiều Worksheet
            COMExcel.Range exRange;
            string sql;
            int hang = 0, cot = 0;
            DataTable tblThongtinHD, tblThongtinHang;
            exBook = exApp.Workbooks.Add(COMExcel.XlWBATemplate.xlWBATWorksheet);
            exSheet = exBook.Worksheets[1];
            // Định dạng chung
            exRange = exSheet.Cells[1, 1];
            exRange.Range["A1:Z300"].Font.Name = "Times new roman"; //Font chữ
            exRange.Range["A1:B3"].Font.Size = 10;
            exRange.Range["A1:B3"].Font.Bold = true;
            exRange.Range["A1:B3"].Font.ColorIndex = 5; //Màu xanh da trời
            exRange.Range["A1:A1"].ColumnWidth = 7;
            exRange.Range["B1:B1"].ColumnWidth = 15;
            exRange.Range["A1:B1"].MergeCells = true;
            exRange.Range["A1:B1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A1:B1"].Value = "Quán ....";
            exRange.Range["A2:B2"].MergeCells = true;
            exRange.Range["A2:B2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:B2"].Value = "";
            exRange.Range["A3:B3"].MergeCells = true;
            exRange.Range["A3:B3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A3:B3"].Value = "Điện thoại: ......";
            exRange.Range["C2:E2"].Font.Size = 16;
            exRange.Range["C2:E2"].Font.Bold = true;
            exRange.Range["C2:E2"].Font.ColorIndex = 3; //Màu đỏ
            exRange.Range["C2:E2"].MergeCells = true;
            exRange.Range["C2:E2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C2:E2"].Value = "HÓA ĐƠN BÁN";
            // Biểu diễn thông tin chung của hóa đơn bán
            sql = "SELECT a.MaHoaDon, a.NgayBan, a.TongTien, b.TenKhachHang, b.DiaChi, b.SDT, c.TenNhanVien FROM tblHoaDon AS a, tblKhachHang AS b, tblNhanVien AS c WHERE a.MaHoaDon = N'" + txtMaHoaDon.Text + "' AND a.MaKhachHang = b.MaKhachHang AND a.MaNhanVien = c.MaNhanVien";
            tblThongtinHD = Database.GetDataToTable(sql);
            exRange.Range["B6:C9"].Font.Size = 12;
            exRange.Range["B6:B6"].Value = "Mã hóa đơn:";
            exRange.Range["C6:E6"].MergeCells = true;
            exRange.Range["C6:E6"].Value = tblThongtinHD.Rows[0][0].ToString();
            exRange.Range["B7:B7"].Value = "Khách hàng:";
            exRange.Range["C7:E7"].MergeCells = true;
            exRange.Range["C7:E7"].Value = tblThongtinHD.Rows[0][3].ToString();
            exRange.Range["B8:B8"].Value = "Địa chỉ:";
            exRange.Range["C8:E8"].MergeCells = true;
            exRange.Range["C8:E8"].Value = tblThongtinHD.Rows[0][4].ToString();
            exRange.Range["B9:B9"].Value = "Điện thoại:";
            exRange.Range["C9:E9"].MergeCells = true;
            exRange.Range["C9:E9"].Value = tblThongtinHD.Rows[0][5].ToString();
            //Lấy thông tin các mặt hàng
            sql = "SELECT b.TenHang, a.SoLuong, b.DonGiaBan, a.GiamGia, a.ThanhTien " +
                  "FROM tblChiTietHoaDon AS a , tblHang AS b WHERE a.MaHoaDon = N'" +
                  txtMaHoaDon.Text + "' AND a.MaHang = b.MaHang";
            tblThongtinHang = Database.GetDataToTable(sql);
            //Tạo dòng tiêu đề bảng
            exRange.Range["A11:F11"].Font.Bold = true;
            exRange.Range["A11:F11"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C11:F11"].ColumnWidth = 12;
            exRange.Range["A11:A11"].Value = "STT";
            exRange.Range["B11:B11"].Value = "Tên hàng";
            exRange.Range["C11:C11"].Value = "Số lượng";
            exRange.Range["D11:D11"].Value = "Đơn giá";
            exRange.Range["E11:E11"].Value = "Giảm giá";
            exRange.Range["F11:F11"].Value = "Thành tiền";
            for (hang = 0; hang < tblThongtinHang.Rows.Count; hang++)
            {
                //Điền số thứ tự vào cột 1 từ dòng 12
                exSheet.Cells[1][hang + 12] = hang + 1;
                for (cot = 0; cot < tblThongtinHang.Columns.Count; cot++)
                //Điền thông tin hàng từ cột thứ 2, dòng 12
                {
                    exSheet.Cells[cot + 2][hang + 12] = tblThongtinHang.Rows[hang][cot].ToString();
                    if (cot == 3) exSheet.Cells[cot + 2][hang + 12] = tblThongtinHang.Rows[hang][cot].ToString() + "%";
                }
            }
            exRange = exSheet.Cells[cot][hang + 14];
            exRange.Font.Bold = true;
            exRange.Value2 = "Tổng tiền:";
            exRange = exSheet.Cells[cot + 1][hang + 14];
            exRange.Font.Bold = true;
            exRange.Value2 = tblThongtinHD.Rows[0][2].ToString();
            exRange = exSheet.Cells[1][hang + 15]; //Ô A1 
            exRange.Range["A1:F1"].MergeCells = true;
            exRange.Range["A1:F1"].Font.Bold = true;
            exRange.Range["A1:F1"].Font.Italic = true;
            exRange.Range["A1:F1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignRight;
            //exRange.Range["A1:F1"].Value = "Bằng chữ: " + Database.ChuyenSoSangChu(tblThongtinHD.Rows[0][2].ToString());
            exRange = exSheet.Cells[4][hang + 17]; //Ô A1 
            exRange.Range["A1:C1"].MergeCells = true;
            exRange.Range["A1:C1"].Font.Italic = true;
            exRange.Range["A1:C1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            DateTime d = Convert.ToDateTime(tblThongtinHD.Rows[0][1]);
            exRange.Range["A1:C1"].Value = "Biên Hoà, ngày " + d.Day + " tháng " + d.Month + " năm " + d.Year;
            exRange.Range["A2:C2"].MergeCells = true;
            exRange.Range["A2:C2"].Font.Italic = true;
            exRange.Range["A2:C2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:C2"].Value = "Nhân viên bán hàng";
            exRange.Range["A6:C6"].MergeCells = true;
            exRange.Range["A6:C6"].Font.Italic = true;
            exRange.Range["A6:C6"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A6:C6"].Value = tblThongtinHD.Rows[0][6];
            exSheet.Name = "Hóa đơn nhập";
            exApp.Visible = true;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (cboMaHD.Text == "")
            {
                MessageBox.Show("Bạn phải chọn một mã hóa đơn để tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaHD.Focus();
                return;
            }
            txtMaHoaDon.Text = cboMaHD.Text;
            LoadInfoHoaDon();
            LoadDataGridViewHoaDon();
            btnHuyHoaDon.Enabled = true;
            btnLuuHoaDon.Enabled = true;
            btnInHoaDon.Enabled = true;
            cboMaHD.SelectedIndex = -1;
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
                e.Handled = false;
            else e.Handled = true;
        }

        private void cboMaHD_DropDown(object sender, EventArgs e)
        {
            Database.FillCombo("SELECT MaHoaDon FROM tblHoaDon", cboMaHD, "MaHoaDon", "MaHoaDon");
            cboMaHD.SelectedIndex = -1;
        }

        private void frmHoaDonBanHang_FormClosed(object sender, FormClosedEventArgs e)
        {
            ResetValuesHoaDon();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
