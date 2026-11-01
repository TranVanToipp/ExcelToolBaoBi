using ExcelTool.Database;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ExcelTool.Features.Quotation
{
    public class QuotationService
    {
        // ==========================================
        // LẤY TẤT CẢ BÁO GIÁ
        // ==========================================

        public List<Quotation> GetAll()
        {
            var quotations = new List<Quotation>();

            using var connection =
                DatabaseService.GetConnection();

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = """
                SELECT
                    q.Id,
                    q.CustomerId,
                    c.FullName AS CustomerName,
                    q.CreatedDate,
                    q.ContactPerson,
                    q.Phone,
                    q.SubTotal,
                    q.Discount,
                    q.TotalAmount,
                    q.Status,
                    q.Note
                FROM Quotation q
                LEFT JOIN Customer c
                    ON q.CustomerId = c.Id
                ORDER BY q.CreatedDate DESC;
                """;

            using var reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                quotations.Add(MapQuotation(reader));
            }

            return quotations;
        }


        // ==========================================
        // LẤY BÁO GIÁ THEO ID
        // ==========================================

        public Quotation? GetById(string id)
        {
            using var connection =
                DatabaseService.GetConnection();

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = """
                SELECT
                    q.Id,
                    q.CustomerId,
                    c.FullName AS CustomerName,
                    q.CreatedDate,
                    q.ContactPerson,
                    q.Phone,
                    q.SubTotal,
                    q.Discount,
                    q.TotalAmount,
                    q.Status,
                    q.Note
                FROM Quotation q
                LEFT JOIN Customer c
                    ON q.CustomerId = c.Id
                WHERE q.Id = $id;
                """;

            command.Parameters.AddWithValue(
                "$id",
                id);

            using var reader =
                command.ExecuteReader();

            if (!reader.Read())
                return null;

            return MapQuotation(reader);
        }


        // ==========================================
        // LẤY CHI TIẾT SẢN PHẨM
        // ==========================================

        public List<QuotationItem> GetItems(
            string quotationId)
        {
            var items =
                new List<QuotationItem>();

            using var connection =
                DatabaseService.GetConnection();

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = """
                SELECT
                    Id,
                    QuotationId,
                    ProductId,
                    ProductName,
                    Quantity,
                    UnitPrice
                FROM QuotationItem
                WHERE QuotationId = $quotationId
                ORDER BY Id;
                """;

            command.Parameters.AddWithValue(
                "$quotationId",
                quotationId);

            using var reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                items.Add(new QuotationItem
                {
                    Id =
                        Convert.ToInt32(
                            reader["Id"]),

                    QuotationId =
                        reader["QuotationId"]
                            ?.ToString() ?? "",

                    ProductId =
                        reader["ProductId"]
                            ?.ToString() ?? "",

                    ProductName =
                        reader["ProductName"]
                            ?.ToString() ?? "",

                    Quantity =
                        Convert.ToDecimal(
                            reader["Quantity"]),

                    UnitPrice =
                        Convert.ToDecimal(
                            reader["UnitPrice"])
                });
            }

            return items;
        }


        // ==========================================
        // LƯU BÁO GIÁ
        // ==========================================

        public string Save(
            Quotation quotation,
            List<QuotationItem> items)
        {
            if (quotation == null)
                throw new ArgumentNullException(
                    nameof(quotation));

            if (items == null)
                throw new ArgumentNullException(
                    nameof(items));

            if (string.IsNullOrWhiteSpace(
                    quotation.CustomerId))
            {
                throw new Exception(
                    "Vui lòng chọn khách hàng.");
            }

            if (items.Count == 0)
            {
                throw new Exception(
                    "Báo giá phải có ít nhất một sản phẩm.");
            }


            // ==========================================
            // TẠO ID BÁO GIÁ
            // ==========================================

            string quotationId =
                GenerateQuotationId();


            // ==========================================
            // TÍNH LẠI GIÁ TRỊ
            // ==========================================

            decimal subTotal = 0;

            foreach (var item in items)
            {
                if (item.Quantity < 0)
                {
                    throw new Exception(
                        $"Số lượng của sản phẩm '{item.ProductName}' không hợp lệ.");
                }

                if (item.UnitPrice < 0)
                {
                    throw new Exception(
                        $"Đơn giá của sản phẩm '{item.ProductName}' không hợp lệ.");
                }

                decimal amount =
                    item.Quantity *
                    item.UnitPrice;

                subTotal += amount;
            }


            decimal discount =
                quotation.Discount;

            if (discount < 0)
                discount = 0;

            if (discount > subTotal)
                discount = subTotal;


            decimal totalAmount =
                subTotal - discount;


            // ==========================================
            // CẬP NHẬT OBJECT QUOTATION
            // ==========================================

            quotation.Id =
                quotationId;

            quotation.CreatedDate =
                quotation.CreatedDate == DateTime.MinValue
                    ? DateTime.Now
                    : quotation.CreatedDate;

            quotation.SubTotal =
                subTotal;

            quotation.Discount =
                discount;

            quotation.TotalAmount =
                totalAmount;

            if (string.IsNullOrWhiteSpace(
                    quotation.Status))
            {
                quotation.Status = "Nháp";
            }


            // ==========================================
            // DATABASE
            // ==========================================

            using var connection =
                DatabaseService.GetConnection();

            connection.Open();


            // ==========================================
            // TRANSACTION
            // ==========================================

            using var transaction =
                connection.BeginTransaction();

            try
            {
                // ======================================
                // INSERT QUOTATION
                // ======================================

                using var quotationCommand =
                    connection.CreateCommand();

                quotationCommand.Transaction =
                    transaction;

                quotationCommand.CommandText = """
                    INSERT INTO Quotation
                    (
                        Id,
                        CustomerId,
                        CreatedDate,
                        ContactPerson,
                        Phone,
                        SubTotal,
                        Discount,
                        TotalAmount,
                        Status,
                        Note
                    )
                    VALUES
                    (
                        $id,
                        $customerId,
                        $createdDate,
                        $contactPerson,
                        $phone,
                        $subTotal,
                        $discount,
                        $totalAmount,
                        $status,
                        $note
                    );
                    """;


                quotationCommand.Parameters.AddWithValue(
                    "$id",
                    quotation.Id);

                quotationCommand.Parameters.AddWithValue(
                    "$customerId",
                    quotation.CustomerId);

                quotationCommand.Parameters.AddWithValue(
                    "$createdDate",
                    quotation.CreatedDate.ToString(
                        "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture));

                quotationCommand.Parameters.AddWithValue(
                    "$contactPerson",
                    quotation.ContactPerson ?? "");

                quotationCommand.Parameters.AddWithValue(
                    "$phone",
                    quotation.Phone ?? "");

                quotationCommand.Parameters.AddWithValue(
                    "$subTotal",
                    quotation.SubTotal);

                quotationCommand.Parameters.AddWithValue(
                    "$discount",
                    quotation.Discount);

                quotationCommand.Parameters.AddWithValue(
                    "$totalAmount",
                    quotation.TotalAmount);

                quotationCommand.Parameters.AddWithValue(
                    "$status",
                    quotation.Status ?? "Nháp");

                quotationCommand.Parameters.AddWithValue(
                    "$note",
                    quotation.Note ?? "");

                quotationCommand.ExecuteNonQuery();


                // ======================================
                // INSERT QUOTATION ITEMS
                // ======================================

                foreach (var item in items)
                {
                    using var itemCommand =
                        connection.CreateCommand();

                    itemCommand.Transaction =
                        transaction;

                    itemCommand.CommandText = """
                        INSERT INTO QuotationItem
                        (
                            QuotationId,
                            ProductId,
                            ProductName,
                            Quantity,
                            UnitPrice,
                            Amount
                        )
                        VALUES
                        (
                            $quotationId,
                            $productId,
                            $productName,
                            $quantity,
                            $unitPrice,
                            $amount
                        );
                        """;


                    decimal amount =
                        item.Quantity *
                        item.UnitPrice;


                    itemCommand.Parameters.AddWithValue(
                        "$quotationId",
                        quotationId);

                    itemCommand.Parameters.AddWithValue(
                        "$productId",
                        item.ProductId ?? "");

                    itemCommand.Parameters.AddWithValue(
                        "$productName",
                        item.ProductName ?? "");

                    itemCommand.Parameters.AddWithValue(
                        "$quantity",
                        item.Quantity);

                    itemCommand.Parameters.AddWithValue(
                        "$unitPrice",
                        item.UnitPrice);

                    itemCommand.Parameters.AddWithValue(
                        "$amount",
                        amount);

                    itemCommand.ExecuteNonQuery();
                }


                // ======================================
                // COMMIT
                // ======================================

                transaction.Commit();

                return quotationId;
            }
            catch
            {
                // ======================================
                // ROLLBACK
                // ======================================

                transaction.Rollback();

                throw;
            }
        }

        // ==========================================
        // CẬP NHẬT BÁO GIÁ
        // ==========================================

        public void Update(
            Quotation quotation,
            List<QuotationItem> items)
        {
            if (quotation == null)
                throw new ArgumentNullException(
                    nameof(quotation));

            if (items == null)
                throw new ArgumentNullException(
                    nameof(items));

            if (string.IsNullOrWhiteSpace(
                    quotation.Id))
            {
                throw new Exception(
                    "Mã báo giá không hợp lệ.");
            }

            if (string.IsNullOrWhiteSpace(
                    quotation.CustomerId))
            {
                throw new Exception(
                    "Vui lòng chọn khách hàng.");
            }

            if (items.Count == 0)
            {
                throw new Exception(
                    "Báo giá phải có ít nhất một sản phẩm.");
            }


            // ==========================================
            // TÍNH LẠI TỔNG TIỀN
            // ==========================================

            decimal subTotal = 0;

            foreach (var item in items)
            {
                if (item.Quantity <= 0)
                {
                    throw new Exception(
                        $"Số lượng của sản phẩm " +
                        $"'{item.ProductName}' phải lớn hơn 0.");
                }

                if (item.UnitPrice < 0)
                {
                    throw new Exception(
                        $"Đơn giá của sản phẩm " +
                        $"'{item.ProductName}' không hợp lệ.");
                }

                subTotal +=
                    item.Quantity *
                    item.UnitPrice;
            }


            decimal discount =
                quotation.Discount;

            if (discount < 0)
                discount = 0;

            if (discount > subTotal)
                discount = subTotal;


            decimal totalAmount =
                subTotal - discount;


            quotation.SubTotal =
                subTotal;

            quotation.TotalAmount =
                totalAmount;


            // ==========================================
            // DATABASE
            // ==========================================

            using var connection =
                DatabaseService.GetConnection();

            connection.Open();


            using var transaction =
                connection.BeginTransaction();

            try
            {
                // ======================================
                // UPDATE QUOTATION
                // ======================================

                using var quotationCommand =
                    connection.CreateCommand();

                quotationCommand.Transaction =
                    transaction;

                quotationCommand.CommandText = """
            UPDATE Quotation
            SET
                CustomerId = $customerId,
                ContactPerson = $contactPerson,
                Phone = $phone,
                SubTotal = $subTotal,
                Discount = $discount,
                TotalAmount = $totalAmount,
                Status = $status,
                Note = $note
            WHERE Id = $id;
            """;


                quotationCommand.Parameters.AddWithValue(
                    "$id",
                    quotation.Id);

                quotationCommand.Parameters.AddWithValue(
                    "$customerId",
                    quotation.CustomerId);

                quotationCommand.Parameters.AddWithValue(
                    "$contactPerson",
                    quotation.ContactPerson ?? "");

                quotationCommand.Parameters.AddWithValue(
                    "$phone",
                    quotation.Phone ?? "");

                quotationCommand.Parameters.AddWithValue(
                    "$subTotal",
                    quotation.SubTotal);

                quotationCommand.Parameters.AddWithValue(
                    "$discount",
                    quotation.Discount);

                quotationCommand.Parameters.AddWithValue(
                    "$totalAmount",
                    quotation.TotalAmount);

                quotationCommand.Parameters.AddWithValue(
                    "$status",
                    quotation.Status ?? "Nháp");

                quotationCommand.Parameters.AddWithValue(
                    "$note",
                    quotation.Note ?? "");

                quotationCommand.ExecuteNonQuery();


                // ======================================
                // XÓA ITEM CŨ
                // ======================================

                using var deleteCommand =
                    connection.CreateCommand();

                deleteCommand.Transaction =
                    transaction;

                deleteCommand.CommandText = """
            DELETE FROM QuotationItem
            WHERE QuotationId = $quotationId;
            """;

                deleteCommand.Parameters.AddWithValue(
                    "$quotationId",
                    quotation.Id);

                deleteCommand.ExecuteNonQuery();


                // ======================================
                // INSERT ITEM MỚI
                // ======================================

                foreach (var item in items)
                {
                    decimal amount =
                        item.Quantity *
                        item.UnitPrice;


                    using var itemCommand =
                        connection.CreateCommand();

                    itemCommand.Transaction =
                        transaction;

                    itemCommand.CommandText = """
                INSERT INTO QuotationItem
                (
                    QuotationId,
                    ProductId,
                    ProductName,
                    Quantity,
                    UnitPrice,
                    Amount
                )
                VALUES
                (
                    $quotationId,
                    $productId,
                    $productName,
                    $quantity,
                    $unitPrice,
                    $amount
                );
                """;


                    itemCommand.Parameters.AddWithValue(
                        "$quotationId",
                        quotation.Id);

                    itemCommand.Parameters.AddWithValue(
                        "$productId",
                        item.ProductId ?? "");

                    itemCommand.Parameters.AddWithValue(
                        "$productName",
                        item.ProductName ?? "");

                    itemCommand.Parameters.AddWithValue(
                        "$quantity",
                        item.Quantity);

                    itemCommand.Parameters.AddWithValue(
                        "$unitPrice",
                        item.UnitPrice);

                    itemCommand.Parameters.AddWithValue(
                        "$amount",
                        amount);

                    itemCommand.ExecuteNonQuery();
                }


                // ======================================
                // COMMIT
                // ======================================

                transaction.Commit();
            }
            catch
            {
                // ======================================
                // ROLLBACK
                // ======================================

                transaction.Rollback();

                throw;
            }
        }


        // ==========================================
        // SINH MÃ BÁO GIÁ
        // ==========================================

        private string GenerateQuotationId()
        {
            string id;

            do
            {
                id =
                    "BG" +
                    DateTime.Now.ToString(
                        "yyyyMMddHHmmssfff");
            }
            while (Exists(id));

            return id;
        }

        // ==========================================
        // XÓA BÁO GIÁ
        // ==========================================

        public void Delete(string quotationId)
        {
            if (string.IsNullOrWhiteSpace(quotationId))
            {
                throw new Exception(
                    "Mã báo giá không hợp lệ.");
            }


            using var connection =
                DatabaseService.GetConnection();

            connection.Open();


            using var transaction =
                connection.BeginTransaction();


            try
            {
                // ======================================
                // XÓA BÁO GIÁ
                // ======================================

                using var command =
                    connection.CreateCommand();

                command.Transaction =
                    transaction;

                command.CommandText = """
            DELETE FROM Quotation
            WHERE Id = $id;
            """;


                command.Parameters.AddWithValue(
                    "$id",
                    quotationId);


                int affectedRows =
                    command.ExecuteNonQuery();


                // ======================================
                // KIỂM TRA
                // ======================================

                if (affectedRows == 0)
                {
                    throw new Exception(
                        "Không tìm thấy báo giá cần xóa.");
                }


                // ======================================
                // COMMIT
                // ======================================

                transaction.Commit();
            }
            catch
            {
                // ======================================
                // ROLLBACK
                // ======================================

                transaction.Rollback();

                throw;
            }
        }


        // ==========================================
        // KIỂM TRA ID
        // ==========================================

        private bool Exists(string id)
        {
            using var connection =
                DatabaseService.GetConnection();

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = """
                SELECT COUNT(*)
                FROM Quotation
                WHERE Id = $id;
                """;

            command.Parameters.AddWithValue(
                "$id",
                id);

            long count =
                Convert.ToInt64(
                    command.ExecuteScalar());

            return count > 0;
        }


        // ==========================================
        // MAP QUOTATION
        // ==========================================

        private Quotation MapQuotation(
            Microsoft.Data.Sqlite.SqliteDataReader reader)
        {
            return new Quotation
            {
                Id =
                    reader["Id"]?.ToString() ?? "",

                CustomerId =
                    reader["CustomerId"]?.ToString() ?? "",

                CustomerName =
                    reader["CustomerName"]?.ToString() ?? "",

                CreatedDate =
                    DateTime.TryParse(
                        reader["CreatedDate"]?.ToString(),
                        out DateTime createdDate)
                        ? createdDate
                        : DateTime.MinValue,

                ContactPerson =
                    reader["ContactPerson"]?.ToString() ?? "",

                Phone =
                    reader["Phone"]?.ToString() ?? "",

                SubTotal =
                    Convert.ToDecimal(
                        reader["SubTotal"]),

                Discount =
                    Convert.ToDecimal(
                        reader["Discount"]),

                TotalAmount =
                    Convert.ToDecimal(
                        reader["TotalAmount"]),

                Status =
                    reader["Status"]?.ToString()
                    ?? "Nháp",

                Note =
                    reader["Note"]?.ToString() ?? ""
            };
        }
    }
}