Hệ thống Quản lý Đặt vé Xem phim//
Đây là dự án giữa kỳ của nhóm E, một ứng dụng console mô phỏng quy trình đặt vé và quản lý của một rạp chiếu phim.

Trong dự án này, nhóm đã vận dụng các kiến thức đã học như Lập trình Hướng đối tượng (OOP), quản lý file, các thuật toán cơ bản, và kiến trúc phân lớp (chia module) để xây dựng một chương trình rõ ràng, dễ quản lý và sử dụng.

Tính năng nổi bật//
  
 Chức năng Người dùng (Customer)
Đăng ký & Đăng nhập: Người dùng có thể tạo tài khoản mới hoặc đăng nhập bằng ID do hệ thống cung cấp.

Xem phim & Lịch chiếu: Cho phép xem danh sách tất cả các phim và các suất chiếu hiện có.

 Đặt vé trực quan:
Hệ thống hiển thị một sơ đồ phòng chiếu 2D bằng ký tự ASCII (O là ghế trống, X là ghế đã đặt).

Người dùng chọn ghế bằng cách nhập tọa độ (ví dụ: A1,B2,C3).

Hệ thống sẽ tự động tính tổng tiền và xác nhận vé.

Hủy vé: Người dùng có thể xem lại vé của mình và thực hiện hủy vé bằng mã đặt vé (Booking ID).

 Chức năng Quản trị viên (Admin)
Quản lý Phim: Admin có đầy đủ chức năng Thêm (Create), Sửa (Update), và Xóa (Delete) phim khỏi hệ thống.

Tìm kiếm & Sắp xếp nâng cao:
Tìm kiếm phim theo tên (không phân biệt hoa thường).
Tìm kiếm phim theo ID (sử dụng thuật toán Binary Search để tối ưu tốc độ).
Sắp xếp toàn bộ danh sách phim theo tên (A-Z).
Báo cáo Doanh thu: Admin có thể xuất một file RevenueReport.csv chi tiết tổng doanh thu từ các vé đã bán.

Tính năng Hệ thống//
Lưu trữ Dữ liệu (Persistence): Toàn bộ dữ liệu (phim, vé, người dùng...) được tự động lưu vào các file .csv khi thoát và được tải lại khi khởi động.

An toàn Dữ liệu: Hệ thống tự động tạo file backup (.bak) trước mỗi lần ghi đè dữ liệu, phòng trường hợp chương trình tắt đột ngột.

Dữ liệu Mẫu (Demo Data): Chương trình sẽ tự động nạp dữ liệu mẫu (phim, suất chiếu) nếu khởi động lần đầu mà không tìm thấy file dữ liệu.


Cấu trúc Dự án//
Dự án được chia thành nhiều module theo kiến trúc phân lớp để dễ quản lý và phát triển:
Cấu trúc Dữ liệu (Module 1): (Gồm Movie.cs, Showtime.cs...) Định nghĩa các "khuôn mẫu" (class/struct) cho toàn bộ dữ liệu của ứng dụng.

FileManager.cs (Module 2): Lớp "Truy cập Dữ liệu" (Data Access Layer), thực hiện mọi tác vụ đọc/ghi file .csv và backup.

BusinessLogic.cs (Module 3): Là "bộ não" (logic layer) của chương trình. Chứa các thuật toán (sắp xếp, tìm kiếm) và các quy tắc nghiệp vụ cốt lõi (như BookSeats).

ConsoleUI.cs (Module 4): Lớp "Giao diện" (Presentation Layer). Chịu trách nhiệm chỉ hiển thị thông tin, vẽ menu và sơ đồ ghế ra console.

UserWorkflow.cs (Module 5) & AdminWorkflow.cs (Module 6): Các lớp "Dịch vụ" (Service/Workflow), chịu trách nhiệm điều phối các bước cho một chức năng (ví dụ: luồng đặt vé sẽ gọi UI -> nhận input -> gọi Logic -> cập nhật DataStorage).

Program.cs (Module 7): Đóng vai trò là "bộ điều phối" (Orchestrator), quản lý vòng đời (Load/Save) và điều hướng (phân quyền) menu chính.


Cách chạy//
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

Tài khoản Demo//
Khi chạy lần đầu, một tài khoản Admin mặc định sẽ được tự động tạo để thuận tiện cho việc kiểm thử:
Admin: ID = 1
Các tài khoản người dùng khác có thể được tạo bằng chức năng "Đăng ký" ở menu chính.

Nhóm Tác giả//
Module 1 (Cấu trúc Dữ liệu): Mai Thanh Ngân
Module 2 (Quản lý File): Hoàng Thị Mỹ Dung
Module 3 & 6 (Logic & Luồng Admin): Nguyễn Hồng Hạnh
Module 4 (Giao diện Console): Huỳnh Yến Trân
Module 5 (Luồng Người dùng): Trần Thị Hoài
Module 7 (Tích hợp Hệ thống): Mai Thị Thuỳ Duyên
