using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace ExcelTool.Database
{
    public static class DatabaseInitializer
    {
        // =========================================================
        // DATABASE PATH
        // =========================================================

        private static readonly string AppFolder =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "ExcelTool");

        public static readonly string DatabasePath =
            Path.Combine(
                AppFolder,
                "exceltool.db");


        // =========================================================
        // KHỞI TẠO DATABASE
        // =========================================================

        public static void Initialize()
        {
            // -----------------------------------------------------
            // Tạo thư mục ứng dụng nếu chưa có
            // -----------------------------------------------------

            Directory.CreateDirectory(
                AppFolder);


            // -----------------------------------------------------
            // Kết nối SQLite
            // -----------------------------------------------------

            var connectionString =
                $"Data Source={DatabasePath}";

            using var connection =
                new SqliteConnection(connectionString);

            connection.Open();


            // -----------------------------------------------------
            // Bật Foreign Key
            // -----------------------------------------------------

            using (var foreignKeyCommand =
                   connection.CreateCommand())
            {
                foreignKeyCommand.CommandText =
                    "PRAGMA foreign_keys = ON;";

                foreignKeyCommand.ExecuteNonQuery();
            }


            // =====================================================
            // TẠO CÁC TABLE
            // =====================================================

            // -----------------------------------------------------
            // CUSTOMER
            // -----------------------------------------------------

            CreateCustomerTable(
                connection);


            // -----------------------------------------------------
            // PRODUCT
            // -----------------------------------------------------

            CreateProductTable(
                connection);


            // -----------------------------------------------------
            // SẢN PHẨM MẶC ĐỊNH
            // -----------------------------------------------------

            InsertDefaultProducts(
                connection);


            // -----------------------------------------------------
            // QUOTATION
            // -----------------------------------------------------

            CreateQuotationTable(
                connection);


            // -----------------------------------------------------
            // QUOTATION ITEM
            // -----------------------------------------------------

            CreateQuotationItemTable(
                connection);


            // -----------------------------------------------------
            // DIFFICULTY SETTING
            // -----------------------------------------------------

            CreateDifficultySettingTable(
                connection);


            // -----------------------------------------------------
            // REPRINT SETTING
            // -----------------------------------------------------

            CreateReprintSettingTable(
                connection);


            // -----------------------------------------------------
            // WASTE SETTING
            // -----------------------------------------------------

            CreateWasteSettingTable(
                connection);


            // -----------------------------------------------------
            // PAPER PRICE SETTING
            // -----------------------------------------------------

            CreatePaperPriceSettingTable(
                connection);

            // -----------------------------------------------------
            // PAPER SIZE SETTING
            // -----------------------------------------------------

            CreatePaperSizeSettingTable(
                connection);

            // -----------------------------------------------------
            // GRIP SETTING
            // NHÍP - TAY KÊ - TRÀN MÀU
            // -----------------------------------------------------

            CreateGripSettingTable(
                connection);


            // -----------------------------------------------------
            // SHIPPING SETTING
            // GIÁ VẬN CHUYỂN
            // -----------------------------------------------------

            CreateShippingSettingTable(
                connection);

            // -----------------------------------------------------
            // BOM SETTING
            // LOẠI SẢN PHẨM
            // -----------------------------------------------------

            CreateBomSettingTable(
                connection);

            // -----------------------------------------------------
            // PRINT BATCH SETTING
            // LÔ IN - LÔ KẼM
            // -----------------------------------------------------

            CreatePrintBatchSettingTable(
                connection);


            // -----------------------------------------------------
            // PRINT TYPE SETTING
            // QUY ƯỚC CÁC KIỂU IN
            // -----------------------------------------------------

            CreatePrintTypeSettingTable(
                connection);


            // -----------------------------------------------------
            // PRINT PRICE SETTING
            // ĐƠN GIÁ KẼM IN VÀ MỰC IN
            // -----------------------------------------------------

            CreatePrintPriceSettingTable(
                connection);


            // -----------------------------------------------------
            // PROCESS PRICE SETTING
            // GIÁ GIA CÔNG
            // -----------------------------------------------------

            CreateProcessPriceSettingTable(
                connection);


            // -----------------------------------------------------
            // MOLD PRICE SETTING
            // GIÁ KHUÔN
            // -----------------------------------------------------

            CreateMoldPriceSettingTable(
                connection);

            // -----------------------------------------------------
            // UNIT SETTING
            // ĐƠN VỊ TÍNH
            // -----------------------------------------------------

            CreateUnitSettingTable(
                connection);

            // =========================================================
            // SUPPORT SETTING - GIÁ SUPPORT
            // =========================================================

            CreateSupportSettingTable(connection);


            // =========================================================
            // GLUE PRICE SETTING
            // GIÁ KEO DÁN
            // =========================================================

            CreateGluePriceSettingTable(connection);


            // =========================================================
            // FILM SETTING - QUY ĐỔI TRỌNG LƯỢNG MÀNG
            // =========================================================

            CreateFilmSettingTable(connection);
        }


        // =========================================================
        // PRODUCT - DỮ LIỆU MẶC ĐỊNH
        // =========================================================

        private static void InsertDefaultProducts(
            SqliteConnection connection)
        {
            const string sql = """
                INSERT OR IGNORE INTO Product
                (
                    Id,
                    ProductCode,
                    ProductName,
                    Unit,
                    UnitPrice,
                    Description
                )
                VALUES
                (
                    $id,
                    $code,
                    $name,
                    $unit,
                    $price,
                    $description
                );
                """;


            var products = new[]
            {
                new
                {
                    Id = "P001",
                    Code = "HCG",
                    Name = "HỘP CAO GAN",
                    Unit = "Cái",
                    Price = 0m,
                    Description = ""
                },

                new
                {
                    Id = "P002",
                    Code = "TG",
                    Name = "TÚI GIẤY",
                    Unit = "Cái",
                    Price = 0m,
                    Description = ""
                },

                new
                {
                    Id = "P003",
                    Code = "HMP",
                    Name = "HỘP MỸ PHẨM",
                    Unit = "Cái",
                    Price = 0m,
                    Description = ""
                }
            };


            foreach (var product in products)
            {
                using var command =
                    connection.CreateCommand();

                command.CommandText =
                    sql;


                command.Parameters.AddWithValue(
                    "$id",
                    product.Id);


                command.Parameters.AddWithValue(
                    "$code",
                    product.Code);


                command.Parameters.AddWithValue(
                    "$name",
                    product.Name);


                command.Parameters.AddWithValue(
                    "$unit",
                    product.Unit);


                command.Parameters.AddWithValue(
                    "$price",
                    product.Price);


                command.Parameters.AddWithValue(
                    "$description",
                    product.Description);


                command.ExecuteNonQuery();
            }
        }


        // =========================================================
        // PRODUCT
        // =========================================================

        private static void CreateProductTable(
            SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS Product
        (
            Id TEXT PRIMARY KEY,

            ProductCode TEXT NOT NULL DEFAULT '',

            CreatedDate TEXT NOT NULL DEFAULT '',

            CustomerId TEXT NOT NULL DEFAULT '',

            ProductName TEXT NOT NULL,

            ProductType TEXT NOT NULL DEFAULT '',

            ReprintType TEXT NOT NULL DEFAULT '',

            Specification TEXT NOT NULL DEFAULT '',

            SpreadSize TEXT NOT NULL DEFAULT '',

            Quantity INTEGER NOT NULL DEFAULT 0,

            Difficulty REAL NOT NULL DEFAULT 0,

            QuotationDeadline TEXT NULL,

            Note TEXT NOT NULL DEFAULT '',

            Unit TEXT NOT NULL DEFAULT '',

            UnitPrice REAL NOT NULL DEFAULT 0,

            Description TEXT NOT NULL DEFAULT ''
        );
        """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;

            command.ExecuteNonQuery();
        }


        // =========================================================
        // CUSTOMER
        // =========================================================

        private static void CreateCustomerTable(
            SqliteConnection connection)
        {
            const string sql = """
                CREATE TABLE IF NOT EXISTS Customer
                (
                    Id TEXT PRIMARY KEY,

                    ShortName TEXT NOT NULL,

                    FullName TEXT NOT NULL,

                    Address TEXT,

                    TaxCode TEXT,

                    ContactPerson TEXT,

                    Email TEXT,

                    Phone TEXT
                );
                """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;

            command.ExecuteNonQuery();
        }


        // =========================================================
        // QUOTATION
        // =========================================================

        private static void CreateQuotationTable(
            SqliteConnection connection)
        {
            const string sql = """
                CREATE TABLE IF NOT EXISTS Quotation
                (
                    Id TEXT PRIMARY KEY,

                    CustomerId TEXT NOT NULL,

                    CreatedDate TEXT NOT NULL,

                    ContactPerson TEXT,

                    Phone TEXT,

                    SubTotal REAL NOT NULL DEFAULT 0,

                    Discount REAL NOT NULL DEFAULT 0,

                    TotalAmount REAL NOT NULL DEFAULT 0,

                    Status TEXT NOT NULL DEFAULT 'Nháp',

                    Note TEXT,

                    FOREIGN KEY (CustomerId)
                        REFERENCES Customer(Id)
                );
                """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;

            command.ExecuteNonQuery();
        }


        // =========================================================
        // QUOTATION ITEM
        // =========================================================

        private static void CreateQuotationItemTable(
            SqliteConnection connection)
        {
            const string sql = """
                CREATE TABLE IF NOT EXISTS QuotationItem
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,

                    QuotationId TEXT NOT NULL,

                    ProductId TEXT,

                    ProductName TEXT NOT NULL,

                    Quantity REAL NOT NULL DEFAULT 0,

                    UnitPrice REAL NOT NULL DEFAULT 0,

                    Amount REAL NOT NULL DEFAULT 0,

                    FOREIGN KEY (QuotationId)
                        REFERENCES Quotation(Id)
                        ON DELETE CASCADE
                );
                """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;

            command.ExecuteNonQuery();
        }


        // =========================================================
        // COMPANY SETTING
        // =========================================================

        private static void CreateCompanySettingTable(
            SqliteConnection connection)
        {
            const string sql = """
                CREATE TABLE IF NOT EXISTS CompanySetting
                (
                    Id INTEGER PRIMARY KEY,

                    CompanyName TEXT NOT NULL DEFAULT '',

                    Address TEXT NOT NULL DEFAULT '',

                    TaxCode TEXT NOT NULL DEFAULT '',

                    Phone TEXT NOT NULL DEFAULT '',

                    Email TEXT NOT NULL DEFAULT '',

                    Website TEXT NOT NULL DEFAULT '',

                    LogoPath TEXT NOT NULL DEFAULT ''
                );
                """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;

            command.ExecuteNonQuery();


            // -----------------------------------------------------
            // TẠO BẢN GHI MẶC ĐỊNH
            // -----------------------------------------------------

            const string insertSql = """
                INSERT OR IGNORE INTO CompanySetting
                (
                    Id,
                    CompanyName,
                    Address,
                    TaxCode,
                    Phone,
                    Email,
                    Website,
                    LogoPath
                )
                VALUES
                (
                    1,
                    '',
                    '',
                    '',
                    '',
                    '',
                    '',
                    ''
                );
                """;


            using var insertCommand =
                connection.CreateCommand();

            insertCommand.CommandText =
                insertSql;

            insertCommand.ExecuteNonQuery();
        }


        // =========================================================
        // DIFFICULTY SETTING
        // =========================================================

        private static void CreateDifficultySettingTable(
            SqliteConnection connection)
        {
            const string sql = """
                CREATE TABLE IF NOT EXISTS DifficultySetting
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,

                    Difficulty REAL NOT NULL DEFAULT 0,

                    WasteRate REAL NOT NULL DEFAULT 0,

                    SortOrder INTEGER NOT NULL DEFAULT 0
                );
                """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;

            command.ExecuteNonQuery();
        }

        // =========================================================
        // REPRINT SETTING
        // =========================================================

        private static void CreateReprintSettingTable(
            SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS ReprintSetting
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            CheckReprint TEXT NOT NULL,

            MoldChange TEXT NOT NULL,

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;

            command.ExecuteNonQuery();
        }

        private static void CreateWasteSettingTable(
    SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS WasteSetting
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            SoTo TEXT NOT NULL,

            Mau01 REAL NOT NULL DEFAULT 0,
            Mau02 REAL NOT NULL DEFAULT 0,
            Mau03 REAL NOT NULL DEFAULT 0,
            Mau04 REAL NOT NULL DEFAULT 0,
            Mau05 REAL NOT NULL DEFAULT 0,
            Mau06 REAL NOT NULL DEFAULT 0,
            Mau07 REAL NOT NULL DEFAULT 0,

            CanMang REAL NOT NULL DEFAULT 0,
            TrangPhu REAL NOT NULL DEFAULT 0,
            Be REAL NOT NULL DEFAULT 0,
            CanVan REAL NOT NULL DEFAULT 0,
            EpNhu REAL NOT NULL DEFAULT 0,
            Boi REAL NOT NULL DEFAULT 0,
            DongKim REAL NOT NULL DEFAULT 0,
            Cat REAL NOT NULL DEFAULT 0,
            Dan REAL NOT NULL DEFAULT 0,

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.ExecuteNonQuery();
        }

        // =========================================================
        // PAPER PRICE SETTING
        // =========================================================

        private static void CreatePaperPriceSettingTable(
            SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS PaperPriceSetting
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            Loai TEXT NOT NULL DEFAULT '',

            MaGiay TEXT NOT NULL DEFAULT '',

            TenGiay TEXT NOT NULL DEFAULT '',

            VndPerM2 REAL NOT NULL DEFAULT 0,

            VndPerTon REAL NOT NULL DEFAULT 0,

            Size79x109 REAL NOT NULL DEFAULT 0,

            Size65x86 REAL NOT NULL DEFAULT 0,

            Size65x84 REAL NOT NULL DEFAULT 0,

            Size80x120 REAL NOT NULL DEFAULT 0,

            Size79x54 REAL NOT NULL DEFAULT 0,

            Size39x54 REAL NOT NULL DEFAULT 0,

            Size60x80 REAL NOT NULL DEFAULT 0,

            Size65x43 REAL NOT NULL DEFAULT 0,

            Size60x42 REAL NOT NULL DEFAULT 0,

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;


            using var command =
                connection.CreateCommand();


            command.CommandText =
                sql;


            command.ExecuteNonQuery();
        }

        // =========================================================
        // PAPER SIZE SETTING
        // =========================================================

        private static void CreatePaperSizeSettingTable(
            SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS PaperSizeSetting
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            PrintSize TEXT NOT NULL DEFAULT '',

            PrinterSize TEXT NOT NULL DEFAULT '',

            PlateSize TEXT NOT NULL DEFAULT '',

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;


            using var command =
                connection.CreateCommand();


            command.CommandText =
                sql;


            command.ExecuteNonQuery();
        }

        // =========================================================
        // GRIP SETTING
        // NHÍP - TAY KÊ - TRÀN MÀU
        // =========================================================

        private static void CreateGripSettingTable(
            SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS GripSettings
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            SettingName TEXT NOT NULL DEFAULT '',

            Value REAL NOT NULL DEFAULT 0,

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;


            using var command =
                connection.CreateCommand();


            command.CommandText =
                sql;


            command.ExecuteNonQuery();
        }

        // =========================================================
        // SHIPPING SETTING
        // TAI DÁN - KHÓA GÀI
        // =========================================================

        private static void CreateShippingSettingTable(
            SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS ShippingSettings
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            Width TEXT NOT NULL DEFAULT '',

            Glue REAL NOT NULL DEFAULT 0,

            Lock REAL NOT NULL DEFAULT 0,

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;


            using var command =
                connection.CreateCommand();


            command.CommandText =
                sql;


            command.ExecuteNonQuery();
        }

        // =========================================================
        // BOM SETTING
        // LOẠI SẢN PHẨM
        // =========================================================

        private static void CreateBomSettingTable(
            SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS BomSettings
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            ProductType TEXT NOT NULL DEFAULT '',

            Specification TEXT NOT NULL DEFAULT '',

            Formula TEXT NOT NULL DEFAULT '',

            PrintingType TEXT NOT NULL DEFAULT '',

            Process1 TEXT NOT NULL DEFAULT '',

            Process2 TEXT NOT NULL DEFAULT '',

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;


            using var command =
                connection.CreateCommand();


            command.CommandText =
                sql;


            command.ExecuteNonQuery();
        }

        // =========================================================
        // PRINT BATCH SETTING
        // LÔ IN - LÔ KẼM
        // =========================================================

        private static void CreatePrintBatchSettingTable(
            SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS PrintBatchSettings
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            LotName TEXT NOT NULL DEFAULT '',

            Quantity REAL NOT NULL DEFAULT 0,

            Unit TEXT NOT NULL DEFAULT '',

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;


            using var command =
                connection.CreateCommand();


            command.CommandText =
                sql;


            command.ExecuteNonQuery();
        }

        // =========================================================
        // PRINT TYPE SETTING
        // QUY ƯỚC CÁC KIỂU IN
        // =========================================================

        private static void CreatePrintTypeSettingTable(
    SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS PrintTypeSettings
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            PrintingTechnology TEXT NOT NULL DEFAULT '',

            PrintType TEXT NOT NULL DEFAULT '',

            PlateSetCount REAL NOT NULL DEFAULT 0,

            PrintCount REAL NOT NULL DEFAULT 0,

            WasteFactor REAL NOT NULL DEFAULT 0,

            PaperFactor REAL NOT NULL DEFAULT 0,

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;

            command.ExecuteNonQuery();
        }

        // =========================================================
        // PRINT PRICE SETTING
        // ĐƠN GIÁ KẼM IN VÀ MỰC IN
        // =========================================================

        private static void CreatePrintPriceSettingTable(
            SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS PrintPriceSettings
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            MachineSize TEXT NOT NULL DEFAULT '',

            PricePerPlate REAL NOT NULL DEFAULT 0,

            Cmyk REAL NOT NULL DEFAULT 0,

            Pantone REAL NOT NULL DEFAULT 0,

            CmykJobPrice REAL NOT NULL DEFAULT 0,

            SpotColorJobPrice REAL NOT NULL DEFAULT 0,

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;


            using var command =
                connection.CreateCommand();


            command.CommandText =
                sql;


            command.ExecuteNonQuery();
        }

        // =========================================================
        // PROCESS PRICE SETTING
        // GIÁ GIA CÔNG
        // =========================================================

        private static void CreateProcessPriceSettingTable(
            SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS ProcessPriceSettings
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            Process TEXT NOT NULL DEFAULT '',

            Stage TEXT NOT NULL DEFAULT '',

            UnitPrice REAL NOT NULL DEFAULT 0,

            Unit TEXT NOT NULL DEFAULT '',

            MinimumBatchPrice REAL NOT NULL DEFAULT 0,

            MinimumQuantity REAL NOT NULL DEFAULT 0,

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;


            using var command =
                connection.CreateCommand();


            command.CommandText =
                sql;


            command.ExecuteNonQuery();
        }

        // =========================================================
        // MOLD PRICE SETTING
        // GIÁ KHUÔN
        // =========================================================

        private static void CreateMoldPriceSettingTable(
            SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS MoldPriceSettings
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            MoldType TEXT NOT NULL DEFAULT '',

            PricePerMold REAL NOT NULL DEFAULT 0,

            PricePerSquareCm REAL NOT NULL DEFAULT 0,

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;


            using var command =
                connection.CreateCommand();


            command.CommandText =
                sql;


            command.ExecuteNonQuery();
        }


        // =========================================================
        // UNIT SETTING
        // ĐƠN VỊ TÍNH
        // =========================================================

        private static void CreateUnitSettingTable(
            SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS UnitSettings
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            Unit TEXT NOT NULL DEFAULT '',

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;


            using var command =
                connection.CreateCommand();


            command.CommandText =
                sql;


            command.ExecuteNonQuery();
        }

        // =========================================================
        // SUPPORT SETTING - GIÁ SUPPORT
        // =========================================================

        private static void CreateSupportSettingTable(
            SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS SupportSettings
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            Loai TEXT NOT NULL DEFAULT '',

            VndPerKg REAL NOT NULL DEFAULT 0,

            VndPerM2 REAL NOT NULL DEFAULT 0,

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;


            using var command =
                connection.CreateCommand();


            command.CommandText =
                sql;


            command.ExecuteNonQuery();
        }


        // =========================================================
        // GLUE PRICE SETTING
        // GIÁ KEO DÁN
        // =========================================================

        private static void CreateGluePriceSettingTable(
            SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS GluePriceSettings
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            LoaiKeo TEXT NOT NULL DEFAULT '',

            VndPerKg REAL NOT NULL DEFAULT 0,

            VndPer10Cm REAL NOT NULL DEFAULT 0,

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;

            command.ExecuteNonQuery();
        }

        // =========================================================
        // FILM SETTING
        // QUY ĐỔI TRỌNG LƯỢNG MÀNG
        // =========================================================

        private static void CreateFilmSettingTable(
            SqliteConnection connection)
        {
            const string sql = """
        CREATE TABLE IF NOT EXISTS FilmSettings
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,

            LoaiMang TEXT NOT NULL DEFAULT '',

            DayMicron REAL NOT NULL DEFAULT 0,

            DaiM REAL NOT NULL DEFAULT 0,

            TrongLuongKgPerM REAL NOT NULL DEFAULT 0,

            M2PerKg REAL NOT NULL DEFAULT 0,

            SortOrder INTEGER NOT NULL DEFAULT 0
        );
        """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;

            command.ExecuteNonQuery();
        }

    }


}