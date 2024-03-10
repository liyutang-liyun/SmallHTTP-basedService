using ManageCustomer.Models;
using System.Collections.Concurrent;

namespace ManageCustomer.Services
{
    public class LeaderboardService: ILeaderboardService
    {
        private readonly ConcurrentDictionary<long, Customer> _customers;
        private int _rankCounter;

        public LeaderboardService()
        {
            _customers = new ConcurrentDictionary<long, Customer>();
            _rankCounter = 0;
        }

        public decimal UpdateScore(long customerId, decimal scoreDelta)
        {
            if (!_customers.ContainsKey(customerId))
            {
                Interlocked.Increment(ref _rankCounter);
                _customers[customerId] = new Customer { CustomerId = customerId, Score = scoreDelta, Rank = _rankCounter };
            }
            else
            {
                _customers[customerId].Score += scoreDelta;
            }

            UpdateRanks();
            return _customers[customerId].Score;
        }

        private void UpdateRanks()
        {
            var orderedCustomers = _customers?.Values.OrderByDescending(c => c.Score).ThenBy(c => c.CustomerId).ToList();
            for (int i = 0; i < orderedCustomers.Count; i++)
            {
                orderedCustomers[i].Rank = i + 1;
            }
        }

        public List<Customer> GetCustomersByRank(int start, int end)
        {
            return _customers?.Values.Where(c => c.Rank >= start && c.Rank <= end).ToList();
        }

        public Customer GetCustomerById(long customerId, int high, int low)
        {
            if (!_customers.ContainsKey(customerId)) return new Customer();

            var customer = _customers[customerId];
            var customers = _customers.Values
                .OrderBy(c => Math.Abs(c.Rank - customer.Rank))
                .Where(c => c.CustomerId != customerId)
                .ToList();

            var index = customers.IndexOf(customer);
            var startIndex = Math.Max(0, index - low);
            var endIndex = Math.Min(customers.Count - 1, index + high);

            return customers?.Skip(startIndex).Take(endIndex - startIndex + 1).ToList().FirstOrDefault();
        }
    }
}
