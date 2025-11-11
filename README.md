Hệ thống Quản lý Đặt vé Xem phim//

1. Giới thiệu 
Đây là dự án giữa kỳ của nhóm E, một ứng dụng console mô phỏng quy trình đặt vé và quản lý của một rạp chiếu phim.
Chương trình được thiết kế theo mô hình mô-đun hóa (Modular Design), gồm các thành phần:
    DataStorage — quản lý dữ liệu phim, vé, người dùng, suất chiếu.
    BusinessLogic — xử lý nghiệp vụ, tính toán, xác thực đặt vé.
    ConsoleUI — hiển thị giao diện dòng lệnh (console menu).
    UserWorkflow — luồng thao tác của khách hàng.
    AdminWorkflow — luồng thao tác của quản trị viên.
    Program — chương trình chính, điều phối toàn hệ thống.

2. Yêu cầu hệ thống
Hệ điều hành: Windows hoặc macOS hỗ trợ .NET 6.0+
IDE khuyến nghị: Visual Studio / Visual Studio Code
Ngôn ngữ: C#
Bộ SDK: .NET 6.0 trở lên

3. Cấu trúc thư mục
├── FileManagement.cs: Lớp "truy cập dữ liệu" (Data Access Layer), thực hiện mọi tác vụ đọc/ghi file .csv và backup.
├── BusinessLogic.cs: Là "bộ não" (logic layer) của chương trình. Chứa các thuật toán (sắp xếp, tìm kiếm) và các quy tắc nghiệp vụ cốt lõi (như BookSeats).
├── ConsoleUI.cs: Lớp "giao diện" (Presentation Layer). Chịu trách nhiệm chỉ hiển thị thông tin, vẽ menu và sơ đồ ghế ra console.
├── UserWorkflow.cs: Các lớp "Dịch vụ" (Service/Workflow), chịu trách nhiệm điều phối các bước cho một chức năng.
├── AdminWorkflow.cs: Các lớp "Dịch vụ" (Service/Workflow), chịu trách nhiệm điều phối các bước cho một chức năng.
├── Program.cs: Đóng vai trò là "bộ điều phối" (Orchestrator), quản lý vòng đời (Load/Save) và điều hướng (phân quyền) menu chính.
├── movies.txt: Danh sách phim.
├── showtimes.txt: Thông tin suất chiếu.
├── users.txt: Danh sách tài khoản
├── bookings.txt: Danh sách vé đã đặt.
├── README.md

4. Hướng dẫn chạy chương trình 
Có hai cách chính để chạy dự án:

Cách 1: Chạy bằng Terminal 
Mở một cửa sổ terminal (Command Prompt, PowerShell).
Di chuyển đến thư mục gốc của dự án (thư mục chứa file .csproj).
Chạy lệnh sau:
dotnet run
Chương trình sẽ tự động biên dịch và khởi chạy trong cửa sổ terminal.

Cách 2: Chạy bằng Visual Studio Code
Mở file .sln bằng Visual Studio Code.
Nhấn nút "Run" (biểu tượng ▶) trên thanh công cụ.
Visual Studio sẽ tự động build và chạy dự án.

Đăng nhập bằng Tài khoản Demo//
Khi chạy lần đầu, tài khoản Admin và Customer mặc định sẽ được tự động tạo để thuận tiện cho việc kiểm thử:
Admin 1 - admin: ID = 1
Customer 2 - nguyenvana: ID = 2
Customer 3 - tranthib: ID = 3
Customer 4 - leminhc: ID = 4
Customer 5 - hoangdungd: ID = 5
Các tài khoản người dùng khác có thể được tạo bằng chức năng "Đăng ký" ở menu chính.

5. Mô tả chức năng chính
5.1. Chức năng Người dùng (Customer)
     Đăng ký & Đăng nhập: Người dùng có thể tạo tài khoản mới hoặc đăng nhập bằng ID do hệ thống cung cấp.
     Xem phim & Lịch chiếu: Cho phép xem danh sách tất cả các phim và các suất chiếu hiện có.
     Tra cứu phim theo tên (có/không phân biệt chữ hoa thường).
     Đặt vé: Hệ thống hiển thị một sơ đồ phòng chiếu 2D bằng ký tự ASCII (O là ghế trống, X là ghế đã đặt). Người dùng chọn ghế bằng cách nhập tọa độ (ví dụ: A1,B2,C3). Hệ thống sẽ tự động tính tổng tiền và xác nhận vé.
     Hủy vé: Người dùng có thể xem lại vé của mình và thực hiện hủy vé bằng mã đặt vé (Booking ID).
5.2. Chức năng Quản trị viên (Admin)
     Quản lý Phim: Admin có đầy đủ chức năng Thêm, Sửa, và Xóa phim khỏi hệ thống.
     Xem danh sách phim.
     Tìm kiếm & Sắp xếp: Sắp xếp phim theo tên (Bubble Sort). Tìm kiếm phim theo ID (Binary Search).
     Thống kê Doanh thu.

6. Tính năng Hệ thống
   Lưu trữ Dữ liệu (Persistence): Toàn bộ dữ liệu (phim, vé, người dùng...) được tự động lưu vào các file .csv khi thoát và được tải lại khi khởi động.
   An toàn Dữ liệu: Hệ thống tự động tạo file backup (.bak) trước mỗi lần ghi đè dữ liệu, phòng trường hợp chương trình tắt đột ngột.
   Dữ liệu Mẫu (Demo Data): Chương trình sẽ tự động nạp dữ liệu mẫu (phim, suất chiếu) nếu khởi động lần đầu khi không tìm thấy file dữ liệu.


Nhóm Tác giả//
Module 1 (Cấu trúc Dữ liệu): Mai Thanh Ngân
Module 2 (Quản lý File): Hoàng Thị Mỹ Dung
Module 3 & 6 (Logic & Luồng Admin): Nguyễn Hồng Hạnh
Module 4 (Giao diện Console): Huỳnh Yến Trân
Module 5 (Luồng Người dùng): Trần Thị Hoài
Module 7 (Tích hợp Hệ thống): Mai Thị Thuỳ Duyên
