using ManageCustomer.Models;

namespace ManageCustomer.Services
{
    public interface ILeaderboardService
    {
        public decimal UpdateScore(long customerId, decimal scoreDelta);
        public List<Customer> GetCustomersByRank(int start, int end);
        public Customer GetCustomerById(long customerId, int high, int low);
    }
}
