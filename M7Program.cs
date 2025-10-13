// ===========================================================
// Module 7 – Main Program & Integration
// Phụ trách: Mai Thị Thuỳ Duyên
// Mục đích: Tích hợp toàn bộ các module, điều hướng chương trình chính.
// Các hàm: Main(), MainMenuLoop(), AdminMenu() và UserMenu()
// ===========================================================
using System;
using System.Linq;

namespace MovieBookingSystem
{
    public class Program
    {
        // Khởi tạo các đối tượng và module cần thiết cho toàn bộ hệ thống
        static readonly DataStorage data = new DataStorage();
        static readonly BusinessLogic logic = new BusinessLogic(); // Cần thiết cho các workflow
        static readonly UserWorkflow userWF = new UserWorkflow();   // M5
        static readonly AdminWorkflow adminWF = new AdminWorkflow(); // M6

        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Hỗ trợ hiển thị tiếng Việt

            LoadData();      // Tải dữ liệu từ file hoặc tạo dữ liệu mẫu
            MainMenuLoop();  // Bắt đầu vòng lặp menu chính
            SaveData();      // Lưu dữ liệu trước khi thoát chương trình
        }

        // -------------------------
        // VÒNG LẶP MENU CHÍNH
        // -------------------------
        static void MainMenuLoop()
        {
            while (true)
            {
                ConsoleUI.DrawMainMenu(); // Vẽ menu chính từ M4
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Đăng nhập
                        User loggedInUser = userWF.UserLogin(data);
                        if (loggedInUser != null)
                        {
                            // Điều hướng dựa trên vai trò của người dùng
                            if (loggedInUser.Role == UserRole.Admin)
                            {
                                AdminMenu();
                            }
                            else
                            {
                                UserMenu(loggedInUser, data, userWF);
                            }
                        }
                        break;

                    case "2": // Đăng ký
                        userWF.UserRegister(data);
                        break;

                    case "3": // Thoát
                        Console.WriteLine("Cảm ơn đã sử dụng hệ thống!");
                        return; // Thoát khỏi vòng lặp để thực thi SaveData()

                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ, vui lòng thử lại.");
                        break;
                }
                ConsoleUI.PromptAndPause();
            }
        }

        // -------------------------
        // MENU DÀNH CHO NGƯỜI DÙNG (CUSTOMER)
        // -------------------------
        static void UserMenu(User currentUser, DataStorage data, UserWorkflow userWF)
        {
            while (true)
            {
                ConsoleUI.DrawUserMenu(); // Gọi giao diện menu từ M4
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Xem phim & lịch chiếu
                        userWF.ShowMoviesAndShowtimes(data);
                        break;

                    case "2": // Tìm phim theo tên
                        userWF.SearchMovie(data, new BusinessLogic());
                        break;

                    case "3": // Đặt vé
                        userWF.BookTicket(data, currentUser);
                        break;

                    case "4": // Hủy vé
                        userWF.CancelBooking(data, currentUser);
                        break;

                    case "5": // Đăng xuất
                        userWF.Logout();
                        return; // Thoát khỏi menu người dùng, quay lại menu chính

                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                        break;
                }

                ConsoleUI.PromptAndPause(); // Tạm dừng để người dùng đọc kết quả
            }
        }

        // -------------------------
        // MENU DÀNH CHO QUẢN TRỊ VIÊN (ADMIN)
        // Đã cập nhật để khớp với M4 và M6
        // -------------------------
        static void AdminMenu()
        {
            while (true)
            {
                ConsoleUI.DrawAdminMenu(); // Vẽ menu admin từ M4
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Thêm phim
                        adminWF.AddMovie(data);
                        break;
                    case "2": // Sửa phim
                        adminWF.EditMovie(data);
                        break;
                    case "3": // Xóa phim
                        adminWF.DeleteMovie(data);
                        break;
                    case "4": // Sắp xếp phim (theo tên)
                        adminWF.SortMovies(data, logic);
                        break;
                    case "5": // Tìm phim theo ID
                        adminWF.FindMovieById(data, logic);
                        break;
                    case "6": // Xem tất cả phim
                        adminWF.ViewAllMovies(data);
                        break;
                    case "7": // Báo cáo doanh thu
                        adminWF.GenerateRevenueReport(data);
                        break;
                    case "8": // Đăng xuất
                        adminWF.Logout();
                        return; // Thoát khỏi menu admin để quay về menu chính
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                        break;
                }
                ConsoleUI.PromptAndPause();
            }
        }

        // -------------------------
        // LOAD & SAVE DATA
        // -------------------------
        static void LoadData()
        {
            // Tải dữ liệu từ file (M2)
            FileManager.LoadAll(data);
            Console.WriteLine("\n[Đã tải dữ liệu từ hệ thống]");

            // Nếu không có người dùng, tạo admin mặc định
            if (!data.Users.Any())
            {
                data.Users.Add(new User { Id = 1, Name = "admin", Role = UserRole.Admin});
                data.Users.Add(new User { Id = 2, Name = "user", Role = UserRole.Customer});
            }

            // Nếu không có phim nào, tải dữ liệu demo
            if (!data.Movies.Any())
            {
                Console.WriteLine("[Hệ thống trống, đang tải dữ liệu mẫu...]");
                DemoData.Load(data);
            }
        }

        static void SaveData()
        {
            // Lưu toàn bộ dữ liệu vào file (M2)
            FileManager.SaveAll(data);
            Console.WriteLine("\n[Dữ liệu đã được lưu thành công!]");
        }
    }
}