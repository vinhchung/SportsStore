using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> _lineCollection = new List<CartLine>();

        public void AddItem(Product product, int quantity)
        {
            CartLine line = _lineCollection.Where(c => c.Product.ProductID == product.ProductID).FirstOrDefault();
            if (line == null)
            {
                _lineCollection.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(int productId)
        {
            _lineCollection.RemoveAll(l => l.Product.ProductID == productId);
        }

        public decimal ComputeTotalValue()
        {
            return _lineCollection.Sum(l => l.Product.UnitPrice.Value * l.Quantity);
        }

        public void Clear()
        {
            _lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return _lineCollection; }
        }
    }
}
