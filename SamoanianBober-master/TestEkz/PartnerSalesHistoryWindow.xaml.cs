using System.Windows;
using Microsoft.EntityFrameworkCore;
using TestEkz.Entities;
using System.Linq;

namespace TestEkz
{
    public partial class PartnerSalesHistoryWindow : Window
    {
        private readonly Partner _partner;

        public PartnerSalesHistoryWindow(Partner partner)
        {
            InitializeComponent();
            _partner = partner;
            LoadPartnerInfo();
            LoadSalesHistory();
        }

        private void LoadPartnerInfo()
        {
            PartnerInfoText.Text = $"{_partner.CompanyName} (ИНН: {_partner.TaxId}) - " +
                                 $"Общий объем продаж: {_partner.TotalSalesVolume:N2} руб., " +
                                 $"Текущая скидка: {_partner.CurrentDiscount}%";
        }

        private void LoadSalesHistory()
        {
            using (var context = new TestEkzContext())
            {
                var history = context.OrderItems
                    .Include(oi => oi.Order)
                    .Include(oi => oi.Product)
                    .Where(oi => oi.Order != null && oi.Order.PartnerId == _partner.PartnerId)
                    .Select(oi => new PartnerSalesViewModel
                    {
                        OrderDate = oi.Order.OrderDate,  // Теперь типы совместимы
                        ProductName = oi.Product.ProductName,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        TotalAmount = oi.Quantity * oi.UnitPrice,
                        Status = oi.Status
                    })
                    .OrderByDescending(h => h.OrderDate)
                    .ToList();

                SalesHistoryDataGrid.ItemsSource = history;
            }
        }
    }

    public class PartnerSalesViewModel
    {
        public DateTime? OrderDate { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}