# 📋 Hướng dẫn Hệ thống Thông báo Bảo mật OLS (Dành cho Docker)

Hệ thống này triển khai **Oracle Label Security (OLS)** trên môi trường Docker.

## 1. Kích hoạt OLS trong Docker (Bắt buộc)

Nếu công cụ `OlsTest` báo OLS chưa bật, bạn hãy thực hiện các lệnh sau trên PowerShell của máy host:

### Bước 1: Truy cập vào SQL*Plus trong Container
```powershell
# Thay <container_id> bằng ID hoặc Name của container Oracle của bạn
docker exec -it <container_id> sqlplus sys/1234567890@localhost:1521/xepdb1 as sysdba
```

### Bước 2: Chạy lệnh kích hoạt OLS
Khi đã ở trong SQL*Plus, hãy nhập:
```sql
EXEC LBACSYS.CONFIGURE_OLS;
EXEC LBACSYS.OLS_ENFORCEMENT.ENABLE_OLS;
EXIT;
```

### Bước 3: Khởi động lại Container (Quan trọng)
```powershell
docker restart <container_id>
```

---

## 2. Cách chạy công cụ Thiết lập & Kiểm tra

Vì máy bạn chưa cấu hình `dotnet` trong biến môi trường, hãy sử dụng lệnh sau:

```powershell
& "$env:USERPROFILE\AppData\Local\Microsoft\dotnet\dotnet" run --project OlsTest
```

Công cụ này sẽ tự động:
1. Tạo Chính sách (Policy): `THONGBAO_POL`
2. Tạo Cấu trúc (Levels, Compartments, Groups)
3. Tạo 8 User (U1-U8) và Ma trận truy cập.
4. Chèn 7 dòng dữ liệu mẫu (t1-t7).

---

## 3. Ma trận Truy cập (Access Matrix)

| User | Quyền hạn tiêu biểu | Nhãn gán |
|:---:|:---|:---|
| **U1** | Giám đốc | Toàn quyền xem mọi thông báo |
| **U8** | NV Tiêu hóa tại Hà Nội | Chỉ xem được thông báo Chung và Khoa TH tại HN |

---

## 4. Chạy Ứng dụng chính WinForms

Để mở giao diện xem thông báo:
```powershell
& "$env:USERPROFILE\AppData\Local\Microsoft\dotnet\dotnet" run --project MedicalModule
```
Đăng nhập bằng `u1` hoặc `u8` (mật khẩu `123`) để thấy sự khác biệt về dữ liệu được lọc bởi OLS.
