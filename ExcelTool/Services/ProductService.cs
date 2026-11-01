using System;
using System.Collections.Generic;

using ExcelTool.Models;
using ExcelTool.Repositories;

namespace ExcelTool.Services
{
    public class ProductService
    {
        // =========================================================
        // REPOSITORY
        // =========================================================

        private readonly ProductRepository
            _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public ProductService()
        {
            _repository =
                new ProductRepository();
        }


        // =========================================================
        // GET ALL
        // =========================================================

        public List<Product> GetAll()
        {
            return _repository.GetAll();
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public Product? GetById(
            string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            return _repository.GetById(id);
        }


        // =========================================================
        // CREATE
        // =========================================================

        public bool Create(
            Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(
                    nameof(product));
            }

            if (string.IsNullOrWhiteSpace(
                    product.Id))
            {
                throw new ArgumentException(
                    "Id sản phẩm không được để trống.",
                    nameof(product));
            }

            if (string.IsNullOrWhiteSpace(
                    product.ProductName))
            {
                throw new ArgumentException(
                    "Tên sản phẩm không được để trống.",
                    nameof(product));
            }

            if (product.Quantity < 0)
            {
                throw new ArgumentException(
                    "Số lượng không được nhỏ hơn 0.",
                    nameof(product));
            }

            return _repository.Insert(product);
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(
                    nameof(product));
            }

            if (string.IsNullOrWhiteSpace(
                    product.Id))
            {
                throw new ArgumentException(
                    "Id sản phẩm không được để trống.",
                    nameof(product));
            }

            if (string.IsNullOrWhiteSpace(
                    product.ProductName))
            {
                throw new ArgumentException(
                    "Tên sản phẩm không được để trống.",
                    nameof(product));
            }

            if (product.Quantity < 0)
            {
                throw new ArgumentException(
                    "Số lượng không được nhỏ hơn 0.",
                    nameof(product));
            }

            return _repository.Update(product);
        }


        // =========================================================
        // DELETE
        // =========================================================

        public bool Delete(
            string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            return _repository.Delete(id);
        }


        // =========================================================
        // DELETE ALL
        // =========================================================

        public bool DeleteAll()
        {
            return _repository.DeleteAll();
        }
    }
}