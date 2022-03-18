using TestPaymob.Models;

namespace TestPaymob.Repos.Interface
{
    public interface IPAyMobRepo
    {
        public Task<string>Purchase(Product product);
    }
}
